using System.Collections.Generic;
using System.Reflection;
using HtmlAgilityPack;
using Warface.Enums;
using Xunit;

namespace Warface.Entities.Tests
{
    public class PlayerStat_Tests
    {
        [Theory]
        [MemberData(nameof(GetStatNodeAndValues))]
        void PlayerStat_ParseNode_CheckValues(HtmlNode statNode, PlayerStat expectedStat)
        {
            var actualStat = PlayerStat.ParseNode(statNode);
            foreach (var propertyInfo in expectedStat.GetType().GetProperties(BindingFlags.Instance|BindingFlags.Public))
            {
                var expectedValue = propertyInfo.GetValue(expectedStat);
                var actualValue = propertyInfo.GetValue(actualStat);
                Assert.Equal(expectedValue, actualValue);
            }
        }

        public static IEnumerable<object[]> GetStatNodeAndValues()
        {
            var node1 = HtmlNode.CreateNode("<stat stat='player_online_time' value='79803084'/>");
            yield return new object[] {node1, new PlayerStat("player_online_time", 79803084)};

            var node2 = HtmlNode.CreateNode("<stat difficulty='' mode='PVP' stat='player_sessions_lost' value='1532'/>");
            yield return new object[] {node2, new PlayerStat("player_sessions_lost", 1532, "", PlayMode.PvP)};

            var node3 = HtmlNode.CreateNode("<stat class='Rifleman' mode='PVP' stat='player_playtime' value='5452532'/>");
            yield return new object[] {node3, new PlayerStat("player_playtime", 5452532, null, PlayMode.PvP, Class.Rifleman)};

            var node4 = HtmlNode.CreateNode("<stat class='Engineer' item_type='smg10_shop' stat='player_wpn_usage' value='2228818'/>");
            yield return new object[] {node4, new PlayerStat("player_wpn_usage", 2228818, null, null, Class.Engineer, "smg10_shop") };
        }
    }
}