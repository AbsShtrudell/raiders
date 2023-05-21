using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    [CreateAssetMenu(fileName = "SimpleBuilding", menuName = "ScriptableObjects/SimpleBuilding", order = 1)]
    public class BuildingData : ScriptableObject, IBuildingData
    {
        [SerializeField, Min(1)] private int _squadSlot;
        [SerializeField, Min(0.01f)] private float _squadRecoveryTime;
        [SerializeField, Min(0)] private uint _income;
        [SerializeField, Min(0)] private uint _upkeep;
        [SerializeField, Min(1)] private int _defenseMultyplier;
        [SerializeField] private BuildingType _type;
        [SerializeField] private SquadTypeInfo _squadTypeInfo;
        [SerializeField] private Mesh _mesh;
        [SerializeField] private BuildingData _previous;
        [SerializeField] private List<BuildingData> _upgrades;
        [SerializeField] private Sprite _icon;
        [SerializeField, Min(0)] private uint _cost;
        public int SquadSlots => _squadSlot;
        public float SquadRecoveryTime => _squadRecoveryTime;
        public uint Income => _income;
        public uint Upkeep => _upkeep;
        public int DefenseMultyplier => _defenseMultyplier;
        public SquadTypeInfo SquadTypeInfo => _squadTypeInfo;
        public Mesh Mesh => _mesh;
        public List<IBuildingData> Upgrades => _upgrades.ToList<IBuildingData>();
        public Sprite Icon => _icon;
        public IBuildingData PreviousLevel => _previous;
        public BuildingType Type => _type;
        public uint Cost => _cost;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            throw new System.NotImplementedException();
        }
    }
}