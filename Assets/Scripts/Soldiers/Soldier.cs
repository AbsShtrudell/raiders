using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Raiders
{
    public class Soldier : MonoBehaviour, IControllable
    {
        [SerializeField] private Side _side;
        [Zenject.Inject(Id = "Arsenal")] private Dictionary<Side, SoldierItems> arsenal;
        private Dictionary<ClothingItem.Type, ClothingItem> _items;
        private Shield _shield;
    	private Weapon _weapon;
    	private NavMeshAgent _agent;

        public Side side
        {
            get => _side;
            set => _side = value;
        }

        private void Awake()
        {
            _items = new Dictionary<ClothingItem.Type, ClothingItem>();

            var children = GetComponentsInChildren<ClothingItem>();

            foreach (var item in children)
            {
                _items.Add(item.type, item);
            }
            _shield = GetComponentInChildren<Shield>();
        	_weapon = GetComponentInChildren<Weapon>();
        	_agent = GetComponent<NavMeshAgent>();
        	_agent.updatePosition = false;
        }
        
private void Update()
    {
        if (!_agent.hasPath)
            return;

        var direction = _agent.nextPosition - transform.position;
        direction.y = 0;

        _shield.Rotate(direction);

        if (direction.x > 0)
        {        
            if (direction.z < 0)
            {
                foreach (var item in _items)
                {
                    item.Value.UnflipX();
                    item.Value.SetSprite(_currentSprites[item.Key].front);
                }
                _weapon.UnflipX();
            }
            else
            {
                foreach (var item in _items)
                {
                    item.Value.FlipX();
                    item.Value.SetSprite(_currentSprites[item.Key].back);
                }
                _weapon.FlipX();
            }

        }
        else
        {
            if (direction.z < 0)
            {
                foreach (var item in _items)
                {
                    item.Value.FlipX();
                    item.Value.SetSprite(_currentSprites[item.Key].front);
                }
                _weapon.UnflipX();
            }
            else
            {
                foreach (var item in _items)
                {
                    item.Value.UnflipX();
                    item.Value.SetSprite(_currentSprites[item.Key].back);
                }
                _weapon.FlipX();
            }
        }

        transform.position = _agent.nextPosition;
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
    }

    public void GoTo(Vector3 destination)
    {
        _agent.SetDestination(destination);        
    }

    public void SetHealth(float health)
    {
        _currentHealth = health;
    }
}
