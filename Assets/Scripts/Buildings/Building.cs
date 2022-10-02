using UnityEngine;
using System;
using Zenject;

public class Building : MonoBehaviour
{
    private int _team = 0;

    private Transform _visual;
    private Transform _outline;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Collider _collider;

    [Inject]
    private IBuildingImp _buildingImp;

    public int Team
    { get { return _team; } }

    public Action<Building> OnSelected;
    public Action<Building> OnDeselected;

    [Inject]
    public void Construct(IBuildingImp buildingImp)
    {
        this._buildingImp = buildingImp;
    }

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
        _buildingImp?.Update();
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

    public void Select()
    {
        //logic
        
        OnSelected?.Invoke(this);
    }

    public void Deselect()
    {
        //logic

        OnDeselected?.Invoke(this);
    }

    public void SendTroops(Building target)
    {
        //logic
    }

    public class Factory : IFactory<Building>
    {
        private DiContainer _container;
        private IBuildingImp _buildingImp;

        public Factory(DiContainer container, IBuildingImp imp)
        {
            _container = container;
            _buildingImp = imp;
        }

        public Building Create()
        {
            Building building = _container.Instantiate<Building>();
            building._buildingImp = this._buildingImp;
            return building;
        }
    }

    public void SetTeam(int t)
    {
        _team = t;
    }
}