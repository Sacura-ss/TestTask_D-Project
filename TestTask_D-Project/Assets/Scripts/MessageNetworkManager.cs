using Messages;
using Mirror;
using UnityEngine;
using Zenject;

public class MessageNetworkManager : NetworkManager
{
    private SubscribeManager _subscribeManager;

    [Inject]
    private void Construct(SubscribeManager subscribeManager)
    {
        _subscribeManager = subscribeManager;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        NetworkServer.RegisterHandler<SubscribeMessage>(OnSubscribeMessageReceived);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<SubscribeMessage>();

        base.OnStopServer();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient connectionToClient)
    {
        _subscribeManager.UnsubscribeAll(connectionToClient);
        base.OnServerDisconnect(connectionToClient);
    }

    private void OnSubscribeMessageReceived(NetworkConnectionToClient connectionToClient, SubscribeMessage message)
    {
        if (message.IsSubscribe)
        {
            Debug.Log("Subscribe Client");

            _subscribeManager.Subscribe(message.MessageType, connectionToClient);
        }
        else
        {
            Debug.Log("Unsubscribe Client");

            _subscribeManager.Unsubscribe(message.MessageType, connectionToClient);
        }
    }
    
    public void SendHelloMessageToSubscribers<T>(T message) where T : struct, NetworkMessage
    {
        if (!NetworkServer.active) return;

        _subscribeManager.SendToSubscribers(message);
    }
}