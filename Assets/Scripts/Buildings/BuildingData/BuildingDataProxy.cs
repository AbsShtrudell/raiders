using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public abstract class BuildingDataProxy : IBuildingData
    {
        private BuildingData _buildingData;

        public BuildingDataProxy(BuildingData buildingData)
        {
            _buildingData = buildingData;
        }

        public virtual int SquadSlots => _buildingData.SquadSlots;

        public virtual float SquadRecoveryTime => _buildingData.SquadRecoveryTime;

        public virtual uint Income => _buildingData.Income;

        public virtual uint Upkeep => _buildingData.Upkeep;

        public virtual int DefenseMultyplier => _buildingData.DefenseMultyplier;

        public virtual SquadTypeInfo SquadTypeInfo => _buildingData.SquadTypeInfo;

        public virtual Mesh Mesh => _buildingData.Mesh;

        public virtual List<IBuildingData> Upgrades => _buildingData.Upgrades;

        public virtual IBuildingData PreviousLevel => _buildingData.PreviousLevel;

        public virtual BuildingType Type => _buildingData.Type;

        public virtual uint Cost => _buildingData.Cost;
    }
}