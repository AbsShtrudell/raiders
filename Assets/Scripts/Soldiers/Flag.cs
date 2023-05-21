using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class Flag : MonoBehaviour
    {
        [SerializeField] Vector3 offset;
        public Transform Tagert { get; set; }

        // Update is called once per frame
        void Update()
        {
            if(Tagert != null)
            {
                transform.position = Tagert.position + offset;
            }
        }
    }
}
