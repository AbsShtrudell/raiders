using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Raiders
{
    public class SoilderItemsInstaller : MonoInstaller
    {
        [SerializeField] private SoldierItems vikingItems;
        [SerializeField] private SoldierItems englishItems;

        public static Dictionary<Side, SoldierItems> arsenal;

        public override void InstallBindings()
        {
            arsenal = null;

            if (arsenal == null)
            {
                vikingItems.Awake();
                englishItems.Awake();

                arsenal = new Dictionary<Side, SoldierItems>
                {
                    { Side.Vikings, vikingItems },
                    { Side.English, englishItems }
                };
            }

            Container.BindInstance(arsenal).WithId("Arsenal").AsSingle();
        }
    }
}