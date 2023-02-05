using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class UpgradesBuildingDataProxy : BuildingDataProxy
    {
        private IBuildingData _previousUpgrade;
        private List<IBuildingData> _nextUpgrades;

        public UpgradesBuildingDataProxy(BuildingData buildingData, IBuildingData previousUpgrade, List<IBuildingData> nextUpgrades) : base(buildingData)
        {
            _previousUpgrade = previousUpgrade;
            _nextUpgrades = nextUpgrades;
        }

        public UpgradesBuildingDataProxy(BuildingData buildingData, IBuildingData previousUpgrade) : base(buildingData)
        {
            _previousUpgrade = previousUpgrade;
            _nextUpgrades = null;
        }

        public UpgradesBuildingDataProxy(BuildingData buildingData, List<IBuildingData> nextUpgrades) : base(buildingData)
        {
            _previousUpgrade = null;
            _nextUpgrades = nextUpgrades;
        }

        public UpgradesBuildingDataProxy(BuildingData buildingData) : base(buildingData)
        {
            _previousUpgrade = null;
            _nextUpgrades = null;
        }

        public override List<IBuildingData> Upgrades => _nextUpgrades != null ? _nextUpgrades : base.Upgrades;

        public override IBuildingData PreviousLevel => _previousUpgrade != null ? _previousUpgrade : base.PreviousLevel;

        public void SetUpgrades(List<IBuildingData> nextUpgrades)
        {
            _nextUpgrades = nextUpgrades;
        }

        public void SetPreviousLevel(IBuildingData previousUpgrade)
        {
            _previousUpgrade = previousUpgrade;
        }
    }
}