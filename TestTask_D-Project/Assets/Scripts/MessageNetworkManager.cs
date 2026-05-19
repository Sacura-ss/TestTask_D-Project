using Mirror;
using UnityEngine;

public class MessageNetworkManager : NetworkManager
{
    private readonly HelloMessage _message = new()
    {
        Text = "Hello Client!"
    };

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

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        conn.Send(_message);
    }

    private void OnHelloMessageReceived(HelloMessage message)
    {
        Debug.Log($"CLIENT GET MESSAGE FROM SERVER: '{message.Text}'");
    }
}