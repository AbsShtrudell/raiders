using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raiders.Graphs;
using System.Linq;
using System.Text;
using Raiders.Util.Collections;
using System;

namespace Raiders.AI
{
    public class WeightedGraph
    { 
        private readonly IGraph<Building> _originGraph;
        private Dictionary<int,WeightedBuilding> _buildingMap = new Dictionary<int, WeightedBuilding>();

        public IReadOnlyGraph<Building> OriginGraph => _originGraph;

        public WeightedGraph(IGraph<Building> graph)
        {
            _originGraph = graph;

            foreach (var node in _originGraph.Nodes)
            {
                WeightedBuilding building = new WeightedBuilding();
                _buildingMap.Add(node.Index, building);

                node.Value.TryGetComponent<BuildingWeightVisualizer>(out var weightVisualizer);
           
                if(weightVisualizer != null)
                    building.Changed += weightVisualizer.ChangeText;
            }
        }

        public WeightedBuilding GetWeights(Building building)
        {
            var node = OriginGraph.Find(building);

            return _buildingMap.GetValueOrDefault(node.Index);
        }

        public WeightedBuilding GetWeights(int index)
        {
            return _buildingMap.GetValueOrDefault(index);
        }

        public IEnumerable<WeightedBuilding> GetNeighbourWeights(Building building)
        {
            var node = OriginGraph.Find(building);

            if (node == null) return Enumerable.Empty<WeightedBuilding>();

            var list = new List<WeightedBuilding>();

            foreach(var neighbour in OriginGraph.GetAdjacents(node))
            {
                _buildingMap.TryGetValue(neighbour.Index, out var weight);

                if(weight != null)
                    list.Add(weight);
            }

            return list;
        }

        public void ProcessGraph()
        {
            CalculateNodeNativeWeights();
        }

        private void CalculateNodeNativeWeights()
        {
            foreach(var node in OriginGraph.Nodes)
            {
                _buildingMap.TryGetValue(node.Index, out var weight);

                if (weight == null) continue;

                var data = node.Value.BuildingImp.BuildingData;
                var slots = node.Value.BuildingImp.SlotList;

                weight.CalculateWeights(data, slots, null);
            }
        }
    }

    public class WeightedBuilding
    {
        public Side Side { get; protected set; }
        public int DefencePotential { get; protected set; }
        public int AttackPotential { get; protected set; }
        public int CurrentDefence { get; protected set; }
        public int CurrentAttack { get; protected set; }
        public int DistanceToEnemy { get; protected set; }
        public int AttackIncoming { get; protected set; }
        public int ReinforcementIncoming { get; protected set; }
        public int SquadsCount { get { return CurrentAttack; } }

        public event Action<WeightedBuilding> Changed;

        public void NotifyChanges()
        {
            Changed?.Invoke(this);
        }

        public void CalculateWeights(IBuildingData data, IReadOnlySlotList slotList, List<Pair<int, WeightedBuilding>> neighbours)
        {
            DefencePotential = CalculateDefencePotential(data, neighbours);
            AttackPotential = CalculateAttackPotential(data, neighbours);
            CurrentDefence = CalculateCurrentAttack(data, slotList, neighbours);
            CurrentAttack = CalculateCurrentAttack(data, slotList, neighbours);

            Changed?.Invoke(this);
        }

        public int CalculateDefencePotential(IBuildingData data, List<Pair<int,WeightedBuilding>> neighbours)
        {
            int returnValue = data.DefenseMultyplier * data.SquadSlots;
            return returnValue;
        }

        public int CalculateAttackPotential(IBuildingData data, List<Pair<int, WeightedBuilding>> neighbours)
        {
            int returnValue = data.SquadTypeInfo.Damage * data.SquadSlots;
            return returnValue;
        }

        public int CalculateCurrentDefence(IBuildingData data, IReadOnlySlotList slots, List<Pair<int, WeightedBuilding>> neighbours)
        {
            int returnValue = ((int)slots.GeneralProgress + slots.ExtraSlotsCount) * data.DefenseMultyplier;
            return returnValue;
        }
        public int CalculateCurrentAttack(IBuildingData data, IReadOnlySlotList slots, List<Pair<int, WeightedBuilding>> neighbours)
        {
            int returnValue = ((int)slots.GeneralProgress + slots.ExtraSlotsCount) * data.SquadTypeInfo.Damage;
            return returnValue;
        }

        public override string ToString()
        {
            return string.Format("Side: {0}\n DefencePotential: {1}\nAttackPotential: {2}\nCurrentDefence: {3}\nCurrentAttack: {4}\nDistancToEnemy: {5}", 
                Side, DefencePotential, AttackPotential, CurrentDefence, CurrentAttack, DistanceToEnemy);

        }



        public bool IsInRisk()
        {
            return false;
        }
    }
}
