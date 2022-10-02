using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SimpleBuildingImp : BuildingImp
{
    public SimpleBuildingImp(BuildingData buildingData) : base(buildingData)
    {  
        
    }

    public override bool SendTroops()
    {
        if(_slotList.ExtraSlotsCount > 0)
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
            if(!_slotList.FillSlot()) _slotList.AddExtraSlot();
        }
    }

    public override void GotAttacked()
    {
        if (_slotList.ExtraSlotsCount > 0)
        {
            _slotList.RemoveExtraSlot();
        }
        else
        {
            _slotList.BlockSlot();
        }
    }

    public new class Factory : IFactory<BuildingData, BuildingImp>
    {
        public BuildingImp Create(BuildingData data)
        {
            BuildingImp imp = new SimpleBuildingImp(data);
            return imp;
        }
    }
}