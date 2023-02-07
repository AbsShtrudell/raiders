using UnityEngine;
using System;
using System.Collections.Generic;

namespace Raiders
{
    public partial class Building : MonoBehaviour
    {
        [SerializeField]
        private List<Road> _roads;

        private MeshRenderer _meshRenderer;
        private SlotsController _slotsUI;
        private UpgradeController _upgradeUI;

        private Side _side;
        private IBuildingImp _buildingImp;
        private BuildingImpCreator _buildingImpCreator;
        private SlotsControllerCreator _slotsContrCreator;

        public Side Side { get { return _side; } set { _side = value; } }
        public List<Road> roads => _roads;
        public IBuildingImp BuildingImp => _buildingImp;

        public Action<Building> Selected;
        public Action<Building> Deselected;
        public Action<IBuildingData, bool, Building> UpgradeQueue;
        public Action Disabled;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnDisable()
        {
            Disabled?.Invoke();
        }

        private void Update()
        {
            _buildingImp?.Update();

            _slotsUI.transform.localPosition = Vector3.up * 2;
        }

        public void Init(Side side, BuildingType type, BuildingImpCreator buildingImpCreator, SlotsControllerCreator slotsControllerCreator)
        {
            _side = side;
            _buildingImpCreator = buildingImpCreator;
            _slotsContrCreator = slotsControllerCreator;

            ChangeBuildingImp(type, _side);
            ChangeUI();
        }

        private void ChangeUI()
        {
            Destroy(_slotsUI.gameObject);

            _slotsUI = _slotsContrCreator?.Create(_side, _buildingImp);
            _slotsUI.gameObject.transform.SetParent(gameObject.transform);
            _upgradeUI = _slotsUI.GetComponent<UpgradeController>();
            if (_upgradeUI != null)
                _upgradeUI.InitButtons(BuildingImp.BuildingData, OnUpgradeQueued);

            if (Side == Side.Vikings)
                _upgradeUI.Show();
            else
                _upgradeUI.Hide();
        }

        private void ChangeBuildingImp(BuildingType type, Side side)
        {
            _buildingImp = _buildingImpCreator?.Create(type, side);

            _buildingImp.Captured += ChangeTeam;
        }

        private void ChangeTeam(Side side)
        {
            _side = side;

            if (_buildingImp.BuildingData.PreviousLevel != null)
            {
                ChangeBuildingImp(_buildingImp.BuildingData.PreviousLevel.Type, side);
            }
            else
            {
                ChangeBuildingImp(_buildingImp.BuildingData.Type, side);
            }
            ChangeUI();
        }

        public void SquadEnter(Side side, TroopsType type)
        {
            if (side != _side)
            {
                _buildingImp.GotAttacked(side, null);
            }
            else
            {
                _buildingImp.Reinforcement();
            }
        }

        public void Select()
        {
            Selected?.Invoke(this);
            _meshRenderer.material.color = Color.red;
        }

        public void Deselect()
        {
            Deselected?.Invoke(this);
            _meshRenderer.material.color = Color.white;
        }

        public void OnUpgradeQueued(int variant)
        {
            if (variant < 0)
            {
                if (BuildingImp.BuildingData.PreviousLevel != null)
                    UpgradeQueue.Invoke(BuildingImp.BuildingData.PreviousLevel, true, this);
            }
            else if (BuildingImp.BuildingData.Upgrades.Count > variant)
            {
                UpgradeQueue.Invoke(BuildingImp.BuildingData.Upgrades[variant], false, this);
            }
        }

        public void ChangeBuilding(IBuildingData buildingData)
        {
            BuildingImp.BuildingData = buildingData;

            ChangeUI();
        }

        public void SendTroops(Building target)
        {
            /*
            var source = graph.Find(this);
            var destination = graph.Find(target);

            var path = graph.pathAlgorithm.FindPath(source, destination, graph);

            if (path == null || !_buildingImp.SendTroops())
            {
                return;
            }

            var pathRoads = new Queue<Tuple<SplineComputer, Squad.Direction>>();
            var squad = container.InstantiatePrefab(squadPrefab).GetComponent<Squad>();
            squad.SetSide(_side);
            squad.SetTarget(target);

            for (int i = 0; i < path.nodes.Count - 1; i++)
            {
                foreach (var road in path.nodes[i].Value.roads)
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
            */
        }

        public class Factory : IBuildingFactory
        {
            private Building _visual;
            private BuildingImpCreator _buildingImpCreator;
            private SlotsControllerCreator _slotsContrCreator;

            public Factory(Building visual, BuildingImpCreator buildingImpCreator, SlotsControllerCreator slotsContrCreator)
            {
                _visual = visual;
                _buildingImpCreator = buildingImpCreator;
                _slotsContrCreator = slotsContrCreator;
            }

            public Building Create(BuildingType type, Side side)
            {
                return null;
            }
        }
    }
}