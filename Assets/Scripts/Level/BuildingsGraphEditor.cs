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
    [SerializeField]
    private List<Road> _roads;

    public Graph<Building> graph => _buildingsGraph;
    public List<Road> roads => _roads;

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
        Undo.RecordObject(this, "Remove Building");
        _buildingsGraph.RemoveNode(b);

        foreach (var road in b.roads)
        {
            roads.Remove(road);
            Undo.DestroyObjectImmediate(road.gameObject);
        }
        Undo.DestroyObjectImmediate(b.gameObject);
        Undo.SetCurrentGroupName("Remove Building");

    }

    public void Update()
    {
        var indices = new List<int>();

        foreach (var b in _buildingsGraph.Nodes)
        {
            if (b.Value == null)
            {
                indices.Add(b.Index);
            }
        }

        if (indices.Count != 0 && Undo.GetCurrentGroupName() == "Delete Game Objects")
        {
            Undo.RecordObject(this, "Remove buildings");

            foreach (var i in indices)
            {
                _buildingsGraph.RemoveNode(i);
            }

            Undo.SetCurrentGroupName("Remove buildings");
        }
    }

    public void BindRandomBuildings()
    {
        if (_buildingsGraph.Nodes.Count < 2) return;

        Node<Building> b1 = _buildingsGraph.Nodes[Random.Range(0, _buildingsGraph.Nodes.Count - 1)];
        Node<Building> b2 = _buildingsGraph.Nodes[Random.Range(0, _buildingsGraph.Nodes.Count - 1)];

        if (_buildingsGraph.AddEdge(b1, b2))
        {
            var road = _roadFactory.Create(b1, b2);
            _roads.Add(road);

        }
    }

    public void ChangeBindStatus(bool bind, int i1, int i2)
    {
        var b1 = _buildingsGraph.Nodes[i1];
        var b2 = _buildingsGraph.Nodes[i2];

        if (bind)
        {
            if (b1.Bind(b2))
            {
                var road = _roadFactory.Create(b1, b2);

                Undo.RegisterCreatedObjectUndo(road.gameObject, "road");

                _roads.Add(road);

                Undo.RecordObject(b1.Value, "b1");
                b1.Value.roads.Add(road);
                Undo.RecordObject(b2.Value, "b2");
                b2.Value.roads.Add(road);

            }
        }
        else
        {

            if (b1.Unbind(b2))
            {
                foreach (var road in b1.Value.roads)
                {
                    if (road.Ends[0].Index == b2.Index || road.Ends[1].Index == b2.Index)
                    {
                        roads.Remove(road);
                        Undo.RecordObject(b1.Value, "b1");
                        b1.Value.roads.Remove(road);
                        Undo.RecordObject(b2.Value, "b2");
                        b2.Value.roads.Remove(road);
                        Undo.DestroyObjectImmediate(road.gameObject);

                        break;
                    }
                }

            }

        }
    }
}
