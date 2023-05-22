using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class Flag : MonoBehaviour
    {
        [field: SerializeField] public Vector3 Offset {get; set;}
        public Transform Tagert { get; set; }

        // Update is called once per frame
        void Update()
        {
            if(Tagert != null)
            {
                transform.position = Tagert.position + Offset;
                transform.rotation = Tagert.rotation;
            }
        }
    }
}
