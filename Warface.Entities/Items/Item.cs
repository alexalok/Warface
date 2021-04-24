using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using aDevLib;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Warface.Enums;
using Warface.Files.GameItems;

namespace Warface.Entities.Items
{
    public partial class Item : ICloneable

    {
        /*<item id='18212036' name='explosivegrenade' attached_to='0' config='dm=0;material=default;pocket_index=1083393' slot='163840' equipped='8' default='1' permanent='0' 
         * expired_confirmed='0' buy_time_utc='0' expiration_time_utc='0' seconds_left='0'/>
                          
          <item id='18212038' name='pt23_shop' attached_to='0' config='' slot='0' equipped='0' default='0' permanent='1' expired_confirmed='0' buy_time_utc='1431805451' 
          total_durability_points='36000' durability_points='36000'/>*/

        public const int SlotBaseValue = 1 << 30; //1073741824

        public string ID         { get; private set; }
        public string Name       { get; private set; }
        public int    AttachedTo { get; private set; }

        public string Config => GetConfig();

        public int            Slot             { get; private set; }
        public int            Equipped         { get; private set; }
        public bool           Default          { get; private set; }
        public bool           Permanent        { get; private set; }
        public bool           ExpiredConfirmed { get; private set; }
        public DateTimeOffset BuyTimeUtc       { get; private set; }

        public DateTimeOffset ExpirationTime
        {
            get
            {
                if (Type != ItemType.Temporary)
                    throw new InvalidOperationException();
                return _expirationTime;
            }
            private set => _expirationTime = value;
        }

        public int SecondsLeft
        {
            get
            {
                if (Type != ItemType.Temporary)
                    throw new InvalidOperationException();
                return _secondsLeft;
            }
            private set => _secondsLeft = value;
        }

        public int TotalDurabilityPoints
        {
            get
            {
                if (Type != ItemType.Permanent)
                    throw new InvalidOperationException();
                return _totalDurabilityPoints;
            }
            private set => _totalDurabilityPoints = value;
        }

        public int DurabilityPoints
        {
            get
            {
                if (Type != ItemType.Permanent)
                    throw new InvalidOperationException();
                return _durabilityPoints;
            }
            private set => _durabilityPoints = value;
        }

        public int Quantity
        {
            get
            {
                if (Type != ItemType.Consumable)
                    throw new InvalidOperationException();
                return _quantity;
            }
            set => _quantity = value;
        }

        public ItemType Type { get; private set; }

        [Obsolete(null, true)]
        public bool HasPocketIndex => PocketIndex != null;
        public bool IsWorn         => Equipped != 0 && Slot != SlotBaseValue;

        int? Dm { get; set; }

        string Material { get; set; }

        int? PocketIndex { get; set; } 

        int            _secondsLeft;
        int            _totalDurabilityPoints;
        int            _durabilityPoints;
        DateTimeOffset _expirationTime;
        int            _quantity;

        //temporary or default
        public Item(string         id,             string name,     int  attachedTo, int  slot,
                    int            equipped,       bool   @default, bool permanent,  bool expiredConfirmed, DateTimeOffset buyTimeUtc,
                    int?           dm,             string material, int? pocketIndex,
                    DateTimeOffset expirationTime, int    secondsLeft) :
            this(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc, dm, material, pocketIndex, ItemType.Temporary)
        {
            ExpirationTime = expirationTime;
            SecondsLeft    = secondsLeft;

            if (@default)
                Type = ItemType.Default;
        }

        //permanent
        public Item(string id,                    string name,     int  attachedTo, int  slot,
                    int    equipped,              bool   @default, bool permanent,  bool expiredConfirmed, DateTimeOffset buyTimeUtc,
                    int?   dm,                    string material, int? pocketIndex,
                    int    totalDurabilityPoints, int    durabilityPoints) :
            this(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc, dm, material, pocketIndex, ItemType.Permanent)
        {
            TotalDurabilityPoints = totalDurabilityPoints;
            DurabilityPoints      = durabilityPoints;
        }

