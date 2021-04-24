using System;
using System.Globalization;
using HtmlAgilityPack;

namespace aDevLib
{
    public static class HtmlAttributeExtensions
    {
        public static bool BoolValue(this HtmlAttribute attribute)
        {
            return Convert.ToInt32(attribute.Value) == 1;
        }

        public static int IntValue(this HtmlAttribute attribute)
        {
            return Convert.ToInt32(attribute.Value);
        }

        public static uint UIntValue(this HtmlAttribute attribute)
        {
            return Convert.ToUInt32(attribute.Value);
        }

        public static bool TryIntValue(this HtmlAttribute attribute, out int value)
        {
            return int.TryParse(attribute.Value, out value);
        }

        public static long LongValue(this HtmlAttribute attribute)
        {
            return Convert.ToInt64(attribute.Value);
        }

        public static bool TryLongValue(this HtmlAttribute attribute, out long value)
        {
            return long.TryParse(attribute.Value, out value);
        }

        public static ulong ULongValue(this HtmlAttribute attribute)
        {
            return Convert.ToUInt64(attribute.Value);
        }

        public static bool TryULongValue(this HtmlAttribute attribute, out ulong value)
        {
            return ulong.TryParse(attribute.Value, out value);
        }

        public static float FloatValue(this HtmlAttribute attribute)
        {
            return Convert.ToSingle(attribute.Value, CultureInfo.InvariantCulture);
        }

        public static double DoubleValue(this HtmlAttribute attribute)
        {
            return Convert.ToDouble(attribute.Value, CultureInfo.InvariantCulture);
        }

        public static TEnum EnumValue<TEnum>(this HtmlAttribute attribute) where TEnum : Enum
        {
            if (attribute.TryIntValue(out int intValue))
                return (TEnum) (object) intValue;
            return (TEnum) Enum.Parse(typeof(TEnum), attribute.Value, true);
        }
    }
}
