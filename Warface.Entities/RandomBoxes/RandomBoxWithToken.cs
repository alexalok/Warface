using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Warface.Entities.Items;

namespace Warface.Entities.RandomBoxes
{
    public partial class RandomBoxWithToken : RandomBox
    {
        Item _item;

        public string TokenName { get; }

        public int OpenedQuantity
        {
            get
            {
                if (!IsTokenLoaded)
                    throw new InvalidOperationException();
                return _item.Quantity;
            }
            set
            {
                if (!IsTokenLoaded)
                    throw new InvalidOperationException();
                _item.Quantity = value;
            }
        }

        public bool IsTokenLoaded => _item != null;

        public RandomBoxWithToken(string tokenName)
        {
            TokenName = tokenName;
        }


        public RandomBoxWithToken(Item tokenItem)
        {
            TokenName = tokenItem.Name;
            SetItem(tokenItem);
        }


        public void SetItem(Item item)
        {
            _item = item;
        }

        /// <summary>
        /// Indicates whether a random box with a given token exists in an internal token dictionary
        /// </summary>
        /// <returns></returns>
        public static bool IsMonitored(string itemName) =>
            Dictionary.ContainsValue(itemName);

        [CanBeNull]
        public static string GetTokenByName(string offerName)
        {
            Dictionary.TryGetValue(offerName, out string token);
            return token;
        }
    }
}