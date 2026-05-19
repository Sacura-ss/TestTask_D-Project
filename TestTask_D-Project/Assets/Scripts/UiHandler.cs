using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UiHandler : NetworkBehaviour
{
    private MessageNetworkManager _networkManager;
    
    [Header("Client Panel")] [SerializeField]
    private GameObject _clientPanelObject;

    [SerializeField] private Button _subscribeButton;
    [SerializeField] private Button _unsubscribeButton;

    [Header("Server Panel")] [SerializeField]
    private GameObject _serverPanelObject;

    [SerializeField] private Button _sendMessageButton;

    [Header("Common Panel")] [SerializeField]
    private TMP_Text _log;

    [Inject]
    private void Construct(MessageNetworkManager networkManager)
    {
        _networkManager = networkManager;
    }

    private void Awake()
    {
        _clientPanelObject.gameObject.SetActive(false);
        _serverPanelObject.gameObject.SetActive(false);

        _log.text = "";
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        _serverPanelObject.gameObject.SetActive(true);

        _sendMessageButton.onClick.AddListener(SendMessage);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        _serverPanelObject.gameObject.SetActive(false);

        _sendMessageButton.onClick.RemoveListener(SendMessage);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        _clientPanelObject.gameObject.SetActive(true);
        SetSubscribeButtonsState(false);

        _subscribeButton.onClick.AddListener(Subscribe);
        _unsubscribeButton.onClick.AddListener(Unsubscribe);

        NetworkClient.RegisterHandler<HelloMessage>(UpdateLog);

    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        _clientPanelObject.gameObject.SetActive(false);

        _subscribeButton.onClick.RemoveListener(Subscribe);
        _unsubscribeButton.onClick.RemoveListener(Unsubscribe);
        
        NetworkClient.UnregisterHandler<HelloMessage>();

    }

    private void Subscribe()
    {
        //
        if (!NetworkClient.isConnected) return;

        SetSubscribeButtonsState(true);

        var message = new SubscribeMessage() { IsSubscribe = true };
        NetworkClient.Send(message);
    }

    private void Unsubscribe()
    {
        //
        if (!NetworkClient.isConnected) return;

        SetSubscribeButtonsState(false);

        var message = new SubscribeMessage() { IsSubscribe = false };
        NetworkClient.Send(message);

        _log.text = "";
    }

    private void SetSubscribeButtonsState(bool isSubscribed)
    {
        _subscribeButton.gameObject.SetActive(!isSubscribed);
        _unsubscribeButton.gameObject.SetActive(isSubscribed);
    }

    private void SendMessage()
    {
        _networkManager.SendHelloMessageToSubscribers();
    }

    private void UpdateLog(HelloMessage message)
    {
        Debug.Log("UpdateLog");
        _log.text = message.Text;
    }
}

