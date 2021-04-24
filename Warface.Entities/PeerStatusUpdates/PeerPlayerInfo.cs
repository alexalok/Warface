using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using aDevLib;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities.PeerStatusUpdates
{
    public class PeerPlayerInfo
    {
        readonly string _clanName;
        readonly ClanRole _clanRole;
        readonly int _clanPosition;
        readonly int _clanPoints;
        readonly DateTimeOffset _clanMemberSince;
        public string OnlineId { get; }
        public string Nickname { get; }
        public string PrimaryWeapon { get; }
        public string PrimaryWeaponSkin { get; }
        public string BannerBadge { get; }
        public string BannerMark { get; }
        public string BannerStripe { get; }
        public int Experience { get; }
        public int PvpRatingRank { get; }
        public int ItemsUnlocked { get; }
        public int ChallengesCompleted { get; }
        public int? MissionsCompleted { get; }
        public int? PvpWins { get; }
        public int? PvpLoses { get; }
        public int? PvpTotalMatches { get; }
        public int? PvpKills { get; }
        public int? PvpDeaths { get; }
        public int? PlaytimeSeconds { get; }
        public float? LeavingsPercentage { get; }
        public int? CoopClimbsPerformed { get; }
        public int? CoopAssistsPerformed { get; }
        public Class? FavoritePvpClass { get; }
        public Class? FavoritePveClass { get; }

        public string ClanName
        {
            get
            {
                if (!IsInClan)
                    throw new PlayerIsNotInClanException();
                return _clanName;
            }
        }

        public ClanRole ClanRole
        {
            get
            {
                if (!IsInClan)
                    throw new PlayerIsNotInClanException();
                return _clanRole;
            }
        }

        public int ClanPosition
        {
            get
            {
                if (!IsInClan)
                    throw new PlayerIsNotInClanException();
                return _clanPosition;
            }
        }

        public int ClanPoints
        {
            get
            {
                if (!IsInClan)
                    throw new PlayerIsNotInClanException();
                return _clanPoints;
            }
        }

        public DateTimeOffset ClanMemberSince
        {
            get
            {
                if (!IsInClan)
                    throw new PlayerIsNotInClanException();
                return _clanMemberSince;
            }
        }

        public bool IsInClan => _clanName != null;
        public int RmLeague => Constants.RmLeagueCount - PvpRatingRank;
        public bool HasLeague => RmLeague != Constants.RmLeagueCount;

        public float? Kdr => PvpDeaths == 0 ? 0 : (float?)PvpKills / PvpDeaths;

        public PeerPlayerInfo(string onlineId, string nickname, string primaryWeapon, string primaryWeaponSkin, string bannerBadge, string bannerMark, string bannerStripe, int experience, int pvpRatingRank, int itemsUnlocked, int challengesCompleted, int? missionsCompleted, int? pvpWins, int? pvpLoses, int? pvpTotalMatches, int? pvpKills, int? pvpDeaths, int? playtimeSeconds, float? leavingsPercentage, int? coopClimbsPerformed, int? coopAssistsPerformed, Class? favoritePvpClass, Class? favoritePveClass, string clanName, ClanRole clanRole, int clanPosition, int clanPoints, DateTimeOffset clanMemberSince)
        {
            OnlineId = onlineId;
            Nickname = nickname;
            PrimaryWeapon = primaryWeapon;
            PrimaryWeaponSkin = primaryWeaponSkin;
            BannerBadge = bannerBadge;
            BannerMark = bannerMark;
            BannerStripe = bannerStripe;
            Experience = experience;
            PvpRatingRank = pvpRatingRank;
            ItemsUnlocked = itemsUnlocked;
            MissionsCompleted = missionsCompleted;
            PvpWins = pvpWins;
            PvpLoses = pvpLoses;
            PvpTotalMatches = pvpTotalMatches;
            PvpKills = pvpKills;
            PvpDeaths = pvpDeaths;
            PlaytimeSeconds = playtimeSeconds;
            LeavingsPercentage = leavingsPercentage;
            CoopClimbsPerformed = coopClimbsPerformed;
            CoopAssistsPerformed = coopAssistsPerformed;
            FavoritePvpClass = favoritePvpClass;
            FavoritePveClass = favoritePveClass;
            _clanName = clanName;
            _clanRole = clanRole;
            _clanPosition = clanPosition;
            _clanPoints = clanPoints;
            _clanMemberSince = clanMemberSince;
            ChallengesCompleted = challengesCompleted;
            ItemsUnlocked = itemsUnlocked;
        }

        public static PeerPlayerInfo ParseNode(HtmlNode peerPlayerInfoNode)
        {
            /*
             * <peer_player_info online_id='505137167@warface/GameClient' nickname='[00:00:55] Бесполезняк' primary_weapon='sr46_shop' primary_weapon_skin='' banner_badge='5001' banner_mark='853' banner_stripe='6245' experience='23046000' pvp_rating_rank='17' items_unlocked='120' challenges_completed='343' missions_completed='1156' pvp_wins='2137' pvp_loses='1532' pvp_total_matches='3669' pvp_kills='66669' pvp_deaths='57425' playtime_seconds='4407237' leavings_percentage='5.2113652' coop_climbs_performed='2647' coop_assists_performed='1720' favorite_pvp_class='0' favorite_pve_class='0' clan_name='Ше_аге_теам' clan_role='3' clan_position='12' clan_points='39981' clan_member_since='5C266AD2'/>
             */

            string onlineId = peerPlayerInfoNode.Attributes["online_id"].Value;
            string nickname = peerPlayerInfoNode.Attributes["nickname"].Value;
            string primaryWeapon = peerPlayerInfoNode.Attributes["primary_weapon"].Value;
            string primaryWeaponSkin = peerPlayerInfoNode.Attributes["primary_weapon_skin"].Value;
            string bannerBadge = peerPlayerInfoNode.Attributes["banner_badge"].Value;
            string bannerMark = peerPlayerInfoNode.Attributes["banner_mark"].Value;
            string bannerStripe = peerPlayerInfoNode.Attributes["banner_stripe"].Value;
            int experience = peerPlayerInfoNode.Attributes["experience"].IntValue();
            int pvpRatingRank = peerPlayerInfoNode.Attributes["pvp_rating_rank"]?.IntValue() ??
                peerPlayerInfoNode.Attributes["pvp_rating_points"].IntValue(); //bot compat
            int itemsUnlocked = peerPlayerInfoNode.Attributes["items_unlocked"].IntValue();
            int challengesCompleted = peerPlayerInfoNode.Attributes["challenges_completed"].IntValue();
            var missionsCompleted = peerPlayerInfoNode.Attributes["missions_completed"]?.IntValue();
            var pvpWins = peerPlayerInfoNode.Attributes["pvp_wins"]?.IntValue();
            var pvpLoses = peerPlayerInfoNode.Attributes["pvp_loses"]?.IntValue();
            var pvpTotalMatches = peerPlayerInfoNode.Attributes["pvp_total_matches"]?.IntValue();
            var pvpKills = peerPlayerInfoNode.Attributes["pvp_kills"]?.IntValue();
            var pvpDeaths = peerPlayerInfoNode.Attributes["pvp_deaths"]?.IntValue();
            var playtimeSeconds = peerPlayerInfoNode.Attributes["playtime_seconds"]?.IntValue();
            var leavingsPercentage = peerPlayerInfoNode.Attributes["leavings_percentage"]?.FloatValue();
            var coopClimbsPerformed = peerPlayerInfoNode.Attributes["coop_climbs_performed"]?.IntValue();
            var coopAssistsPerformed = peerPlayerInfoNode.Attributes["coop_assists_performed"]?.IntValue();
            var favoritePvpClass = (Class?)peerPlayerInfoNode.Attributes["favorite_pvp_class"]?.IntValue();
            var favoritePveClass = (Class?)peerPlayerInfoNode.Attributes["favorite_pve_class"]?.IntValue();
            string clanName = null;
            var clanRole = ClanRole.Master;
            int clanPosition = -1;
            int clanPoints = -1;
            var clanMemberSince = DateTimeOffset.MinValue;
            if (peerPlayerInfoNode.Attributes.Contains("clan_name"))
            {
                clanName = peerPlayerInfoNode.Attributes["clan_name"].Value;
                clanRole = (ClanRole)peerPlayerInfoNode.Attributes["clan_role"].IntValue();
                clanPosition = peerPlayerInfoNode.Attributes["clan_position"].IntValue();
                clanPoints = peerPlayerInfoNode.Attributes["clan_points"].IntValue();
                string clanMemberSinceStr = peerPlayerInfoNode.Attributes["clan_member_since"].Value;
                long clanMemberSinceUnixSeconds = Convert.ToInt64(clanMemberSinceStr, 16);
                clanMemberSince = DateTimeOffset.FromUnixTimeSeconds(clanMemberSinceUnixSeconds);
            }
            return new PeerPlayerInfo(onlineId, nickname, primaryWeapon, primaryWeaponSkin, bannerBadge, bannerMark, bannerStripe, experience, pvpRatingRank, itemsUnlocked, challengesCompleted, missionsCompleted, pvpWins, pvpLoses, pvpTotalMatches, pvpKills, pvpDeaths, playtimeSeconds, leavingsPercentage, coopClimbsPerformed, coopAssistsPerformed, favoritePvpClass, favoritePveClass, clanName, clanRole, clanPosition, clanPoints, clanMemberSince);
        }
    }
}