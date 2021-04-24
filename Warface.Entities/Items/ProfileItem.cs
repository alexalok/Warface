using System;
using aDevLib;
using HtmlAgilityPack;

namespace Warface.Entities.Items
{
    public class ProfileItem
    {
        public string Name { get; }

        public int OfferId { get; }

        public string AddedExpiration { get; }

        public int AddedQuantity { get; }

        public Item Item { get; }

        public bool IsGoldPrize => Item.Type == ItemType.Permanent && Item.Name.Contains("gold");

        public ProfileItem(string name, int offerId, string addedExpiration, int addedQuantity, Item item)
        {
            Name = name;
            OfferId = offerId;
            AddedExpiration = addedExpiration;
            AddedQuantity = addedQuantity;
            Item = item;
        }

        public string GetDescription()
        {
            var resultString = $"Item: {Name}";
            if (AddedExpiration != "0") //temp item
                resultString +=
                    $" (+{AddedExpiration}), expires at: {Item.ExpirationTime}";
            else if (AddedQuantity != 0) //consumable
                resultString += $" (+{AddedQuantity} pcs), total: {Item.Quantity} pcs";
            else if (Item.Permanent) //perma
            {
                var durabilityPercent =
                    Math.Round((float) Item.TotalDurabilityPoints /
                        Item.DurabilityPoints * 100);
                resultString += $" (PERMANENT), total durability: {durabilityPercent}%";
            }
            else //GOLD 
                resultString += $"(☞ ͡ ͡° ͜ ʖ ͡ ͡°)☞ (GOLD) ᕙ( ͡° ͜ʖ ͡°)ᕗ";

            return resultString;
        }

        public static ProfileItem ParseNode(HtmlNode profileItemNode)
        {
            /*<profile_item name='sniper_fbs_cf_01' profile_item_id='21326167' offerId='18590' added_expiration='1 day' added_quantity='0' error_status='0'>
                <item id='21326167' name='sniper_fbs_cf_01' attached_to='0' config='dm=0;material=default;pocket_index=0' slot='0' equipped='0' default='0' permanent='0' 
                    expired_confirmed='0' buy_time_utc='1498731065' expiration_time_utc='1539425465' seconds_left='37473203'/>*/

            var name = profileItemNode.Attributes["name"].Value;
            var offerId = profileItemNode.Attributes["offerId"].IntValue();
            var addedExpiration = profileItemNode.Attributes["added_expiration"].Value;
            var addedQuantity = profileItemNode.Attributes["added_quantity"].IntValue();

            var itemNode = profileItemNode.SelectSingleNode("./item");
            var item = Item.ParseNode(itemNode);

            return new ProfileItem(name, offerId, addedExpiration, addedQuantity, item);
        }
    }

    enum ProfileItemType 
    {
    }
}