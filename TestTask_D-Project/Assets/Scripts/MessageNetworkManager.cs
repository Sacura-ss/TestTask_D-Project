using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MessageNetworkManager : NetworkManager
{
    private readonly HashSet<NetworkConnectionToClient> _subscribers = new();

    private readonly HelloMessage _message = new()
    {
        Text = "Hello Client!"
    };

    public void SendHelloMessageToSubscribers()
    {
        //
        if (!NetworkServer.active) return;
        
        var subscribersCopy = new List<NetworkConnectionToClient>(_subscribers);
        foreach (var connectionToClient in subscribersCopy)
        {
            // 
            if (connectionToClient != null && connectionToClient.isReady)
            {
                connectionToClient.Send(_message);
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        Debug.Log("OnStartServer");
        
        NetworkServer.RegisterHandler<SubscribeMessage>(OnSubscribeMessageReceived);
    }

    public override void OnStopServer()
    {
        Debug.Log("OnStopServer");
        
        _subscribers.Clear();
        NetworkServer.UnregisterHandler<SubscribeMessage>();
        
        base.OnStopServer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        Debug.Log("OnStartClient");
        
        //NetworkClient.RegisterHandler<HelloMessage>(OnHelloMessageReceived);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        
        Debug.Log("OnStopClient");
        
        //NetworkClient.UnregisterHandler<HelloMessage>();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient connectionToClient)
    {
        _subscribers.Remove(connectionToClient);
        base.OnServerDisconnect(connectionToClient);
    }

    private void OnSubscribeMessageReceived(NetworkConnectionToClient connectionToClient, SubscribeMessage message)
    {
        if (message.IsSubscribe)
        {
            Debug.Log("Subscribe Client");
            _subscribers.Add(connectionToClient);
        }
        else
        {
            Debug.Log("Unsubscribe Client");
            _subscribers.Remove(connectionToClient);
        }
    }

    // private void OnHelloMessageReceived(HelloMessage message)
    // {
    //     Debug.Log(message.Text);
    // }
}