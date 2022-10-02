using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildingImpCreator : IInitializable
{
    [Inject(Id = Side.Vikings)]
    private SideFactory _vikingFactory;
    [Inject(Id = Side.English)]
    private SideFactory _englishFactory;
    [Inject(Id = Side.Rebels)]
    private SideFactory _rebelFactory;

    public void Initialize()
    { }

    public BuildingImp Create(BuildingType type, Side side)
    {
        BuildingImp building = null;

        switch (side)
        {
            case Side.Vikings:
                building = _vikingFactory.Create(type);
                break;
            case Side.English:
                building = _englishFactory.Create(type);
                break;
            case Side.Rebels:
                building = _rebelFactory.Create(type);
                break;
        }

        return building;
    }
}
