using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoldierItems", fileName = "New Soldier Items")]
public class SoldierItems : ScriptableObject
{
    [SerializeField] private Sprite[] _bodies;
    [SerializeField] private Sprite[] _helmets;
    [SerializeField] private Sprite[] _faces;
    [SerializeField] private Sprite[] _shields;
    [SerializeField] private Sprite[] _weapons;
    
    public Dictionary<ClothingItem.Type, Sprite[]> items { get; private set; }

    public void Awake()
    {
        items = new Dictionary<ClothingItem.Type, Sprite[]>();

        items.Add(ClothingItem.Type.Body, _bodies);        
        items.Add(ClothingItem.Type.Helmet, _helmets);
        items.Add(ClothingItem.Type.Face, _faces);
        items.Add(ClothingItem.Type.Shield, _shields);
        items.Add(ClothingItem.Type.Weapon, _weapons);
    }
}
