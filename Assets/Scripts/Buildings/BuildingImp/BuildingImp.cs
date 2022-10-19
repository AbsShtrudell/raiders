using System;

public abstract class BuildingImp
{
    private BuildingData _buildingData;
    protected SlotList _slotList;

    public BuildingData BuildingData => _buildingData;
    public SlotList SlotList => _slotList;

    public event Action<Side> Captured;

    public BuildingImp(BuildingData data)
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

    public class Factory : Zenject.PlaceholderFactory<BuildingImp> { }
}