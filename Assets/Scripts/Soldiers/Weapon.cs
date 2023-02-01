using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float _initialXCoord;
    private float _initialXScale;

    private void Awake()
    {
        _initialXCoord = transform.localPosition.x;
        _initialXScale = transform.localScale.x;
    }

    public void FlipX()
    {
        var position = transform.localPosition;
        position.x = -_initialXCoord;
        transform.localPosition = position;

        var scale = transform.localScale;
        scale.x = -_initialXScale;
        transform.localScale = scale;
    }

    public void UnflipX()
    {
        var position = transform.localPosition;
        position.x = _initialXCoord;
        transform.localPosition = position;

        var scale = transform.localScale;
        scale.x = _initialXScale;
        transform.localScale = scale;
    }
}
