using UnityEngine;
using System;
using Zenject;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Netcode;

namespace Raiders
{
    public class Building : NetworkBehaviour
    {
        [Inject]
        private DiContainer container;
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

        public event Action OnSideChanged;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            Init();
        }

        private void Init()
        {
            InitBuildingImp(_type, _side);
            InitUI();
        }

        private void Update()
        {
            _slotsUI.transform.localPosition = Vector3.up * 5;
            BuildingImp?.Update();

            if (!IsHost) return;

            ClientSlotList.NetworkData data = new ClientSlotList.NetworkData(
                BuildingImp.SlotList.OccupyingSide,
                BuildingImp.SlotList.IsBlocked,
                BuildingImp.SlotList.GeneralProgress,
                BuildingImp.SlotList.Slots.ToArray(),
                BuildingImp.SlotList.ExtraSlots.ToArray()
            );

            UpdateSlotsClientRpc(data);
        }

        private void InitUI()
        {
            _slotsUI = _slotsContrCreator?.Create(_side, BuildingImp);
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

            BuildingImp.Captured += (side) =>
            {
                ChangeTeamClientRpc(side);
                ChangeTeam(side);
            };
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
            ChangeVisual(BuildingImp.BuildingData);
            Destroy(_slotsUI.gameObject);
            InitUI();

            OnSideChanged?.Invoke();
        }

        public void SquadEnter(Side side, SquadTypeInfo type)
        {
            if (!IsHost) return;

            if (side != _side)
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
            if (!IsHost)
            {
                UpgradeQueueServerRpc(variant);
            }
            else
            {
                if (variant < 0)
                {
                    if (BuildingImp.BuildingData.PreviousLevel != null)
                        if (IsHost)
                            if (BuildingQueueHandler.Upgrade(BuildingImp.BuildingData.PreviousLevel, true, this))
                                UpgradeClientRpc(variant);
                }
                else if (BuildingImp.BuildingData.Upgrades.Count > variant)
                {
                    if (BuildingQueueHandler.Upgrade(BuildingImp.BuildingData.Upgrades[variant], false, this))
                        UpgradeClientRpc(variant);
                }
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

        public bool SendTroops(Building target) //sync with client
        {
            if(!IsHost)
            {
                SendTroopsServerRpc(target); 
                return true;
            }

            var path = BuildingQueueHandler.GetPath(target, this);

            if (path == null || !BuildingImp.SendTroops())
                return false;

            var pathRoads = new Queue<ValueTuple<SplineComputer, Squad.Direction>>();
            var squad = Instantiate(SquadPrefab).GetComponent<Squad>();
            container.Inject(squad);
            squad.transform.position = transform.position;
            squad.SetSide(_side);
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

        //--------------------Client----------------------//

        [ClientRpc]
        public void ChangeTeamClientRpc(Side side)
        {
            if (IsOwner) return;

            ChangeTeam(side);
        }

        [ClientRpc]
        public void UpdateSlotsClientRpc(ClientSlotList.NetworkData data)
        {
            if (IsOwner) return;

            if(((ClientSlotList)BuildingImp.SlotList) != null) 
                ((ClientSlotList)BuildingImp.SlotList).SetData(data);
        }

        [ClientRpc]
        public void UpgradeClientRpc(int variant)
        {
            if (IsOwner) return;

            if (variant < 0)
            {
                if (BuildingImp.BuildingData.PreviousLevel != null)
                    ChangeBuilding(BuildingImp.BuildingData.PreviousLevel);
            }
            else if (BuildingImp.BuildingData.Upgrades.Count > variant)
            {
                ChangeBuilding(BuildingImp.BuildingData.Upgrades[variant]);
            }
        }

        //--------------------Server----------------------//

        [ServerRpc(RequireOwnership = false)]
        public void UpgradeQueueServerRpc(int variant)
        {
            OnUpgradeQueued(variant);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendTroopsServerRpc(NetworkBehaviourReference targetBuilding)
        {
            if (targetBuilding.TryGet(out Building building))
            {
                SendTroops(building);
            }
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