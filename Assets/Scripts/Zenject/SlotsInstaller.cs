using UnityEngine;
using Zenject;

public class SlotsInstaller : MonoInstaller
{
    [SerializeField] private SlotsController _slotsControllerRef;
    [SerializeField] private SircleSlot _vikingSlotRef;
    [SerializeField] private SircleSlot _englishSlotRef;
    [SerializeField] private SircleSlot _rebelsSlotRef;
    [SerializeField] private SircleSlot _extraSlotRef;

    public override void InstallBindings()
    {
        BindSlotControllerFactory(Side.Vikings, _vikingSlotRef);
        BindSlotControllerFactory(Side.English, _englishSlotRef);
        BindSlotControllerFactory(Side.Rebels, _rebelsSlotRef);

        Container.BindInterfacesAndSelfTo<SlotsControllerCreator>().AsSingle();
    }

    private void BindSlotControllerFactory(Side side, SircleSlot sircleSlotRef)
    {
        Container.Bind<SlotsController.Factory>().WithId(side).FromInstance(new SlotsController.Factory(new SircleSlot.Factory(sircleSlotRef), new SircleSlot.Factory(_extraSlotRef), _slotsControllerRef));
    }
}