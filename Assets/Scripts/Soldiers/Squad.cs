using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class Squad : MonoBehaviour, IControllable
    {
        [Serializable]
        public enum Direction { Forward = 1, Backward = -1 }

        [SerializeField] private Side _side;
        [SerializeField, Min(1)] private int soldiersAmount = 3;
        [SerializeField, Min(0f)] private float _speed = 5f;
        [SerializeField] private float distanceFromPrimary = 1f;
        [SerializeField] private float xDistance = 0.05f;
        [field: SerializeField] public SquadTypeInfo SquadInfo { get; set; }
        [SerializeField] private GameObject vikingFlagPrefab;
        [SerializeField] private GameObject englishFlagPrefab;
        [SerializeField] private Vector3 flagPosition;
        [SerializeField] private bool _isSplineMovement = true;

        [Header("Followers")]        
        [SerializeField] private float _followingSpeedModifier = 2f;
        [SerializeField] private float _followingAccelerationModifier = 5f;
        [SerializeField] private float _stoppingDistanceModifier = 5f;

        private Soldier[] _soldiers;
        private PrimaryFollowerBehavior primaryFollower = null;
        private SecondaryFollowerBehavior[] secondaryFollowers;
        private Building building;

        private IFactory<Transform, Soldier> _soldierFactory;

        public Queue<ValueTuple<SplineComputer, Direction>> Roads { get; set; }
        public IControllable mainControllable => this;
        public float followingSpeedModifier => _followingSpeedModifier;
        public float followingAccelerationModifier => _followingAccelerationModifier;
        public float stoppingDistanceModifier => _stoppingDistanceModifier;
        public bool isSplineMovement => _isSplineMovement;

        private void Awake()
        {
            _soldiers = new Soldier[soldiersAmount];
            secondaryFollowers = new SecondaryFollowerBehavior[soldiersAmount - 1];
        }

        private void InitializeSoldier(int i)
        {
            _soldiers[i] = _soldierFactory.Create(transform);
            _soldiers[i].AddRenderPriority(_soldiers.Length - i);
            _soldiers[i].squad = this;
            _soldiers[i].SetHealth(SquadInfo.UnitInfo.Health);
            _soldiers[i].side = _side;
            _soldiers[i].SetTroopType(SquadInfo.TroopType);
            _soldiers[i].ChangeItems();
        }

        public void SpawnEmptySoldiers()
        {
            InitializeSoldier(0);

            var leader = new Leader(_soldiers[0], this);

            var flag = Instantiate(_side == Side.Vikings ? vikingFlagPrefab : englishFlagPrefab).GetComponent<Flag>();
            flag.Tagert = _soldiers[0].GetComponentInChildren<Weapon>().transform;
            flag.GetComponent<NetworkObject>().Spawn();
            flag.Offset = flagPosition;

            leader.ReachedDestination += () =>
            {
                building.SquadEnter(_side, SquadInfo);
                foreach (var soldier in _soldiers)
                {
                    Destroy(soldier.gameObject);

                };
                Destroy(flag.gameObject);
                Destroy(gameObject);
            };
            _soldiers[0].squadRole = leader;

            for (int i = 1; i < _soldiers.Length; i++)
            {
                InitializeSoldier(i);
                _soldiers[i].squadRole = new Follower(_soldiers[i], this);
            }

            GoTo(building.transform.position);
        }

        private void OnValidate()
        {
            if (primaryFollower == null)
                return;

            primaryFollower.SetSpeed(_speed);

            for (int i = 0; i < secondaryFollowers.Length; i++)
            {
                secondaryFollowers[i].SetDistance(distanceFromPrimary * (i / 2 + 1));
                secondaryFollowers[i].SetXDistance(xDistance * (i + 1) * (i % 2 == 1 ? 1 : -1));
            }
        }

        public void SetSide(Side side)
        {
            _side = side;
        }

        public void SetTarget(Building building)
        {
            this.building = building;
        }

        public void GoTo(Vector3 destination)
        {
            (_soldiers[0].squadRole as Leader).currentDestination = destination;
            _soldiers[0].squadRole.Move();

            for (int i = 1; i < _soldiers.Length; i++)
            {
                var columnPosition = Vector3.zero;
                columnPosition.z = distanceFromPrimary * ((i + 1) / 2);
                columnPosition.x = xDistance * (i % 2 == 1 ? 1 : -1);

                var follower = _soldiers[i].squadRole as Follower;
                follower.leader = _soldiers[0];
                follower.columnPosition = columnPosition;
                follower.Move();
            }
        }

        public class Factory : IFactory<Squad>
        {
            private IFactory<Transform, Soldier> _soldierFactory;
            private Transform _squadPrefab;

            public Factory(IFactory<Transform, Soldier> soldierFactory, Transform squadPrefab)
            {
                _soldierFactory = soldierFactory;
                _squadPrefab = squadPrefab;
            }

            public Squad Create()
            {
                Instantiate(_squadPrefab).TryGetComponent(out Squad squad);

                if (!squad) return null;

                squad._soldierFactory = _soldierFactory;

                return squad;
            }
        }
    }
}
