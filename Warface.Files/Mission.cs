using System;
using aDevLib;
using HtmlAgilityPack;

namespace Warface.Files
{
    public class Mission
    {
        public string Name { get; }

        public Guid Uid { get; }

        public bool ReleaseMission { get; }

        public string DescriptionIcon { get; }

        Mission(string name, Guid uid, bool releaseMission, string descriptionIcon)
        {
            Name            = name;
            Uid             = uid;
            ReleaseMission  = releaseMission;
            DescriptionIcon = descriptionIcon;
        }

        public static Mission ParseNode(HtmlNode missionNode)
        {
            string name   = missionNode.Attributes["name"].Value;
            string uidStr = missionNode.Attributes["uid"].Value;
            var    uid    = Guid.Parse(uidStr);
            bool   releaseMission; //Blackwood Games strikes again!
            if (missionNode.Attributes.Contains("release_mission"))
                releaseMission = missionNode.Attributes["release_mission"].BoolValue();
            else if (missionNode.Attributes.Contains("test_mission"))
                releaseMission = !missionNode.Attributes["test_mission"].BoolValue();
            else
                throw new NotSupportedException();
            string icon = missionNode.SelectSingleNode("./ui/description").Attributes["icon"].Value;
            return new Mission(name, uid, releaseMission, icon);
        }
    }
}