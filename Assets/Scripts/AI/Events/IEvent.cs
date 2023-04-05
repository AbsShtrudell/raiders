using Raiders.AI.Tasks;

namespace Raiders.AI.Events
{
    public interface IEvent
    {
        public ITask Solve(IEventSolver solver);
    }
}
