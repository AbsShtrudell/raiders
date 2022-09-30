using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs.Path
{
    public class APathBuildings : IPathAlgorithm<Building>
    {
        public APathBuildings()
        {
        }

        public Path<Building> FindPath(Node<Building> sourceNode, Node<Building> targetNode)
        {
            throw new System.NotImplementedException();
        }
    }
}
