using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using Zenject;

[RequireComponent(typeof(PathCreator), typeof(RoadMeshCreator))]
public class Road : MonoBehaviour
{
    private Building[] _ends;
    private PathCreator _pathCreator;

    [Inject]
    public void Construct(Building end1, Building end2)
    {
        _ends = new Building[] {end1, end2};
    }

    private void Awake()
    {
        _pathCreator = GetComponent<PathCreator>();
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        if (_ends == null || _ends.Length < 2) return;

        Transform[] points = { _ends[0].transform, _ends[1].transform };

        BezierPath bezierPath = new BezierPath(points, false, PathSpace.xz);
        _pathCreator.bezierPath = bezierPath;
        _pathCreator.bezierPath.Space = PathSpace.xyz;
    }

    public class Factory : PlaceholderFactory<Building, Building, Road>
    {

    }
}
