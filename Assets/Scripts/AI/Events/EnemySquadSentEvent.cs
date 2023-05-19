using Raiders.AI.Tasks;

namespace Raiders.AI.Events
{
    public class EnemySquadSentEvent : IEvent
    {
        private Building _target;
        private Building _origin;
        private SquadTypeInfo _squadInfo;

        public EnemySquadSentEvent(Building origin, Building target,SquadTypeInfo squadInfo)
        {
            _origin = origin;
            _target = target;
            _squadInfo = squadInfo;
        }

        public ITask Solve(IEventSolver solver)
        {
            return solver.forEnemySquadSent(_origin, _target, _squadInfo);
        }
    }
}
