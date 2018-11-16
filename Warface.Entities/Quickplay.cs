using System.Collections.Generic;
using JetBrains.Annotations;
using Warface.Entities.Missions;
using Warface.Entities.Profiles;
using Warface.Enums;

namespace Warface.Entities
{
    public class Quickplay
    {
        public string Uid { get; }

        public string MasterJid { get; }

        public RoomType RoomType { get; }

        [CanBeNull]
        public GameMode? GameMode { get; }

        [CanBeNull]
        public Mission Mission { get; }

        public List<ProfileInfo> InvitedPlayers { get; }

        public bool IsSearching { get; set; }

        public Quickplay(string uid, string masterJid, RoomType roomType, GameMode? gameMode, Mission mission)
        {
            Uid            = uid;       //used when inviting, starting, cancelling and on_started, on_canceled events
            MasterJid      = masterJid; //used in IsMaster check and during p2p sends
            RoomType       = roomType;  //used when starting and inviting
            GameMode       = gameMode;  //used when starting and inviting
            Mission        = mission;   //used when starting and inviting
            InvitedPlayers = new List<ProfileInfo>();
        }
    }
}