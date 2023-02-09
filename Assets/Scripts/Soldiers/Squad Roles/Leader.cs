using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class Leader : SquadRole
    {
        public Vector3 currentDestination { get; set; }

        public Leader(Soldier soldier, Squad squad) : base(soldier, squad)
        {
        }

        public override void Move()
        {
            _agent.SetDestination(currentDestination);
        }
    }
}
