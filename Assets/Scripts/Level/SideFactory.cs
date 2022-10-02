public class SideFactory : Zenject.IFactory<BuildingType ,BuildingImp>
{
    private SideBuildingsData _buildingsData;

    public SideFactory(SideBuildingsData buildings)
    {
        _buildingsData = buildings;
    }

    public BuildingImp Create(BuildingType type)
    {
        BuildingImp buildingImp = null;

        switch (type)
        {
            case BuildingType.Simple:
                buildingImp = new SimpleBuildingImp(_buildingsData.simpleBuilding);
                break;
            case BuildingType.Defensive:
                buildingImp = new SimpleBuildingImp(_buildingsData.defensiveBuilding);
                break;
            case BuildingType.Economic:
                buildingImp = new SimpleBuildingImp(_buildingsData.economicsBuilding);
                break;
            case BuildingType.Army:
                buildingImp = new SimpleBuildingImp(_buildingsData.armyBuilding);
                break;
        }

        return buildingImp;
    }
}
