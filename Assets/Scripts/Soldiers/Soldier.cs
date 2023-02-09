using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Raiders
{
    public class Soldier : MonoBehaviour, IControllable
    {
        [SerializeField] private Side _side;
        [SerializeField] private float _currentHealth;

        [Zenject.Inject(Id = "Arsenal")] private Dictionary<Side, SoldierItems> arsenal;
        private Dictionary<ClothingItem.Type, ClothingItem> _items;
        private Dictionary<ClothingItem.Type, SoldierItems.FrontBackSprites> _currentSprites;
        private Shield _shield;
    	private Weapon _weapon;
    	private NavMeshAgent _agent;

        public Squad squad { get; set; }
        public SquadRole squadRole { get; set; }
        public Vector3 direction { get; set; }

        public Side side
        {
            get => _side;
            set => _side = value;
        }

        public bool hasPath => _agent.hasPath;
        public bool pathPending => _agent.pathPending;
        public IControllable mainControllable => squad;

        private void Awake()
        {
            _items = new Dictionary<ClothingItem.Type, ClothingItem>();
            _currentSprites = new Dictionary<ClothingItem.Type, SoldierItems.FrontBackSprites>();

            var children = GetComponentsInChildren<ClothingItem>();

            foreach (var item in children)
            {
                _items.Add(item.type, item);
            }
            _shield = GetComponentInChildren<Shield>();
        	_weapon = GetComponentInChildren<Weapon>();
        	_agent = GetComponent<NavMeshAgent>();
        	_agent.updatePosition = false;
            _agent.updateRotation = false;
            transform.position = _agent.nextPosition;
        }
        
        private void Update()
        {
            if (!_agent.hasPath)
                return;

            if (_agent.nextPosition != transform.position)
            {
                direction = _agent.nextPosition - transform.position;
                direction.Set(direction.x, 0, direction.z);
                direction = direction.normalized;

                LookTowardDirection();
            }

            transform.position = _agent.nextPosition;
        }

        public void LookTowardDirection()
        {
            _shield.Rotate(direction);

            if (direction.x > 0)
            {
                if (direction.z < 0)
                    LookSouthEast();
                else
                    LookNorthEast();
            }
            else
            {
                if (direction.z < 0)
                    LookSouthWest();
                else
                    LookNorthWest();
            }
        }

        private void LookNorthWest()
        {
            foreach (var item in _items)
            {
                item.Value.UnflipX();
                item.Value.SetSprite(_currentSprites[item.Key].back);
            }
            _weapon.FlipX();
        }

        private void LookSouthWest()
        {
            foreach (var item in _items)
            {
                item.Value.FlipX();
                item.Value.SetSprite(_currentSprites[item.Key].front);
            }
            _weapon.UnflipX();
        }

        private void LookNorthEast()
        {
            foreach (var item in _items)
            {
                item.Value.FlipX();
                item.Value.SetSprite(_currentSprites[item.Key].back);
            }
            _weapon.FlipX();
        }

        private void LookSouthEast()
        {
            foreach (var item in _items)
            {
                item.Value.UnflipX();
                item.Value.SetSprite(_currentSprites[item.Key].front);
            }
            _weapon.UnflipX();
        }

        public void ChangeItems()
        {
            foreach (var item in _items)
            {
                var sprites = arsenal[side].items[item.Key];

            	_currentSprites[item.Key] = sprites[Random.Range(0, sprites.Length)];

            	item.Value.SetSprite(_currentSprites[item.Key].front);
            }
        }

        public void AddRenderPriority(int amount)
        {
            foreach (var item in _items)
            {
                item.Value.AddRenderPriority(amount * _items.Count);
            }
        }    

        public void GoTo(Vector3 destination)
        {
            _agent.SetDestination(destination);
        }

        public void SetHealth(float health)
        {
            _currentHealth = health;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}