        //consumable
        public Item(string id,       string name,     int  attachedTo, int  slot,
                    int    equipped, bool   @default, bool permanent,  bool expiredConfirmed, DateTimeOffset buyTimeUtc,
                    int?   dm,       string material, int? pocketIndex,
                    int    quantity) :
            this(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc, dm, material, pocketIndex, ItemType.Consumable)
        {
            Quantity = quantity;
        }

        //basic
        public Item(string id,               string         name,       int  attachedTo, int    slot,     int  equipped, bool @default, bool permanent,
                    bool   expiredConfirmed, DateTimeOffset buyTimeUtc, int? dm,         string material, int? pocketIndex) :
            this(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc, dm, material, pocketIndex, ItemType.Basic)
        {
        }

        Item(string id,               string         name,       int  attachedTo, int    slot,     int  equipped,    bool     @default, bool permanent,
             bool   expiredConfirmed, DateTimeOffset buyTimeUtc, int? dm,         string material, int? pocketIndex, ItemType type)
        {
            ID               = id;
            Name             = name;
            AttachedTo       = attachedTo;
            Slot             = slot;
            Equipped         = equipped;
            Default          = @default;
            Permanent        = permanent;
            ExpiredConfirmed = expiredConfirmed;
            BuyTimeUtc       = buyTimeUtc;
            Dm               = dm;
            Material         = material;
            PocketIndex      = pocketIndex;
            Type             = type;
        }

        public static Item ParseNode(HtmlNode itemNode)
        {
            //exists for all types
            string id               = itemNode.Attributes["id"].Value;
            string name             = itemNode.Attributes["name"].Value;
            int    attachedTo       = itemNode.Attributes["attached_to"].IntValue();
            string config           = itemNode.Attributes["config"].Value;
            int    slot             = itemNode.Attributes["slot"].IntValue();
            int    equipped         = itemNode.Attributes["equipped"].IntValue();
            bool   @default         = itemNode.Attributes["default"].BoolValue();
            bool   permanent        = itemNode.Attributes["permanent"].BoolValue();
            bool   expiredConfirmed = itemNode.Attributes["expired_confirmed"].BoolValue();
            var    buyTimeUtc       = DateTimeOffset.FromUnixTimeSeconds(itemNode.Attributes["buy_time_utc"].IntValue());

            var (dm, material, pocketIndex) = ParseConfig(config);

            if (permanent)
            {
                //permanent
                int totalDurabilityPoints = itemNode.Attributes["total_durability_points"].IntValue();
                int durabilityPoints      = itemNode.Attributes["durability_points"].IntValue();

                return new Items.Item(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc,
                    dm, material, pocketIndex, totalDurabilityPoints, durabilityPoints);
            }

            if (itemNode.Attributes.Contains("expiration_time_utc"))
            {
                //temporary
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(itemNode.Attributes["expiration_time_utc"].LongValue()).DateTime;
                int secondsLeft    = itemNode.Attributes["seconds_left"].IntValue();

                return new Items.Item(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc,
                    dm, material, pocketIndex, expirationTime, secondsLeft);
            }

            if (itemNode.Attributes.Contains("quantity"))
            {
                //consumable
                int quantity = itemNode.Attributes["quantity"].IntValue();
                return new Items.Item(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc,
                    dm, material, pocketIndex, quantity);
            }

            //basic
            return new Items.Item(id, name, attachedTo, slot, equipped, @default, permanent, expiredConfirmed, buyTimeUtc, dm, material, pocketIndex);
        }

