namespace Warface.Entities.Items
{
    public partial class Item
    {
        public enum BaseSlot
        {
            //Weapons
            Primary = 1,
            Special = 2,
            Pistol = 3,
            Melee = 4,
            Pocket = 5,
            Consumables = 22,

            //Armor
            Helmet = 12,
            Vest = 17,
            Gloves = 7,
            Boots = 16,

            //cosmetics
            CharacterSkins = 23,
        }
    }
}