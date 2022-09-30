using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs.Path
{
    public class Path<T>
    {
        public List<Node<T>> nodes
        { get; private set; }

        Path(List<Node<T>> nodes) => this.nodes = nodes;
    }
}
