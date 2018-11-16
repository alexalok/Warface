using System;
using System.Xml;
using JetBrains.Annotations;

namespace Warface.Files.GameItems
{
    public partial struct GameItem
    {
        public string     Name          { get; }
        public Type       ItemType      { get; }
        public UIStats?   ItemUIStats   { get; }
        public RandomBox? ItemRandomBox { get; }
        public MmoStats?  ItemMmoStats  { get; }

        public GameItem(string name, Type itemType, [CanBeNull] UIStats? uiStats, RandomBox? itemRandomBox, MmoStats? itemMmoStats)
        {
            Name          = name;
            ItemType      = itemType;
            ItemUIStats   = uiStats;
            ItemRandomBox = itemRandomBox;
            ItemMmoStats  = itemMmoStats;
        }

        public static GameItem ParseText(string xml, out bool isParsed)
        {
            isParsed = false;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var gameItemNode = xmlDoc.LastChild;

            string name = gameItemNode.Attributes["name"]?.Value;
            if (name == null)
                return default;

            var    type    = Type.Unspecified;
            string typeStr = gameItemNode.Attributes["type"]?.Value;
            if (!string.IsNullOrWhiteSpace(typeStr) && !Enum.TryParse(typeStr.Replace("_", string.Empty), true, out type))
                throw new NotSupportedException("Unknown type: " + typeStr);

            UIStats? uiStats     = null;
            var      uiStatsNode = gameItemNode.SelectSingleNode("./UI_stats");
            if (uiStatsNode != null)
                uiStats = UIStats.ParseNode(uiStatsNode);

            RandomBox? randomBox = null;
            if (type == Type.RandomBox)
            {
                var randomBoxNode = gameItemNode.SelectSingleNode("./random_box");
                randomBox = RandomBox.ParseNode(randomBoxNode);
            }

            MmoStats? mmoStats     = null;
            var       mmoStatsNode = gameItemNode.SelectSingleNode("./mmo_stats");
            if (mmoStatsNode != null)
                mmoStats = MmoStats.ParseNode(mmoStatsNode);

            isParsed = true;
            return new GameItem(name, type, uiStats, randomBox, mmoStats);
        }

        /*
            <?xml version="1.0" ?>
              <GameItem name="shared_helmet_hlw_01" type="armor">
                  <mmo_stats>
                      <param name="item_category" value="Helmet"/>
                      <param name="shopcontent" value="1"/>
                      <param name="classes" value="MERS"/>
                      <param name="durability" value="36000"/>
                      <param name="repair_cost" value="624"/>
                  </mmo_stats>
                  <UI_stats>
                      <param name="name" value="@ui_armor_shared_helmet_hlw_01_name"/>
                      <param name="description" value="@ui_armor_shared_helmet_hlw_01"/>
                      <param name="icon" value="shared_helmet_hlw_01"/>
                  </UI_stats>
                  <slots>
                      <slot main="1" name="helmet">
                          <Materials>
                              <Material default="1" display_name="grey" icon="mat_grey" name="default" surface_type="mat_armor_head"/>
                          </Materials>
                          <assets>
                              <asset display="all" mode="tp" name="shared_helmet_hlw_01" teamId="1"/>
                              <asset display="all" mode="tp" name="shared_helmet_hlw_01_b" teamId="2"/>
                          </assets>
                      </slot>
                  </slots>
                  <GameParams>
                      <param name="blind_amount_mul" value="0.3"/>
                      <param name="flashbang_time_mul" value="0.4"/>
                      <param name="res_head" value="0.6"/>
                      <param name="claymore_detector_radius" value="10.0"/>
                  </GameParams>
              </GameItem>
            */
    }
}