using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using Dreamteck.Splines;

[ExecuteAlways]
[RequireComponent(typeof(SplineComputer), typeof(PathGenerator))]
public class Road : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Building[] _ends;
    private SplineComputer _splineComputer;

    public Building[] Ends => _ends;
    public SplineComputer PathCreator => _splineComputer;

    private void Awake()
    {
        _splineComputer = GetComponent<SplineComputer>();
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        if (_ends == null || _ends.Length != 2) return;

        SplinePoint[] points = new SplinePoint[2];

        for(int i = 0; i < 2; i++)
        {
            points[i] = new SplinePoint(_ends[i].transform.position);
        }

        _splineComputer.SetPoints(points);
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
