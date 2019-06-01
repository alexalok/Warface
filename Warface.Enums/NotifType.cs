using System;

namespace Warface.Enums
{
    [Flags]
    public enum NotifType
    {
        MissionPerf      = 1 << 1,
        Achievement      = 1 << 2,
        CustomMessage    = 1 << 3,
        ClanInvite       = 1 << 4,
        ClanInviteResult = 1 << 5,
        FriendRequest    = 1 << 6,
        InviteResult     = 1 << 7,
        GiveItem         = 1 << 8,
        Announcement     = 1 << 9,
        Contract         = 1 << 10,
        GiveMoney        = 1 << 11,
        ItemUnvailable   = 1 << 12,
        GiveRandomBox    = 1 << 13,
        Unknown14        = 1 << 14,
        ItemUnlocked     = 1 << 15,
        RepairResult     = 1 << 16,
        NewRank          = 1 << 17,
        Message          = 1 << 18,
        Unknown19        = 1 << 19,
        UnlockMission    = 1 << 20,
        DeletedItem      = 1 << 21,
        ProfileBan       = 1 << 22,
        Unknown23        = 1 << 23,
        SessionReward    = 1 << 24,
        RatingPlayerInfo = 1 << 25,
    };
}