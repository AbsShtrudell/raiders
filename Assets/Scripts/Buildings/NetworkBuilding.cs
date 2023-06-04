using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    [RequireComponent(typeof (Building))]
    public class NetworkBuilding : NetworkBehaviour
    {
        private Building _building;

        public Building Building { get => _building; }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _building = GetComponent<Building>();

            if(IsHost)
            {
                _building.SideVariable.OnValueChanged += (Side o, Side n) =>
                {
                    ChangeTeamClientRpc(n);
                };
            }
        }

        private void Update()
        {
            if(IsHost && _building != null && _building.BuildingImp != null)
            {
                UpdateSlotList();
            }
        }

        //--------------------Client----------------------//

        private void UpdateSlotList()
        {
            IBuildingImp buildingImp = _building.BuildingImp;

            ClientSlotList.NetworkData data = new ClientSlotList.NetworkData(
                buildingImp.SlotList.OccupyingSide,
                buildingImp.SlotList.IsBlocked,
                buildingImp.SlotList.GeneralProgress,
                buildingImp.SlotList.Slots.ToArray(),
                buildingImp.SlotList.ExtraSlots.ToArray()
            ); ;

            UpdateSlotsClientRpc(data);
        }

        [ClientRpc]
        private void ChangeTeamClientRpc(Side side)
        {
            if (IsOwner) return;

            _building.ChangeTeam(side);
        }

        [ClientRpc]
        private void UpdateSlotsClientRpc(ClientSlotList.NetworkData data)
        {
            if (IsOwner) return;

            if (((ClientSlotList)_building.BuildingImp.SlotList) != null)
                ((ClientSlotList)_building.BuildingImp.SlotList).SetData(data);
        }

        [ClientRpc]
        private void UpgradeClientRpc(int variant)
        {
            if (IsOwner) return;

            if (variant < 0)
            {
                if (_building.BuildingImp.BuildingData.PreviousLevel != null)
                    _building.ChangeBuilding(_building.BuildingImp.BuildingData.PreviousLevel);
            }
            else if (_building.BuildingImp.BuildingData.Upgrades.Count > variant)
            {
                _building.ChangeBuilding(_building.BuildingImp.BuildingData.Upgrades[variant]);
            }
        }

        //--------------------Server----------------------//

        [ServerRpc(RequireOwnership = false)]
        private void UpgradeQueueServerRpc(int variant)
        {
            _building.OnUpgradeQueued(variant);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendTroopsServerRpc(NetworkBehaviourReference targetBuilding)
        {
            if (targetBuilding.TryGet(out NetworkBuilding building))
            {
                _building.SendTroops(building.Building);
            }
        }
    }
}
