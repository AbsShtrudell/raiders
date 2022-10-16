using System.Collections.Generic;
using UnityEngine;
using Graphs;
using UnityEditor;
using System;
using Graphs.Path;

[ExecuteAlways]
public class BuildingsGraphEditor : MonoBehaviour
{
    [SerializeField]
    private LevelController _levelController;
    [Space]
    [SerializeField]
    private Building _buildingVisual;
    [SerializeField]
    private Road _roadVisual;
    [SerializeField]
    private GameObject squadPrefab;

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
        if (Application.isPlaying)
        {
            _buildingsGraph.pathAlgorithm = new APathBuildings();
            foreach (var node in _buildingsGraph.Nodes)
            {
                node.Value.graph = _buildingsGraph;
                node.Value.squadPrefab = squadPrefab;
            }
        }
        else
        {
            _buildingFactory = new Building.Factory(null, _buildingVisual);
            _roadFactory = new Road.Factory(_roadVisual);
            if(_buildingFactory == null)
                _buildingsGraph = new Graph<Building>(new Graphs.Path.APathBuildings());
        }
    }

    public void AddBuilding()
    {
        Building building = _buildingFactory.Create();

        Undo.RegisterCreatedObjectUndo(building.gameObject, "create building");
        Undo.RecordObject(this, "add buildnig");

        _buildingsGraph.AddNode(building);

        Undo.SetCurrentGroupName("Add Building");
    }

    private void Update()
    {
        if (Application.isPlaying)
            return;

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
                var tempRoads = b.Value.roads;
                for (int i = 0; i < tempRoads.Count; i++)
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
            //Debug.Log("destroyedBuildingsNodeIndices.Count: " + destroyedBuildingsNodeIndices.Count);
            //Debug.Log("destroyedRoadIndices.Count: " + destroyedRoadIndices.Count);
            //Debug.Log("destroyedRoadInBuilding.Count: " + destroyedRoadInBuilding.Count);
            //Debug.Log("connectedRoadsToDestroy.Count: " + connectedRoadsToDestroy.Count);
            //Debug.Log("unconnectedRoadsToDestroy.Count: " + unconnectedRoadsToDestroy.Count);
            //Debug.Log("nodesToUnbind.Count: " + nodesToUnbind.Count);
            if (Undo.GetCurrentGroupName() == groupName2)
            {
                var recordedObjects = new List<Building>();

                Undo.RegisterCompleteObjectUndo(this, "Remove buildings");

                while (destroyedRoadIndices.Count > 0)
                {
                    _roads.RemoveAt(destroyedRoadIndices.Pop());
                }
                
                while (destroyedRoadInBuilding.Count > 0)
                {
                    var br = destroyedRoadInBuilding.Pop();

                    if (!recordedObjects.Contains(br.Item1))
                    {
                        recordedObjects.Add(br.Item1);
                        Undo.RegisterCompleteObjectUndo(br.Item1, "b");
                    }
                    br.Item1.roads.RemoveAt(br.Item2);
                }

                foreach (var br in connectedRoadsToDestroy)
                {
                    _roads.Remove(br.Item2);
                    if (!recordedObjects.Contains(br.Item1))
                    {
                        recordedObjects.Add(br.Item1);
                        Undo.RegisterCompleteObjectUndo(br.Item1, "b");
                    }
                    br.Item1.roads.Remove(br.Item2);
                    Undo.DestroyObjectImmediate(br.Item2.gameObject);
                }

                foreach (var road in unconnectedRoadsToDestroy)
                {
                    if (road == null) continue; 

                    _roads.Remove(road);
                    Undo.DestroyObjectImmediate(road.gameObject);
                }

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

                
                while (destroyedRoadInBuilding.Count > 0)
                {
                    var br = destroyedRoadInBuilding.Pop();

                    Undo.RegisterCompleteObjectUndo(br.Item1, "ilya ne bei");
                    Undo.RevertAllInCurrentGroup();
                }

                
                if (destroyedRoadIndices.Count != 0)
                {
                    while (destroyedRoadIndices.Count > 0)
                    {
                        _roads.RemoveAt(destroyedRoadIndices.Pop());
                    }

                    foreach (var node in _buildingsGraph.Nodes)
                    {
                        foreach (var road in node.Value.roads)
                        {
                            if (!_roads.Contains(road))
                            {
                                _roads.Add(road);
                            }
                        }
                    }
                }

                foreach (var road in unconnectedRoadsToDestroy)
                {
                    Undo.RegisterCompleteObjectUndo(road, "ilya ne bei");
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
            if (b1.Bind(b2))
            {
                var road = _roadFactory.Create(b1, b2);

                Undo.RegisterCreatedObjectUndo(road.gameObject, "road");

                _roads.Add(road);

                b1.Value.roads.Add(road);
                b2.Value.roads.Add(road);

            }
        }
        else
        {
            if (b1.Unbind(b2))
            {
                foreach (var road in b1.Value.roads)
                {
                    if (road.HasConnectionWith(b2.Index))
                    {
                        _roads.Remove(road);
                        b1.Value.roads.Remove(road);
                        b2.Value.roads.Remove(road);
                        Undo.DestroyObjectImmediate(road.gameObject);

                        break;
                    }
                }
            }
        }
    }
}
