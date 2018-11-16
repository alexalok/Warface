using System;
using System.ComponentModel;
using aDevLib.Extensions;

namespace Warface.Entities
{
    public enum Currency
    {
        [Description("Варбаксы")] Wardollars,
        [Description("Кредиты")]  Kredits,
        [Description("Короны")]   Crowns
    }

    public static class CurrencyExtensions
    {
        public static string GetFriendlyName(this Currency currency, bool shortVariant = false)
        {
            if (!shortVariant)
                return currency.GetDescription();

            switch (currency)
            {
                case Currency.Wardollars:
                    return "W$";
                case Currency.Kredits:
                    return "K";
                case Currency.Crowns:
                    return "КРН";
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency), currency, null);
            }
        }
    }
}