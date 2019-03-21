using System;

namespace Warface.Entities
{
    public enum RuRegion
    {
        Global,
        Krasnodar,
        Novosibirsk,
        Khabarovsk
    }

    public static class RuRegionExtensions
    {
        public static string ToFriendlyString(this RuRegion region)
        {
            switch (region)
            {
                case RuRegion.Global:
                    return "Москва";
                case RuRegion.Krasnodar:
                    return "Краснодар";
                case RuRegion.Novosibirsk:
                    return "Новосибирск";
                case RuRegion.Khabarovsk:
                    return "Хабаровск";
                default:
                    throw new ArgumentOutOfRangeException(nameof(region), region, null);
            }
        }
    }
}