using UnityEngine;
using PathCreation;
using PathCreation.Examples;

[RequireComponent(typeof(PathCreator), typeof(RoadMeshCreator))]
public class Road : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Building[] _ends;
    private PathCreator _pathCreator;

    public Building[] Ends => _ends;
    public PathCreator PathCreator => _pathCreator;

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
        if (_ends == null || _ends.Length != 2) return;

        Transform[] points = { _ends[0].transform, _ends[1].transform };

        BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
        _pathCreator.bezierPath = bezierPath;
        _pathCreator.bezierPath.Space = PathSpace.xz;
    }

    public class Factory : Zenject.IFactory<Building, Building, Road>
    {
        private Road _visuals;

        public Factory(Road visuals) => _visuals = visuals;

        public Road Create(Building param1, Building param2)
        {
            Road road = GameObject.Instantiate(_visuals.gameObject).GetComponent<Road>();
            road._ends = new Building[] { param1, param2 };
            return road;
        }
    }
}
