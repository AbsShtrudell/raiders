using Raiders.Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raiders.AI;
using Raiders.AI.Events;
using Unity.Netcode;
using Raiders.Util.Collections;
using System;

namespace Raiders
{
    [RequireComponent(typeof(BuildingsGraphEditor))]
    public class LevelController : NetworkBehaviour, IBuildingQueueHandler
    {
        public struct NetworkData : INetworkSerializable
        {
            SideMoney[] _sides;

            public SideMoney[] SideMoney => _sides;

            public NetworkData(List<SideMoney> sideMoney)
            {
                _sides = sideMoney.ToArray();
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                int length = 0;

                if (!serializer.IsReader)
                {
                    length = _sides.Length;
                }

                serializer.SerializeValue(ref length);

                if (serializer.IsReader)
                {
                    _sides = new SideMoney[length];
                }

                for (int n = 0; n < length; ++n)
                {
                    if(serializer.IsReader)
                        _sides[n] = new SideMoney();

                    _sides[n].NetworkSerialize(serializer);
                }
            }
        }
        public struct SideMoney : INetworkSerializable
        {
            private Side _side;
            private int _amount;

            public Side Side => _side;
            public int Money => _amount;

            public SideMoney(Side side, int amount)
            {
                _side = side;
                _amount = amount;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref _side);
                serializer.SerializeValue(ref _amount);
            }
        }

        private Graph<Building> _graph;
        private Dictionary<Side, SideController> _sideControllers = new Dictionary<Side, SideController>();
        private bool _isUpdateStoped = false;

        [SerializeField]
        private float _incomeFrequency;
        [SerializeField]
        private List<Side> _sides;

        public Dictionary<Side, SideController> SideControllers => _sideControllers;

        public event Action<Side> OnGameEnd;

        private void Awake()
        {
            BuildingsGraphEditor graphEditor = GetComponent<BuildingsGraphEditor>();

            _graph = graphEditor.graph;

            foreach (var node in _graph.Nodes)
            {
                node.Value.BuildingQueueHandler = this;

                node.Value.OnSideChanged += DecideWinner;
            }

            if(IsHost)
                GetComponent<NetworkObject>().Spawn();

            InitSideControllers();
        }

        private void Start()
        {
            if(IsHost)
                StartCoroutine(BuildingsUpdateTimer());
        }

        private void OnDisable()
        {
            _isUpdateStoped = true;
        }

        private void Update()
        {
            if (!IsHost) return;

            List<SideMoney> money = new List<SideMoney>();
            
            foreach (var side in _sideControllers)
                money.Add(new SideMoney(side.Key, side.Value.Coins));

            NetworkData data = new NetworkData(money);

            UpdateMoneyClientRpc(data);
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
                            sideController.Value.AddCoins(building.Value.BuildingImp.BuildingData.Income);

                    foreach (Node<Building> building in _graph.Nodes)
                        if (building.Value.Side == sideController.Key)
                            sideController.Value.SpendCoins(building.Value.BuildingImp.BuildingData.Upkeep);
                }
            }
        }

        private void DecideWinner()
        {
            int viking = 0;
            int english = 0;
            foreach (var node in _graph.Nodes)
            {
                if (node.Value.Side == Side.Vikings)
                    viking++;
                else if (node.Value.Side == Side.English)
                    english++;
            }

            if (viking == 0)
                OnGameEnd?.Invoke(Side.English);
            else if (english == 0)
                OnGameEnd?.Invoke(Side.Vikings);
        }

        //-------------------- IBuildingQueue -----------------------

        public bool Upgrade(IBuildingData buildingData, bool free, Building notifyer) 
        {
            SideController sideController = _sideControllers[notifyer.Side];

            if (sideController == null) return false;

            if (!free)
                if (sideController.CanSpendCoins(buildingData.Cost))
                    sideController.SpendCoins(buildingData.Cost);
                else return false;

            notifyer.ChangeBuilding(buildingData);
            return true;
        }

        public Path<Building> GetPath(Building target, Building notifyer)
        {
            return _graph.PathAlgorithm.FindPath(_graph.Find(notifyer), _graph.Find(target), _graph);
        }

        public void SquadSent(Building destination, SquadTypeInfo squadTypeInfo, Building notifyer)
        {
            //_sideAi.RecieveEvent(new EnemySquadSentEvent(notifyer, destination, squadTypeInfo));
        }

        [ClientRpc]
        public void UpdateMoneyClientRpc(NetworkData networkData)
        {
            foreach(var side in networkData.SideMoney)
            {
                _sideControllers.TryGetValue(side.Side, out SideController controller);

                controller.Coins = side.Money;
            }
        }
    }
}