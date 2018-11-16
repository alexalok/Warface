using System;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Warface.Entities.Missions;
using Warface.Enums;

namespace Warface.Entities
{
    public class PlayerStat
    {
        public string Stat { get; }

        public int Value { get; }

        [CanBeNull]
        public string Difficulty { get; }

        public PlayMode? Mode { get; }

        public Class? Class { get; }

        [CanBeNull]
        public string ItemType { get; }

        public PlayerStat(string stat, int value, [CanBeNull] string difficulty = null, PlayMode? mode = null, Class? @class = null, [CanBeNull] string itemType = null)
        {
            Stat       = stat;
            Value      = value;
            Difficulty = difficulty;
            Mode       = mode;
            Class      = @class;
            ItemType   = itemType;
        }

        public static PlayerStat ParseNode(HtmlNode statNode)
        {
            //<stat stat='player_online_time' value='79803084'/>
            string stat  = statNode.Attributes["stat"].Value;
            int    value = statNode.Attributes["value"].IntValue();

            //<stat difficulty='' mode='PVP' stat='player_sessions_lost' value='1532'/>
            string difficulty = null;
            if (statNode.Attributes.Contains("difficulty"))
                difficulty = statNode.Attributes["difficulty"].Value;

            PlayMode? mode = null;
            if (statNode.Attributes.Contains("mode"))
            {
                if (!Enum.TryParse<PlayMode>(statNode.Attributes["mode"].Value, true, out var tempMode))
                    throw new ArgumentOutOfRangeException();
                mode = tempMode;
            }

            //<stat class='Rifleman' mode='PVP' stat='player_hits' value='48707'/>
            Class? @class = null;
            if (statNode.Attributes.Contains("class"))
            {
                @class = statNode.Attributes["class"].Value.ToClass();
            }

            //<stat class='Rifleman' item_type='ar28_shop' stat='player_wpn_usage' value='1278227'/>
            string itemType = null;
            if (statNode.Attributes.Contains("item_type"))
                itemType = statNode.Attributes["item_type"].Value;

            return new PlayerStat(stat, value, difficulty, mode, @class, itemType);
        }
    }
}