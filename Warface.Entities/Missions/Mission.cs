using System;
using HtmlAgilityPack;

namespace Warface.Entities.Missions
{
    public class Mission
    {
        public string Key { get; }
        public string Type { get; }
        public MissionMode Mode { get; }
        public bool NoTeams { get; }

        protected Mission(string key, string type, MissionMode mode, bool noTeams)
        {
            Key = key;
            Type = type;
            Mode = mode;
            NoTeams = noTeams;
        }

        public static Mission ParseNode(HtmlNode missionNode)
        {
            var mode = (MissionMode) Enum.Parse(typeof(MissionMode), missionNode.Attributes["mode"].Value, true);
            switch (mode)
            {
                case MissionMode.Pve:
                    return PvEMission.ParseNode(missionNode);
                case MissionMode.Pvp:
                    return PvPMission.ParseNode(missionNode);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}