using System;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using Warface.Entities.PeerStatusUpdates;
using Warface.Enums;

namespace Warface.Entities.Profiles
{
    public class ClanMember : Profile

    {
        //<clan_member_info nickname='гарик00200' profile_id='823131' experience='23046000'
        //clan_points='161' invite_date='1530293779' clan_role='3' jid='242033587@warface/GameClient' status='33'/>

        public int            ClanPoints { get; private set; }
        public DateTimeOffset InviteDate { get; }
        public ClanRole       ClanRole   { get; private set; }

        public ClanMember(string nickname, int profileID, int experience, int clanPoints, DateTimeOffset inviteDate, ClanRole clanRole, string jid, PlayerStatus status) :
            base(nickname, profileID, jid, status, experience)
        {
            ClanPoints = clanPoints;
            InviteDate = inviteDate;
            ClanRole   = clanRole;
        }

        public static ClanMember ParseNode(HtmlNode clanMemberInfoNode)
        {
            string nickname   = clanMemberInfoNode.Attributes["nickname"].Value;
            int    profileID  = clanMemberInfoNode.Attributes["profile_id"].IntValue();
            int    experience = clanMemberInfoNode.Attributes["experience"].IntValue();
            int    clanPoints = clanMemberInfoNode.Attributes["clan_points"].IntValue();
            var    inviteDate = DateTimeOffset.FromUnixTimeSeconds(clanMemberInfoNode.Attributes["invite_date"].IntValue());
            var    clanRole   = (ClanRole) clanMemberInfoNode.Attributes["clan_role"].IntValue();
            string jid        = clanMemberInfoNode.Attributes["jid"].Value;
            var    status     = (PlayerStatus) clanMemberInfoNode.Attributes["status"].IntValue();

            return new ClanMember(nickname, profileID, experience, clanPoints,
                inviteDate, clanRole, jid, status);
        }

        public void Update(ClanMember newClanMemberEntry)
        {
            Experience = newClanMemberEntry.Experience;
            ClanPoints = newClanMemberEntry.ClanPoints;
            ClanRole   = newClanMemberEntry.ClanRole;
            JID        = newClanMemberEntry.JID;
            Status     = newClanMemberEntry.Status;
        }

        public void Update(PeerClanMemberUpdate update, string jid)
        {
            base.Update(update, jid);
            ClanPoints = update.ClanPoints;
            ClanRole   = update.Role;
        }
    }
}