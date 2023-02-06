using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

namespace Raiders
{
    public class Squad : MonoBehaviour
    {
        [System.Serializable]
        public enum Direction { Forward = 1, Backward = -1 }

        [SerializeField] private GameObject soldierPrefab;
        [SerializeField] private Side _side;
        [SerializeField, Min(1)] private int soldiersAmount = 3;
        private Queue<Tuple<SplineComputer, Direction>> _roads;
        [SerializeField, Min(0f)] private float _speed = 5f;
        [SerializeField] private float distanceFromPrimary = 1f;
        [SerializeField] private float xDistance = 0.05f;
        [SerializeField] private UnitInfo _unitInfo;
        [Zenject.Inject] private Zenject.DiContainer container;
        private Soldier[] soldiers;
        private PrimaryFollowerBehavior primaryFollower = null;
        private SecondaryFollowerBehavior[] secondaryFollowers;
        private Building building;

        private void Awake()
        {
            soldiers = new Soldier[soldiersAmount];
            secondaryFollowers = new SecondaryFollowerBehavior[soldiersAmount - 1];
        }

        public void MakeSoldiers()
        {
            InitializeSoldier(0);
            var f = soldiers[0].GetComponent<TestPathFollower>();
            primaryFollower = f.MakePrimary(_roads, _speed);
            primaryFollower.ReachedDestination += () => { building.SquadEnter(_side, TroopsType.Default); };
            for (int i = 1; i < soldiers.Length; i++)
            {
                InitializeSoldier(i);
                f = soldiers[i].GetComponent<TestPathFollower>();
                secondaryFollowers[i - 1] =
                    f.MakeSecondary(primaryFollower, distanceFromPrimary * ((i + 1) / 2), xDistance * i * (i % 2 == 0 ? 1 : -1));
            }
        }

        private void InitializeSoldier(int i)
        {
            soldiers[i] = container.InstantiatePrefab(soldierPrefab, transform.position, transform.rotation, transform).GetComponent<Soldier>();
            soldiers[i].side = _side;
            soldiers[i].ChangeItems();
            soldiers[i].AddRenderPriority(soldiers.Length - i);
            soldiers[i].squad = this;
            soldiers[i].SetHealth(_unitInfo.Health);
        }

        public void SpawnEmptySoldiers()
        {
            for (int i = 0; i < soldiers.Length; i++)
            {
                InitializeSoldier(i);
            }
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

        public void SetRoads(Queue<Tuple<SplineComputer, Direction>> roads)
        {
            _roads = roads;
        }

        public void SetSide(Side side)
        {
            _side = side;
        }

        public void SetTarget(Building building)
        {
            this.building = building;
        }
    }
}
