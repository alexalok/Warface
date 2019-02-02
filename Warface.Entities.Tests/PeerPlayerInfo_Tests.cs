using System;
using HtmlAgilityPack;
using Warface.Entities.PeerStatusUpdates;
using Warface.Enums;
using Xunit;

namespace Warface.Entities.Tests
{
    public class PeerPlayerInfo_Tests
    {
        [Fact]
        void ParseNode()
        {
            var onlineId            = "505137167@warface/GameClient";
            var nickname            = "[00:00:55] Бесполезняк";
            var primaryWeapon       = "sr46_shop";
            var primaryWeaponSkin   = "";
            var bannerBadge         = 5001.ToString();
            var bannerMark          = 853.ToString();
            var bannerStripe        = 6245.ToString();
            var experience          = 23046000;
            var pvpRatingRank       = 17;
            var itemsUnlocked       = 120;
            var challengesCompleted = 343;
            var missionsCompleted   = 1156;
            var pvpWins             = 2137;
            var pvpLoses            = 1532;
            var pvpTotalMatches     = 3669;
            var pvpKills            = 66669;
            var pvpDeaths           = 57425;
            var playtimeSeconds     = 4407237;
            var leavingsPercentage  = 5.2113652f;
            var coopClimbsPerformed = 2647;
            var coopAsistsPerformed = 1720;
            var favoritePvpClass    = (Class) 0;
            var favoritePveClass    = (Class) 0;
            var clanName            = "Ше_аге_теам";
            var clanRole            = (ClanRole) 3;
            var clanPosition        = 12;
            var clanPoints          = 39981;
            var clanMemberSince     = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("5C266AD2", 16));
            int rmLeague            = Constants.RmLeagueCount - pvpRatingRank;
            var isInClan            = true;

            var node = HtmlNode.CreateNode("<peer_player_info online_id='505137167@warface/GameClient' nickname='[00:00:55] Бесполезняк' primary_weapon='sr46_shop' primary_weapon_skin='' banner_badge='5001' banner_mark='853' banner_stripe='6245' experience='23046000' pvp_rating_rank='17' items_unlocked='120' challenges_completed='343' missions_completed='1156' pvp_wins='2137' pvp_loses='1532' pvp_total_matches='3669' pvp_kills='66669' pvp_deaths='57425' playtime_seconds='4407237' leavings_percentage='5.2113652' coop_climbs_performed='2647' coop_assists_performed='1720' favorite_pvp_class='0' favorite_pve_class='0' clan_name='Ше_аге_теам' clan_role='3' clan_position='12' clan_points='39981' clan_member_since='5C266AD2'/>");

            var peerPlayerInfo = PeerPlayerInfo.ParseNode(node);
            Assert.Equal(nickname,            peerPlayerInfo.Nickname);
            Assert.Equal(primaryWeapon,       peerPlayerInfo.PrimaryWeapon);
            Assert.Equal(primaryWeaponSkin,   peerPlayerInfo.PrimaryWeaponSkin);
            Assert.Equal(bannerBadge,         peerPlayerInfo.BannerBadge);
            Assert.Equal(bannerMark,          peerPlayerInfo.BannerMark);
            Assert.Equal(bannerStripe,        peerPlayerInfo.BannerStripe);
            Assert.Equal(experience,          peerPlayerInfo.Experience);
            Assert.Equal(pvpRatingRank,       peerPlayerInfo.PvpRatingRank);
            Assert.Equal(itemsUnlocked,       peerPlayerInfo.ItemsUnlocked);
            Assert.Equal(challengesCompleted, peerPlayerInfo.ChallengesCompleted);
            Assert.Equal(missionsCompleted,   peerPlayerInfo.MissionsCompleted);
            Assert.Equal(pvpWins,             peerPlayerInfo.PvpWins);
            Assert.Equal(pvpLoses,            peerPlayerInfo.PvpLoses);
            Assert.Equal(pvpTotalMatches,     peerPlayerInfo.PvpTotalMatches);
            Assert.Equal(pvpKills,            peerPlayerInfo.PvpKills);
            Assert.Equal(pvpDeaths,           peerPlayerInfo.PvpDeaths);
            Assert.Equal(playtimeSeconds,     peerPlayerInfo.PlaytimeSeconds);
            Assert.Equal(leavingsPercentage,  peerPlayerInfo.LeavingsPercentage);
            Assert.Equal(coopClimbsPerformed, peerPlayerInfo.CoopClimbsPerformed);
            Assert.Equal(coopAsistsPerformed, peerPlayerInfo.CoopAssistsPerformed);
            Assert.Equal(favoritePvpClass,    peerPlayerInfo.FavoritePvpClass);
            Assert.Equal(favoritePveClass,    peerPlayerInfo.FavoritePveClass);
            Assert.Equal(clanName,            peerPlayerInfo.ClanName);
            Assert.Equal(clanRole,            peerPlayerInfo.ClanRole);
            Assert.Equal(clanPosition,        peerPlayerInfo.ClanPosition);
            Assert.Equal(clanPoints,          peerPlayerInfo.ClanPoints);
            Assert.Equal(clanMemberSince,     peerPlayerInfo.ClanMemberSince);
            Assert.Equal(rmLeague,            peerPlayerInfo.RmLeague);
            Assert.Equal(isInClan,            peerPlayerInfo.IsInClan);
        }

        [Fact]
        void ParseNode2()
        {
            var node = HtmlNode.CreateNode("<peer_player_info online_id='295902664@warface/GameClient' nickname='_Злой_-Клоун_' primary_weapon='smg14_shop' primary_weapon_skin='' banner_badge='124' banner_mark='283' banner_stripe='473' experience='12151122' pvp_rating_rank='0' items_unlocked='116' challenges_completed='0' clan_name='Скифы.' clan_role='2' clan_position='2' clan_points='3735615' clan_member_since='521646C7'/>");
            var ppi = PeerPlayerInfo.ParseNode(node);
            Assert.Null(ppi.Kdr);
        }
    }
}