using System.Collections.Generic;

public class BuildingImpCreator
{
    private Dictionary<Side, SideFactory> _sideFactories;

    public BuildingImpCreator(Dictionary<Side, SideFactory> sideFactories)
    {
        _sideFactories = sideFactories;
    }

    public BuildingImp Create(BuildingType type, Side side)
    {
        SideFactory sideFactory;

        _sideFactories.TryGetValue(side, out sideFactory);

        if (sideFactory == null) return null;

        return sideFactory.Create(type);
    }
}
