using System.Collections.Generic;
using Mirror;

public class SubscribeManager
{
    private static readonly Dictionary<string, HashSet<NetworkConnectionToClient>> _subscriptions = new();

    public void Subscribe(string messageType, NetworkConnectionToClient connectionToClient)
    {
        if (!_subscriptions.ContainsKey(messageType))
            _subscriptions[messageType] = new HashSet<NetworkConnectionToClient>();
        _subscriptions[messageType].Add(connectionToClient);
    }

    public void Unsubscribe(string messageType, NetworkConnectionToClient connectionToClient)
    {
        if (_subscriptions.TryGetValue(messageType, out var set))
            set.Remove(connectionToClient);
    }

    public void UnsubscribeAll(NetworkConnectionToClient connectionToClient)
    {
        foreach (var set in _subscriptions.Values)
            set.Remove(connectionToClient);
    }

    public void SendToSubscribers<T>(T message) where T : struct, NetworkMessage
    {
        string typeName = typeof(T).AssemblyQualifiedName;
        if (typeName != null && _subscriptions.TryGetValue(typeName, out var set))
        {
            var copy = new List<NetworkConnectionToClient>(set);
            foreach (var connectionToClient in copy)
            {
                if (connectionToClient != null && connectionToClient.isReady)
                    connectionToClient.Send(message);
            }
        }
        
       
    }
}