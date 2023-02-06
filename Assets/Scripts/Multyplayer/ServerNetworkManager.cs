using Riptide.Utils;
using Riptide;
using UnityEngine;

public class ServerNetworkManager : MonoBehaviour
{
    private static ServerNetworkManager _instance;

    public static ServerNetworkManager Instance
    {
        get => _instance;
        private set
        {
            if(_instance == null)
                _instance = value;
            else if (_instance != value)
            {
                Debug.Log($"{nameof(ServerNetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set; }

    [SerializeField]
    private ushort port;
    [SerializeField]
    private ushort maxClientCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
    }

    private void FixedUpdate()
    {
        Server.Update();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
}
