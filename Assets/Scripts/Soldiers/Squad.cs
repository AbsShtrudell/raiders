using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class Squad : MonoBehaviour, IControllable
    {
        [System.Serializable]
        public enum Direction { Forward = 1, Backward = -1 }

        [SerializeField] private GameObject soldierPrefab;
        [SerializeField] private Side _side;
        [SerializeField, Min(1)] private int soldiersAmount = 3;
        [SerializeField, Min(0f)] private float _speed = 5f;
        [SerializeField] private float distanceFromPrimary = 1f;
        [SerializeField] private float xDistance = 0.05f;
        [field: SerializeField] public SquadTypeInfo SquadInfo { get; set; }
        [SerializeField] private GameObject flagPrefab;
        [SerializeField] private Vector3 flagPosition;
        [SerializeField] private Vector3 flagEuler = new Vector3(0, 188, 0);
        [SerializeField] private bool _isSplineMovement = true;

        [Header("Followers")]        
        [SerializeField] private float _followingSpeedModifier = 2f;
        [SerializeField] private float _followingAccelerationModifier = 5f;
        [SerializeField] private float _stoppingDistanceModifier = 5f;

        [Zenject.Inject] private Zenject.DiContainer container;
        [Zenject.Inject(Id = "Arsenal")] public Dictionary<Side, SoldierItems> arsenal;
        private Soldier[] _soldiers;
        private PrimaryFollowerBehavior primaryFollower = null;
        private SecondaryFollowerBehavior[] secondaryFollowers;
        private Building building;

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
            _soldiers[i] = Instantiate(soldierPrefab, transform.position, transform.rotation).GetComponent<Soldier>();
            _soldiers[i].AddRenderPriority(_soldiers.Length - i);
            _soldiers[i].squad = this;
            _soldiers[i].SetHealth(SquadInfo.UnitInfo.Health);
            _soldiers[i].GetComponent<NetworkObject>().Spawn();
            _soldiers[i].side = _side;
            _soldiers[i].SetTroopType(SquadInfo.TroopType);
            _soldiers[i].ChangeItems();
        }

        public void SpawnEmptySoldiers()
        {
            InitializeSoldier(0);

            var leader = new Leader(_soldiers[0], this);
            leader.ReachedDestination += () =>
            {
                building.SquadEnter(_side, SquadInfo);
                foreach (var soldier in _soldiers)
                {
                    Destroy(soldier.gameObject);
                };

                Destroy(gameObject);
            };
            _soldiers[0].squadRole = leader;

            var flag = container.InstantiatePrefab(flagPrefab);
            flag.transform.parent = _soldiers[0].GetComponentInChildren<Weapon>().transform;
            flag.transform.localPosition = flagPosition;
            flag.transform.localEulerAngles = flagEuler;

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
    }
}
