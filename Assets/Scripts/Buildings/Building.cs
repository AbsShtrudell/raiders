using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BuildingType
{
    Simple, Defensive, Economic, Army
}

public class Building : MonoBehaviour
{
    private int _team = 0;

    private Transform _visual;
    private Transform _outline;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Collider _collider;

    public Action<Building> OnSelected;
    public Action<Building> OnDeselected;

    private void Awake()
    {
        if( _visual != null )
        {
            _meshRenderer = _visual.GetComponent<MeshRenderer>();
            _meshFilter = _visual.GetComponent<MeshFilter>();
            _collider = _visual.GetComponent<Collider>();
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
    }

    public void SquadEnter(int team, TroopsType type)
    {
        if(team != _team)
        {
            //enemy interaction
        }
        else
        {
            //ally interaction
        }
    }
}
