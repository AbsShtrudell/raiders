using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphs;
using Zenject;

[ExecuteInEditMode]
public class BuildingsGraphEditor : MonoBehaviour
{
    [SerializeField]
    private LevelController _levelController;
    [Space]
    [SerializeField]
    private Building _buildingVisual;
    [SerializeField]
    private Road _roadVisual;

    private Building.Factory _buildingFactory;
    private Road.Factory _roadFactory;

    [SerializeField]
    private Graph<Building> _buildingsGraph;

    private void OnEnable()
    {
        _buildingFactory = new Building.Factory(null, _buildingVisual);
        _roadFactory = new Road.Factory(_roadVisual);
        if(_buildingFactory == null)
            _buildingsGraph = new Graph<Building>(new Graphs.Path.APathBuildings());
    }

    public void AddBuilding()
    {
        Building building = _buildingFactory.Create();
        _buildingsGraph.AddNode(building);
    }

    public void BindRandomBuildings()
    {
        if (_buildingsGraph.Nodes.Count < 2) return;

        Node<Building> b1 = _buildingsGraph.Nodes[Random.Range(0, _buildingsGraph.Nodes.Count - 1)];
        Node<Building> b2 = _buildingsGraph.Nodes[Random.Range(0, _buildingsGraph.Nodes.Count - 1)];

        if (_buildingsGraph.AddEdge(b1, b2))
        {
            _roadFactory.Create(b1.Value, b2.Value);
        }
    }
}
