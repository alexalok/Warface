using System;

namespace Warface.Enums
{
    public enum Class
    {
        Rifleman, //cannot be flags bc rifleman is 0
        Heavy,
        Sniper,
        Medic,
        Engineer,
    };

    public static class ClassExtensions
    {
        public static Class ToClass(this string internalName)
        {
            if (Enum.TryParse<Class>(internalName, false, out var @class))
                return @class;
            if (internalName.ToLowerInvariant() == "recon")
                return Class.Sniper;
            throw new ArgumentOutOfRangeException(nameof(internalName), internalName, "Unknown class name");
        }

        public static string ToFriendlyString(this Class @class, RussianCase @case = RussianCase.Nominative)
        {
            switch (@class)
            {
                case Class.Rifleman:
                    switch (@case)
                    {
                        case RussianCase.Nominative:
                            return "Штурмовик";
                        case RussianCase.Genitive:
                            return "Штурмовика";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
                    }
                case Class.Medic:
                    switch (@case)
                    {
                        case RussianCase.Nominative:
                            return "Медик";
                        case RussianCase.Genitive:
                            return "Медика";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
                    }
                case Class.Engineer:
                    switch (@case)
                    {
                        case RussianCase.Nominative:
                            return "Инженер";
                        case RussianCase.Genitive:
                            return "Инженера";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
                    }
                case Class.Sniper:
                    switch (@case)
                    {
                        case RussianCase.Nominative:
                            return "Снайпер";
                        case RussianCase.Genitive:
                            return "Снайпера";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
                    }
                case Class.Heavy:
                    switch (@case)
                    {
                        case RussianCase.Nominative:
                            return "СЭД";
                        case RussianCase.Genitive:
                            return "СЭДа";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(@case), @case, null);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(@class), @class, null);
            }
        }
    }
}