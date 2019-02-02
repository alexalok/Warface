using System.Linq;
using HtmlAgilityPack;
using Warface.Entities.Sponsors;
using Xunit;

namespace Warface.Entities.Tests
{
    public class SponsorInfoUpdated_Tests
    {
        [Fact]
        void SponsorInfoUpdated_ParseNode()
        {
            var node               = HtmlNode.CreateNode("<sponsor_info_updated sponsor_id='2' sponsor_points='128' total_sponsor_points='84653' next_unlock_item='ugl01'> <unlocked_items> <item id='18446744073709551455' name='bn02' attached_to='0' config='' slot='20' equipped='1' default='0' permanent='0' expired_confirmed='0' buy_time_utc='0' expiration_time_utc='0' seconds_left='0' profile_item_id='18446744073709551455'/> </unlocked_items> </sponsor_info_updated>");
            var sponsorInfoUpdated = SponsorInfoUpdated.ParseNode(node);
            Assert.Single(sponsorInfoUpdated.UnlockedItems);
        }

    }
}