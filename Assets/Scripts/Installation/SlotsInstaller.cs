using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Raiders.Installation
{
    public class SlotsInstaller : MonoInstaller
    {
        [SerializeField] private SlotsController _slotsControllerRef;

        [SerializeField] private List<SideSlotsRefs> _slotsRef;

        public override void InstallBindings()
        {
            CheckSlotsData();

            Container.Bind<SlotsControllerCreator>().FromInstance(new SlotsControllerCreator(GetSlotControllerFactories())).AsSingle();
        }

        private Dictionary<Side, ISlotsControllerFactory> GetSlotControllerFactories()
        {
            var factories = new Dictionary<Side, ISlotsControllerFactory>();

            foreach (var slots in _slotsRef)
            {
                var defaultSlotFactory = new SircleSlot.Factory(slots.defaultSlotRef);
                var extraSlotFactory = new SircleSlot.Factory(slots.extraSlotRef);

                factories.TryAdd(slots.side, new SlotsController.Factory(defaultSlotFactory, extraSlotFactory, _slotsControllerRef));
            }

            return factories;
        }

        private void CheckSlotsData()
        {
            foreach (var side in GetUnusedSides())
                Debug.LogWarning(string.Format("Slots for {0} side missing", side));

            foreach (var slotRef in _slotsRef)
            {
                if (slotRef.defaultSlotRef == null)
                    Debug.LogWarning(string.Format("Default Slot for {0} side did't assigned", slotRef.side));

                if (slotRef.extraSlotRef == null)
                    Debug.LogWarning(string.Format("Extra Slot for {0} side did't assigned", slotRef.side));

            }
        }

        private List<Side> GetUnusedSides()
        {
            var sides = Enum.GetValues(typeof(Side)).Cast<Side>();

            return sides.Except(_slotsRef.Select(t => t.side)).ToList();
        }

        [Serializable]
        private struct SideSlotsRefs
        {
            public Side side;
            public SircleSlot defaultSlotRef;
            public SircleSlot extraSlotRef;
        }
    }
}