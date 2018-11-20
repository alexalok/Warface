using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Warface.Enums;

namespace Warface.Files.GameItems
{
    public partial struct GameItem
    {
        public struct MmoStats
        {
            public ItemCategory? ItemCategory { get; }

            public bool? ShopContent { get; }

            public IEnumerable<Class> Classes { get; }

            public int? Durability { get; }

            public int?  RepairCost { get; }
            public bool? Stackable  { get; }

            public MmoStats(ItemCategory? itemCategory, bool? shopContent, IEnumerable<Class> classes, int? durability, int? repairCost, bool? stackable)
            {
                ItemCategory = itemCategory;
                ShopContent  = shopContent;
                Classes      = classes;
                Durability   = durability;
                RepairCost   = repairCost;
                Stackable    = stackable;
            }

            public static MmoStats ParseNode(XmlNode mmoStatsNode)
            {
                ItemCategory? itemCategory = null;
                bool?         shopContent  = null;
                List<Class>   classes      = null;
                int?          durability   = null;
                int?          repairCost   = null;
                bool?         stackable    = null;

                var paramNodes = mmoStatsNode.SelectNodes("./param");
                foreach (XmlNode paramNode in paramNodes)
                {
                    string name  = paramNode.Attributes["name"].Value;
                    string value = paramNode.Attributes["value"].Value;
                    switch (name)
                    {
                        case "item_category":
                        case "category":
                            itemCategory = string.IsNullOrWhiteSpace(value) ?
                                (ItemCategory?) null : (ItemCategory) Enum.Parse(typeof(ItemCategory), value, true);
                            break;
                        case "shopcontent":
                            shopContent = value == "1";
                            break;
                        case "classes":
                            classes = new List<Class>(4);
                            foreach (char classChar in value)
                            {
                                switch (classChar)
                                {
                                    case 'S':
                                        classes.Add(Class.Sniper);
                                        break;
                                    case 'R':
                                        classes.Add(Class.Rifleman);
                                        break;
                                    case 'M':
                                        classes.Add(Class.Medic);
                                        break;
                                    case 'E':
                                        classes.Add(Class.Engineer);
                                        break;
                                    case 'H':
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(classChar), classChar, "Unknown class");
                                }
                            }
                            break;
                        case "durability":
                            durability = (int?) Convert.ToUInt32(value);
                            break;
                        case "repair_cost":
                            repairCost = (int?) Convert.ToUInt32(value);
                            break;
                        case "stackable":
                            stackable = value == "1";
                            break;
                    }
                }

                Debug.Assert(classes?.Any() == true, nameof(classes) + ".Any()");

                return new MmoStats(itemCategory, shopContent, classes, durability, repairCost, stackable);
            }

            /*
             * <mmo_stats>
			        <param name="item_category" value="Pistol"/>
			        <param name="shopcontent" value="1"/>
			        <param name="classes" value="SRME"/>
			        <param name="durability" value="36000"/>
			        <param name="repair_cost" value="2000"/>
		        </mmo_stats>
             */
        }
    }
}