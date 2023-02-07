using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class Weapon : MonoBehaviour
    {
        private float _initialXCoord;
        private float _initialYRotation;
    
        private void Awake()
        {
            _initialXCoord = transform.localPosition.x;
            _initialYRotation = transform.localEulerAngles.y;
        }
    
        public void FlipX()
        {
            var position = transform.localPosition;
            position.x = -_initialXCoord;
            transform.localPosition = position;
    
            var euler = transform.localEulerAngles;
            euler.y = _initialYRotation + 180;
            transform.localEulerAngles = euler;
        }
    
        public void UnflipX()
        {
            var position = transform.localPosition;
            position.x = _initialXCoord;
            transform.localPosition = position;
    
            var euler = transform.localEulerAngles;
            euler.y = _initialYRotation;
            transform.localEulerAngles = euler;
        }
    }
}