using System.Collections.Generic;
using System.Linq;
using aDevLib;
using HtmlAgilityPack;
using Warface.Entities.Profiles;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomCore
    {
        public bool IsPrivate { get; }

        public List<RoomPlayer> Players { get; }

        public bool CanStart { get; }

        public int Revision { get; }

        public GameroomCore(bool isPrivate, List<RoomPlayer> players, bool canStart, int revision)
        {
            IsPrivate = isPrivate;
            Players   = players;
            CanStart  = canStart;
            Revision  = revision;
        }


        public static GameroomCore ParseNode(HtmlNode coreNode)
        {
            /*<core teams_switched='0' room_name='xueplet&apos;s GAME ROOM' private='0'
                players='10' can_start='1' team_balanced='1' min_ready_players='4' revision='235'>*/

            bool             canStart    = coreNode.Attributes["can_start"].BoolValue();
            bool             isPrivate   = coreNode.Attributes["private"].BoolValue();
            int              revision    = coreNode.Attributes["revision"].IntValue();
            var              playerNodes = coreNode.SelectNodes("./players/player");
            var players     = new List<RoomPlayer>();
            if (playerNodes != null)
                players = playerNodes.Select(RoomPlayer.ParsePlayerNode).ToList();

            return new GameroomCore(isPrivate, players, canStart, revision);
        }
    }
}