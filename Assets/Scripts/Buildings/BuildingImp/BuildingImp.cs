using System;

public abstract class BuildingImp
{
    private IBuildingData _buildingData;
    protected SlotList _slotList;

    public IBuildingData BuildingData => _buildingData;
    public SlotList SlotList => _slotList;

    public event Action<Side> Captured;

    public BuildingImp(IBuildingData data)
    {
        _buildingData = data;
        _slotList = new SlotList(_buildingData.SquadSlots, _buildingData.SquadRecoveryTime);
    }

    public abstract void Update();

    public abstract bool SendTroops();

    public abstract void Reinforcement();

    public abstract void GotAttacked(Side side);

    protected void OnCaptured(Side side)
    {
        Captured?.Invoke(side);
    }

    public void ChangeBuildingData(IBuildingData buildingData)
    {
        _buildingData = buildingData;
        _slotList = new SlotList(_buildingData.SquadSlots, _buildingData.SquadRecoveryTime);
    }

    public class Factory : Zenject.PlaceholderFactory<BuildingImp> { }
}