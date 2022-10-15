using UnityEngine;
using Dreamteck.Splines;
using Graphs;

[ExecuteAlways]
[RequireComponent(typeof(SplineComputer), typeof(PathGenerator))]
public class Road : MonoBehaviour
{
    [SerializeField]//, HideInInspector]
    private Node<Building>[] _ends;
    private SplineComputer _splineComputer;

    public Node<Building>[] Ends => _ends;
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
        if (!IsConnected()) return;

        SplinePoint[] points = new SplinePoint[2];

        for(int i = 0; i < 2; i++)
        {
            points[i] = new SplinePoint(_ends[i].Value.transform.position);
        }

        _splineComputer.SetPoints(points);
    }

    public bool HasConnectionWith(int index)
    {
        return _ends != null && _ends.Length == 2 && (_ends[0].Index == index || _ends[1].Index == index);
    }

    public bool IsConnected()
    {
        return _ends != null && _ends.Length == 2 && _ends[0].Value != null && _ends[1].Value != null;
    }

    private void OnDestroy()
    {
        
    }

    public class Factory : Zenject.IFactory<Node<Building>, Node<Building>, Road>
    {
        private Road _visuals;

        public Factory(Road visuals) => _visuals = visuals;

        public Road Create(Node<Building> param1, Node<Building> param2)
        {
            Road road = GameObject.Instantiate(_visuals.gameObject).GetComponent<Road>();
            road._ends = new Node<Building>[] { param1, param2 };
            return road;
        }
    }
}
