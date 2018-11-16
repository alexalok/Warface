using System;
using System.Collections.Generic;
using System.ComponentModel;
using aDevLib.Extensions;

namespace Warface.Entities
{
    [Flags]
    public enum PlayerStatus
    {
        [Description("Не в сети")] Offline = 0,

        [Description("В сети")] Online = 1 << 0,

        [Description("Вышел")] Left = 1 << 1,

        [Description("АФК")] Afk = 1 << 2,

        [Description("В лобби")] Lobby = 1 << 3,

        [Description("В комнате")] Room = 1 << 4,

        [Description("В бою")] Playing = 1 << 5,

        [Description("в магазине")] Shop = 1 << 6,

        [Description("на складе")] Inventory = 1 << 7,

        [Description("На РМ")] Rating = 1 << 8,

        [Description("В обучении")] Tutorial = 1 << 9,
    }

    public static class PlayerStatusExtensions
    {
        public static string ToFriendlyString(this PlayerStatus status)
        {
            if (status == PlayerStatus.Offline)
                return PlayerStatus.Offline.GetDescription();

            var resultList = new List<string>();

            if (status.Has(PlayerStatus.Online))
                resultList.Add(PlayerStatus.Online.GetDescription());
            if (status.Has(PlayerStatus.Left))
                resultList.Add(PlayerStatus.Left.GetDescription());
            if (status.Has(PlayerStatus.Lobby))
                resultList.Add(PlayerStatus.Lobby.GetDescription());
            if (status.Has(PlayerStatus.Room))
                resultList.Add(PlayerStatus.Room.GetDescription());
            if (status.Has(PlayerStatus.Playing))
                resultList.Add(PlayerStatus.Playing.GetDescription());
            if (status.Has(PlayerStatus.Shop))
                resultList.Add(PlayerStatus.Shop.GetDescription());
            if (status.Has(PlayerStatus.Inventory))
                resultList.Add(PlayerStatus.Inventory.GetDescription());
            if (status.Has(PlayerStatus.Rating))
                resultList.Add(PlayerStatus.Rating.GetDescription());
            if (status.Has(PlayerStatus.Tutorial))
                resultList.Add(PlayerStatus.Tutorial.GetDescription());

            if (status.Has(PlayerStatus.Afk))
                resultList.Add(PlayerStatus.Afk.GetDescription());

            return string.Join(", ", resultList);
        }
    }
}