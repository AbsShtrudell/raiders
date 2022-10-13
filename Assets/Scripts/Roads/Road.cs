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
        if (_ends == null || _ends.Length != 2) return;

        SplinePoint[] points = new SplinePoint[2];

        for(int i = 0; i < 2; i++)
        {
            points[i] = new SplinePoint(_ends[i].Value.transform.position);
        }

        _splineComputer.SetPoints(points);
    }

    public void DestroySelf()
    {
        _ends[0].Value.OnDisabled -= DestroySelf;
        _ends[1].Value.OnDisabled -= DestroySelf;

        //if (_meshCreator?.MeshHolder) DestroyImmediate(_meshCreator.MeshHolder);
        if (gameObject) DestroyImmediate(gameObject);
    }

    public void DestroySelfOnUnbind(Node<Building> b1, Node<Building> b2)
    {
        if (_ends[0] == b1 && _ends[1] == b2 ||
            _ends[1] == b1 && _ends[0] == b2)
        {
            DestroySelf();
        }
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) //Was Deleted
        {
            if (_ends.Length != 0)
            {
            }
        }
        else //Was Cleaned Up on Scene Closure
        {
            
        }
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
