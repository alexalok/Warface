using System;
using Warface.Entities.Items;

namespace Warface.Entities.Items
{
    public partial class Item
    {
        public enum BaseSlot
        {
            //Weapons
            Primary = 1,
            Pistol = 3,
            Knife = 4,

            //Armor
            Gloves = 7,
            Helmet = 12,
            Boots = 16,
            Vest = 17,

            //cosmetics
            Skin = 23,

            //new pockets
            PocketFragGrenade = 29,
            PocketFlashGrenade = 30,
            PocketSmokeGrenade = 31,
            PocketClaymore = 32,
            PocketKit = 33,
            PocketSpecial = 34,
        }
    }


    public static class BaseSlotExtensions
    {
        public static bool IsMandatory(this Item.BaseSlot slot)
        {
            switch (slot)
            {
                case Item.BaseSlot.Primary:
                case Item.BaseSlot.Pistol:
                case Item.BaseSlot.Knife:
                case Item.BaseSlot.Gloves:
                case Item.BaseSlot.Helmet:
                case Item.BaseSlot.Boots:
                case Item.BaseSlot.Vest:
                case Item.BaseSlot.PocketKit: // mandatory if class has it
                case Item.BaseSlot.PocketSpecial: // mandatory if class has it
                    return true;
                case Item.BaseSlot.PocketFragGrenade:
                case Item.BaseSlot.PocketFlashGrenade:
                case Item.BaseSlot.PocketSmokeGrenade:
                case Item.BaseSlot.PocketClaymore:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }
    }
}