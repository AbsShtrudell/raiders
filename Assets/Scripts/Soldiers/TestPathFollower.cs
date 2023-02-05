using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

namespace Raiders
{
    [System.Serializable]
    abstract public class FollowerBehavior
    {
        abstract public SplineComputer path { get; protected set; }

        [SerializeField] protected Transform transform;
        [SerializeField] protected float _distanceTravelled = 0f;

        public float distanceTravelled => _distanceTravelled;

        public event Action ReachedDestination;

        public FollowerBehavior(Transform transform)
        {
            this.transform = transform;
        }

        public void OnReachedDestination()
        {
            ReachedDestination?.Invoke();
        }

        public virtual void Move()
        {
            var newPos = path.EvaluatePosition(_distanceTravelled);

            newPos.y += transform.localScale.y;

            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(newPos.x - transform.position.x);
            transform.localScale = scale;


            transform.position = newPos;
        }
    }

    public class PrimaryFollowerBehavior : FollowerBehavior
    {
        public override SplineComputer path { get; protected set; }
        [SerializeField] protected float _speed = 5f;
        protected Queue<Tuple<SplineComputer, Squad.Direction>> _paths;
        protected Squad.Direction _direction;
        protected float _destination = 1f;

        public float speed => _speed;

        public PrimaryFollowerBehavior(Queue<Tuple<SplineComputer, Squad.Direction>> paths, Transform transform, float speed)
            : base(transform)
        {
            _paths = paths;
            _speed = speed / 100f;

            CalculateCurrentPath();
        }

        private void CalculateCurrentPath()
        {
            var tuple = _paths.Dequeue();
            path = tuple.Item1;
            _direction = tuple.Item2;

            _distanceTravelled = _direction == Squad.Direction.Forward ? 0f : 1f;
            _destination = _direction == Squad.Direction.Forward ? 1f : 0f;
        }

        public override void Move()
        {
            if (_distanceTravelled == _destination)
            {
                if (_paths.Count != 0)
                {
                    CalculateCurrentPath();
                }
                else
                {
                    OnReachedDestination();
                }
            }

            _distanceTravelled = Mathf.MoveTowards(_distanceTravelled, _destination, Mathf.Abs(speed * Time.deltaTime));

            base.Move();
        }

        public void SetSpeed(float s)
        {
            _speed = s / 100f;
        }
    }

    public class SecondaryFollowerBehavior : FollowerBehavior
    {
        [SerializeField] private PrimaryFollowerBehavior primaryFollower;
        [SerializeField] private float distanceFromPrimary;
        [SerializeField] private float _x;

        public override SplineComputer path { get => primaryFollower.path; protected set { } }

        public SecondaryFollowerBehavior(Transform transform, PrimaryFollowerBehavior primaryFollower, float distance, float x)
            : base(transform)
        {
            this.primaryFollower = primaryFollower;
            distanceFromPrimary = distance / 100f;
            _distanceTravelled = primaryFollower.distanceTravelled;
            _x = x;

            primaryFollower.ReachedDestination += () => { OnReachedDestination(); };
            base.Move();
        }

        public override void Move()
        {

            float diff = primaryFollower.distanceTravelled - _distanceTravelled;

            if (Mathf.Abs(diff) > distanceFromPrimary)
            {
                _distanceTravelled = primaryFollower.distanceTravelled - distanceFromPrimary * Mathf.Sign(diff);

                base.Move();

                float tempPoint = _distanceTravelled + 0.1f;
                if (tempPoint > 1f)
                    tempPoint -= 0.2f;
                Vector3 temp = path.EvaluatePosition(tempPoint);
                Vector3 normal = Vector3.Cross(temp - transform.position, Vector3.up).normalized;

                transform.position += _x * normal;
            }
        }

        public void SetDistance(float d)
        {
            distanceFromPrimary = d / 100f;
        }

        public void SetXDistance(float x)
        {
            _x = x;
        }
    }

    public class TestPathFollower : MonoBehaviour
    {
        [SerializeReference] private FollowerBehavior _behavior = null;

        [HideInInspector]
        public FollowerBehavior behavior => _behavior;

        private void Update()
        {
            _behavior?.Move();
        }

        public PrimaryFollowerBehavior MakePrimary(Queue<Tuple<SplineComputer, Squad.Direction>> paths, float speed)
        {
            _behavior = new PrimaryFollowerBehavior(paths, transform, speed);
            _behavior.ReachedDestination += () => { Destroy(this.gameObject); };
            return behavior as PrimaryFollowerBehavior;
        }

        public SecondaryFollowerBehavior MakeSecondary(PrimaryFollowerBehavior primaryFollower, float distance, float x)
        {
            _behavior = new SecondaryFollowerBehavior(transform, primaryFollower, distance, x);
            _behavior.ReachedDestination += () => { Destroy(this.gameObject); };
            return behavior as SecondaryFollowerBehavior;
        }
    }
}