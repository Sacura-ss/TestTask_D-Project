using Mirror;
using UnityEngine;

public class MessageNetworkManager : NetworkManager
{
    private readonly HelloMessage _message = new()
    {
        Text = "Hello Client!"
    };

    public override void OnStartServer()
    {
        Debug.Log("OnStartServer");
        base.OnStartServer();
        NetworkServer.OnConnectedEvent += OnClientConnected;
    }

    public override void OnStopServer()
    {
        Debug.Log("OnStopServer");
        base.OnStopServer();
        NetworkServer.OnConnectedEvent -= OnClientConnected;
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");
        base.OnStartClient();
        NetworkClient.RegisterHandler<HelloMessage>(OnHelloMessageReceived);
    }

    public override void OnStopClient()
    {
        Debug.Log("OnStopClient");
        base.OnStopClient();
        NetworkClient.UnregisterHandler<HelloMessage>();
    }

    private void OnClientConnected(NetworkConnectionToClient connectionToClient)
    {
        connectionToClient.Send(_message);
    }

    private void OnHelloMessageReceived(HelloMessage message)
    {
        Debug.Log($"CLIENT GET MESSAGE FROM SERVER: '{message.Text}'");
    }
}