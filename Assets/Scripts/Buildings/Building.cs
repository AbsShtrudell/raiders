using UnityEngine;
using System;
using Zenject;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Netcode;
using Raiders.Infrastructure;

namespace Raiders
{
    public class Building : MonoBehaviour
    {
        [Inject]
        private IFactory<Squad> _squadFactory;
        [Inject]
        private BuildingImpCreator _buildingImpCreator;
        [Inject]
        private SlotsControllerCreator _slotsContrCreator;

        [field: SerializeField]
        public ObservableVariable<Side> SideVariable { get; private set; }
        [field: SerializeField]
        public BuildingType Type { get; private set; }
        [SerializeField]
        public MeshRenderer _meshRenderer { get; private set; }
        [field: SerializeField]
        public List<Road> Roads { get; private set; }

        public IBuildingImp BuildingImp { get; private set; }
        public IBuildingQueueHandler BuildingQueueHandler { private get; set; }

        private SlotsController _slotsUI;
        private UpgradeController _upgradeUI;

        public Side Side
        {
            get { return SideVariable.Value; } private set { SideVariable.Value = value; } 
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            Init();
        }

        private void Init()
        {
            InitBuildingImp(Type, Side);
            InitUI();
        }

        private void Update()
        {
            _slotsUI.transform.localPosition = Vector3.up * 5;
            BuildingImp?.Update();
        }

        private void InitUI()
        {
            _slotsUI = _slotsContrCreator?.Create(Side, BuildingImp);
            _slotsUI.gameObject.transform.SetParent(transform);
            _upgradeUI = _slotsUI.GetComponent<UpgradeController>();
            if (_upgradeUI != null)
                _upgradeUI.InitButtons(BuildingImp.BuildingData, OnUpgradeQueued);

            if (NetworkManager.Singleton.IsHost? Side == Side.Vikings : Side == Side.English)
                _upgradeUI.Show();
            else
                _upgradeUI.Hide();
        }

        private void InitBuildingImp(BuildingType type, Side side)
        {
            BuildingImp = _buildingImpCreator?.Create(type, side);

            BuildingImp.Captured += (side) => ChangeTeam(side);
        }

        public void ChangeTeam(Side side)
        {
            Side = side;

            if (BuildingImp.BuildingData.PreviousLevel != null)
            {
                InitBuildingImp(BuildingImp.BuildingData.PreviousLevel.Type, side);
                Type = BuildingImp.BuildingData.Type;
            }
            else
            {
                InitBuildingImp(BuildingImp.BuildingData.Type, side);
            }
            ChangeVisual(BuildingImp.BuildingData);
            Destroy(_slotsUI.gameObject);
            InitUI();
        }

        public void SquadEnter(Side side, SquadTypeInfo type)
        {
            if (side != Side)
            {
                BuildingImp.GotAttacked(side, type);
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
            //if (!IsHost)
            //{
            //UpgradeQueueServerRpc(variant);
            //}
            //else
            if (variant < 0)
            {
                if (BuildingImp.BuildingData.PreviousLevel != null)
                    //if (IsHost)
                    BuildingQueueHandler.Upgrade(BuildingImp.BuildingData.PreviousLevel, true, this);
                //UpgradeClientRpc(variant);
            }
            else if (BuildingImp.BuildingData.Upgrades.Count > variant)
            {
                BuildingQueueHandler.Upgrade(BuildingImp.BuildingData.Upgrades[variant], false, this);
                //UpgradeClientRpc(variant);
            }
        }

        public void ChangeBuilding(IBuildingData buildingData)
        {
            BuildingImp.BuildingData = buildingData;
            ChangeVisual(buildingData);
            Destroy(_slotsUI.gameObject);
            InitUI();
        }

        public void ChangeVisual(IBuildingData data)
        {
            if(data.Mesh != null)
                GetComponent<MeshFilter>().mesh = data.Mesh;
        }

        public bool SendTroops(Building target)
        {
            //if(!IsHost)
            //{
                //SendTroopsServerRpc(target); 
                //return true;
            //}

            var path = BuildingQueueHandler.GetPath(target, this);

            if (path == null || !BuildingImp.SendTroops())
                return false;

            var pathRoads = new Queue<ValueTuple<SplineComputer, Squad.Direction>>();
            var squad = _squadFactory.Create();
            squad.transform.position = transform.position;
            squad.SetSide(Side);
            squad.SetTarget(target);

            for (int i = 0; i < path.nodes.Count - 1; i++)
            {
                foreach (var road in path.nodes[i].Value.Roads)
                {
                    if (road.HasConnectionWith(path.nodes[i + 1].Index))
                    {
                        var direction = road.Ends[1].Index == path.nodes[i].Index ? Squad.Direction.Backward : Squad.Direction.Forward;

                        pathRoads.Enqueue(new ValueTuple<SplineComputer, Squad.Direction>(road.PathCreator, direction));

                        break;
                    }
                }
            }

            BuildingQueueHandler.SquadSent(target, BuildingImp.BuildingData.SquadTypeInfo, this);

            squad.SquadInfo = BuildingImp.BuildingData.SquadTypeInfo;
            squad.Roads = pathRoads;
            squad.SpawnEmptySoldiers();

            return true;
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