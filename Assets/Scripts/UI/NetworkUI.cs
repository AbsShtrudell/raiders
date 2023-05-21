using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Raiders
{
    public class NetworkUI : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private AddresUIController addresUIController;
        private bool client = false;
        public void Awake()
        {

            addresUIController.onBack += Show;

            hostButton.onClick.AddListener(() =>
            {
                client = false;
                Hide();
                addresUIController.onApply += StartHost;
            });
            clientButton.onClick.AddListener(() =>
            {
                client = true;
                Hide();
                addresUIController.onApply += StartClient;
                
            });
        }

        public void Show()
        {
            addresUIController.Hide();
            gameObject.SetActive(true);

            if(!client)
                addresUIController.onApply -= StartHost;
            else
                addresUIController.onApply -= StartClient;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            addresUIController.Show();
        }

        public void StartHost(string ip, int port)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ip;
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)port;

            NetworkManager.Singleton.OnServerStarted += () =>
            {
                gameObject.SetActive(false);
                NetworkManager.Singleton.OnClientConnectedCallback += (f) =>
                {
                    if (NetworkManager.Singleton.ConnectedClients.Count > 1)
                        NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
                };
            };
            NetworkManager.Singleton.StartHost();
        }

        public void StartClient(string ip, int port)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ip;
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)port;

            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.OnClientStarted += () =>
            {
                gameObject.SetActive(false);
            };
        }

    }
}
