using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Warface.Entities.Items;
using Warface.Enums;
using Warface.Files.GameItems;

namespace Warface.Entities.Loadout
{
    public abstract class BasicLoadout
    {
        [NotNull]   Item _primary;
        [NotNull]   Item _secondary;
        [NotNull]   Item _melee;
        [CanBeNull] Item _pocket1;
        [CanBeNull] Item _pocket2;
        [CanBeNull] Item _consumable1;
        [CanBeNull] Item _consumable2;
        Class            _class;

        [NotNull]
        public Item Primary
        {
            get => _primary;
            set
            {
                _primary = value;
                PropertyOnSetCheck();
            }
        }

        [NotNull]
        public Item Secondary
        {
            get => _secondary;
            set
            {
                _secondary = value;
                PropertyOnSetCheck();
            }
        }

        [NotNull]
        public Item Melee
        {
            get => _melee;
            set
            {
                _melee = value;
                PropertyOnSetCheck();
            }
        }

        [CanBeNull]
        public Item Pocket1
        {
            get => _pocket1;
            set
            {
                _pocket1 = value;
                PropertyOnSetCheck();
            }
        }

        [CanBeNull]
        public Item Pocket2
        {
            get => _pocket2;
            set
            {
                _pocket2 = value;
                PropertyOnSetCheck();
            }
        }

        [CanBeNull]
        public Item Consumable1
        {
            get => _consumable1;
            set
            {
                _consumable1 = value;
                PropertyOnSetCheck();
            }
        }

        [CanBeNull]
        public Item Consumable2
        {
            get => _consumable2;
            set
            {
                _consumable2 = value;
                PropertyOnSetCheck();
            }
        }

        public Class Class
        {
            get => _class;
            set
            {
                _class = value;
                PropertyOnSetCheck();
            }
        }

        protected List<Item> ItemsToResync { get; } = new List<Item>();

        protected BasicLoadout([NotNull] Item primary, [NotNull] Item secondary, [NotNull] Item melee, [CanBeNull] Item pocket1, [CanBeNull] Item pocket2, [CanBeNull] Item consumable1, [CanBeNull] Item consumable2, Class @class)
        {
            Primary     = primary   ?? throw new ArgumentNullException(nameof(primary));
            Secondary   = secondary ?? throw new ArgumentNullException(nameof(secondary));
            Melee       = melee     ?? throw new ArgumentNullException(nameof(melee));
            Pocket1     = pocket1;
            Pocket2     = pocket2;
            Consumable1 = consumable1;
            Consumable2 = consumable2;
            Class       = @class;
        }

        public T ApplyDesiredLoadout<T>(T desiredLoadout) where T : BasicLoadout
        {
            if (typeof(T) != GetType())
                throw new InvalidOperationException();
            foreach (var propertyInfo in GetAllItemPropertyInfos())
            {
                var currentItem = (Item) propertyInfo.GetValue(this);
                var desiredItem = (Item) propertyInfo.GetValue(desiredLoadout);

                if (propertyInfo.Name.Contains("Pocket") || propertyInfo.Name.Contains("Consumable"))
                    ApplyWithPocket(currentItem, desiredItem);
            }
            throw new NotImplementedException();
        }

        void ApplyWithPocket([CanBeNull] Item currentItem, [CanBeNull] Item desiredItem)
        {
            if (currentItem == null) //we don't have anything in this place
            {
            }

            Debug.Assert(currentItem.HasPocketIndex);
            Debug.Assert(desiredItem.HasPocketIndex);

            int  currentPocketIndex = currentItem.GetPocketOffsetForClass(Class);
            int  desiredPocketIndex = desiredItem.GetPocketOffsetForClass(Class);
            bool isSameOffset       = currentPocketIndex == desiredPocketIndex;

            if (currentItem.Name == desiredItem.Name) //same item, just change offset if needed
            {
                if (isSameOffset) //offset is the same, we're done
                {
                    return;
                }
                else //offsets are different, move our item to another slot 
                {
                    currentItem.ChangePocketOffsetForClass(Class, desiredPocketIndex);
                    ItemsToResync.Add(currentItem); //item now needs to be resynced
                }
                return;
            }
            else //names are different
            {
            }
        }

