using UnityEngine;
using System;
using Zenject;

public class Building : MonoBehaviour
{
    [Inject] 
    private BuildingImpCreator _bImpCreator;

    [SerializeField] 
    private Side _side = Side.Rebels;
    [SerializeField] 
    private BuildingType _type = BuildingType.Simple;

    public Side Side => _side;
    public BuildingType Type => _type;

    [SerializeField]
    private Transform _visual;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Collider _collider;

    private BuildingImp _buildingImp;

    public int Team
    { get { return 0; } }

    public Action<Building> OnSelected;
    public Action<Building> OnDeselected;

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
    }

    public void Deselect()
    {
        OnDeselected?.Invoke(this);
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