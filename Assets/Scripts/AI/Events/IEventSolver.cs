using Raiders.AI.Tasks;

namespace Raiders.AI.Events
{
    public interface IEventSolver
    {
        public ITask forEnemySquadSent(Building origin, Building target, SquadTypeInfo squadInfo);
        public ITask forAllySquadSent(Building origin, Building target, SquadTypeInfo squadInfo);
    }
}
