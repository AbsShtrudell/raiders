using UnityEngine;
using System;
using Zenject;
using System.Collections.Generic;
using Dreamteck.Splines;

public class Building : MonoBehaviour
{
    [Inject]
    private DiContainer container;
    [Inject] 
    private BuildingImpCreator _bImpCreator;
    [Inject]
    private SlotsControllerCreator _sContrCreator;
    [SerializeField] 
    private Side _side = Side.Rebels;
    [SerializeField] 
    private BuildingType _type = BuildingType.Simple;

    public Side Side
    { get { return _side; } set { _side = value; } }
    public BuildingType Type => _type;

    public GameObject squadPrefab {get; set;}
    [SerializeField]
    private Transform _visual;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Collider _collider;
    private SlotsController _slotsUI;

    [SerializeField]
    private List<Road> _roads;
    public List<Road> roads => _roads;

    private BuildingImp _buildingImp;


    public Graphs.Graph<Building> graph { get; set; }

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
        InitBuildingImp(_type, _side);
        _slotsUI = _sContrCreator?.Create(_side, _buildingImp);
        _slotsUI.gameObject.transform.SetParent(_visual);
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

        _slotsUI.transform.localPosition = Vector3.up * 2;
    }

    private void InitBuildingImp(BuildingType type, Side side)
    {
        _buildingImp = _bImpCreator?.Create(type, side);

        _buildingImp.Captured += ChangeTeam;
    }

    private void ChangeTeam(Side side)
    {
        _side = side;

        if (_buildingImp.BuildingData.PreviousLevel)
        {
            InitBuildingImp(_buildingImp.BuildingData.PreviousLevel.Type, side);
            _type = _buildingImp.BuildingData.Type;
        }
        else
        {
            InitBuildingImp(_buildingImp.BuildingData.Type, side);
        }
        Destroy(_slotsUI.gameObject);
        _slotsUI = _sContrCreator?.Create(_side, _buildingImp);
        _slotsUI.transform.SetParent(_visual);
    }

    public void SquadEnter(Side side, TroopsType type)
    {
        if(side != _side)
        {
            _buildingImp.GotAttacked(side);
        }
        else
        {
            _buildingImp.Reinforcement();
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
        var source = graph.Find(this);
        var destination = graph.Find(target);

        var path = graph.pathAlgorithm.FindPath(source, destination, graph);
        
        if (path == null || !_buildingImp.SendTroops())
        {
            return;
        }

        var pathRoads = new Queue<Tuple<SplineComputer, Squad.Direction>>();
        var squad = container.InstantiatePrefab(squadPrefab).GetComponent<Squad>();
        squad.SetSide(_side);
        squad.SetTarget(target);

        for (int i = 0; i < path.nodes.Count - 1; i++)
        {
            foreach (var road in path.nodes[i].Value.roads)
            {
                if (road.HasConnectionWith(path.nodes[i + 1].Index))
                {
                    var direction = road.Ends[1].Index == path.nodes[i].Index ? Squad.Direction.Backward : Squad.Direction.Forward;
                    
                    pathRoads.Enqueue(new Tuple<SplineComputer, Squad.Direction>(road.PathCreator, direction));

                    break;
                }
            }
        }

        squad.SetRoads(pathRoads);
        squad.MakeSoldiers();
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