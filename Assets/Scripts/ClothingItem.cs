using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingItem : MonoBehaviour
{
    public enum Type
    {
        Body, Helmet, Face, Shield, Weapon
    }

    [SerializeField] private Type _type;
    private SpriteRenderer _spriteRenderer;

    public Type type => _type;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void AddRenderPriority(int amount)
    {
        _spriteRenderer.sortingOrder += amount;
    } 
}
