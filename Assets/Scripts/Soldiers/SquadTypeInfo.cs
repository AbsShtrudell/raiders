using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SquadTypeInfo", fileName = "New Squad Type Info")]
    public class SquadTypeInfo : ScriptableObject
    {
        [SerializeField, Min(0)] private int _damage = 1;
        [SerializeField] private UnitInfo _unitInfo;
        [field: SerializeField] public TroopsType TroopType {get; private set;}

        public int Damage => _damage;
        public UnitInfo UnitInfo=> _unitInfo;
    }
}
