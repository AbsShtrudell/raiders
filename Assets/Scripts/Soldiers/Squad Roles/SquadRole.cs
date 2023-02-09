using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Raiders
{
    public abstract class SquadRole
    {
        protected NavMeshAgent _agent;

        public Soldier soldier { get; set; }
        public Squad squad { get; set; }

        public SquadRole(Soldier soldier, Squad squad)
        {
            this.soldier = soldier;
            this.squad = squad;

            _agent = soldier.GetComponent<NavMeshAgent>();
        }

        public abstract void Move();
    }
}
