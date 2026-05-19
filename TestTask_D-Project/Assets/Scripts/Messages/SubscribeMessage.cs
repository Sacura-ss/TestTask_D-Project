using Mirror;

namespace Messages
{
    public struct SubscribeMessage : NetworkMessage
    {
        public string MessageType;
        public bool IsSubscribe;
    }
}