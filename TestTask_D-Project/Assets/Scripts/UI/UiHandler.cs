using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UiHandler : NetworkBehaviour
    {
        private MessageHandler _messageHandler;
        
        [Header("Client Panel")] 
        [SerializeField] private GameObject _clientPanelObject;
        [SerializeField] private Button _subscribeButton;
        [SerializeField] private Button _unsubscribeButton;

        [Header("Server Panel")] 
        [SerializeField] private GameObject _serverPanelObject;
        [SerializeField] private Button _sendMessageButton;
        
        [Header("Common Panel")] 
        [SerializeField] private TMP_Text _log;

        [Inject]
        private void Construct(MessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }
        
        private void Awake()
        {
            _clientPanelObject.gameObject.SetActive(false);
            _serverPanelObject.gameObject.SetActive(false);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            _serverPanelObject.gameObject.SetActive(true);

            _sendMessageButton.onClick.AddListener(OnClickSendMessage);
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            _serverPanelObject.gameObject.SetActive(false);

            _sendMessageButton.onClick.RemoveListener(OnClickSendMessage);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            _messageHandler.OnHelloMessageReceivedEvent += UpdateLogText;

            _clientPanelObject.gameObject.SetActive(true);
            UpdateUI(false);

            _subscribeButton.onClick.AddListener(SubscribeHelloMessage);
            _unsubscribeButton.onClick.AddListener(UnsubscribeHelloMessage);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            
            _messageHandler.OnHelloMessageReceivedEvent -= UpdateLogText;

            _clientPanelObject.gameObject.SetActive(false);

            _subscribeButton.onClick.RemoveListener(SubscribeHelloMessage);
            _unsubscribeButton.onClick.RemoveListener(UnsubscribeHelloMessage);
        }

        private void SubscribeHelloMessage()
        {
            if (!NetworkClient.isConnected) return;
            
            _messageHandler.SendSubscribeHelloMessage(true);
            
            UpdateUI(true);
        }

        private void UnsubscribeHelloMessage()
        {
            if (!NetworkClient.isConnected) return;

            _messageHandler.SendSubscribeHelloMessage(false);
            
            UpdateUI(false);
            UpdateLogText("");
        }

        private void UpdateUI(bool isSubscribed)
        {
            _subscribeButton.gameObject.SetActive(!isSubscribed);
            _unsubscribeButton.gameObject.SetActive(isSubscribed);
        }
        
        private void UpdateLogText(string text)
        {
            _log.text = text;
        }

        private void OnClickSendMessage()
        {
            _messageHandler.SendHelloMessage();
        }
    }
}