using System.Xml;

namespace Warface.Files.GameItems
{
    public partial struct GameItem
    {
        public struct UIStats
        {
            public string Name { get; }

            UIStats(string name)
            {
                Name = name;
            }

            public static UIStats ParseNode(XmlNode uiStatsNode)
            {
                string uiStatsName = null;

                var paramNodes = uiStatsNode.ChildNodes;
                foreach (XmlNode paramNode in paramNodes)
                {
                    if (paramNode.NodeType == XmlNodeType.Comment ||
                        paramNode.NodeType == XmlNodeType.Text)
                    {
                        continue;
                    }

                    string nameAttr  = paramNode.Attributes["name"]?.Value;
                    string valueAttr = paramNode.Attributes["value"].Value;
                    if (nameAttr == "name")
                        uiStatsName = valueAttr.TrimStart('@');
                }
                if (!string.IsNullOrWhiteSpace(uiStatsName))
                    return new UIStats(uiStatsName);
                return default;
            }

            /*
             <UI_stats>
               <param name="name" value="@ui_armor_shared_helmet_hlw_01_name"/>
               <param name="description" value="@ui_armor_shared_helmet_hlw_01"/>
               <param name="icon" value="shared_helmet_hlw_01"/>
             </UI_stats>
             */
        }
    }
}