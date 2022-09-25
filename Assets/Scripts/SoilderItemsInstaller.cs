using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SoilderItemsInstaller : MonoInstaller
{
    [SerializeField] private SoldierItems vikingItems;
    [SerializeField] private SoldierItems englishItems;
    private Dictionary<Side, SoldierItems> arsenal = null;

    public override void InstallBindings()
    {
        if (arsenal == null)
        {
            arsenal = new Dictionary<Side, SoldierItems>();
            arsenal.Add(Side.Viking, vikingItems);
            arsenal.Add(Side.English, englishItems);
        }

        Container.BindInstance(arsenal).WithId("Arsenal").AsSingle();
    }
}