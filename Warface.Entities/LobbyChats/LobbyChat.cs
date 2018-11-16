using System;
using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.LobbyChats
{
    public class LobbyChat
    {
        public LobbyChatType Type { get; }

        public string Id { get; }

        public string ServiceId { get; }

        public string Conference => $"{Id}@{ServiceId}";

        public LobbyChat(LobbyChatType type, string id, string serviceId)
        {
            Type      = type;
            Id        = id;
            ServiceId = serviceId;
        }

        public HtmlNode GetAsNode()
        {
            return HtmlNode.CreateNode($"<lobbychat_getchannelid channel='{(int) Type}' channel_id='{Id}' service_id='{ServiceId}'/>");
        }

        public static LobbyChat ParseNode(HtmlNode lobbyChatGetChannelIdNode)
        {
            /*<lobbychat_getchannelid channel='0' channel_id='global.pvp_pro_032_r1' service_id='conference.warface'></lobbychat_getchannelid>*/

            var    type      = (LobbyChatType) lobbyChatGetChannelIdNode.Attributes["channel"].IntValue();
            string channelId = lobbyChatGetChannelIdNode.Attributes["channel_id"].Value;
            string serviceId = lobbyChatGetChannelIdNode.Attributes["service_id"].Value;

            return new LobbyChat(type, channelId, serviceId);
        }

        public static LobbyChat ParseConference(string conference)
        {
            //clan.1584972@conference.warface

            var    splitConference = conference.Split('@');
            string channelId       = splitConference[0];
            string serviceId       = splitConference[1];

            var           splitChannelId = channelId.Split('.');
            string        typeStr        = splitChannelId[0];
            LobbyChatType type;
            switch (typeStr)
            {
                case "global":
                    type = LobbyChatType.Global;
                    break;
                case "room":
                    type = LobbyChatType.Room;
                    break;
                case "team":
                    type = LobbyChatType.Team;
                    break;
                case "clan":
                    type = LobbyChatType.Clan;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return new LobbyChat(type, channelId, serviceId);
        }
    }
}