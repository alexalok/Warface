using System;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomMission
    {
        public string     Key        { get; }
        public string     Mode       { get; }
        public string     Type       { get; }
        public string     ModeName   { get; }
        public string     Name       { get; }
        public Difficulty Difficulty { get; }
        public string     Image      { get; }

        //[CanBeNull]
        //public string GetFriendlyName() //null if mission list is not loaded or pve refreshed
        //{
        //    string key = Key;
        //    return _missionsController.List.FirstOrDefault(m => m.Key == key)?.Type;
        //}

        GameroomMission(string key, string mode, string type, string modeName, string name, Difficulty difficulty, string image)
        {
            Key        = key;
            Mode       = mode;
            Type       = type;
            ModeName   = modeName;
            Name       = name;
            Difficulty = difficulty;
            Image      = image;
        }

        public static GameroomMission ParseNode(HtmlNode gameroomMissionNode)
        {
            /*<mission mission_key='cd54d2eb-f00e-4ccc-bbd4-d4c0f2cc935e' no_teams='0' name='@pvp_mission_display_name_stm_wharf' 
             * setting='pvp/stm_wharf' mode='stm' mode_name='@pvp_stm_game_mode_desc' mode_icon='tdm_icon' 
             * description='@pvp_stm_mission_desc' image='mapImgStmWharf' difficulty='normal' type='' time_of_day='09.30' revision='668'>*/

            string key      = gameroomMissionNode.Attributes["mission_key"].Value;
            string mode     = gameroomMissionNode.Attributes["mode"].Value;
            string modeName = gameroomMissionNode.Attributes["mode_name"].Value;
            string name     = gameroomMissionNode.Attributes["name"].Value;
            Enum.TryParse(gameroomMissionNode.Attributes["name"].Value, true, out Difficulty difficulty);
            string type  = gameroomMissionNode.Attributes["type"].Value;
            string image = gameroomMissionNode.Attributes["image"].Value;

            return new GameroomMission(key, mode, type, modeName, name, difficulty, image);
        }
    }
}