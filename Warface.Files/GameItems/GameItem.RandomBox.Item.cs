using System;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;

namespace Warface.Files.GameItems
{
    public partial struct GameItem
    {
        public partial struct RandomBox
        {
            public struct Item
            {
                public string Name { get; }

                public int Weight { get; }

                public int? Amount { get; }

                [CanBeNull]
                public string Expiration { get; }

                [CanBeNull]
                public string FriendlyExpiration => GetExpirationFriendlyString();

                [CanBeNull]
                public string TopPrizeToken { get; }

                public Item(string name, int weight, int? amount, [CanBeNull] string expiration, [CanBeNull] string topPrizeToken)
                {
                    Name = name;
                    Weight = weight;
                    Amount = amount;
                    Expiration = expiration;
                    TopPrizeToken = topPrizeToken;
                }

                public static Item ParseNode(XmlNode itemNode)
                {
                    string name = itemNode.Attributes["name"].Value;
                    int weight = Convert.ToInt32(itemNode.Attributes["weight"].Value);
                    string amountStr = itemNode.Attributes["amount"]?.Value;
                    var amount = amountStr == null ? (int?) null : Convert.ToInt32(amountStr);
                    string expiration = itemNode.Attributes["expiration"]?.Value;
                    string topPrizeToken = itemNode.Attributes["top_prize_token"]?.Value;

                    return new Item(name, weight, amount, expiration, topPrizeToken);
                }

                [CanBeNull]
                string GetExpirationFriendlyString()
                {
                    if (Expiration == null)
                        return null;
                    string value = Expiration.Substring(0, Expiration.Length - 1);
                    string unit;
                    switch (Expiration.Last())
                    {
                        case 'h':
                            unit = "ч.";
                            break;
                        case 'd':
                            unit = "д.";
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    return $"{value} {unit}";
                }
            }
        }

        /*
            <item name="sr04_shop" weight="10"/>
			<item name="sr04_gold01_shop" weight="1" top_prize_token="box_token_cry_money_04" win_limit="1000"/>
			<item name="sr04_shop" expiration="1h" weight="80"/>
         */
    }
}