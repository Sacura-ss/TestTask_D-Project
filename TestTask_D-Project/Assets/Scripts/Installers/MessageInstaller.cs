using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MessageInstaller : MonoInstaller
    {
        [SerializeField]  private MessageNetworkManager _networkManager;

        public override void InstallBindings()
        {
            Container.Bind<MessageNetworkManager>().FromInstance(_networkManager).AsSingle();
        }
    }
}