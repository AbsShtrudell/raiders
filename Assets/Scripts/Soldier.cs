using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Viking, English
}

public class Soldier : MonoBehaviour
{
    [SerializeField] private Side _side;
    [Zenject.Inject(Id = "Arsenal")] private Dictionary<Side, SoldierItems> arsenal;
    private Dictionary<ClothingItem.Type, ClothingItem> _items;

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
    }

    public void ChangeItems()
    {
        foreach (var item in _items)
        {
            var sprites = arsenal[side].items[item.Key];

            item.Value.SetSprite(sprites[Random.Range(0, sprites.Length)]);
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
