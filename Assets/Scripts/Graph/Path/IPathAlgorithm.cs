using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs.Path
{
    public interface IPathAlgorithm<T>
    {
        public Path<T> FindPath(Node<T> sourceNode, Node<T> targetNode);
    }
}
