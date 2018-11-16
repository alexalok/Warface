using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.Sessions
{
    public partial class BroadcastSessionResult
    {
        public class PlayerResult
        {
            public string Nickname                  { get; }
            public int    Experience                { get; }
            public int    Money                     { get; }
            public int    GainedCrownMoney          { get; }
            public bool   NoCrownRewards            { get; }
            public int    SponsorPoints             { get; }
            public int    ClanPoints                { get; }
            public int    BonusExperience           { get; }
            public int    BonusMoney                { get; }
            public int    BonusSponsorPoints        { get; }
            public int    ExperienceBoost           { get; }
            public int    MoneyBoost                { get; }
            public int    SponsorPointsBoost        { get; }
            public double ExperienceBoostPercent    { get; }
            public double MoneyBoostPercent         { get; }
            public double SponsorPointsBoostPercent { get; }
            public int    CompletedStages           { get; }
            public bool   IsVip                     { get; }
            public int    Score                     { get; }
            public bool   FirstWin                  { get; }
            public string DynamicMultipliersInfo    { get; }
            public int    DynamicCrownMultiplier    { get; }

            public PlayerResult(string nickname, int experience, int money, int gainedCrownMoney, bool noCrownRewards, int sponsorPoints, int clanPoints, int bonusExperience, int bonusMoney, int bonusSponsorPoints, int experienceBoost, int moneyBoost, int sponsorPointsBoost, double experienceBoostPercent, double moneyBoostPercent, double sponsorPointsBoostPercent, int completedStages, bool isVip, int score, bool firstWin, string dynamicMultipliersInfo, int dynamicCrownMultiplier)
            {
                Nickname                  = nickname;
                Experience                = experience;
                Money                     = money;
                GainedCrownMoney          = gainedCrownMoney;
                NoCrownRewards            = noCrownRewards;
                SponsorPoints             = sponsorPoints;
                ClanPoints                = clanPoints;
                BonusExperience           = bonusExperience;
                BonusMoney                = bonusMoney;
                BonusSponsorPoints        = bonusSponsorPoints;
                ExperienceBoost           = experienceBoost;
                MoneyBoost                = moneyBoost;
                SponsorPointsBoost        = sponsorPointsBoost;
                ExperienceBoostPercent    = experienceBoostPercent;
                MoneyBoostPercent         = moneyBoostPercent;
                SponsorPointsBoostPercent = sponsorPointsBoostPercent;
                CompletedStages           = completedStages;
                IsVip                     = isVip;
                Score                     = score;
                FirstWin                  = firstWin;
                DynamicMultipliersInfo    = dynamicMultipliersInfo;
                DynamicCrownMultiplier    = dynamicCrownMultiplier;
            }

            public static PlayerResult ParseNode(HtmlNode playerResultNode)
            {
                string nickname                  = playerResultNode.Attributes["nickname"].Value;
                int    experience                = playerResultNode.Attributes["experience"].IntValue();
                int    money                     = playerResultNode.Attributes["money"].IntValue();
                int    gainedCrownMoney          = playerResultNode.Attributes["gained_crown_money"].IntValue();
                bool   noCrownRewards            = playerResultNode.Attributes["no_crown_rewards"].BoolValue();
                int    sponsorPoints             = playerResultNode.Attributes["sponsor_points"].IntValue();
                int    clanPoints                = playerResultNode.Attributes["clan_points"].IntValue();
                int    bonusExperience           = playerResultNode.Attributes["bonus_experience"].IntValue();
                int    bonusMoney                = playerResultNode.Attributes["bonus_money"].IntValue();
                int    bonusSponsorPoints        = playerResultNode.Attributes["bonus_sponsor_points"].IntValue();
                int    experienceBoost           = playerResultNode.Attributes["experience_boost"].IntValue();
                int    moneyBoost                = playerResultNode.Attributes["money_boost"].IntValue();
                int    sponsorPointsBoost        = playerResultNode.Attributes["sponsor_points_boost"].IntValue();
                double experienceBoostPercent    = playerResultNode.Attributes["experience_boost_percent"].DoubleValue();
                double moneyBoostPercent         = playerResultNode.Attributes["money_boost_percent"].DoubleValue();
                double sponsorPointsBoostPercent = playerResultNode.Attributes["sponsor_points_boost_percent"].DoubleValue();
                int    completedStages           = playerResultNode.Attributes["completed_stages"].IntValue();
                bool   isVip                     = playerResultNode.Attributes["is_vip"].BoolValue();
                int    score                     = playerResultNode.Attributes["score"].IntValue();
                bool   firstWin                  = playerResultNode.Attributes["first_win"].BoolValue();
                string dynamicMultipliersInfo    = playerResultNode.Attributes["dynamic_multipliers_info"].Value;
                int    dynamicCrownMultiplier    = playerResultNode.Attributes["dynamic_crown_multiplier"].IntValue();

                return new PlayerResult(nickname, experience, money, gainedCrownMoney, noCrownRewards, sponsorPoints, clanPoints, bonusExperience, bonusMoney, bonusSponsorPoints, experienceBoost, moneyBoost, sponsorPointsBoost, experienceBoostPercent, moneyBoostPercent, sponsorPointsBoostPercent, completedStages, isVip, score, firstWin, dynamicMultipliersInfo, dynamicCrownMultiplier);
            }

            /*<player_result nickname='..каштыр..' experience='0' money='338' gained_crown_money='0' no_crown_rewards='1' sponsor_points='0' clan_points='38' bonus_experience='0' bonus_money='0' bonus_sponsor_points='0' experience_boost='444' money_boost='160' sponsor_points_boost='0' experience_boost_percent='1.15' money_boost_percent='0.9' sponsor_points_boost_percent='0.65' completed_stages='0' is_vip='1' score='1310' first_win='0' dynamic_multipliers_info='' dynamic_crown_multiplier='1'>
				<profile_progression_update profile_id='13143324' mission_unlocked='none,trainingmission,easymission,normalmission,hardmission,survivalmission,zombieeasy,zombienormal,zombiehard,campaignsections,campaignsection1,campaignsection2,campaignsection3,volcanoeasy,volcanonormal,volcanohard,volcanosurvival,anubiseasy,anubisnormal,anubishard,zombietowereasy,zombietowernormal,zombietowerhard,icebreakereasy,icebreakernormal,icebreakerhard,anubiseasy2,anubisnormal2,anubishard2,chernobyleasy,chernobylnormal,japaneasy,japannormal,all' tutorial_unlocked='7' tutorial_passed='7' class_unlocked='29'/>
			</player_result>*/
        }
    }
}