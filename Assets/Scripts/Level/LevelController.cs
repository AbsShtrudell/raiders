using Raiders.Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    [RequireComponent(typeof(BuildingsGraphEditor))]
    public class LevelController : MonoBehaviour
    {
        private Graph<Building> _graph;
        private List<Road> _roads;
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

            _graph = graphEditor.graph;
            _roads = graphEditor.roads;

            InitSideControllers();
        }

        private void OnEnable()
        {
            StartCoroutine(BuildingsUpdateTimer());

            foreach (Node<Building> building in _graph.Nodes)
            {
                building.Value.UpgradeQueue += UpgradeBuilding;
            }
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
                    Debug.Log("Update coins for: " + sideController.Key.ToString());

                    foreach (Node<Building> building in _graph.Nodes)
                        if (building.Value.Side == sideController.Key)
                            sideController.Value.AddCoins((uint)building.Value.BuildingImp.BuildingData.Income);

                    foreach (Node<Building> building in _graph.Nodes)
                        if (building.Value.Side == sideController.Key)
                            sideController.Value.SpendCoins((uint)building.Value.BuildingImp.BuildingData.Upkeep);
                }
            }
        }

        private void UpgradeBuilding(IBuildingData data, bool free, Building building)
        {
            SideController sideController = _sideControllers[building.Side];

            if (sideController == null) return;

            if (!free)
                if (sideController.CanSpendCoins(data.Cost))
                    sideController.SpendCoins(data.Cost);
                else return;

            building.ChangeBuilding(data);
        }
    }
}