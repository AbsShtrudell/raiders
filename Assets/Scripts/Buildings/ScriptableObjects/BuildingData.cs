using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleBuilding", menuName = "ScriptableObjects/SimpleBuilding", order = 1)]
public class BuildingData : ScriptableObject
{
    [SerializeField, Min(1)] private int _squadSlot;
    [SerializeField, Min(0.01f)] private float _squadRecoveryTime;
    [SerializeField, Min(0)] private int _income;
    [SerializeField, Min(1)] private int _defenseMultyplier;
    [SerializeField] private BuildingType _type;
    [SerializeField] private TroopsType _troopsType;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private BuildingData _previous;
    [SerializeField] private BuildingData _upgrade;

    public int SquadSlots => _squadSlot;
    public float SquadRecoveryTime => _squadRecoveryTime;
    public int Income => _income;
    public int DefenseMultyplier => _defenseMultyplier;
    public TroopsType TroopsType => _troopsType;
    public Mesh Mesh => _mesh;
    public BuildingData Upgrade => _upgrade;
    public BuildingData PreviousLevel => _previous;
    public BuildingType Type => _type;
}
