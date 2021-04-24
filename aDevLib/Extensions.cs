using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace aDevLib
{
    public static class Extensions
    {
        public static string GetDescription(this Enum enumMember)
        {
            return enumMember.GetType().GetMember(enumMember.ToString()).First().GetCustomAttribute<DescriptionAttribute>().Description;
        }

        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return ((int) (object) type & (int) (object) value) == (int) (object) value;
            }
            catch
            {
                return false;
            }
        }

        public static string EscapeXML(this string input)
        {
            return input.
                Replace("&",  "&amp;").
                Replace("<",  "&lt;").
                Replace(">",  "&gt;").
                Replace("\"", "&quot;").
                Replace("'",  "&apos;");
        }
    }
}
