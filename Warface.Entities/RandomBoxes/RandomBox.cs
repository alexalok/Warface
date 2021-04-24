using System;
using System.Collections.Generic;
using System.Linq;
using aDevLib;
using JetBrains.Annotations;
using Warface.Entities.Shop;

namespace Warface.Entities.RandomBoxes
{
    public class RandomBox
    {
        string _friendlyName;

        readonly List<ShopOffer> _offers = new List<ShopOffer>();

        [CanBeNull]
        public string FriendlyName
        {
            get => _friendlyName?.EscapeXML();
            set => _friendlyName = value;
        }

        [CanBeNull]
        public string Name => _offers.FirstOrDefault()?.Name;

        public bool HasGoldenItem => _offers.FirstOrDefault()?.
                WinItems.Any(winItem => winItem.Name.Contains("gold")) ??
            throw new RandomBoxHasNoOffersException();

        public bool HasGoldenItemInfo => HasAnyOffer;

        public Currency Currency => _offers.FirstOrDefault()?.Currency ??
            throw new RandomBoxHasNoOffersException();

        public bool HasCurrencyInfo => HasAnyOffer;


        public IEnumerable<ShopOffer> GetOffers() => _offers.ToList();

        bool HasAnyOffer => _offers.Any();

        public void AddOrUpdateOffer(ShopOffer rbOffer)
        {
            lock (_offers)
            {
                var existingOffer = _offers.FirstOrDefault(o => o.Id == rbOffer.Id);
                if (existingOffer != null) //this offer exists
                {
                    _offers.Remove(existingOffer);
                    _offers.Add(rbOffer);
                    return;
                }

                //this offer doesn't exist
                if (_offers.Any() && _offers.First().Name != rbOffer.Name) //if there are offers already, check they're for the same item
                    throw new InvalidOperationException();

                _offers.Add(rbOffer);
            }
        }
    }
}