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
    private float _initialXCoord;

    public Type type => _type;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialXCoord = transform.localPosition.x;
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void AddRenderPriority(int amount)
    {
        _spriteRenderer.sortingOrder += amount;
    } 

    public void FlipX()
    {
        _spriteRenderer.flipX = true;
        
        var position = transform.localPosition;
        position.x = -_initialXCoord;
        transform.localPosition = position;
    }

    public void UnflipX()
    {
        _spriteRenderer.flipX = false;
        
        var position = transform.localPosition;
        position.x = _initialXCoord;
        transform.localPosition = position;
    }
}
