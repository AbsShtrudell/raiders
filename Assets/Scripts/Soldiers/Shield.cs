using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class Shield : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _distance = 0.554f;
    
        private void Start()
        {
            transform.localPosition = new Vector3(_distance, 0f, 0f);
            transform.forward = Vector3.right;
        }
    
        public void Rotate(Vector3 direction)
        {
            float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.up);
    
            transform.localPosition = Quaternion.Euler(0f, angle, 0f) * new Vector3(_distance, 0f, 0f);
            transform.forward = direction;
        }
    }
}
