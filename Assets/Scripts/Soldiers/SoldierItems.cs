using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
	[CreateAssetMenu(menuName = "ScriptableObjects/SoldierItems", fileName = "New Soldier Items")]
    public class SoldierItems : ScriptableObject
    {
        [System.Serializable]
        public class FrontBackSprites
        {
            [SerializeField] private Sprite _front;
            [SerializeField] private Sprite _back;
            [field: SerializeField] public TroopsType TroopType {get; private set;}

            public Sprite front => _front;
            public Sprite back => _back;
        }

        [SerializeField] private FrontBackSprites[] _bodies;
        [SerializeField] private FrontBackSprites[] _helmets;
        [SerializeField] private FrontBackSprites[] _faces;
        [SerializeField] private FrontBackSprites[] _shields;
        [SerializeField] private FrontBackSprites[] _weapons;

        public Dictionary<ClothingItem.Type, FrontBackSprites[]> items { get; private set; }

        public void Awake()
        {
            items = new Dictionary<ClothingItem.Type, FrontBackSprites[]>();
    
            items.Add(ClothingItem.Type.Body, _bodies);        
            items.Add(ClothingItem.Type.Helmet, _helmets);
            items.Add(ClothingItem.Type.Face, _faces);
            items.Add(ClothingItem.Type.Shield, _shields);
            items.Add(ClothingItem.Type.Weapon, _weapons);
        }
    
    }
}