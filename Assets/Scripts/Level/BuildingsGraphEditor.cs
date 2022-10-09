using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphs;
using Zenject;
using UnityEditor;

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

    public Graph<Building> graph => _buildingsGraph;

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

        Undo.RegisterCreatedObjectUndo(building.gameObject, "create building");
        Undo.RecordObject(this, "add buildnig");

        _buildingsGraph.AddNode(building);

        Undo.SetCurrentGroupName("Add Building");
    }

    public void RemoveBuilding(int index)
    {
        if (_buildingsGraph.Nodes.Count == 0)
            return;

        var b = _buildingsGraph.Nodes[index].Value;
        _buildingsGraph.RemoveNode(b);
        if (b.OnDisabled != null) b.OnDisabled();
        DestroyImmediate(b.gameObject);
    }

    public void BindRandomBuildings()
    {
        if (_buildingsGraph.Nodes.Count < 2) return;

        Node<Building> b1 = _buildingsGraph.Nodes[Random.Range(0, _buildingsGraph.Nodes.Count - 1)];
        Node<Building> b2 = _buildingsGraph.Nodes[Random.Range(0, _buildingsGraph.Nodes.Count - 1)];

        if (_buildingsGraph.AddEdge(b1, b2))
        {
            var road = _roadFactory.Create(b1, b2);

            b1.Value.OnDisabled += road.DestroySelf;
            b2.Value.OnDisabled += road.DestroySelf;
        }
    }

    public void ChangeBindStatus(bool bind, int i1, int i2)
    {
        var b1 = _buildingsGraph.Nodes[i1];
        var b2 = _buildingsGraph.Nodes[i2];

        if (bind)
        {
            if (_buildingsGraph.AddEdge(b1, b2))
            {
                var road = _roadFactory.Create(b1, b2);

                b1.Value.OnDisabled += road.DestroySelf;
                b2.Value.OnDisabled += road.DestroySelf;
                b1.onUnbind += road.DestroySelfOnUnbind;
                b2.onUnbind += road.DestroySelfOnUnbind;
            }
        }
        else
        {
            b1.Unbind(b2);
        }
    }
}
