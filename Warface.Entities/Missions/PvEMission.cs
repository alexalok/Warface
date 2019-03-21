using System;
using System.ComponentModel;
using System.Diagnostics;
using aDevLib.Extensions;
using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.Missions
{
    public class PvEMission : Mission
    {
        public     string               FriendlyName    => Type.GetDescription();
        public new PvEMissionType       Type            { get; }
        public     PvEMissionGlobalType GlobalType      => GetGlobalType();
        public     CrownRewards         CrownRewards    { get; }
        public     bool                 HasCrownRewards => CrownRewards != null;
        public     bool                 IsSpecOps       => GlobalType   != PvEMissionGlobalType.Regular;

        PvEMission(string key, PvEMissionType type, CrownRewards crownRewards) : base(key, type.ToString(), MissionMode.Pve, true)
        {
            Type         = type;
            CrownRewards = crownRewards;
        }

        public new static PvEMission ParseNode(HtmlNode missionNode)
        {
            /*<mission mission_key='8b40c308-6ba5-4d3c-9de9-0b477dc7b5f9' no_teams='1' name='@na_mission_volcano_01' setting='survival/africa_survival_base' mode='pve' mode_name='@PvE_game_mode_desc' mode_icon='pve_icon' description='@na_mission_volcano_desc_01' image='mapImgNAvolcano_e' difficulty='easy' type='volcanoeasy' time_of_day='9:06'>
		    <objectives factor='1'>
			    <objective id='0' type='primary'/>
		    </objectives>
		    <CrownRewardsThresholds>
			    <TotalPerformance bronze='1130300' silver='1358000' gold='1520000'/>
			    <Time bronze='4190944' silver='4191784' gold='4192204'/>
		    </CrownRewardsThresholds>
		    <CrownRewards bronze='6' silver='17' gold='32'/>
	     </mission>*/

            string missionKey = missionNode.Attributes["mission_key"].Value;
            var    type       = (PvEMissionType) Enum.Parse(typeof(PvEMissionType), missionNode.Attributes["type"].Value, true);
            var    mode       = (MissionMode) Enum.Parse(typeof(MissionMode),       missionNode.Attributes["mode"].Value, true);
            if (mode != MissionMode.Pve)
                throw new InvalidOperationException();

            bool noTeams = missionNode.Attributes["no_teams"].BoolValue();
            Debug.Assert(noTeams);

            var          crownRewardsThresholdsNode = missionNode.SelectSingleNode(".//crownrewardsthresholds");
            CrownRewards crownRewards               = null;
            if (crownRewardsThresholdsNode != null)
            {
                var totalPerformanceNode = crownRewardsThresholdsNode.SelectSingleNode(".//totalperformance");
                int bronzeScore          = totalPerformanceNode.Attributes["bronze"].IntValue();
                int silverScore          = totalPerformanceNode.Attributes["silver"].IntValue();
                int goldScore            = totalPerformanceNode.Attributes["gold"].IntValue();
                var timeNode             = crownRewardsThresholdsNode.SelectSingleNode(".//time");
                var bronzeTime           = TimeSpan.FromSeconds((1 << 22) - timeNode.Attributes["bronze"].IntValue());
                var silverTime           = TimeSpan.FromSeconds((1 << 22) - timeNode.Attributes["silver"].IntValue());
                var goldTime             = TimeSpan.FromSeconds((1 << 22) - timeNode.Attributes["gold"].IntValue());
                var crownRewardsNode     = missionNode.SelectSingleNode(".//crownrewards");
                int bronzeCrownAmount    = crownRewardsNode.Attributes["bronze"].IntValue();
                int silverCrownAmount    = crownRewardsNode.Attributes["silver"].IntValue();
                int goldCrownAmount      = crownRewardsNode.Attributes["gold"].IntValue();
                crownRewards = new CrownRewards()
                {
                    Bronze = new CrownReward()
                    {
                        Amount = bronzeCrownAmount,
                        Score  = bronzeScore,
                        Time   = bronzeTime
                    },
                    Gold = new CrownReward()
                    {
                        Amount = goldCrownAmount,
                        Score  = goldScore,
                        Time   = goldTime
                    },
                    Silver = new CrownReward()
                    {
                        Amount = silverCrownAmount,
                        Score  = silverScore,
                        Time   = silverTime
                    }
                };
            }

            return new PvEMission(missionKey, type, crownRewards);
        }

        PvEMissionGlobalType GetGlobalType()
        {
            switch (Type)
            {
                case PvEMissionType.TrainingMission:
                case PvEMissionType.EasyMission:
                case PvEMissionType.NormalMission:
                case PvEMissionType.HardMission:
                    return PvEMissionGlobalType.Regular;

                case PvEMissionType.SurvivalMission:
                    return PvEMissionGlobalType.TowerRaid;

                case PvEMissionType.CampaignSection1:
                case PvEMissionType.CampaignSection2:
                case PvEMissionType.CampaignSection3:
                case PvEMissionType.CampaignSections:
                    return PvEMissionGlobalType.Marathon;

                case PvEMissionType.VolcanoEasy:
                case PvEMissionType.VolcanoNormal:
                case PvEMissionType.VolcanoHard:
                case PvEMissionType.VolcanoSurvival:
                    return PvEMissionGlobalType.EarthShaker;

                case PvEMissionType.AnubisEasy:
                case PvEMissionType.AnubisNormal:
                case PvEMissionType.AnubisHard:
                    return PvEMissionGlobalType.Anubis;

                case PvEMissionType.ZombieTowerEasy:
                case PvEMissionType.ZombieTowerNormal:
                case PvEMissionType.ZombieTowerHard:
                    return PvEMissionGlobalType.BlackShark;

                case PvEMissionType.AnubisEasy2:
                case PvEMissionType.AnubisNormal2:
                case PvEMissionType.AnubisHard2:
                    return PvEMissionGlobalType.Blackout;

                case PvEMissionType.IcebreakerEasy:
                case PvEMissionType.IcebreakerNormal:
                case PvEMissionType.IcebreakerHard:
                    return PvEMissionGlobalType.IceBreaker;

                case PvEMissionType.ChernobylEasy:
                case PvEMissionType.ChernobylNormal:
                case PvEMissionType.ChernobylHard:
                    return PvEMissionGlobalType.Chernobyl;

                case PvEMissionType.ZombieEasy:
                case PvEMissionType.ZombieNormal:
                case PvEMissionType.ZombieHard:
                    return PvEMissionGlobalType.CyberHorde;

                case PvEMissionType.JapanEasy:
                case PvEMissionType.JapanNormal:
                case PvEMissionType.JapanHard:
                    return PvEMissionGlobalType.Japan;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class CrownRewards
    {
        public CrownReward Gold   = new CrownReward();
        public CrownReward Bronze = new CrownReward();
        public CrownReward Silver = new CrownReward();
    }

    public class CrownReward
    {
        public TimeSpan Time;
        public int      Score;
        public int      Amount;
    }

    public enum PvEMissionType
    {
        //everydays
        [Description("Тренировка")] TrainingMission,
        [Description("Легко")]      EasyMission,
        [Description("Сложно")]     NormalMission,
        [Description("Профи")]      HardMission,

        //tower raid
        [Description("Белая акула")] SurvivalMission,

        //Marathon
        [Description("Острие")]  CampaignSection1, //spearhead
        [Description("Засада")]  CampaignSection2, //ambush
        [Description("Зенит")]   CampaignSection3, //zenith
        [Description("Марафон")] CampaignSections, //marathon

        //cyber horde
        [Description("Опасный эксперимент (легко)")]
        ZombieEasy,

        [Description("Опасный эксперимент (сложно)")]
        ZombieNormal,

        [Description("Опасный эксперимент (профи)")]
        ZombieHard,

        //Earth Shaker
        [Description("Вулкан (легко)")]   VolcanoEasy,
        [Description("Вулкан (сложно)")]  VolcanoNormal,
        [Description("Вулкан (профи)")]   VolcanoHard,
        [Description("Вулкан (хардкор)")] VolcanoSurvival,

        //anubis
        [Description("Анубис (легко)")]  AnubisEasy,
        [Description("Анубис (сложно)")] AnubisNormal,
        [Description("Анубис (профи)")]  AnubisHard,

        //black shark
        [Description("Черная акула (легко)")]  ZombieTowerEasy,
        [Description("Черная акула (сложно)")] ZombieTowerNormal,
        [Description("Черная акула (профи)")]  ZombieTowerHard,

        //blackout
        [Description("Затмение (легко)")]  AnubisEasy2,
        [Description("Затмение (сложно)")] AnubisNormal2,
        [Description("Затмение (профи)")]  AnubisHard2,

        //icebreaker
        [Description("Ледокол (легко)")]  IcebreakerEasy,
        [Description("Ледокол (сложно)")] IcebreakerNormal,
        [Description("Ледокол (профи)")]  IcebreakerHard,

        //chernobyl
        [Description("Припять (легко)")]  ChernobylEasy,
        [Description("Припять (сложно)")] ChernobylNormal,
        [Description("Припять (профи)")]  ChernobylHard,

        //japan
        [Description("Восход (легко)")]  JapanEasy,
        [Description("Восход (сложно)")] JapanNormal,
        [Description("Восход (профи)")]  JapanHard,
    }

    public enum PvEMissionGlobalType
    {
        [Description("Ежедневные")]          Regular,
        [Description("Белая акула")]         TowerRaid,
        [Description("Снежный бастион")]     Marathon,
        [Description("Опасный эксперимент")] CyberHorde,
        [Description("Вулкан")]              EarthShaker,
        [Description("Анубис")]              Anubis,
        [Description("Черная акула")]        BlackShark,
        [Description("Затмение")]            Blackout,
        [Description("Ледокол")]             IceBreaker,
        [Description("Чернобыль")]           Chernobyl,
        [Description("Восход")]              Japan
    }
}