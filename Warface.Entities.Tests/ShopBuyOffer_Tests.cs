using System.Collections.Generic;
using HtmlAgilityPack;
using Warface.Entities.Shop;
using Xunit;

namespace Warface.Entities.Tests
{
    public class ShopBuyOffer_Tests
    {
        [Theory]
        [MemberData(nameof(ShopBuyOffer_Tests_Resources.MultipleGetNodeAndExpectedValues), MemberType = typeof(ShopBuyOffer_Tests_Resources))]
        void ShopBuyOffer_ParseNode(HtmlNode shopBuyOfferNode, int expectedError, int expectedGameMoney, int expectedCryMoney, int expectedCrownMoney)
        {
            var shopBuyOffer = ShopBuyOffer.ParseNode(shopBuyOfferNode);

            Assert.Equal(expectedError,      (int) shopBuyOffer.Error);
            Assert.Equal(expectedGameMoney,  shopBuyOffer.GameMoney);
            Assert.Equal(expectedCryMoney,   shopBuyOffer.CryMoney);
            Assert.Equal(expectedCrownMoney, shopBuyOffer.CrownMoney);
        }
    }

    public static class ShopBuyOffer_Tests_Resources
    {
        public static IEnumerable<object[]> MultipleGetNodeAndExpectedValues()
        {
            var shopBuyOfferNode = HtmlNode.CreateNode("<shop_buy_offer offer_id='31348' error_status='0'> <purchased_item> <profile_item name='shared_helmet_hlw_01' profile_item_id='1264206484' offerId='31348' added_expiration='1 day' added_quantity='0' error_status='0'> <item id='1264206484' name='shared_helmet_hlw_01' attached_to='0' config='' slot='0' equipped='0' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1540570204' expiration_time_utc='1540656604' seconds_left='86399'/> </profile_item> </purchased_item> <money game_money='221207' cry_money='1' crown_money='23236'/> </shop_buy_offer>");
            yield return new object[] {shopBuyOfferNode, 0, 221207, 1, 23236};

            var shopBuyMultipleOfferNode = HtmlNode.CreateNode("<shop_buy_multiple_offer error_status='0'> <purchased_item> <exp name='exp_item_01' added='50' total='8939796' offerid='30678'/> <profile_item name='flashbang' profile_item_id='1020973757' offerid='30678' added_expiration='3 day' added_quantity='0' error_status='0'> <item id='1020973757' name='flashbang' attached_to='0' config='dm=0;material=;pocket_index=2048' slot='5120' equipped='4' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1459621215' expiration_time_utc='1606332015' seconds_left='65569961'/> </profile_item> <exp name='exp_item_01' added='50' total='8939846' offerid='30679'/> <profile_item name='flashbang' profile_item_id='1020973757' offerid='30679' added_expiration='1 day' added_quantity='0' error_status='0'> <item id='1020973757' name='flashbang' attached_to='0' config='dm=0;material=;pocket_index=2048' slot='5120' equipped='4' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1459621215' expiration_time_utc='1606418415' seconds_left='65656361'/> </profile_item> <exp name='exp_item_01' added='125' total='8939971' offerid='30680'/> <profile_item name='smokegrenade04_c' profile_item_id='1020973783' offerid='30680' added_expiration='0' added_quantity='3' error_status='0'> <item id='1020973783' name='smokegrenade04_c' attached_to='0' config='dm=0;material=;pocket_index=1083394' slot='23812118' equipped='29' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1476468845' quantity='446'/> </profile_item> <exp name='exp_item_01' added='125' total='8940096' offerid='30681'/> <profile_item name='smokegrenade04_c' profile_item_id='1020973783' offerid='30681' added_expiration='0' added_quantity='1' error_status='0'> <item id='1020973783' name='smokegrenade04_c' attached_to='0' config='dm=0;material=;pocket_index=1083394' slot='23812118' equipped='29' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1476468845' quantity='447'/> </profile_item> <exp name='exp_item_01' added='50' total='8940146' offerid='30682'/> <profile_item name='claymoreexplosive04_c' profile_item_id='1020973781' offerid='30682' added_expiration='0' added_quantity='3' error_status='0'> <item id='1020973781' name='claymoreexplosive04_c' attached_to='0' config='dm=0;material=;pocket_index=0' slot='0' equipped='0' default='0' permanent='0' expired_confirmed='0' buy_time_utc='1474170565' quantity='233'/> </profile_item> </purchased_item> <money game_money='217007' cry_money='1' crown_money='23236'/> </shop_buy_multiple_offer>");
            yield return new object[] {shopBuyMultipleOfferNode, 0, 217007, 1, 23236};
        }
    }
}