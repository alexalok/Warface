using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomMaster
    {
        public int Master { get; }

        GameroomMaster(int master)
        {
            Master = master;
        }

        public static GameroomMaster ParseNode(HtmlNode node)
        {
            //<room_master master='14146060' revision='4'/>
            int master = node.Attributes["master"].IntValue();

            return new GameroomMaster(master);
        }
    }
}