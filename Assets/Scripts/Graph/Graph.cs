using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Raiders.Graphs
{
    [System.Serializable]
    public class Graph<T> : IGraph<T>
    {
        public IPathAlgorithm<T> PathAlgorithm
        { get; set; }

        [SerializeField]
        private List<Node<T>> _nodes = new List<Node<T>>();

        public List<Node<T>> Nodes
        { get { return _nodes; } }

        public Graph(IPathAlgorithm<T> pathAlgorithm) => PathAlgorithm = pathAlgorithm;
        public Graph() { }
 
        public Node<T> this[int index]
        {
            get => Find(index);
        }

        public void AddNode(T value)
        {
            _nodes.Add(new Node<T>(value, value.GetHashCode()));
        }

        public bool RemoveNode(T value)
        {
            return RemoveNode_(Find(value));
        }

        public bool RemoveNode(int index)
        {
            return RemoveNode_(Find(index));
        }

        private bool RemoveNode_(Node<T> node)
        {
            if(node == null) return false;

            foreach (var id in node.Adjacents)
                Find(id).Adjacents.Remove(node.Index);

            return _nodes.Remove(node);
        }

        public bool AddEdge(Node<T> node1, Node<T> node2)
        {
            return node1.Bind(node2);
        }

        public bool RemoveEdge(Node<T> node1, Node<T> node2)
        {
            return node1.Unbind(node2);
        }

        public Node<T> Find(int id)
        {
            foreach(var node in Nodes)
            {
                if (node.Index == id)
                    return node;
            }
            return null;
        }

        public Node<T> Find(T value)
        {
            foreach(var node in Nodes)
            {
                if (node.Value.Equals(value))
                    return node;
            }
            return null;
        }

        public IEnumerable<Node<T>> GetAdjacents(Node<T> node)
        {
            return GetAdjacents_(node);
        }

        public IEnumerable<Node<T>> GetAdjacents(int id)
        {
            return GetAdjacents_(Find(id));
        }

        private IEnumerable<Node<T>> GetAdjacents_(Node<T> node)
        {
            HashSet<Node<T>> adj = new HashSet<Node<T>>();

            foreach (int indexes in node.Adjacents)
            {
                adj.Add(Find(indexes));
            }

            return adj;
        }
    }
}
