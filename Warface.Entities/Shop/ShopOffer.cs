using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using aDevLib;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Warface.Entities.RandomBoxes;

namespace Warface.Entities.Shop
{
    public class ShopOffer
    {
        /*<offer id='8440' expirationTime='1 day' durabilityPoints='0' repair_cost='0' quantity='0' name='sniper_helmet_crown_02' item_category_override='' offer_status='normal' 
         * supplier_id='1' discount='0' rank='0' game_price='0' cry_price='0' crown_price='250' game_price_origin='0' cry_price_origin='0' crown_price_origin='250' key_item_name=''/>*/

        int       _repairCost;
        WinItem[] _winItems;

        public int    Id               { get; }
        public string ExpirationTime   { get; }
        public int    DurabilityPoints { get; }

        public int RepairCost
        {
            get
            {
                if (HasWinItems)
                    throw new InvalidOperationException();
                return _repairCost;
            }
            set => _repairCost = value;
        }

        public int    Quantity             { get; }
        public string Name                 { get; }
        public string ItemCategoryOverride { get; }
        public string OfferStatus          { get; }
        public int    SupplierId           { get; }
        public int    Discount             { get; }
        public int    SortingIndex         { get; }
        public int    Rank                 { get; }
        public int    GamePrice            { get; }
        public int    CryPrice             { get; }
        public int    CrownPrice           { get; }
        public int    GamePriceOrigin      { get; }
        public int    CryPriceOrigin       { get; }
        public int    CrownPriceOrigin     { get; }
        public string KeyItemName          { get; }

        public WinItem[] WinItems
        {
            get
            {
                if (!HasWinItems)
                    throw new InvalidOperationException();
                return _winItems;
            }
            set => _winItems = value;
        }

        public bool HasWinItems => _repairCost == -1;

        public Currency Currency
        {
            get
            {
                if (GamePrice != 0)
                    return Currency.Wardollars;
                if (CryPrice != 0)
                    return Currency.Kredits;
                if (CrownPrice != 0)
                    return Currency.Crowns;
                throw new InvalidOperationException();
            }
        }

        protected ShopOffer(int id, string expirationTime, int durabilityPoints, int repairCost, int quantity, string name, string itemCategoryOverride, string offerStatus, int supplierId, int discount, int sortingIndex, int rank, int gamePrice, int cryPrice, int crownPrice, int gamePriceOrigin, int cryPriceOrigin, int crownPriceOrigin, string keyItemName, [CanBeNull] WinItem[] winItems)
        {
            Id                   = id;
            ExpirationTime       = expirationTime;
            DurabilityPoints     = durabilityPoints;
            RepairCost           = repairCost;
            Quantity             = quantity;
            Name                 = name;
            ItemCategoryOverride = itemCategoryOverride;
            OfferStatus          = offerStatus;
            SupplierId           = supplierId;
            Discount             = discount;
            SortingIndex         = sortingIndex;
            Rank                 = rank;
            GamePrice            = gamePrice;
            CryPrice             = cryPrice;
            CrownPrice           = crownPrice;
            GamePriceOrigin      = gamePriceOrigin;
            CryPriceOrigin       = cryPriceOrigin;
            CrownPriceOrigin     = crownPriceOrigin;
            KeyItemName          = keyItemName;
            WinItems             = winItems;
        }

        public string GetPriceWithCurrency(bool shortVariant)
        {
            switch (Currency)
            {
                case Currency.Wardollars:
                    return CombineString(GamePrice);
                case Currency.Kredits:
                    return CombineString(CryPrice);
                case Currency.Crowns:
                    return CombineString(CrownPrice);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            string CombineString(int price)
            {
                return $"{price} {Currency.GetFriendlyName(shortVariant)}";
            }
        }

        public static ShopOffer ParseNode(HtmlNode offerNode)
        {
            int id = offerNode.Attributes["id"].IntValue();
            string expirationTime   = offerNode.Attributes["expirationTime"].Value;
            int    durabilityPoints = offerNode.Attributes["durabilityPoints"].IntValue();

            string        repairCostRaw = offerNode.Attributes["repair_cost"].Value;
            int           repairCost    = -1;
            List<WinItem> winItems      = null;
            if (string.IsNullOrEmpty(repairCostRaw))
            {
                repairCost = 0;
            }
            else if (repairCostRaw.All(char.IsDigit))
            {
                repairCost = Convert.ToInt32(repairCostRaw);
            }
            else if (repairCostRaw.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).Length > 0)
            {
                var repairSplit = repairCostRaw.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                winItems = new List<WinItem>();
                foreach (string repairEntry in repairSplit)
                {
                    //sr31_shop,5400,36000
                    var    repairEntrySplit = repairEntry.Split(',');
                    string itemName         = repairEntrySplit[0];
                    int    itemRepairCost   = int.Parse(repairEntrySplit[1]);
                    int    itemDurability   = int.Parse(repairEntrySplit[2]);
                    winItems.Add(new WinItem(itemName, itemRepairCost, itemDurability));
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            int    quantity             = offerNode.Attributes["quantity"].IntValue();
            string name                 = offerNode.Attributes["name"].Value;
            string itemCategoryOverride = offerNode.Attributes["item_category_override"].Value;
            string offerStatus          = offerNode.Attributes["offer_status"].Value;
            int    supplierId           = offerNode.Attributes["supplier_id"].IntValue();
            int    discount             = offerNode.Attributes["discount"].IntValue();
            int    sortingIndex         = offerNode.Attributes["sorting_index"].IntValue();
            int    rank                 = offerNode.Attributes["rank"].IntValue();
            int    gamePrice            = offerNode.Attributes["game_price"].IntValue();
            int    cryPrice             = offerNode.Attributes["cry_price"].IntValue();
            int    crownPrice           = offerNode.Attributes["crown_price"].IntValue();
            int    gamePriceOrigin      = offerNode.Attributes["game_price_origin"].IntValue();
            int    cryPriceOrigin       = offerNode.Attributes["cry_price_origin"].IntValue();
            int    crownPriceOrigin     = offerNode.Attributes["crown_price_origin"].IntValue();
            string keyItemName          = offerNode.Attributes["key_item_name"].Value;


            return new ShopOffer(id, expirationTime, durabilityPoints, repairCost, quantity, name, itemCategoryOverride, offerStatus,
                supplierId, discount, sortingIndex, rank, gamePrice, cryPrice, crownPrice, gamePriceOrigin, cryPriceOrigin, crownPriceOrigin, keyItemName, winItems?.ToArray());
        }
    }
}