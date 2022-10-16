using Graphs.Path;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs
{
    [System.Serializable]
    public class Graph<T>
    {
        public Path.IPathAlgorithm<T> pathAlgorithm
        { get; set; }

        [SerializeField]
        private List<Node<T>> _nodes = new List<Node<T>>();
        public List<Node<T>> Nodes
        { get { return _nodes; } }

        public Graph(IPathAlgorithm<T> pathAlgorithm) => this.pathAlgorithm = pathAlgorithm;

        public void AddNode(T value)
        {
            _nodes.Add(new Node<T>(value, value.GetHashCode()));
        }

        public bool RemoveNode(T value)
        {
            Node<T> buf = null;
            foreach (var node in _nodes)
            {
                if (node.Value.Equals(value))
                {
                    buf = node;
                    _nodes.Remove(node);
                    break;
                }
            }
            if (buf == null) return false;

            foreach (var id in buf.Adjacents)
            {
                //node.Adjacents.Remove(buf);
            }

            return true;
        }

        public bool RemoveNode(int index)
        {
            Node<T> buf = null;
            foreach (var node in _nodes)
            {
                if (node.Index == index)
                {
                    buf = node;
                    _nodes.Remove(node);
                    break;
                }
            }
            if (buf == null) return false;

            foreach (var id in buf.Adjacents)
            {
                //node.Adjacents.Remove(buf);
            }

            return true;
        }

        public bool AddEdge(Node<T> node1, Node<T> node2)
        {
            return node1.Bind(node2);
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
    }
}
