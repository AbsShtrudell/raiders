using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

[System.Serializable]
public class FollowerBehavior
{
    protected PathCreator path;
    [SerializeField] protected Transform transform;
    [SerializeField] protected float _distanceTravelled = 0f;
    [SerializeField] protected EndOfPathInstruction _endOfPathInstruction;

    public float distanceTravelled => _distanceTravelled;
    public EndOfPathInstruction endOfPathInstruction => _endOfPathInstruction;    

    public FollowerBehavior(PathCreator vertexPath, Transform transform, float distanceTravelled, EndOfPathInstruction endOfPathInstruction)
    {
        path = vertexPath;
        this.transform = transform;
        _distanceTravelled = distanceTravelled;
        _endOfPathInstruction = endOfPathInstruction;
    }

    public virtual void Move()
    {
        var newPos = path.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(newPos.x - transform.position.x);
        transform.localScale = scale;

        transform.position = newPos;
    }
}

public class PrimaryFollowerBehavior : FollowerBehavior
{
    [SerializeField] protected float _speed = 5f;

    public float speed => _speed;

    public PrimaryFollowerBehavior(PathCreator vertexPath, Transform transform, float speed, float distanceTravelled, EndOfPathInstruction endOfPathInstruction)
        : base(vertexPath, transform, distanceTravelled, endOfPathInstruction)
    {
        _speed = speed;
    }

    public override void Move()
    {
        _distanceTravelled += speed * Time.deltaTime;

        base.Move();
    }

    public void SetSpeed(float s)
    {
        _speed = s;
    }
}

public class SecondaryFollowerBehavior : FollowerBehavior
{
    [SerializeField] private PrimaryFollowerBehavior primaryFollower;
    [SerializeField] private float distanceFromPrimary;

    public SecondaryFollowerBehavior(PathCreator vertexPath, Transform transform, float distanceTravelled, EndOfPathInstruction endOfPathInstruction, PrimaryFollowerBehavior primaryFollower, float distance)
        : base(vertexPath, transform, distanceTravelled, endOfPathInstruction)
    {
        this.primaryFollower = primaryFollower;
        distanceFromPrimary = distance;
    }

    public override void Move()
    {
        float diff = primaryFollower.distanceTravelled - distanceTravelled;

        if (Mathf.Abs(diff) > distanceFromPrimary)
            _distanceTravelled = primaryFollower.distanceTravelled - distanceFromPrimary * Mathf.Sign(diff);

        base.Move();
    }

    public void SetDistance(float d)
    {
        distanceFromPrimary = d;
    }
}

public class TestPathFollower : MonoBehaviour
{
    [SerializeReference] private FollowerBehavior _behavior = null;
    
    public FollowerBehavior behavior => _behavior;

    private void Update()
    {
        _behavior?.Move();
    }
    
    public void MakePrimary(PathCreator path, float speed, float distanceTravelled, EndOfPathInstruction endOfPathInstruction)
    {
        _behavior = new PrimaryFollowerBehavior(path, transform, speed, distanceTravelled, endOfPathInstruction);
    }

    public void MakeSecondary(PathCreator path, PrimaryFollowerBehavior primaryFollower, float distanceTravelled, EndOfPathInstruction endOfPathInstruction, float distance)
    {
        _behavior = new SecondaryFollowerBehavior(path, transform, distanceTravelled, endOfPathInstruction, primaryFollower, distance);
    }
}
