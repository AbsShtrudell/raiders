using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.Graphs
{
    public class Path<T>
    {
        public List<Node<T>> nodes
        { get; private set; }

        public Path(List<Node<T>> nodes) => this.nodes = nodes;
    }
}
