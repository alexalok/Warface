namespace Warface.XMPP
{
    public enum WarfaceProtoPacketType
    {
        Plain     = 0,
        Encrypted = 1,
        ClientKey = 2,
        ServerKey = 3,
        ClientAck = 4,
    }
}