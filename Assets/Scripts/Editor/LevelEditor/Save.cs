using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Graphs;

namespace LevelEditor
{
    public static class Save
    {
        public static void SaveData(List<Node> nodes, List<Connection> connections, string file)
        {
            Graph<NodeData> graph = new Graph<NodeData>(null);

            foreach(var node in nodes)
            {
                graph.AddNode(node.data);
            }
            foreach(var connection in connections)
            {
                var node1 = graph.Nodes.Find((Node<NodeData> data)=> { if (data.Value == connection.node1.data) return true; else return false; });
                var node2 = graph.Nodes.Find((Node<NodeData> data) => { if (data.Value == connection.node2.data) return true; else return false; });
                graph.AddEdge(node1, node2);
            }

            string json = JsonUtility.ToJson(graph);

            File.WriteAllText(file, json);
        }
    }
}
