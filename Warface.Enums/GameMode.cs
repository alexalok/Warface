using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using aDevLib.Extensions;

namespace Warface.Enums
{
    public enum GameMode
    {
        [Description("Командный бой")]     Tdm,
        [Description("Подрыв")]            Ptb,
        [Description("Штурм")]             Stm,
        [Description("Доминация")]         Dmn,
        [Description("Мясорубка")]         Ffa,
        [Description("Выживание")]         Hnt,
        [Description("Захват")]            Ctf,
        [Description("Уничтожение")]       Dst,
        [Description("Блиц")]              Tbs,
        [Description("Королевская битва")] Lms
    }

    public static class GameModeExtensions
    {
        public static string ToInternalString(this GameMode gameMode)
        {
            return gameMode.ToString().ToLowerInvariant();
        }

        public static string ToFriendlyString(this GameMode gameMode)
        {
            return gameMode.GetDescription();
        }

        public static GameMode FromString(string gameModeAsString)
        {
            if (Enum.TryParse<GameMode>(gameModeAsString, true, out var res))
                return res;

            return typeof(GameMode).GetEnumValues().Cast<GameMode>().First(g => g.GetDescription() == gameModeAsString);
        }
    }
}