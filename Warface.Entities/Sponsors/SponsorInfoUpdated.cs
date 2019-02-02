using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Warface.Entities.Items;

namespace Warface.Entities.Sponsors
{
    public class SponsorInfoUpdated
    {
        public IEnumerable<Item> UnlockedItems { get; }

        public SponsorInfoUpdated(IEnumerable<Item> unlockedItems)
        {
            UnlockedItems = unlockedItems;
        }

        public static SponsorInfoUpdated ParseNode(HtmlNode sponsorInfoUpdatedNode)
        {
            /*
             * <sponsor_info_updated sponsor_id='2' sponsor_points='128' total_sponsor_points='84653' next_unlock_item='ugl01'>
			        <unlocked_items>
				        <item id='18446744073709551455' name='bn02' attached_to='0' config='' slot='20' equipped='1' default='0' permanent='0' expired_confirmed='0' buy_time_utc='0' expiration_time_utc='0' seconds_left='0' profile_item_id='18446744073709551455'/>
			        </unlocked_items>
		        </sponsor_info_updated>
             */

            var unlockedItemNodes = sponsorInfoUpdatedNode.SelectNodes("./unlocked_items/item");
            var unlockedItems     = new List<Item>(1);
            if (unlockedItemNodes != null)
                unlockedItems = unlockedItemNodes.Select(Item.ParseNode).ToList();
            return new SponsorInfoUpdated(unlockedItems);
        }
    }
}