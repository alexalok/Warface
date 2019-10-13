using System;
using System.Collections.Generic;
using Warface.Entities.Items;
using Warface.Files.GameItems;

namespace Warface.Entities.Extensions
{
    public static class GameItemExtensions
    {
        public static Item.BaseSlot? GetBaseSlot(this GameItem gameItem)
        {
            if (!gameItem.ItemMmoStats.HasValue)
                throw new ArgumentException($"{nameof(Item.BaseSlot)} can only be determined for items with {nameof(GameItem.ItemMmoStats)}");

            switch (gameItem.ItemMmoStats.Value.ItemCategory)
            {
                case ItemCategory.AssaultRifle:
                case ItemCategory.Machinegun:
                case ItemCategory.Shotgun:
                case ItemCategory.Smg:
                case ItemCategory.SniperRifle:
                    return Item.BaseSlot.Primary;
                case ItemCategory.Pistol:
                    return Item.BaseSlot.Pistol;
                case ItemCategory.Melee:
                    return Item.BaseSlot.Knife;
                case ItemCategory.Medkit:
                case ItemCategory.ArmorKit:
                case ItemCategory.AmmoPack:
                    return Item.BaseSlot.PocketKit;
                case ItemCategory.FragGrenade:
                    return Item.BaseSlot.PocketFragGrenade;
                case ItemCategory.FlashGrenade:
                    return Item.BaseSlot.PocketFlashGrenade;
                case ItemCategory.SmokeGrenade:
                    return Item.BaseSlot.PocketSmokeGrenade;
                case ItemCategory.Claymore:
                    return Item.BaseSlot.PocketClaymore;
                case ItemCategory.Defibrillator:
                    return Item.BaseSlot.PocketSpecial;
                case ItemCategory.Helmet:
                    return Item.BaseSlot.Helmet;
                case ItemCategory.Vest:
                    return Item.BaseSlot.Vest;
                case ItemCategory.Gloves:
                    return Item.BaseSlot.Gloves;
                case ItemCategory.Boots:
                    return Item.BaseSlot.Boots;
                default:
                    return null;
            }
        }
    }
}