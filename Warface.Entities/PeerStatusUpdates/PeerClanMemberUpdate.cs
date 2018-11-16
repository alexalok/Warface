using HtmlAgilityExtended;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities.PeerStatusUpdates
{
    public class PeerClanMemberUpdate : PeerStatusUpdate
    {
        public int      ClanPoints { get; }
        public ClanRole Role       { get; }

        public PeerClanMemberUpdate(string nickname, int profileID, PlayerStatus status, int experience, string placeToken, string placeInfoToken, string modeInfoToken, string missionInfoToken, int clanPoints, ClanRole role) : base(nickname, profileID, status, experience, placeToken, placeInfoToken, modeInfoToken, missionInfoToken)
        {
            ClanPoints = clanPoints;
            Role       = role;
        }

        public new static PeerClanMemberUpdate ParseNode(HtmlNode peerClanMemberUpdateNode)
        {
            //<peer_clan_member_update nickname='ТНЕ.СеРДИТ' profile_id='9069477' status='0'
            //place_token='' place_info_token='' mode_info_token='' mission_info_token='' experience='8785924'
            //clan_points='0' clan_role='3'/>

            var peerStatusUpdate = PeerStatusUpdate.ParseNode(peerClanMemberUpdateNode);
            int clanPoints       = peerClanMemberUpdateNode.Attributes["clan_points"].IntValue();
            var clanRole         = (ClanRole) peerClanMemberUpdateNode.Attributes["clan_role"].IntValue();
            return new PeerClanMemberUpdate(peerStatusUpdate.Nickname, peerStatusUpdate.ProfileID, peerStatusUpdate.Status, peerStatusUpdate.Experience, peerStatusUpdate.PlaceToken, peerStatusUpdate.PlaceInfoToken, peerStatusUpdate.ModeInfoToken, peerStatusUpdate.MissionInfoToken, clanPoints, clanRole);
        }
    }
}