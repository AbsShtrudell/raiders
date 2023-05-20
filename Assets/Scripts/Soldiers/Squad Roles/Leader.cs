using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Raiders
{
    public class Leader : SquadRole
    {
        private Coroutine _movingRoutine;

        public Vector3 currentDestination { get; set; }

        public event System.Action ReachedDestination;

        public Leader(Soldier soldier, Squad squad) : base(soldier, squad)
        {
        }

        public override void Move()
        {            
            if (_movingRoutine != null)
                soldier.StopCoroutine(_movingRoutine);                
            
            if (squad.isSplineMovement)
                _movingRoutine = soldier.StartCoroutine(Moving());
            else
                _agent.SetDestination(currentDestination);

        }

        private IEnumerator Moving()
        {
            foreach (var (spline, direction) in squad.Roads)
            {
                int i, inc, target;
                if (direction == Squad.Direction.Forward)
                {
                    i = 0;
                    inc = 1;
                    target = spline.samples.Length;
                }
                else
                {
                    i = spline.samples.Length - 1;
                    inc = -1;
                    target = -1;
                }

                for (; i != target; i += inc)
                {
                    NavMesh.SamplePosition(spline.samples[i].position, out var hit, 100f, 1);

                    _agent.SetDestination(hit.position);

                    yield return new WaitUntil(() => Vector3.Distance(soldier.transform.position, hit.position) <= 1);
                }
            }

            ReachedDestination?.Invoke();

            //foreach (var sample in squad.spline.samples)
            //
            //   NavMesh.SamplePosition(sample.position, out var hit, 100f, 1);
            //
            //   _agent.SetDestination(hit.position);
            //
            //   yield return new WaitUntil(() => Vector3.Distance(soldier.transform.position, hit.position) <= 1);
            //
        }
    }
}
