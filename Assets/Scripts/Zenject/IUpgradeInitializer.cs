using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeInitializer
{
    public Dictionary<BuildingType, IBuildingData> initUpgrades(Dictionary<BuildingType, BuildingData> buildings);
}
