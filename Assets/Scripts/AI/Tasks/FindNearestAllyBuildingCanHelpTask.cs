using ModestTree;
using Raiders.Graphs;
using System.Collections.Generic;

namespace Raiders.AI.Tasks
{
    public class FindNearestAllyBuildingCanHelpTask : ITask
    {
        private readonly Building _target;
        private readonly WeightedGraph _weightedGraph;
        private int deapthLimit = 2;

        public Building Candidate { get; protected set; }

        public Priority Priority => Priority.Medium;
        public int Delay => 0;

        public FindNearestAllyBuildingCanHelpTask(Building target, WeightedGraph weightedGraph)
        {
            _target = target;
            _weightedGraph = weightedGraph;
        }

        public bool Solve()
        {
            ISet<int> visited = new HashSet<int>();

            Queue<int> queue = new Queue<int>();

            Node<Building> candidate = null;
            int candidateDistance = 0;

            int current = _weightedGraph.OriginGraph.Find(_target).Index;
            int deapth = 0;

            visited.Add(current);
            queue.Enqueue(current);

            while(!queue.IsEmpty())
            {
                if (deapth == deapthLimit)
                    break;

                current = queue.Dequeue();

                foreach (var node in _weightedGraph.OriginGraph.GetAdjacents(current))
                {
                    if (!visited.Contains(node.Index) && _target.Side == node.Value.Side)
                    {
                        visited.Add(node.Index);
                        queue.Enqueue(node.Index);

                        var weights = _weightedGraph.GetWeights(node.Index);

                        if(!weights.IsInRisk() )
                        {
                            if (candidate == null) {
                                candidate = node;
                                candidateDistance = deapth;
                            }
                            else
                            {
                                if (candidateDistance !< deapth && _weightedGraph.GetWeights(candidate.Index).SquadsCount < weights.SquadsCount)
                                {
                                    candidate = node;
                                    candidateDistance = deapth;
                                }
                            }
                        }
                    }
                }

                deapth++;
            }

            Candidate = candidate?.Value;

            return candidate != null;
        }
    }
}
