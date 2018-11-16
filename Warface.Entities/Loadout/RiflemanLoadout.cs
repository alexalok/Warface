using System.Collections.Generic;
using JetBrains.Annotations;
using Warface.Entities.Items;
using Warface.Enums;

namespace Warface.Entities.Loadout
{
    public class RiflemanLoadout : BasicLoadout
    {
        public Item Special { get; }

        public RiflemanLoadout(Item             primary,     Item             secondary, Item melee, Item special,
                               [CanBeNull] Item pocket1,     [CanBeNull] Item pocket2,
                               [CanBeNull] Item consumable1, [CanBeNull] Item consumable2) :
            base(primary, secondary, melee, pocket1, pocket2, consumable1, consumable2, Class.Rifleman)
        {
            Special = special;
        }
    }
}