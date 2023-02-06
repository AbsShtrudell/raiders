using System;

namespace Raiders
{
    public abstract class BaseBuildingImp : IBuildingImp
    {
        private IBuildingData _buildingData;
        protected ISlotList _slotList;

        ISlotList IBuildingImp.SlotList => _slotList;
        IBuildingData IBuildingImp.BuildingData 
        { 
            get => _buildingData; 
            set {
                _buildingData = value;
                _slotList = new SlotList(_buildingData.SquadSlots, _buildingData.SquadRecoveryTime);
            }
        }

        public event Action<Side> Captured;

        public BaseBuildingImp(IBuildingData data)
        {
            _buildingData = data;
            _slotList = new SlotList(_buildingData.SquadSlots, _buildingData.SquadRecoveryTime);
        }

        public abstract void Update();

        public abstract bool SendTroops();

        public abstract void Reinforcement();

        public abstract void GotAttacked(Side side, SquadTypeInfo squadInfo);

        protected void OnCaptured(Side side)
        {
            Captured?.Invoke(side);
        }
    }
}