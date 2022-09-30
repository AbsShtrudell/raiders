using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs
{
    public class Node<T>
    {
        private List<Node<T>> _adjacents = new List<Node<T>>();
        private T _value;

        public T Value
        { get { return _value; } }
        public List<Node<T>> Adjacents
        { get { return _adjacents; } }

        public Node(T value) => _value = value;

        public bool Bind(Node<T> node)
        {
            if (node == null || _adjacents.Contains(node)) return false;

            if (!node.Adjacents.Contains(this)) node.Adjacents.Add(this);
            _adjacents.Add(node);
            return true;
        }

        public bool Unbind(Node<T> node)
        {
            if (node == null || !_adjacents.Contains(node)) return false;

            node.Adjacents.Remove(this);
            return _adjacents.Remove(node);
        }
    }
}