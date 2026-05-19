using UnityEngine;
using Zenject;

namespace Installers
{
    public class MessageInstaller : MonoInstaller
    {
        [SerializeField] private MessageNetworkManager _networkManager;
        [SerializeField] private MessageHandler _messageHandler;

        public override void InstallBindings()
        {
            Container.Bind<MessageNetworkManager>().FromInstance(_networkManager).AsSingle();
            Container.Bind<MessageHandler>().FromInstance(_messageHandler).AsSingle();
            Container.Bind<SubscribeManager>().AsSingle();
        }
    }
}