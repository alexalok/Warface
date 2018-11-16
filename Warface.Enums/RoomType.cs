namespace Warface.Enums
{
    public enum RoomType
    {
        PvePrivate   = 1 << 0, //1
        PvpPublic    = 1 << 1, //2
        PvpClanwar   = 1 << 2, //4
        PvpQuickplay = 1 << 3, //8
        PveQuickplay = 1 << 4, //16
        PvpRating    = 1 << 5, //32
    };
}