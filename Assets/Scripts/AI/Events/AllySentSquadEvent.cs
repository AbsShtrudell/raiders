using Raiders.AI.Tasks;

namespace Raiders.AI.Events
{
    internal class AllySquadSentEvent
    {
        private Building _target;
        private Building _origin;
        private SquadTypeInfo _squadInfo;

        public AllySquadSentEvent(Building origin, Building target, SquadTypeInfo squadInfo)
        {
            _origin = origin;
            _target = target;
            _squadInfo = squadInfo;
        }

        public ITask Solve(IEventSolver solver)
        {
            return solver.forAllySquadSent(_origin, _target, _squadInfo);
        }
    }
}
