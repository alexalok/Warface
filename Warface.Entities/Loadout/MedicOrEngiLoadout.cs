using JetBrains.Annotations;
using Warface.Entities.Items;
using Warface.Enums;

namespace Warface.Entities.Loadout
{
    public abstract class MedicOrEngiLoadout : BasicLoadout
    {
        [CanBeNull]
        public Item Pocket3 { get; }

        public MedicOrEngiLoadout(Item             primary,     Item             secondary, Item             melee,
                                  [CanBeNull] Item pocket1,     [CanBeNull] Item pocket2,   [CanBeNull] Item pocket3,
                                  [CanBeNull] Item consumable1, [CanBeNull] Item consumable2,
                                  Class @class) :
            base(primary, secondary, melee, pocket1, pocket2, consumable1, consumable2, @class)
        {
            Pocket3 = pocket3;
        }
    }
}