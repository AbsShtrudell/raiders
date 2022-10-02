using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphs;
using Graphs.Path;
using Zenject;

public class BuildingsGraph : MonoBehaviour
{
    private Graph<Building> _graph;

    private void Start()
    {
        _graph = new Graph<Building>(new APathBuildings());

        var buildings = GetComponentsInChildren<Building>();

        foreach (var b in buildings)
        {
            _graph.AddNode(b);
        }

        buildings[0].SetTeam(1);
        buildings[1].SetTeam(2);
        buildings[4].SetTeam(2);

        _graph.Nodes[0].Bind(_graph.Nodes[1]);
        _graph.Nodes[1].Bind(_graph.Nodes[2]);
        _graph.Nodes[1].Bind(_graph.Nodes[4]);
        _graph.Nodes[4].Bind(_graph.Nodes[3]);
        _graph.Nodes[3].Bind(_graph.Nodes[5]);
        _graph.Nodes[0].Bind(_graph.Nodes[5]);

        (_graph.pathAlgorithm as APathBuildings).currentTeam = 1;
        var path = _graph.pathAlgorithm.FindPath(_graph.Nodes[0], _graph.Nodes[4]);

        string p = "";
        foreach (var n in path.nodes)
        {
            p += n.Value.gameObject.name;
        }

        Debug.Log(p);
    }

}
