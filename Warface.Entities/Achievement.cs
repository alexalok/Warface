using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities
{
    public class Achievement
    {
        //<chunk achievement_id='51' progress='100000' completion_time='1489274661'>

        public int             Id             { get; }
        public int             Progress       { get; }
        public DateTimeOffset? CompletionTime { get; }
        public bool            IsCompleted    => CompletionTime.HasValue;

        public Achievement(int id, int progress, DateTimeOffset? completionTime)
        {
            Id             = id;
            Progress       = progress;
            CompletionTime = completionTime;
        }

        public static (int profileId, IEnumerable<Achievement> achievements) ParseAchievementNode(HtmlNode achievementNode)
        {
            //<achievement profile_id='3078393'><chunk achievement_id='51' progress='100000' completion_time='1489274661'>...
            int profileId    = achievementNode.Attributes["profile_id"].IntValue();
            var chunkNodes   = achievementNode.SelectNodes("./chunk");
            var achievements = chunkNodes.Select(ParseNode);
            return (profileId, achievements);
        }

        public static Achievement ParseNode(HtmlNode chunkNode)
        {
            int  achievementId      = chunkNode.Attributes["achievement_id"].IntValue();
            int  progress           = chunkNode.Attributes["progress"].IntValue();
            long completionUnixTime = chunkNode.Attributes["progress"].LongValue();
            var  completionTime     = completionUnixTime == 0 ? (DateTimeOffset?)null : DateTimeOffset.FromUnixTimeSeconds(completionUnixTime);
            return new Achievement(achievementId, progress, completionTime);
        }
    }
}