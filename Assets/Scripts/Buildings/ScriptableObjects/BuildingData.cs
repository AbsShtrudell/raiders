using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleBuilding", menuName = "ScriptableObjects/SimpleBuilding", order = 1)]
public class BuildingData : ScriptableObject
{
    [SerializeField, Min(1)] private int _squadSlot;
    [SerializeField, Min(0.01f)] private float _squadRecoveryTime;
    [SerializeField, Min(0)] private uint _income;
    [SerializeField, Min(0)] private uint _upkeep;
    [SerializeField, Min(1)] private int _defenseMultyplier;
    [SerializeField] private BuildingType _type;
    [SerializeField] private TroopsType _troopsType;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private BuildingData _previous;
    [SerializeField] private List<BuildingData> _upgrades;
    [SerializeField, Min(0)] private uint _cost;
    public int SquadSlots => _squadSlot;
    public float SquadRecoveryTime => _squadRecoveryTime;
    public uint Income => _income;
    public uint Upkeep => _upkeep;
    public int DefenseMultyplier => _defenseMultyplier;
    public TroopsType TroopsType => _troopsType;
    public Mesh Mesh => _mesh;
    public List<BuildingData> Upgrades => _upgrades;
    public BuildingData PreviousLevel => _previous;
    public BuildingType Type => _type;
    public uint Cost => _cost;
}
