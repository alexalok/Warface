using System;
using aDevLib.Extensions;

namespace Warface.Files.GameItems
{
    public enum ItemCategory
    {
        AssaultRifle,
        Machinegun,
        Shotgun,
        Smg,
        SniperRifle,

        Pistol,
        Melee,

        ArmorKit,
        AmmoPack,
        Medkit,

        FragGrenade,
        FlashGrenade,
        SmokeGrenade,
        Claymore,

        Defibrillator,

        Helmet,
        Vest,
        Gloves,
        Boots,

        PrimarySkin,
        SecondarySkin,
        MeleeSkin,
        Skin, //character skin

        PveOnlyShared,
        RandomBox,
        Attachment,
        Face,
        Decal,
        Misc,
        Emblem,
        Booster,
        Bundle,
        Contract,
        TopPrizeToken,
        ResurrectionCoins,
        MissionAccessToken,
        ClanCreation,
        Explosive,
        Grenade
    }
    public static class ItemCategoryExtensions
    {
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public static ItemCategory ParseGameString(string gameString)
        {
            if(!Enum.TryParse(gameString, true, out ItemCategory category)
            && !Enum.TryParse(gameString.Replace("_", ""), true, out category))
            {
                throw new ArgumentOutOfRangeException(nameof(gameString));
            }
            return category;
        }
    }
}