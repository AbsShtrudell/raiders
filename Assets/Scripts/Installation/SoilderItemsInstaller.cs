using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Raiders
{
    public class SoilderItemsInstaller : MonoInstaller
    {
        [SerializeField] private SoldierItems vikingItems;
        [SerializeField] private SoldierItems englishItems;
        [SerializeField] private Transform _squadPrefab;
        [SerializeField] private Transform _soldierPrefab;

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

            Container.Bind<IFactory<Squad>>().FromInstance(new Squad.Factory(new Soldier.Factory(_soldierPrefab, arsenal), _squadPrefab));
        }
    }
}