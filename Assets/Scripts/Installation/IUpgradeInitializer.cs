using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.Installation
{
    public interface IUpgradeInitializer
    {
        public Dictionary<BuildingType, IBuildingData> initUpgrades(Dictionary<BuildingType, BuildingData> buildings);
    }
}