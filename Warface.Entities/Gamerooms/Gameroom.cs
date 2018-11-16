using System;
using System.Linq;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Warface.Entities.Profiles;
using Warface.Enums;

namespace Warface.Entities.Gamerooms
{
    public class Gameroom
    {
        public string ID { get; }
        public string ChannelName { get; }
        public RoomType Type { get; }
        public bool IsPve => Type == RoomType.PveQuickplay || Type == RoomType.PvePrivate;

        [CanBeNull]
        public RoomPlayer? MasterPlayer
        {
            get
            {
                var master = Master?.Master;
                return Core?.Players.FirstOrDefault(p => p.ProfileId == master);
            }
        }

        [CanBeNull]
        public RoomPlayer? GetPlayer(long profileID) => Core?.Players.FirstOrDefault(p => p.ProfileId == profileID);

        [CanBeNull]
        public GameroomCore? Core { get; }

        [CanBeNull]
        public GameroomSession? Session { get; }

        [CanBeNull]
        public GameroomCustomParams? CustomParams { get; }

        [CanBeNull]
        public GameroomMission? Mission { get; }

        [CanBeNull]
        public GameroomAutostart? Autostart { get; }

        [CanBeNull]
        public GameroomRegions? Regions { get; }

        [CanBeNull]
        public GameroomMaster? Master { get; }

        Gameroom(string id, string channelName, RoomType type,
                 [CanBeNull] GameroomCore? core, [CanBeNull] GameroomSession? session, [CanBeNull] GameroomCustomParams? customParams, [CanBeNull] GameroomMission? mission,
                 [CanBeNull] GameroomAutostart? autostart, [CanBeNull] GameroomRegions? regions, [CanBeNull] GameroomMaster? master)
        {
            ID = id;
            Type = type;
            Core = core;
            Session = session;
            CustomParams = customParams;
            Mission = mission;
            Autostart = autostart;
            Regions = regions;
            Master = master;
            ChannelName = channelName;
        }

        public static Gameroom ParseNode(HtmlNode gameroomNode, string channelNameOrJid)
        {
            string id = gameroomNode.Attributes["room_id"].Value;
            var type = (RoomType) gameroomNode.Attributes["room_type"].IntValue();

            if (channelNameOrJid.Contains("@")) //jid is provided
                channelNameOrJid = channelNameOrJid.Split('/')[1]; //masterserver@warface/pve_010_r1

            var coreNode = gameroomNode.SelectSingleNode("./core");
            GameroomCore? core = null;
            if (coreNode != null)
                core = GameroomCore.ParseNode(coreNode);

            var sessionNode = gameroomNode.SelectSingleNode("./session");
            GameroomSession? session = null;
            if (sessionNode != null)
                session = GameroomSession.ParseNode(sessionNode);

            var customParamsNode = gameroomNode.SelectSingleNode("./custom_params");
            GameroomCustomParams? customParams = null;
            if (customParamsNode != null)
                customParams = GameroomCustomParams.ParseNode(customParamsNode);

            var missionNode = gameroomNode.SelectSingleNode("./mission");
            GameroomMission? mission = null;
            if (missionNode != null)
                mission = GameroomMission.ParseNode(missionNode);

            var roomMasterNode = gameroomNode.SelectSingleNode("./room_master");
            GameroomMaster? master = null;
            if (roomMasterNode != null)
                master = GameroomMaster.ParseNode(roomMasterNode);

            var regionsNode = gameroomNode.SelectSingleNode("./regions");
            GameroomRegions? regions = null;
            if (regionsNode != null)
                regions = GameroomRegions.ParseNode(regionsNode);

            var autostartNode = gameroomNode.SelectSingleNode("./auto_start");
            GameroomAutostart? autostart = null;
            if (autostartNode != null)
                autostart = GameroomAutostart.ParseNode(autostartNode);

            //TODO process clanwar node

            return new Gameroom(id, channelNameOrJid, type, core, session, customParams, mission, autostart, regions, master);
        }

        public static Gameroom Merge(Gameroom gameroom1, Gameroom gameroom2)
        {
            if (gameroom1.ID != gameroom2.ID)
                throw new ArgumentException();
            return new Gameroom(
                gameroom1.ID, gameroom1.ChannelName, gameroom2.Type,
                gameroom2.Core ?? gameroom1.Core,
                gameroom2.Session ?? gameroom1.Session,
                gameroom2.CustomParams ?? gameroom1.CustomParams,
                gameroom2.Mission ?? gameroom1.Mission,
                gameroom2.Autostart ?? gameroom1.Autostart,
                gameroom2.Regions ?? gameroom1.Regions,
                gameroom2.Master ?? gameroom1.Master
            );
        }
    }
}