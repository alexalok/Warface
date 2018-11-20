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
                    return Item.BaseSlot.Melee;
                case ItemCategory.Medkit:
                    return Item.BaseSlot.Pocket;
                case ItemCategory.Explosive:
                case ItemCategory.Grenade:
                    return gameItem.ItemMmoStats.Value.Stackable == true ? Item.BaseSlot.Consumables : Item.BaseSlot.Pocket;
                case ItemCategory.ArmorKit:
                case ItemCategory.AmmoPack:
                case ItemCategory.Defibrillator:
                    return Item.BaseSlot.Special;
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