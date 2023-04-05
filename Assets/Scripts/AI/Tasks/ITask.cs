namespace Raiders.AI.Tasks
{
    public interface ITask
    {
        Priority Priority { get; }
        int Delay { get; }

        public bool Solve( ); 
    }
}
