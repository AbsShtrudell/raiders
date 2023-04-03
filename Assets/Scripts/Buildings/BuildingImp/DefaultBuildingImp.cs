
namespace Raiders
{
    public class DefaultBuildingImp : AbstractBuildingImp
    {
        public DefaultBuildingImp(IBuildingData buildingData) : base(buildingData)
        {

        }

        public override bool SendTroops()
        {
            if (_slotList.ExtraSlotsCount > 0)
            {
                _slotList.RemoveExtraSlot();
                return true;
            }
            else
            {
                return _slotList.EmptySlot();
            }
        }

        public override void Reinforcement()
        {
            if (_slotList.IsBlocked)
            {
                _slotList.UnblockSlot();
            }
            else
            {
                if (!_slotList.FillSlot()) _slotList.AddExtraSlot();
            }
        }

        public override void GotAttacked(Side side, SquadTypeInfo squadInfo)
        {
            if (_slotList.ExtraSlotsCount > 0)
            {
                _slotList.RemoveExtraSlot();
            }
            else
            {
                if (_slotList.IsBlocked && _slotList.OccupyingSide != side)
                    _slotList.UnblockSlot();
                else
                    _slotList.BlockSlot(side);
            }
        }

        public override void Update()
        {
            _slotList.Update();

            if (_slotList.IsBlocked && _slotList.GeneralProgress <= 0) OnCaptured(_slotList.OccupyingSide);
        }

        public class Factory : IBuildingImpFactory
        {
            public IBuildingImp Create(BuildingData data) => new DefaultBuildingImp(data);
        }
    }
}