using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private List<Pair<Side, List<Pair<BuildingType, BuildingData>>>> buildings;

    public override void InstallBindings()
    {
        Container.Bind<BuildingImpCreator>().FromInstance(new BuildingImpCreator(getSideFactories())).AsSingle();
    }

    public Dictionary<Side, SideFactory> getSideFactories()
    {
        Dictionary<Side, SideFactory> sideFactories = new Dictionary<Side, SideFactory>();

        foreach (var building in buildings) {
            //sideFactories.Add(building.First, new SideFactory(building.Second));
        }

        return sideFactories;
    }
}