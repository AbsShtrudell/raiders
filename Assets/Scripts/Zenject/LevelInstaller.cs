using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private SideBuildingsData vikingBuildings;
    [SerializeField] private SideBuildingsData englishBuildings;
    [SerializeField] private SideBuildingsData rebelBuildings;
    public override void InstallBindings()
    {
        BindSide(rebelBuildings, Side.Rebels);
        BindSide(englishBuildings, Side.English);
        BindSide(vikingBuildings, Side.Vikings);

        Container.BindInterfacesAndSelfTo<BuildingImpCreator>().AsSingle();
    }

    public void BindSide(SideBuildingsData buildings, Side side)
    {
        Container.Bind<SideFactory>().WithId(side).FromInstance(new SideFactory(buildings));
    }
}

[System.Serializable]
public struct SideBuildingsData
{
    public BuildingData simpleBuilding;
    public BuildingData defensiveBuilding;
    public BuildingData economicsBuilding;
    public BuildingData armyBuilding;
}