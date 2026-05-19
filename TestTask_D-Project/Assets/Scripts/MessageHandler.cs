using System;
using Messages;
using Mirror;
using UnityEngine;
using Zenject;

public class MessageHandler : NetworkBehaviour
{
    public event Action<string> OnHelloMessageReceivedEvent;

    private MessageNetworkManager _networkManager;

    private readonly HelloMessage _helloMessage = new()
    {
        Text = "Hello Client!"
    };

    [Inject]
    private void Construct(MessageNetworkManager networkManager)
    {
        _networkManager = networkManager;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkClient.RegisterHandler<HelloMessage>(OnHelloMessageReceived);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        NetworkClient.UnregisterHandler<HelloMessage>();
    }

    private void OnHelloMessageReceived(HelloMessage message)
    {
        Debug.Log(message.Text);

        OnHelloMessageReceivedEvent?.Invoke(message.Text);
    }

    public void SendHelloMessage()
    {
        _networkManager.SendHelloMessageToSubscribers(_helloMessage);
    }

    public void SendSubscribeHelloMessage(bool isSubscribe)
    {
        var message = new SubscribeMessage()
            { MessageType = typeof(HelloMessage).AssemblyQualifiedName, IsSubscribe = isSubscribe };
        NetworkClient.Send(message);
    }
}