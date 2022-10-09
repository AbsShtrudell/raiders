using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using Graphs;

[ExecuteInEditMode, RequireComponent(typeof(PathCreator), typeof(RoadMeshCreator))]
public class Road : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Node<Building>[] _ends;
    private PathCreator _pathCreator;
    private RoadMeshCreator _meshCreator;

    public Node<Building>[] Ends => _ends;
    public PathCreator PathCreator => _pathCreator;

    private void Awake()
    {
        _pathCreator = GetComponent<PathCreator>();
        _meshCreator = GetComponent<RoadMeshCreator>();
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        if (_ends == null || _ends.Length != 2) return;

        Transform[] points = { _ends[0].Value.transform, _ends[1].Value.transform };

        BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
        _pathCreator.bezierPath = bezierPath;
        _pathCreator.bezierPath.Space = PathSpace.xz;
    }

    public void DestroySelf()
    {
        _ends[0].Value.OnDisabled -= DestroySelf;
        _ends[1].Value.OnDisabled -= DestroySelf;
        _ends[0].onUnbind -= DestroySelfOnUnbind;
        _ends[1].onUnbind -= DestroySelfOnUnbind;

        if (_meshCreator?.MeshHolder) DestroyImmediate(_meshCreator.MeshHolder);
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
