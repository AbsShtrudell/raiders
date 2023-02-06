using UnityEngine;
using Graphs;
using Graphs.Path;

namespace Test.PathFinding
{
    public class TestPathFinfingController : MonoBehaviour
    {
        [SerializeField] private Graph<Building> _graph;

        private void Start()
        {
            _graph = new Graph<Building>(new APathBuildings());
            var buildings = GetComponentsInChildren<Building>();
            foreach (var b in buildings)
            {
                _graph.AddNode(b);
            }
            buildings[0].Side = Side.Vikings;
            buildings[1].Side = Side.English;
            buildings[4].Side = Side.English;
            _graph.Nodes[0].Bind(_graph.Nodes[1]);
            _graph.Nodes[1].Bind(_graph.Nodes[2]);
            _graph.Nodes[1].Bind(_graph.Nodes[4]);
            _graph.Nodes[4].Bind(_graph.Nodes[3]);
            _graph.Nodes[3].Bind(_graph.Nodes[5]);
            _graph.Nodes[0].Bind(_graph.Nodes[5]);
            var path = _graph.pathAlgorithm.FindPath(_graph.Nodes[4], _graph.Nodes[0], _graph);
            string p = "";
            if(path.nodes != null)
            foreach (var n in path.nodes)
            {
                p += n.Value.gameObject.name;
            }
            Debug.Log(p);
        }
    }
}