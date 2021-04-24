using aDevLib;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities
{
    public class GameroomQuickplay
    {
        public GameroomQuickplay(RoomType roomType, string roomName, string missionId, string gameMode, int status, Team teamId, Class classId, int missionsHash, int contentHash, int timestamp, string uid)
        {
            RoomType     = roomType;
            RoomName     = roomName;
            MissionId    = missionId;
            GameMode     = gameMode;
            Status       = status;
            TeamId       = teamId;
            ClassId      = classId;
            MissionsHash = missionsHash;
            ContentHash  = contentHash;
            Timestamp    = timestamp;
            Uid          = uid;
        }

        public RoomType RoomType { get; }

        public string RoomName { get; }

        public string MissionId { get; }

        public string GameMode { get; }

        public int Status { get; }

        public Team TeamId { get; }

        public Class ClassId { get; }

        public int MissionsHash { get; }

        public int ContentHash { get; }

        public int Timestamp { get; }

        public string Uid { get; }

        public static GameroomQuickplay ParseNode(HtmlNode gameroomQuickplayNode)
        {
            var    roomType     = (RoomType) gameroomQuickplayNode.Attributes["room_type"].IntValue();
            string roomName     = gameroomQuickplayNode.Attributes["room_name"].Value;
            string missionID    = gameroomQuickplayNode.Attributes["mission_id"].Value;
            string gameMode     = gameroomQuickplayNode.Attributes["game_mode"].Value;
            int    status       = gameroomQuickplayNode.Attributes["status"].IntValue();
            var    teamID       = (Team) gameroomQuickplayNode.Attributes["team_id"].IntValue();
            var    classID      = (Class) gameroomQuickplayNode.Attributes["class_id"].IntValue();
            int    missionsHash = gameroomQuickplayNode.Attributes["missions_hash"].IntValue();
            int    contentHash  = gameroomQuickplayNode.Attributes["content_hash"].IntValue();
            int    timestamp    = gameroomQuickplayNode.Attributes["timestamp"].IntValue();
            string uid          = gameroomQuickplayNode.Attributes["uid"].Value;

            return new GameroomQuickplay(roomType, roomName, missionID, gameMode, status, teamID, classID, missionsHash, contentHash, timestamp, uid);
        }

        /*
         * <gameroom_quickplay room_type='16' room_name='Комната игрока ТНЕ.СеРДИТ' mission_id='64cd1dbd-6457-44dc-971f-ccc73c19b603' game_mode='' status='0' team_id='0' class_id='4' missions_hash='-80550272' content_hash='609720826' timestamp='0' uid='abd4a69b-f25a-4bd4-ba8f-f3f1098fa4c4'/>
         */
    }
}