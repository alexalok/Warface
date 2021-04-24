using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using aDevLib;
using HtmlAgilityPack;

namespace Warface.Entities
{
    public class Achievement
    {
        //<chunk achievement_id='51' progress='100000' completion_time='1489274661'>

        public int             Id                          { get; }
        public int             Progress                    { get; }
        public long            CompletionTimeUnixTimestamp { get; }
        public DateTimeOffset? CompletionTime              { get; }
        public bool            IsCompleted                 => CompletionTimeUnixTimestamp != 0;

        public Achievement(int id, int progress, long completionTimeUnixTimestamp)
        {
            Id                          = id;
            Progress                    = progress;
            CompletionTimeUnixTimestamp = completionTimeUnixTimestamp;
            if (CompletionTimeUnixTimestamp != 0)
                CompletionTime = DateTimeOffset.FromUnixTimeSeconds(CompletionTimeUnixTimestamp);

            if (CompletionTimeUnixTimestamp == 0)
                Debug.Assert(!CompletionTime.HasValue);
            else
                Debug.Assert(CompletionTime.Value.ToUnixTimeSeconds() == CompletionTimeUnixTimestamp);
        }

        public static (int profileId, IEnumerable<Achievement> achievements) ParseAchievementNode(HtmlNode achievementNode)
        {
            //<achievement profile_id='3078393'><chunk achievement_id='51' progress='100000' completion_time='1489274661'>...
            int                      profileId    = achievementNode.Attributes["profile_id"].IntValue();
            var                      chunkNodes   = achievementNode.SelectNodes("./chunk");
            IEnumerable<Achievement> achievements = new Achievement[0];
            if (chunkNodes != null)
                achievements = chunkNodes.Select(ParseNode);
            return (profileId, achievements);
        }

        public static Achievement ParseNode(HtmlNode chunkNode)
        {
            int  achievementId      = chunkNode.Attributes["achievement_id"].IntValue();
            int  progress           = chunkNode.Attributes["progress"].IntValue();
            long completionUnixTime = chunkNode.Attributes["completion_time"].LongValue();
            var  completionTime     = completionUnixTime == 0 ? (DateTimeOffset?) null : DateTimeOffset.FromUnixTimeSeconds(completionUnixTime);
            return new Achievement(achievementId, progress, completionUnixTime);
        }
    }
}