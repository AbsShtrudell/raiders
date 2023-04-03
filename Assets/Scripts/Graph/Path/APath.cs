using System.Collections.Generic;

namespace Raiders.Graphs
{
    public class APathBuildings : IPathAlgorithm<Building>
    {
        public APathBuildings()
        {

        }

        public Path<Building> FindPath(Node<Building> sourceNode, Node<Building> targetNode, Graph<Building> graph)
        {
            if(sourceNode == null || targetNode == null) return null;

            Side currentSide = sourceNode.Value.Side;

            var queue = new Queue<List<Node<Building>>>();
            queue.Enqueue(new List<Node<Building>>());
            queue.Peek().Add(sourceNode);

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var lastNode = path[path.Count - 1];

                if (lastNode == targetNode)
                    return new Path<Building>(path);
                else if (lastNode.Value.Side != currentSide)
                    continue;

                foreach (var adjacent in lastNode.Adjacents)
                {
                    var newPath = new List<Node<Building>>(path);
                    Node<Building> adj = graph.Find(adjacent);
                    if (adj == null) continue;
                    newPath.Add(adj);
                    queue.Enqueue(newPath);
                }
            }

            return null;
        }
    }
}
