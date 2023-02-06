using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour, IControllable
{
    [SerializeField] private Side _side;
    [SerializeField] private float _currentHealth;
    [Zenject.Inject(Id = "Arsenal")] private Dictionary<Side, SoldierItems> _arsenal;
    private Dictionary<ClothingItem.Type, ClothingItem> _items;
    private Dictionary<ClothingItem.Type, SoldierItems.FrontBackSprites> _currentSprites;
    private Shield _shield;
    private Weapon _weapon;
    private NavMeshAgent _agent;
    
    public Squad squad { get; set; }
    
    public Side side
    {
        get => _side;
        set => _side = value;
    }
    
    private void Awake()
    {
        _items = new Dictionary<ClothingItem.Type, ClothingItem>();
        _currentSprites = new Dictionary<ClothingItem.Type, SoldierItems.FrontBackSprites>();

        var children = GetComponentsInChildren<ClothingItem>();

        foreach (var item in children)
        {
            _items.Add(item.type, item);

            //hueta, chisto dlya testa nezavisimogo unita
            //foreach (var i in _arsenal[side].items[item.type])
            //{
            //    if (i == null)
            //        continue;
//
            //    if (i.front == item.currentSprite || i.back == item.currentSprite)
            //    {
            //        _currentSprites.Add(item.type, i);
            //        break;
            //    }
            //}
        }

        _shield = GetComponentInChildren<Shield>();
        _weapon = GetComponentInChildren<Weapon>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        transform.position = _agent.nextPosition;
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
            var sprites = _arsenal[side].items[item.Key];

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
}
