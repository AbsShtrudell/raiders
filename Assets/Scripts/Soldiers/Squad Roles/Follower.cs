using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class Follower : SquadRole
    {
        private Coroutine _followRoutine;
        private float _defaultSpeed;
        private float _defaultAcceleration;

        public Soldier leader { get; set; }
        public Vector3 columnPosition { get; set; }

        public Follower(Soldier soldier, Squad squad) : base(soldier, squad)
        {
            _defaultSpeed = _agent.speed;
            _defaultAcceleration  = _agent.acceleration;
        }

        public override void Move()
        {
            if (_followRoutine != null)
                soldier.StopCoroutine(_followRoutine);

            _followRoutine = soldier.StartCoroutine(Following());
        }


        private IEnumerator Following()
        {
            yield return new WaitUntil(() => leader.hasPath);
            yield return new WaitWhile(() => leader.pathPending);

            while (leader.hasPath)
            {
                float angle = Vector3.SignedAngle(Vector3.forward, leader.direction, Vector3.up);

                var delta = Quaternion.Euler(0, angle, 0) * columnPosition;

                var destination = leader.transform.position - delta;
                _agent.SetDestination(destination);

                if (Vector3.Distance(soldier.transform.position, destination) > _agent.stoppingDistance * squad.stoppingDistanceModifier)
                {
                    _agent.speed = _defaultSpeed * squad.followingSpeedModifier;
                    _agent.acceleration = _defaultAcceleration * squad.followingAccelerationModifier;
                }
                else
                {
                    _agent.speed = _defaultSpeed;
                    _agent.acceleration = _defaultAcceleration;
                }

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitWhile(() => _agent.hasPath);

            soldier.direction = leader.direction;
            soldier.LookTowardDirection();
        }
    }
}
