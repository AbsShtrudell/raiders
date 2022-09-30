using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBuildingImp : IBuildingImp
{
    private BuildingData _buildingData;
    private SlotList _slotList;

    public SlotList SlotList
    { get { return _slotList; } }

    public SimpleBuildingImp(BuildingData buildingData)
    {
        _buildingData = buildingData;

        _slotList = new SlotList(_buildingData.SquadSlots, _buildingData.SquadRecoveryTime);
    }

    public SlotList GetSlotList()
    {
        return _slotList;
    }

    public BuildingData GetData()
    {
        return _buildingData;
    }

    public void Update()
    {
        _slotList.Update();
    }

    public bool SendTroops()
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

    public void Reinforcement()
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

    public void GotAttacked()
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

    public class Factory : Zenject.PlaceholderFactory<BuildingData, IBuildingImp> { }
}