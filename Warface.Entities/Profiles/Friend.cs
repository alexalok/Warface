using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.Profiles
{
    public class Friend : Profile
    {
        //<friend jid='24166912@warface/GameClient' profile_id='431348' nickname='FeelMyBoobie' status='1' experience='4731607' location=''/>
        //<friend jid='' profile_id='16655813' nickname='ЮхухушечкаБраво' status='0' experience='59387' location=''>

        public string Location { get; private set; }


        public Friend(string nickname, int profileID, string jid, PlayerStatus status, int exp) :
            base(nickname, profileID, jid, status, exp)
        {
        }

        public Friend(string nickname,   int    profileID, string jid, PlayerStatus status,
                      int    experience, string location) : base(nickname, profileID, jid, status, experience)
        {
            Location = location;
        }

        public string GetListEntryString()
        {
            return $"<friend jid='{JID ?? string.Empty}' profile_id='{ProfileID}' nickname='{Nickname}' status='{(int) Status}' experience='{Experience}' location='{Location}'/>";
        }

        public void Update(Friend friendUpdate)
        {
            JID        = friendUpdate.JID;
            Status     = friendUpdate.Status;
            Experience = friendUpdate.Experience;
            Location   = friendUpdate.Location;
        }

        public static Friend ParseNode(HtmlNode friendNode)
        {
            string jid        = friendNode.Attributes["jid"].Value;
            int    profileId  = friendNode.Attributes["profile_id"].IntValue();
            string nickname   = friendNode.Attributes["nickname"].Value;
            var    status     = (PlayerStatus) friendNode.Attributes["status"].IntValue();
            int    experience = friendNode.Attributes["experience"].IntValue();
            string location   = friendNode.Attributes["location"].Value;
            var    friend     = new Friend(nickname, profileId, jid, status, experience, location);
            return friend;
        }
    }
}