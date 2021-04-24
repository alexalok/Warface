using aDevLib;
using HtmlAgilityPack;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomRegions
    {
        public string RegionID { get; }
        public int    Revision { get; }

        public GameroomRegions(string regionID, int revision)
        {
            RegionID = regionID;
            Revision = revision;
        }

        public static GameroomRegions ParseNode(HtmlNode regionsNode)
        {
            //<regions region_id='global' revision='410'/>
            string regionID = regionsNode.Attributes["region_id"].Value;
            int    revision = regionsNode.Attributes["revision"].IntValue();

            return new GameroomRegions(regionID, revision);
        }
    }
}