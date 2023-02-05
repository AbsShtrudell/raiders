using Raiders.Util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Raiders.Installation
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private List<SideBuildingsData> buildingsDataSet;
        public override void InstallBindings()
        {
            checkDataSet();

            Container.Bind<BuildingImpCreator>().FromInstance(new BuildingImpCreator(getSideFactories())).AsSingle();
        }

        public Dictionary<Side, SideFactory> getSideFactories()
        {
            var sideFactories = new Dictionary<Side, SideFactory>();

            foreach (var buildingsSet in buildingsDataSet)
            {
                var upgradesInitializer = new TriadUpgradesInitializer();
                
                var buildingsDataDictionary = new Dictionary<BuildingType, BuildingData>();

                var sideFactory = new SideFactory(upgradesInitializer.initUpgrades(buildingsDataDictionary.ConverListPair(buildingsSet.buildingsData)));

                sideFactories.Add(buildingsSet.side, sideFactory);
            }

            return sideFactories;
        }

        private void checkDataSet()
        {
            List<Side> sides = getUnusedSides();

            foreach(var side in sides)
                Debug.LogWarning(string.Format("Buildings Data for {0} side missing", side));

            foreach(var dataSet in buildingsDataSet)
                foreach(var missingType in dataSet.getUnusedTypes())
                    Debug.LogWarning(string.Format("Buildings Data of {0} building type of {1} side missing", missingType, dataSet.side));
        }

        private List<Side> getUnusedSides()
        {
            var sides = Enum.GetValues(typeof(Side)).Cast<Side>();

            return sides.Except(buildingsDataSet.Select(t => t.side)).ToList();
        }

        [Serializable]
        private struct SideBuildingsData
        {
            public Side side;
            public List<Pair<BuildingType, BuildingData>> buildingsData;

            public List<BuildingType> getUnusedTypes()
            {
                var buildingTypes = Enum.GetValues(typeof(BuildingType)).Cast<BuildingType>();

                return buildingTypes.Except(buildingsData.Select(t => t.First)).ToList();
            }
        }
    }
}