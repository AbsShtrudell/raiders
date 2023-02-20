using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raiders.Graphs;

namespace Raiders
{
    public class AIWeightsGraph : Graph<WeightedBuilding>
    {
        public AIWeightsGraph() : base(null)
        {
        }

        public void ProcessGraph(Graph<Building> graph)
        {
            foreach(var building in graph.Nodes)
            {
                Nodes[building.Index].Value.DefencePotential = building.Value.BuildingImp.BuildingData.DefenseMultyplier * building.Value.BuildingImp.BuildingData.SquadSlots;
                Nodes[building.Index].Value.AttackPotential = building.Value.BuildingImp.BuildingData.SquadSlots * building.Value.BuildingImp.BuildingData.SquadTypeInfo.Damage;

                foreach (var adj in graph.GetAdjacents(building))
                {
                    Nodes[building.Index].Value.DefencePotential += adj.Value.BuildingImp.BuildingData.SquadSlots;
                    Nodes[building.Index].Value.AttackPotential += (adj.Value.BuildingImp.BuildingData.SquadSlots * building.Value.BuildingImp.BuildingData.SquadTypeInfo.Damage) / 2;

                }
            }
        }
    }

    public class WeightedBuilding
    {
        public Building Building { get; set; }
        public Side Side { get; set; }
        public int DefencePotential { get; set; }
        public int AttackPotential { get; set; }
        public int CurrentDefence { get; set; }
        public int CurrentAttack { get; set; }
        public int DistancToEnemy { get; set; }

    }
}
