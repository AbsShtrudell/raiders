using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SlotsInstaller : MonoInstaller
{
    [SerializeField] private SlotsController _slotsControllerRef;

    [SerializeField] private List<SideSlotsRefs> _slotsRef;

    public override void InstallBindings()
    {
        checkSlotsIfSomethingMissing();

        foreach (var slots in _slotsRef)
        {
            BindSlotControllerFactory(slots.side, slots.defaultSlotRef, slots.extraSlotRef);
        }

        Container.BindInterfacesAndSelfTo<SlotsControllerCreator>().AsSingle();
    }

    private void BindSlotControllerFactory(Side side, SircleSlot sircleSlotRef, SircleSlot extraSlotRef)
    {
        Container.Bind<SlotsController.Factory>().
            WithId(side).
            FromInstance(new SlotsController.Factory(
                            new SircleSlot.Factory(sircleSlotRef), 
                            new SircleSlot.Factory(extraSlotRef), 
                            _slotsControllerRef));
    }

    private void checkSlotsIfSomethingMissing()
    {
        foreach(var side in Enum.GetValues(typeof(Side)).Cast<Side>())
        {
            bool found = false;

            foreach(var slotRef in _slotsRef)
                if (side == slotRef.side)
                {
                    found = true;

                    if (slotRef.defaultSlotRef == null)
                        Debug.LogWarning(String.Format("Default Slot for {0} side did't assigned", slotRef.side));

                    if (slotRef.extraSlotRef == null)
                        Debug.LogWarning(String.Format("Extra Slot for {0} side did't assigned", slotRef.side));

                    break;
                }

            if(!found)
                Debug.LogWarning(String.Format("Slots for {0} side did't assigned", side));
        }
    }
}

[Serializable]
internal struct SideSlotsRefs
{
    public Side side;
    public SircleSlot defaultSlotRef;
    public SircleSlot extraSlotRef;
}