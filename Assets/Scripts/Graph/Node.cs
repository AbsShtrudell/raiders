using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphs
{
    [System.Serializable]
    public class Node<T>
    {
        [SerializeField]
        private int _index;
        [SerializeField]
        private List<int> _adjacents = new List<int>();
        [SerializeReference]
        private T _value;

        public T Value => _value;
        public int Index => _index;
        public List<int> Adjacents => _adjacents;

        public Action<Node<T>, Node<T>> onUnbind;

        public Node(T value, int index)
        {
            _value = value;
            _index = index;
        }

        public bool Bind(Node<T> node)
        {
            if (node == null || _adjacents.Contains(node._index) || node == this) return false;

            if (!node.Adjacents.Contains(Index)) node.Adjacents.Add(Index);
            _adjacents.Add(node._index);
            return true;
        }

        public bool Unbind(Node<T> node)
        {
            if (node == null || !_adjacents.Contains(node.Index)) return false;

            node.Adjacents.Remove(Index);

            onUnbind?.Invoke(this, node);

            return _adjacents.Remove(node.Index);
        }
    }
}