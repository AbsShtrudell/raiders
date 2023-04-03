using Raiders.Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raiders.AI;

namespace Raiders
{
    [RequireComponent(typeof(BuildingsGraphEditor))]
    public class LevelController : MonoBehaviour, IBuildingQueueHandler
    {
        private Graph<Building> _graph;
        private Dictionary<Side, SideController> _sideControllers = new Dictionary<Side, SideController>();
        private bool _isUpdateStoped = false;

        [SerializeField]
        private float _incomeFrequency;
        [SerializeField]
        private List<Side> _sides;

        public Dictionary<Side, SideController> SideControllers => _sideControllers;

        private void Awake()
        {
            BuildingsGraphEditor graphEditor = GetComponent<BuildingsGraphEditor>();
            SideAI sideAI = GetComponent<SideAI>();

            _graph = graphEditor.graph;

            foreach (var node in _graph.Nodes)
                node.Value.BuildingQueueHandler = this;

            sideAI._buildings = _graph;

            InitSideControllers();
        }

        private void OnEnable()
        {
            StartCoroutine(BuildingsUpdateTimer());
        }

        private void OnDisable()
        {
            _isUpdateStoped = true;
        }

        private void InitSideControllers()
        {
            foreach (Side side in _sides)
            {
                _sideControllers.Add(side, new SideController());
            }
        }

        private IEnumerator BuildingsUpdateTimer()
        {
            while (_isUpdateStoped != true)
            {
                yield return new WaitForSeconds(_incomeFrequency);

                foreach (var sideController in _sideControllers)
                {
                    foreach (Node<Building> building in _graph.Nodes)
                        if (building.Value.Side == sideController.Key)
                            sideController.Value.AddCoins((uint)building.Value.BuildingImp.BuildingData.Income);

                    foreach (Node<Building> building in _graph.Nodes)
                        if (building.Value.Side == sideController.Key)
                            sideController.Value.SpendCoins((uint)building.Value.BuildingImp.BuildingData.Upkeep);
                }
            }
        }

        //-------------------- IBuildingQueue -----------------------

        public void Upgrade(IBuildingData buildingData, bool free, Building notifyer) 
        {
            SideController sideController = _sideControllers[notifyer.Side];

            if (sideController == null) return;

            if (!free)
                if (sideController.CanSpendCoins(buildingData.Cost))
                    sideController.SpendCoins(buildingData.Cost);
                else return;

            notifyer.ChangeBuilding(buildingData);
        }

        public Path<Building> GetPath(Building target, Building notifyer)
        {
            return _graph.PathAlgorithm.FindPath(_graph.Find(notifyer), _graph.Find(target), _graph);
        }

        public void SquadSent(Building destination, SquadTypeInfo squadTypeInfo, Building notifyer)
        {
            throw new System.NotImplementedException();
        }
    }
}