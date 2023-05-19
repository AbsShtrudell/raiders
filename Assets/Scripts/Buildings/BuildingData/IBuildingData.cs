using Dreamteck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public interface IBuildingData
    {
        public int SquadSlots { get; }
        public float SquadRecoveryTime { get; }
        public uint Income { get; }
        public uint Upkeep { get; }
        public int DefenseMultyplier { get; }
        public SquadTypeInfo SquadTypeInfo { get; }
        public Mesh Mesh { get; }
        public List<IBuildingData> Upgrades { get; }
        public IBuildingData PreviousLevel { get; }
        public BuildingType Type { get; }
        public uint Cost { get; }
    }
}