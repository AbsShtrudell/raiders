using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SquadTypeInfo", fileName = "New Squad Type Info")]
    public class SquadTypeInfo : ScriptableObject
    {
        [SerializeField] private uint _damage = 1;
        [SerializeField] private UnitInfo _unitInfo;

        public uint Damage => _damage;
        public UnitInfo UnitInfo=> _unitInfo;
    }
}
