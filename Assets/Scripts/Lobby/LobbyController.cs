using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class LobbyController : NetworkBehaviour
    {
        [SerializeField] private RectTransform _playersHolder;
        [SerializeField] private Transform playerLobbyPrefab;

        private List<PlayerLobbyController> playerLobbyControllers = new List<PlayerLobbyController>();


        private void Start()
        {
            GetComponent<NetworkObject>().Spawn();
        }

        public void Ready()
        {
            if (!IsHost)
                ReadyServerRpc();
            else
                Ready(0);
        }

        private void Ready(int i)
        {
            if (!IsHost)
            {

            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ReadyServerRpc()
        {
            Ready(1);
        }
        
    }
}
