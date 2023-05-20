using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Raiders
{
    public class NetworkUI : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        public void Awake()
        {

            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.OnServerStarted += () =>
                {
                    gameObject.SetActive(false);
                    NetworkManager.Singleton.OnClientConnectedCallback += (f) =>
                    {
                        if(NetworkManager.Singleton.ConnectedClients.Count > 1)
                            NetworkManager.Singleton.SceneManager.LoadScene("BasicGameplayTest", LoadSceneMode.Single);
                    };
                };
                NetworkManager.Singleton.StartHost();
            });

            clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
                NetworkManager.Singleton.OnClientStarted += () =>
                {
                    gameObject.SetActive(false);
                };
            });
        }
    }
}
