using Raiders.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiders.Graphs
{
    public interface IReadOnlyGraph<T>
    {
        public List<Node<T>> Nodes { get; }

        public Node<T> Find(int id);
        public Node<T> Find(T value);
        public IEnumerable<Node<T>> GetAdjacents(Node<T> node);
        public IEnumerable<Node<T>> GetAdjacents(int id);
    }
}
