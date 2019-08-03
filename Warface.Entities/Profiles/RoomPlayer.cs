using System;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities.Profiles
{
    public class RoomPlayer
    {
        public int UserId => Convert.ToInt32(OnlineId.Split('@')[0]); // 21531320@warface/GameClient

        public long ProfileId { get; }

        public Team TeamId { get; }

        public GameroomPlayerStatus Status { get; }

        public bool Observer { get; }

        public float Skill { get; }

        public string Nickname { get; }

        public string ClanName { get; }

        public Class ClassId { get; }

        public string OnlineId { get; } // 21531320@warface/GameClient

        public string GroupId { get; }

        public PlayerStatus Presense { get; }

        public int Experience { get; }

        public int Rank { get; }

        public string BannerBadge { get; }

        public string BannerMark { get; }

        public string BannerStripe { get; }

        public string RegionId { get; }

        public RoomPlayer(
            long profileId, Team teamId, GameroomPlayerStatus status, bool observer, float skill, string nickname,
            string clanName, Class classId, string onlineId, string groupId, PlayerStatus presense, int experience,
            int rank, string bannerBadge, string bannerMark, string bannerStripe, string regionId)
        {
            ProfileId = profileId;
            TeamId = teamId;
            Status = status;
            Observer = observer;
            Skill = skill;
            Nickname = nickname;
            ClanName = clanName;
            ClassId = classId;
            OnlineId = onlineId;
            GroupId = groupId;
            Presense = presense;
            Experience = experience;
            Rank = rank;
            BannerBadge = bannerBadge;
            BannerMark = bannerMark;
            BannerStripe = bannerStripe;
            RegionId = regionId;
        }

        public static RoomPlayer ParsePlayerNode(HtmlNode playerNode)
        {
            //<player profile_id='444155' team_id='1' status='0' observer='0' skill='80.000' nickname='coordinator1337' clanName='Jackass' 
            //class_id='2' online_id='18660405@warface/GameClient' group_id='' presence='17' experience='17364000' rank='80' 
            //banner_badge='4294967295' banner_mark='4294967295' banner_stripe='4294967295' region_id='global'/>

            long profileId = playerNode.Attributes["profile_id"].LongValue();
            var teamId = (Team)playerNode.Attributes["team_id"].IntValue();
            var status = (GameroomPlayerStatus)playerNode.Attributes["status"].IntValue();
            bool observer = playerNode.Attributes["observer"].BoolValue();
            float skill = playerNode.Attributes["skill"].FloatValue();
            string nickname = playerNode.Attributes["nickname"].Value;
            string clanName = playerNode.Attributes["clanName"].Value;
            var classId = (Class)playerNode.Attributes["class_id"].IntValue();
            string onlineId = playerNode.Attributes["online_id"].Value;
            string groupId = playerNode.Attributes["group_id"].Value;
            var presence = (PlayerStatus)playerNode.Attributes["presence"].IntValue();
            int experience = playerNode.Attributes["experience"].IntValue();
            int rank = playerNode.Attributes["rank"].IntValue();
            string bannerBadge = playerNode.Attributes["banner_badge"].Value;
            string bannerMark = playerNode.Attributes["banner_mark"].Value;
            string bannerStripe = playerNode.Attributes["banner_stripe"].Value;
            string regionId = playerNode.Attributes["region_id"].Value;

            return new RoomPlayer(profileId, teamId, status, observer, skill, nickname, clanName, classId,
                onlineId, groupId, presence, experience, rank, bannerBadge, bannerMark, bannerStripe, regionId);
        }
    }
}