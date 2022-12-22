using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SlotsControllerCreator : IInitializable
{
    [Inject(Id = Side.Vikings)]
    private SlotsController.Factory _vikFactory;
    [Inject(Id = Side.English)]
    private SlotsController.Factory _engFactory;
    [Inject(Id = Side.Rebels)]
    private SlotsController.Factory _rebFactory;

    public SlotsController Create(Side side, BuildingImp imp)
    {
        switch(side)
        {
            case Side.Vikings:
                return _vikFactory.Construct(imp);
            case Side.English:
                return _engFactory.Construct(imp);
            case Side.Rebels:
                return _rebFactory.Construct(imp);
        }
        return null;
    }

    public void Initialize()
    {
    }
}
