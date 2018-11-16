using System;

namespace Warface.Enums
{
    public enum Class
    {
        Rifleman, //cannot be flags bc rifleman is 0
        Sniper = 2,
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
    }
}