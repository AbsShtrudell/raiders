using UnityEngine;

namespace Raiders
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UnitInfo", fileName = "New Unit Info")]
    public class UnitInfo : ScriptableObject
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _damage = 20f;

        public float Health => _health;
        public float Speed => _speed;
        public float Damage => _damage;
    }
}
