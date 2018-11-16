using HtmlAgilityExtended;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Warface.Entities.PeerStatusUpdates
{
    public class PeerStatusUpdate
    {
        public string       Nickname   { get; }
        public int          ProfileID  { get; }
        public PlayerStatus Status     { get; }
        public int          Experience { get; }

        [CanBeNull]
        public string PlaceToken { get; }

        [CanBeNull]
        public string PlaceInfoToken { get; }

        [CanBeNull]
        public string ModeInfoToken { get; }

        [CanBeNull]
        public string MissionInfoToken { get; }

        public PeerStatusUpdate(string nickname,   int    profileID,      PlayerStatus status,        int    experience,
                                string placeToken, string placeInfoToken, string       modeInfoToken, string missionInfoToken)
        {
            Nickname         = nickname;
            ProfileID        = profileID;
            Status           = status;
            Experience       = experience;
            PlaceToken       = string.IsNullOrWhiteSpace(placeToken) ? null : placeToken;
            PlaceInfoToken   = string.IsNullOrWhiteSpace(placeInfoToken) ? null : placeInfoToken;
            ModeInfoToken    = string.IsNullOrWhiteSpace(modeInfoToken) ? null : modeInfoToken;
            MissionInfoToken = string.IsNullOrWhiteSpace(missionInfoToken) ? null : missionInfoToken;
        }

        public static PeerStatusUpdate ParseNode(HtmlNode peerStatusUpdateNode)
        {
            /* <peer_status_update nickname='ЧАфроАмериканец2' profile_id='14879968' status='9' experience='0'
             * place_token='@ui_playerinfo_inlobby' place_info_token='' mode_info_token='' mission_info_token=''/> */

            string nickname         = peerStatusUpdateNode.Attributes["nickname"].Value;
            int    profileId        = peerStatusUpdateNode.Attributes["profile_id"].IntValue();
            var    status           = (PlayerStatus) peerStatusUpdateNode.Attributes["status"].IntValue();
            int    experience       = peerStatusUpdateNode.Attributes["experience"].IntValue();
            string placeToken       = peerStatusUpdateNode.Attributes["place_token"].Value;
            string placeInfoToken   = peerStatusUpdateNode.Attributes["place_info_token"].Value;
            string modeInfoToken    = peerStatusUpdateNode.Attributes["mode_info_token"].Value;
            string missionInfoToken = peerStatusUpdateNode.Attributes["mission_info_token"].Value;

            return new PeerStatusUpdate(nickname, profileId, status, experience, placeToken, placeInfoToken, modeInfoToken, missionInfoToken);
        }
    }
}