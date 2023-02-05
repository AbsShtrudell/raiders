using System;
using System.Collections.Generic;

namespace Raiders.Installation
{
    public class TriadUpgradesInitializer : IUpgradeInitializer
    {
        public Dictionary<BuildingType, IBuildingData> initUpgrades(Dictionary<BuildingType, BuildingData> buildings)
        {
            buildings.TryGetValue(BuildingType.Simple, out var simpleBuildingData);
            buildings.TryGetValue(BuildingType.Defensive, out var defenseBuildingData);
            buildings.TryGetValue(BuildingType.Army, out var armyBuildingData);
            buildings.TryGetValue(BuildingType.Economic, out var economyBuildingData);

            if (simpleBuildingData == null ||
                defenseBuildingData == null ||
                armyBuildingData == null ||
                economyBuildingData == null) throw new Exception();

            var simpleBuildingDataProxy = new UpgradesBuildingDataProxy(simpleBuildingData);
            var defenseBuildingDataProxy = new UpgradesBuildingDataProxy(defenseBuildingData, simpleBuildingDataProxy);
            var armyBuildingDataProxy = new UpgradesBuildingDataProxy(armyBuildingData, simpleBuildingDataProxy);
            var economyBuildingDataProxy = new UpgradesBuildingDataProxy(economyBuildingData, simpleBuildingDataProxy);

            simpleBuildingDataProxy.SetUpgrades(new List<IBuildingData> { defenseBuildingDataProxy, armyBuildingDataProxy, economyBuildingDataProxy });

            var initializedBuildings = new Dictionary<BuildingType, IBuildingData>
        {
            { BuildingType.Simple, simpleBuildingDataProxy },
            { BuildingType.Defensive, defenseBuildingDataProxy },
            { BuildingType.Army, armyBuildingDataProxy },
            { BuildingType.Economic, economyBuildingDataProxy }
        };

            foreach (var building in buildings)
                initializedBuildings.TryAdd(building.Key, building.Value);

            return initializedBuildings;
        }
    }
}
