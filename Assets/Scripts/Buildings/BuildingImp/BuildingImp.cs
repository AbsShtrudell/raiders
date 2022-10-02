using System;

public abstract class BuildingImp
{
    private BuildingData _buildingData;
    protected SlotList _slotList;

    public BuildingData BuildingData => _buildingData;
    public SlotList SlotList => _slotList;
    
    public BuildingImp(BuildingData data)
    {
        _buildingData = data;
        _slotList = new SlotList(_buildingData.SquadSlots, _buildingData.SquadRecoveryTime);
    }

    public void Update()
    {
        _slotList.Update();
    }

    public abstract bool SendTroops();
    public abstract void Reinforcement();
    public abstract void GotAttacked();

    public class Factory : Zenject.PlaceholderFactory<BuildingImp> { }
}