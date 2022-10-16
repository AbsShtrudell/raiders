using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphs;

[RequireComponent(typeof(BuildingsGraphEditor))]
public class LevelController : MonoBehaviour
{
    private Graph<Building> _graph;
    private List<Road> _roads;

    private void Awake()
    {
        BuildingsGraphEditor graphEditor = GetComponent<BuildingsGraphEditor>();

        _graph = graphEditor.graph;
        _roads = graphEditor.roads;
    }
}
