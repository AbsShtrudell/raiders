using ModestTree;
using Raiders.AI.Events;
using Raiders.AI.Tasks;
using Raiders.Graphs;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.AI
{
    public class SideAI : MonoBehaviour
    {
        public Graph<Building> _buildings;
        public SideController sideController;

        private WeightedGraph _weightedGraph;

        //side controller

        private Queue<IEvent> _eventsQueue = new Queue<IEvent>();
        private PriorityQueue<>
        private IEventSolver _eventSolver;

        [SerializeField]
        private Side _side = Side.English;

        public Side Side { get => _side; set => _side = value; }

        private void Start()
        {
            if(_buildings != null )
                _weightedGraph = new WeightedGraph(_buildings);

            _eventSolver = new DefaultEventSolver(_weightedGraph);
        }

        private void Update()
        {
            _weightedGraph?.ProcessGraph();

            while(!_eventsQueue.IsEmpty())
            {
                _eventsQueue.Dequeue().Solve(_eventSolver).Solve();
            }
        }

        public void RecieveEvent(IEvent evennt)
        {
            if (evennt == null) return;
              
            _eventsQueue.Enqueue(evennt);
        }
    }
}
