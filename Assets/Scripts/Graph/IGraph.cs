using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.Graphs
{
    public interface IGraph<T> : IReadOnlyGraph<T>
    {
        public void AddNode(T value);
        public bool RemoveNode(T value);
        public bool RemoveNode(int index);
        public bool AddEdge(Node<T> node1, Node<T> node2);
        public bool RemoveEdge(Node<T> node1, Node<T> node2);
    }
}
