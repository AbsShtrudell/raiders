using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TroopsType
{
    Default, Heavy
}

[CreateAssetMenu(fileName = "SimpleBuilding", menuName = "ScriptableObjects/SimpleBuilding", order = 1)]
public class BuildingData : ScriptableObject
{
    [SerializeField, Min(1)] private int _squadSlot;
    [SerializeField, Min(0.01f)] private float _squadRecoveryTime ;
    [SerializeField, Min(0)] private int _income;
    [SerializeField, Min(1)] private int _defenseMultyplier;
    [SerializeField] private TroopsType _troopsType;
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private Mesh _mesh;

    public int SquadSlots
    { get { return _squadSlot; } }
    public float SquadRecoveryTime
    { get { return _squadRecoveryTime; } }
    public int Income
    { get { return _income; } }
    public int DefenseMultyplier
    { get { return _defenseMultyplier; } }
    public TroopsType TroopsType
    { get { return _troopsType; } }
    public BuildingType BuildingType
    { get { return _buildingType; } }
    public Mesh Mesh
    { get { return _mesh; } }
}
