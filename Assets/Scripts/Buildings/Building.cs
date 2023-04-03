using UnityEngine;
using System;
using Zenject;
using System.Collections.Generic;
using Dreamteck.Splines;
using Raiders.Graphs;

namespace Raiders
{
    public class Building : MonoBehaviour
    {
        [Inject]
        private BuildingImpCreator _buildingImpCreator;
        [Inject]
        private SlotsControllerCreator _slotsContrCreator;

        [SerializeField]
        private Side _side = Side.Rebels;
        [SerializeField]
        private BuildingType _type;
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private List<Road> _roads;

        private SlotsController _slotsUI;
        private UpgradeController _upgradeUI;

        public Side Side { get { return _side; } set { _side = value; } }
        public BuildingType Type { get { return _type; } set { _type = value; } } 
        public GameObject SquadPrefab { get; set; }
        public List<Road> Roads => _roads;
        public IBuildingImp BuildingImp { get; private set; }
        public IBuildingQueueHandler BuildingQueueHandler { private get; set; }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            InitBuildingImp(_type, _side);
            InitUI();
        }

        private void Start()
        {
        }

        private void Update()
        {
            BuildingImp?.Update();

            _slotsUI.transform.localPosition = Vector3.up * 2;
        }

        private void InitUI()
        {
            _slotsUI = _slotsContrCreator?.Create(_side, BuildingImp);
            _slotsUI.gameObject.transform.SetParent(transform);
            _upgradeUI = _slotsUI.GetComponent<UpgradeController>();
            if (_upgradeUI != null)
                _upgradeUI.InitButtons(BuildingImp.BuildingData, OnUpgradeQueued);

            if (Side == Side.Vikings)
                _upgradeUI.Show();
            else
                _upgradeUI.Hide();
        }

        private void InitBuildingImp(BuildingType type, Side side)
        {
            BuildingImp = _buildingImpCreator?.Create(type, side);

            BuildingImp.Captured += ChangeTeam;
        }

        private void ChangeTeam(Side side)
        {
            _side = side;

            if (BuildingImp.BuildingData.PreviousLevel != null)
            {
                InitBuildingImp(BuildingImp.BuildingData.PreviousLevel.Type, side);
                _type = BuildingImp.BuildingData.Type;
            }
            else
            {
                InitBuildingImp(BuildingImp.BuildingData.Type, side);
            }
            Destroy(_slotsUI.gameObject);
            InitUI();
        }

        public void SquadEnter(Side side, TroopsType type)
        {
            if (side != _side)
            {
                BuildingImp.GotAttacked(side, null);
            }
            else
            {
                BuildingImp.Reinforcement();
            }
        }

        public void Select()
        {
            _meshRenderer.material.color = Color.red;
        }

        public void Deselect()
        {
            _meshRenderer.material.color = Color.white;
        }

        public void OnUpgradeQueued(int variant)
        {
            if (variant < 0)
            {
                if (BuildingImp.BuildingData.PreviousLevel != null)
                    BuildingQueueHandler.Upgrade(BuildingImp.BuildingData.PreviousLevel, true, this);
            }
            else if (BuildingImp.BuildingData.Upgrades.Count > variant)
            {
                BuildingQueueHandler.Upgrade(BuildingImp.BuildingData.Upgrades[variant], false, this);
            }
        }

        public void ChangeBuilding(IBuildingData buildingData)
        {
            BuildingImp.BuildingData = buildingData;

            Destroy(_slotsUI.gameObject);
            InitUI();
        }

        public void SendTroops(Building target)
        {
            var path = BuildingQueueHandler.GetPath(target, this);

            if (path == null || !BuildingImp.SendTroops())
                return;

            var pathRoads = new Queue<Tuple<SplineComputer, Squad.Direction>>();
            var squad = container.InstantiatePrefab(SquadPrefab).GetComponent<Squad>();
            squad.SetSide(_side);
            squad.SetTarget(target);

            for (int i = 0; i < path.nodes.Count - 1; i++)
            {
                foreach (var road in path.nodes[i].Value.Roads)
                {
                    if (road.HasConnectionWith(path.nodes[i + 1].Index))
                    {
                        var direction = road.Ends[1].Index == path.nodes[i].Index ? Squad.Direction.Backward : Squad.Direction.Forward;

                        pathRoads.Enqueue(new Tuple<SplineComputer, Squad.Direction>(road.PathCreator, direction));

                        break;
                    }
                }
            }

            squad.SetRoads(pathRoads);
            squad.MakeSoldiers();
        }

        public class Factory : IFactory<Building>
        {
            private DiContainer _container;
            private Building _visual;

            public Factory(DiContainer container, Building visual)
            {
                _container = container;
                _visual = visual;
            }

            public Building Create()
            {
                Building building;

                if (_container != null)
                {
                    building = _container.InstantiatePrefabForComponent<Building>(_visual);
                }
                else
                {
                    building = Instantiate(_visual.gameObject).GetComponent<Building>();
                }

                return building;
            }
        }
    }
}