public class SideFactory : Zenject.IFactory<BuildingType ,Building>
{
    private Building.Factory _simpleFactory;
    private Building.Factory _defensiveFactory;
    private Building.Factory _economicFactory;
    private Building.Factory _armyFactory;

    public SideFactory(Building.Factory simple, Building.Factory defense, Building.Factory economic, Building.Factory army)
    {
        _simpleFactory = simple;
        _defensiveFactory = defense;
        _economicFactory = economic;
        _armyFactory = army;
    }

    public Building Create(BuildingType type)
    {
        Building building = null;

        switch (type)
        {
            case BuildingType.Simple:
                building = _simpleFactory.Create();
                break;
            case BuildingType.Defensive:
                building = _defensiveFactory.Create();
                break;
            case BuildingType.Economic:
                building = _economicFactory.Create();
                break;
            case BuildingType.Army:
                building = _armyFactory.Create();
                break;
        }

        return building;
    }
}
