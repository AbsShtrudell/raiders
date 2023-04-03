using Raiders.Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.AI
{
    public class SideAI : MonoBehaviour
    {
        public Graph<Building> _buildings;
        private WeightedGraph _weightedGraph;

        [SerializeField]
        private Side _side = Side.English;

        private void Start()
        {
            if(_buildings != null )
                _weightedGraph = new WeightedGraph(_buildings);
        }

        private void Update()
        {
            _weightedGraph?.ProcessGraph();
        }
    }
}