        void ApplyWithoutPocket()
        {
        }

        protected IEnumerable<PropertyInfo> GetAllItemPropertyInfos()
        {
            return GetType().
                GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(t => t.PropertyType is Item);
        }

        protected void PropertyOnSetCheck([CallerMemberName] string propName = null)
        {
            if (propName == null) throw new ArgumentNullException(nameof(propName));
            var propInfo  = GetType().GetProperty(propName);
            var propValue = (Item) propInfo.GetValue(this);
            AllowedClassesGuard(propValue, propName);
            SlotGuard(propValue, propName);
        }

        void AllowedClassesGuardAll()
        {
            foreach (var propertyInfo in GetAllItemPropertyInfos())
            {
                var propValue = (Item) propertyInfo.GetValue(this);
                AllowedClassesGuard(propValue, propertyInfo.Name);
            }
        }

        void AllowedClassesGuard(Item propertyToCheck, string propName)
        {
            if ( /*!propertyToCheck.AllowedClasses.Contains(Class)*/true)
                throw new ArgumentException($"Provided {propName} item is not valid for {Class} class");
        }

        void SlotGuardAll()
        {
            foreach (var propertyInfo in GetAllItemPropertyInfos())
            {
                var propValue = (Item) propertyInfo.GetValue(this);
                SlotGuard(propValue, propertyInfo.Name);
            }
        }

        void SlotGuard([CanBeNull] Item propertyToCheck, string propName)
        {
            //info from H:\z_warfacebackups\ru\pak\25-10-2018\GameData.pak\Libs\Config\ClassModifiersData.xml

            if (propertyToCheck == null)
                return;

            var category = ItemCategory.PrimarySkin; //propertyToCheck.ItemCategory;
            switch (propName)
            {
                case nameof(Primary):
                    CheckPrimary();
                    break;
                case nameof(Secondary):
                    CheckSecondary();
                    break;
                case nameof(Melee):
                    CheckMelee();
                    break;
                case nameof(Pocket1):
                case nameof(Pocket2):
                    CheckPocket();
                    break;
                case nameof(Consumable1):
                case nameof(Consumable2):
                    CheckConsumable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propName), propName);
            }

            void CheckPrimary()
            {
                switch (Class)
                {
                    case Class.Rifleman:
                        if (category == ItemCategory.AssaultRifle || category == ItemCategory.Machinegun)
                            return;
                        break;
                    case Class.Sniper:
                        if (category == ItemCategory.SniperRifle)
                            return;
                        break;
                    case Class.Medic:
                        if (category == ItemCategory.Shotgun)
                            return;
                        break;
                    case Class.Engineer:
                        if (category == ItemCategory.Smg)
                            return;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                throw new InvalidOperationException($"{propName} item has an invalid category for {Class} class: {category}");
            }

            void CheckSecondary()
            {
                if (category == ItemCategory.Pistol)
                    return;
                throw new InvalidOperationException($"{propName} item has an invalid category: {category}");
            }

            void CheckMelee()
            {
                if (category == ItemCategory.Melee)
                    return;
                throw new InvalidOperationException($"{propName} item has an invalid category: {category}");
            }

            void CheckPocket()
            {
                switch (category)
                {
                    case ItemCategory.Explosive:
                    case ItemCategory.Grenade:
                    case ItemCategory.Medkit when Class == Class.Medic:
                        return;
                    default:
                        throw new InvalidOperationException($"{propName} item has an invalid category for {Class} class: {category}");
                }
            }

            void CheckConsumable()
            {
                if (category == ItemCategory.Explosive || category == ItemCategory.Grenade)
                    return;
                throw new InvalidOperationException($"{propName} item has an invalid category: {category}");
            }
        }
    }
}