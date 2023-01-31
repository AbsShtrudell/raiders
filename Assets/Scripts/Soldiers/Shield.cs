using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _distance = 0.554f;

    void Start()
    {
        transform.localPosition = new Vector3(_distance, 0f, 0f);
    }

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), float.PositiveInfinity, LayerMask.GetMask("Terrain")))
        {
            Debug.Log("d");
        }
    }
}
