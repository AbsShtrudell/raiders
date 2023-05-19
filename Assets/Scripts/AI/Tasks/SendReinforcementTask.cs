namespace Raiders.AI.Tasks
{
    public class SendReinforcementTask : ITask
    {
        private readonly Building _target;
        private readonly WeightedGraph _weightedGraph;

        public Priority Priority => Priority.VeryHigh;
        public int Delay => 5;

        public SendReinforcementTask(Building target, WeightedGraph weightedGraph)
        {
            _target = target;
            _weightedGraph = weightedGraph;
        }

        public bool Solve()
        {
            var findNearestTask = new FindNearestAllyBuildingCanHelpTask(_target, _weightedGraph);

            if(!findNearestTask.Solve()) return false;

            var sendSquad = new SendSquadTask(findNearestTask.Candidate, _target);

            if (!sendSquad.Solve()) return false;

            return true;
        }
    }
}
