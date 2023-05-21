using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class ClientBuildingImp : IBuildingImp
    {
        private ClientSlotList _slotList;

        public IBuildingData BuildingData { get; set; }

        public IReadOnlySlotList SlotList => _slotList;

        public event Action<Side> Captured;

        public ClientBuildingImp(IBuildingData buildingData)
        {

            BuildingData = buildingData;
            _slotList = new ClientSlotList();
        }

        public void GotAttacked(Side side, SquadTypeInfo squadInfo)
        {
            
        }

        public void Reinforcement()
        {
            
        }

        public bool SendTroops()
        {
            return false;
        }

        public void Update()
        {
            
        }

        public class Factory : IBuildingImpFactory
        {
            public IBuildingImp Create(BuildingData data) => new ClientBuildingImp(data);
        }
    }
}
