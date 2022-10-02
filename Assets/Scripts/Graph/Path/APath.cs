using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs.Path
{
    public class APathBuildings : IPathAlgorithm<Building>
    {
        public int currentTeam { get; set; } = 0;

        public APathBuildings()
        {
        }

        public Path<Building> FindPath(Node<Building> sourceNode, Node<Building> targetNode)
        {
            //var queue = new Queue<List<Node<Building>>>();
            //queue.Enqueue(new List<Node<Building>>());
            //queue.Peek().Add(sourceNode);

            //while (queue.Count > 0)
            //{
            //    var path = queue.Dequeue();
            //    var lastNode = path[path.Count - 1];

            //    if (lastNode == targetNode)
            //        return new Path<Building>(path);
            //    else if (lastNode.Value.Team != 0 && lastNode.Value.Team != currentTeam)
            //        continue;

            //    foreach (var adjacent in lastNode.Adjacents)
            //    {
            //        var newPath = new List<Node<Building>>(path);
            //        newPath.Add(adjacent);
            //        queue.Enqueue(newPath);
            //    }
            //}

            return new Path<Building>(null);
        }
    }
}
