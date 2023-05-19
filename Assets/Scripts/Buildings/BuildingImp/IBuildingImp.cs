using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public interface IBuildingImp
    {
        public IBuildingData BuildingData { get; set; }

        public IReadOnlySlotList SlotList { get; }

        public event Action<Side> Captured;

        public void Update();

        public bool SendTroops();

        public void Reinforcement();

        public void GotAttacked(Side side, SquadTypeInfo squadInfo);
    }
}
