using System.Collections.Generic;
using UnityEngine;
using Graphs;
using UnityEditor;
using System;

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

    private void Update()
    {
        const string groupName = "Remove buildings";
        const string groupName2 = "Delete Game Objects";

        var destroyedRoadIndices = new Stack<int>();
        var destroyedBuildingsNodeIndices = new List<int>();
        var destroyedRoadInBuilding = new Stack<Tuple<Building, int>>();
        var connectedRoadsToDestroy = new List<Tuple<Building, Road>>();
        var unconnectedRoadsToDestroy = new List<Road>();
        var nodesToUnbind = new List<Tuple<Node<Building>, Node<Building>>>();

        for (int i = 0; i < _roads.Count; i++)
        {
            if (_roads[i] == null)
            {
                destroyedRoadIndices.Push(i);
            }
            else if (_roads[i].Ends[0].Value == null && _roads[i].Ends[1].Value == null)
            {
                unconnectedRoadsToDestroy.Add(_roads[i]);
            }
        }

        foreach (var b in _buildingsGraph.Nodes)
        {
            if (b.Value == null)
            {
                destroyedBuildingsNodeIndices.Add(b.Index);
            }
            else
            {
                for (int i = 0; i < b.Value.roads.Count; i++)
                {
                    if (b.Value.roads[i] == null)
                    {
                        destroyedRoadInBuilding.Push(new Tuple<Building, int>(b.Value, i));
                    }
                }

                foreach (var i in b.Adjacents)
                {
                    var a = graph.Find(i);
                    var road = b.Value.roads.Find(r => r == null ? false : r.HasConnectionWith(i));

                    if (a.Value == null || road == null)
                    {
                        if (road != null)
                        {
                            connectedRoadsToDestroy.Add(new Tuple<Building, Road>(b.Value, road));
                        }

                        nodesToUnbind.Add(new Tuple<Node<Building>, Node<Building>>(b, a));
                    }
                }
            
            }
        }

        if (destroyedBuildingsNodeIndices.Count +
        destroyedRoadIndices.Count +
        destroyedRoadInBuilding.Count +
        connectedRoadsToDestroy.Count +
        unconnectedRoadsToDestroy.Count +
        nodesToUnbind.Count != 0)
        {
            if (Undo.GetCurrentGroupName() == groupName2)
            {
                Undo.RecordObject(this, "Remove buildings");

                while (destroyedRoadIndices.Count > 0)
                {
                    _roads.RemoveAt(destroyedRoadIndices.Pop());
                }
                
                while (destroyedRoadInBuilding.Count > 0)
                {
                    var br = destroyedRoadInBuilding.Pop();

                    Undo.RecordObject(br.Item1, "b");
                    br.Item1.roads.RemoveAt(br.Item2);
                }

                foreach (var br in connectedRoadsToDestroy)
                {
                    Undo.RecordObject(this, "Remove buildings");
                    _roads.Remove(br.Item2);
                    Undo.RecordObject(br.Item1, "b");
                    br.Item1.roads.Remove(br.Item2);
                    Undo.DestroyObjectImmediate(br.Item2.gameObject);
                }

                foreach (var road in unconnectedRoadsToDestroy)
                {
                    Undo.RecordObject(this, "Remove buildings");
                    _roads.Remove(road);
                    Undo.DestroyObjectImmediate(road.gameObject);
                }

                Undo.RecordObject(this, "Remove buildings");
                foreach (var nn in nodesToUnbind)
                {
                    nn.Item1.Unbind(nn.Item2);
                }

                foreach (var i in destroyedBuildingsNodeIndices)
                {
                    _buildingsGraph.RemoveNode(i);
                }

                Undo.SetCurrentGroupName(groupName);
            }
            else
            {
                foreach (var i in destroyedBuildingsNodeIndices)
                {
                    var node =  graph.Find(i);
                    var adsasdd = _roadFactory.Create(node, node).gameObject;
                    Undo.RegisterCreatedObjectUndo(adsasdd, "ilya ne bei");
                    Undo.RevertAllInCurrentGroup();
                }
            }
        }
    }

    public void ChangeBindStatus(bool bind, int i1, int i2)
    {
        var b1 = _buildingsGraph.Nodes[i1];
        var b2 = _buildingsGraph.Nodes[i2];

        if (bind)
        {
            Undo.RecordObject(this, "ed");
            if (b1.Bind(b2))
            {
                var road = _roadFactory.Create(b1, b2);

                Undo.RegisterCreatedObjectUndo(road.gameObject, "road");

                Undo.RecordObject(this, "ed");
                _roads.Add(road);

                Undo.RecordObject(b1.Value, "b1");
                b1.Value.roads.Add(road);
                Undo.RecordObject(b2.Value, "b2");
                b2.Value.roads.Add(road);

            }
        }
        else
        {
            Undo.RecordObject(this, "ed");
            if (b1.Unbind(b2))
            {
                foreach (var road in b1.Value.roads)
                {
                    if (road.Ends[0].Index == b2.Index || road.Ends[1].Index == b2.Index)
                    {
                        Undo.RecordObject(this, "ed");
                        _roads.Remove(road);
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
