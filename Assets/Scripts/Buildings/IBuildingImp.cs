using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingImp
{
    public void Update();
    public BuildingData GetData();
    public SlotList GetSlotList();
    public bool SendTroops();
    public void Reinforcement();
    public void GotAttacked();
}