        public void UpdateFromShort(HtmlNode itemShortNode)
        {
            //<item id='1210933425' name='claymoreexplosive08' attached_to='0' slot='22528' slot='dm=0;material=;pocket_index=1024'/>

            string id         = itemShortNode.Attributes["id"].Value;
            string name       = itemShortNode.Attributes["name"].Value;
            int    attachedTo = itemShortNode.Attributes["attached_to"].IntValue();
            string config     = itemShortNode.Attributes["config"].Value;
            int    slot       = itemShortNode.Attributes["slot"].IntValue();

            var (dm, material, pocketIndex) = ParseConfig(config);

            if (id != ID || name != Name)
                throw new InvalidOperationException();
            AttachedTo  = attachedTo;
            Slot        = slot;
            Dm          = dm;
            Material    = material;
            PocketIndex = pocketIndex;
        }

        public string GetItemAsShortString()
        {
            return $"<item id='{ID}' name='{Name}' attached_to='{AttachedTo}' slot='{Slot}' config='{Config}' />";
        }

        public string GetItemAsLongString()
        {
            throw new NotImplementedException();
        }

        static (int? dm, string material, int? pocketIndex) ParseConfig(string config)
        {
            //dm=0;material=;pocket_index=1083393
            var splitConfig = config.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            if (splitConfig.Length == 0)
                return (null, null, null);

            var    dmSplit = splitConfig[0].Split('=');
            string dmStr   = dmSplit[1];
            int    dm      = Convert.ToInt32(dmStr);

            var    materialSplit = splitConfig[1].Split('=');
            string material      = materialSplit[1];

            int? pocketIndex = null;
            if (splitConfig.Length == 3)
            {
                var    pocketIndexSplit = splitConfig[2].Split('=');
                string pocketIndexStr   = pocketIndexSplit[1];
                pocketIndex = Convert.ToInt32(pocketIndexStr);
            }

            return (dm, material, pocketIndex);
        }

        string GetConfig()
        {
            string config = string.Empty;
            if (Dm.HasValue) //some items have empty config
                config += $"dm={Dm};";
            if (Material != null)
                config += $"material={Material};";
            if (PocketIndex.HasValue)
                config += $"pocket_index={PocketIndex}";
            else if (!string.IsNullOrEmpty(config))
                config += "pocket_index=0";
            return config;
        }

        public BaseSlot GetBaseSlot()
        {
            if (Equipped == 0 ||
                Slot     == SlotBaseValue)
                throw new NotSupportedException("Base slot can only be determined for worn items");
            int classMultipliersSum = GetClassesMultSum();
            int baseSlotInt         = (Slot >> 30) / classMultipliersSum;
            var baseSlot            = (BaseSlot) baseSlotInt;
            return baseSlot;
        }

        int GetClassesMultSum()
        {
            int classMultipliersSum = GetEquippedClasses().Sum(GetClassMult); //slot = baseSlot * classMultSum
            return classMultipliersSum;
        }

        public IEnumerable<Class> GetEquippedClasses()
        {
            foreach (Class @class in Enum.GetValues(typeof(Class)))
            {
                if ((Equipped & (1 << (int) @class)) != 0) //Equipped AND (1 << class_id) 
                    yield return @class;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        /// <param name="baseSlot"></param>
        public void AddClass(Class @class, BaseSlot? baseSlot = null)
        {
            Slot += (int) (baseSlot ?? GetBaseSlot()) * GetClassMult(@class);

            Equipped = Equipped | (1 << (int) @class); //must be set AFTER setting slot - otherwise GetBaseSlot will return wrong value
        }

        public void RemoveClass(Class @class)
        {
            if (!GetEquippedClasses().Contains(@class))
                return;

            Slot -= (int) GetBaseSlot() * GetClassMult(@class);

            Equipped = Equipped ^ (1 << (int) @class); //must be set AFTER setting slot - otherwise GetBaseSlot will return wrong value

            if (Equipped == 0) //we removed all classes => need to remove slot and pocketIndex values
            {
                Slot = SlotBaseValue;
                PocketIndex = 0;
            }
        }

        static int GetClassMult(Class @class) => 1 << 5 * (int) @class;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}