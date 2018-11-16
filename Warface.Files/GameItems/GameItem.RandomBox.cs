using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;

namespace Warface.Files.GameItems
{
    public partial struct GameItem
    {
        public partial struct RandomBox
        {
            public List<List<Item>> Groups { get; }

            public RandomBox(List<List<Item>> groups)
            {
                Groups = groups;
            }

            [CanBeNull]
            public string GetTokenName()
            {
                foreach (var group in Groups)
                {
                    foreach (var item in group)
                    {
                        if (!string.IsNullOrEmpty(item.TopPrizeToken))
                            return item.TopPrizeToken;
                    }
                }
                return null;
            }

            public static RandomBox ParseNode(XmlNode randomBoxNode)
            {
                var groupNodes = randomBoxNode.SelectNodes("./group");
                var groupsList = new List<List<Item>>();
                foreach (XmlNode groupNode in groupNodes)
                {
                    var group = groupNode.SelectNodes("./item").Cast<XmlNode>().Select(Item.ParseNode).ToList();
                    groupsList.Add(group);
                }

                return new RandomBox(groupsList);
            }
        }

        /*
           <random_box>
		        <group>
			        <item name="sr04_shop" weight="10"/>
			        <item name="sr04_gold01_shop" weight="1" top_prize_token="box_token_cry_money_04" win_limit="1000"/>
			        <item name="sr04_shop" expiration="1h" weight="80"/>
			        <item name="sr04_shop" expiration="3h" weight="50"/>
			        <item name="sr04_shop" expiration="1d" weight="29"/>
			        <item name="sniper_fbs_cf_01" expiration="1d" weight="270"/>
			        <item name="sniper_fbs_usf_01" expiration="1d" weight="270"/>
			        <item name="sr15_shop" expiration="1d" weight="190"/>
			        <item name="sr15_shop" expiration="3d" weight="100"/>
		        </group>
		        <group>
			        <item name="pt26_shop" expiration="3d" weight="20"/>
			        <item name="pt26_shop" expiration="7d" weight="10"/>
			        <item name="kn06" expiration="3d" weight="16"/>
			        <item name="kn06" expiration="7d" weight="8"/>
			        <item name="smokegrenade04_c" amount="7" weight="10"/>
			        <item name="smokegrenade04_c" amount="3" weight="30"/>
			        <item name="claymoreexplosive04_c" amount="2" weight="10"/>
			        <item name="claymoreexplosive04_c" amount="1" weight="15"/>
		        </group>
		        <group>
			        <item name="exp_item_01"  amount="1500" weight="2"/>
			        <item name="exp_item_01"  amount="1000" weight="3"/>
			        <item name="exp_item_01"  amount="500" weight="15"/>
			        <item name="exp_item_01"  amount="200" weight="80"/>
		        </group>
	        </random_box>
            */
    }
}