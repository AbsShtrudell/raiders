using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UnitInfo", fileName = "New Unit Info")]
    public class UnitInfo : ScriptableObject
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _damage = 20f;
    
        public float health => _health;
        public float speed => _speed;
        public float damage => _damage;
    }
}