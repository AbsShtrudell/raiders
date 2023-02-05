namespace Raiders.Graphs
{
    public interface IPathAlgorithm<T>
    {
        public Path<T> FindPath(Node<T> sourceNode, Node<T> targetNode, Graph<Building> graph);
    }
}
