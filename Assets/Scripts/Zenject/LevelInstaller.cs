using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private SideBuildings vikingBuildings;
    [SerializeField] private SideBuildings englishBuildings;
    [SerializeField] private SideBuildings rebelBuildings;

    public override void InstallBindings()
    {
        BindSide(rebelBuildings, Side.Rebels);
        BindSide(englishBuildings, Side.English);
        BindSide(vikingBuildings, Side.Vikings);
    }

    public void BindSide(SideBuildings buildings, Side side)
    {
        Building.Factory simple =    new Building.Factory(Container, new SimpleBuildingImp(buildings.simpleBuilding));
        Building.Factory defensive = new Building.Factory(Container, new SimpleBuildingImp(buildings.defensiveBuilding));
        Building.Factory economics = new Building.Factory(Container, new SimpleBuildingImp(buildings.economicsBuilding));
        Building.Factory army =      new Building.Factory(Container, new SimpleBuildingImp(buildings.economicsBuilding));
        Container.Bind<SideFactory>().WithId(side).FromInstance(new SideFactory(simple, defensive, economics, army)).AsSingle();
    }

    public struct SideBuildings
    {
        //to-do add impl type
        public BuildingData simpleBuilding;
        public BuildingData defensiveBuilding;
        public BuildingData economicsBuilding;
        public BuildingData armyBuilding;
    }
}