using UnityEngine;
using System;
using Zenject;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    [Inject] 
    private BuildingImpCreator _bImpCreator;

    [SerializeField] 
    private Side _side = Side.Rebels;
    [SerializeField] 
    private BuildingType _type = BuildingType.Simple;

    public Side Side
    { get { return _side; } set { _side = value; } }
    public BuildingType Type => _type;

    [SerializeField]
    private Transform _visual;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Collider _collider;

    [SerializeField]
    private List<Road> _roads;
    public List<Road> roads => _roads;

    private BuildingImp _buildingImp;

    public Action<Building> OnSelected;
    public Action<Building> OnDeselected;
    public Action OnDisabled;

    private void Awake()
    {
        if (_visual == null) _visual = transform;

        _meshRenderer = _visual.GetComponent<MeshRenderer>();
        _meshFilter = _visual.GetComponent<MeshFilter>();
        _collider = _visual.GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _buildingImp = _bImpCreator?.Create(_type, _side);
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }

    private void Start()
    {
    }

    private void Update()
    {
        _buildingImp?.Update();
    }

    public void SquadEnter(Side side, TroopsType type)
    {
        if(side != _side)
        {  

        }
        else
        {

        }
    }

    public void Select()
    {    
        OnSelected?.Invoke(this);
        _meshRenderer.material.color = Color.red;
    }

    public void Deselect()
    {
        OnDeselected?.Invoke(this);
        _meshRenderer.material.color = Color.white;
    }

    public void SendTroops(Building target)
    {
    }

    public class Factory : IFactory<Building>
    {
        private DiContainer _container;
        private Building _visual;

        public Factory(DiContainer container, Building visual)
        {
            _container = container;
            _visual = visual;
        }

        public Building Create()
        {
            Building building;

            if(_container != null)
            {
                building = _container.InstantiatePrefabForComponent<Building>(_visual);
            }
            else
            {
                building = GameObject.Instantiate(_visual.gameObject).GetComponent<Building>();
            }
            return building;
        }
    }
}