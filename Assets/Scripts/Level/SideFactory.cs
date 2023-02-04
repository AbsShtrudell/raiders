using System.Collections.Generic;

public class SideFactory : Zenject.IFactory<BuildingType ,BuildingImp>
{
    private Dictionary<BuildingType, BuildingData> _buildingsData;

    public SideFactory(Dictionary<BuildingType, BuildingData> buildings)
    {
        _buildingsData = buildings;
    }

    public BuildingImp Create(BuildingType type)
    {
        /*
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
        */
        return null;
    }
}
