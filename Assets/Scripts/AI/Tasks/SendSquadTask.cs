using Raiders.AI.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiders.AI.Tasks
{
    public class SendSquadTask : ITask
    {
        private readonly Building _start;
        private readonly Building _targer;

        public SendSquadTask(Building start, Building targer)
        {
            _start = start;
            _targer = targer;
        }

        public Priority Priority => Priority.Medium;
        public int Delay => 0;

        public bool Solve()
        {
            return _start.SendTroops(_targer);
        }
    }
}
