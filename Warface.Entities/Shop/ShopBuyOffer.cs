using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using aDevLib;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Warface.Entities.Items;

namespace Warface.Entities.Shop
{
    public class ShopBuyOffer
    {
        public ErrorStatus Error { get; }

        public bool HasProfileItems => ProfileItems != null && ProfileItems.Any();

        public List<ProfileItem> ProfileItems { get; }

        public List<int> OfferIds { get; }


        public int? GameMoney { get; }

        public int? CryMoney { get; }

        public int? CrownMoney { get; }

        public bool HasMoneyUpdate => GameMoney != null && CryMoney != null && CrownMoney != null;

        ShopBuyOffer(ErrorStatus                   error,
                     [CanBeNull] List<ProfileItem> profileItems, [CanBeNull] List<int> offerIds,
                     int?                          gameMoney,    int?                  cryMoney, int? crownMoney)
        {
            Error = error;

            ProfileItems = profileItems;
            OfferIds     = offerIds;

            GameMoney  = gameMoney;
            CryMoney   = cryMoney;
            CrownMoney = crownMoney;
        }

        public static ShopBuyOffer ParseNode(HtmlNode shopBuyOfferNode)
        {
            var errorStatus = (ErrorStatus) shopBuyOfferNode.Attributes["error_status"].IntValue();

            var               profileItemNodes = shopBuyOfferNode.SelectNodes("./purchased_item/profile_item");
            List<ProfileItem> profileItems     = null;
            List<int>         offerIds         = null;
            if (profileItemNodes != null)
            {
                profileItems = profileItemNodes.Select(ProfileItem.ParseNode).ToList();
                offerIds     = profileItems.Select(p => p.OfferId).Distinct().OrderBy(o => o).ToList();
                Debug.Assert(offerIds.Count >= 1 && offerIds.Count <= 5);
            }

            var  moneyNode  = shopBuyOfferNode.SelectSingleNode("./money");
            int? gameMoney  = null;
            int? cryMoney   = null;
            int? crownMoney = null;
            if (moneyNode != null)
            {
                gameMoney  = moneyNode.Attributes["game_money"].IntValue();
                cryMoney   = moneyNode.Attributes["cry_money"].IntValue();
                crownMoney = moneyNode.Attributes["crown_money"].IntValue();
            }

            return new ShopBuyOffer(errorStatus, profileItems, offerIds, gameMoney, cryMoney, crownMoney);
        }

        /*
         * <shop_buy_offer offer_id='31348' error_status='0'>
			    <purchased_item>
				    <profile_item name='shared_helmet_hlw_01' profile_item_id='1264206484' offerId='31348' added_expiration='1 day' added_quantity='0' error_status='0'>
					    <item id='1264206484' name='shared_helmet_hlw_01' attached_to='0' config='' slot='0' equipped='0' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1540570204' expiration_time_utc='1540656604' seconds_left='86399'/>
				    </profile_item>
			    </purchased_item>
			    <money game_money='221207' cry_money='1' crown_money='23236'/>
		    </shop_buy_offer>
         */

        /*
         * <shop_buy_multiple_offer error_status='0'>
			    <purchased_item>
				    <exp name='exp_item_01' added='50' total='8939796' offerid='30678'/>
				    <profile_item name='flashbang' profile_item_id='1020973757' offerid='30678' added_expiration='3 day' added_quantity='0' error_status='0'>
					    <item id='1020973757' name='flashbang' attached_to='0' config='dm=0;material=;pocket_index=2048' slot='5120' equipped='4' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1459621215' expiration_time_utc='1606332015' seconds_left='65569961'/>
				    </profile_item>
				    <exp name='exp_item_01' added='50' total='8939846' offerid='30679'/>
				    <profile_item name='flashbang' profile_item_id='1020973757' offerid='30679' added_expiration='1 day' added_quantity='0' error_status='0'>
					    <item id='1020973757' name='flashbang' attached_to='0' config='dm=0;material=;pocket_index=2048' slot='5120' equipped='4' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1459621215' expiration_time_utc='1606418415' seconds_left='65656361'/>
				    </profile_item>
				    <exp name='exp_item_01' added='125' total='8939971' offerid='30680'/>
				    <profile_item name='smokegrenade04_c' profile_item_id='1020973783' offerid='30680' added_expiration='0' added_quantity='3' error_status='0'>
					    <item id='1020973783' name='smokegrenade04_c' attached_to='0' config='dm=0;material=;pocket_index=1083394' slot='23812118' equipped='29' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1476468845' quantity='446'/>
				    </profile_item>
				    <exp name='exp_item_01' added='125' total='8940096' offerid='30681'/>
				    <profile_item name='smokegrenade04_c' profile_item_id='1020973783' offerid='30681' added_expiration='0' added_quantity='1' error_status='0'>
					    <item id='1020973783' name='smokegrenade04_c' attached_to='0' config='dm=0;material=;pocket_index=1083394' slot='23812118' equipped='29' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1476468845' quantity='447'/>
				    </profile_item>
				    <exp name='exp_item_01' added='50' total='8940146' offerid='30682'/>
				    <profile_item name='claymoreexplosive04_c' profile_item_id='1020973781' offerid='30682' added_expiration='0' added_quantity='3' error_status='0'>
					    <item id='1020973781' name='claymoreexplosive04_c' attached_to='0' config='dm=0;material=;pocket_index=0' slot='0' equipped='0' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1474170565' quantity='233'/>
				    </profile_item>
			    </purchased_item>
			    <money game_money='217007' cry_money='1' crown_money='23236'/>
		    </shop_buy_multiple_offer>
         */

        public enum ErrorStatus
        {
            [Description("Ошибки нет")]           None,
            [Description("Закончились деньги")]   OutOfMoney,
            [Description("Ограничение по рангу")] Restricted,
            [Description("Распродано")]           SoldOut,

            [Description("Достигнут лимит покупок")]
            LimitReached,
            [Description("Не найдено")]   NotFound,
            [Description("Неверный тег")] BadTag,

            [Description("Истекло время ожидания")]
            Timeout = 8
        }
    }

    public static class ShopBuyOfferExtensions
    {
        public static string GetFriendlyString(this ShopBuyOffer.ErrorStatus errorStatus) =>
            errorStatus.GetDescription();
    }
}