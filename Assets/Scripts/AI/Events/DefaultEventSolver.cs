using Raiders.AI.Tasks;
using System;

namespace Raiders.AI.Events
{
    public class DefaultEventSolver : IEventSolver
    {
        private readonly WeightedGraph _weightedGraph;

        public DefaultEventSolver(WeightedGraph weightedGraph)
        {
            _weightedGraph = weightedGraph;
        }

        public ITask forAllySquadSent(Building origin, Building target, SquadTypeInfo squadInfo)
        {
            throw new NotImplementedException();
        }

        public ITask forEnemySquadSent(Building origin, Building target, SquadTypeInfo squadInfo)
        {
            return new SendReinforcementTask(target, _weightedGraph);
        }
    }
}
