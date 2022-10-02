using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphs;

public class LevelController : MonoBehaviour
{
    private Graph<Building> _graph;

    public void Initialize(Graph<Building> graph)
    {
        _graph = graph;
    }

    private void OnEnable()
    {
        Debug.Log(_graph);
    }
}
