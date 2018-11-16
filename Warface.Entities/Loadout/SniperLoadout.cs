using JetBrains.Annotations;
using Warface.Entities.Items;
using Warface.Enums;

namespace Warface.Entities.Loadout
{
    public class SniperLoadout : BasicLoadout
    {
        public SniperLoadout(Item             primary,     Item             secondary, Item melee,
                             [CanBeNull] Item pocket1,     [CanBeNull] Item pocket2,
                             [CanBeNull] Item consumable1, [CanBeNull] Item consumable2) :
            base(primary, secondary, melee, pocket1, pocket2, consumable1, consumable2, Class.Sniper)
        {
        }
    }
}