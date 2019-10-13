using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomCustomParams
    {
        /*<custom_params friendly_fire='0' enemy_outlines='0' auto_team_balance='1' dead_can_chat='1' join_in_the_process='1' 
             * max_players='16' round_limit='0' preround_time='-1' class_restriction='253' inventory_slot='2113929215' 
             * locked_spectator_camera='0' high_latency_autokick='1' overtime_mode='0' revision='502'/>*/

        public bool FriendlyFire          { get; }
        public bool EnemyOutlines         { get; }
        public bool AutoTeamBalance       { get; }
        public bool JoinInTheProcess      { get; }
        public int  RoundLimit            { get; }
        public int  PreroundTime          { get; }
        public ulong  InventorySlot         { get; }
        public bool LockedSpectatorCamera { get; }
        public bool HighLatencyAutokick   { get; }
        public bool OvertimeMode          { get; }
        public int  MaxPlayers            { get; }
        public int  ClassRestriction      { get; } //TODO to enum?

        GameroomCustomParams(
            bool friendlyFire, bool enemyOutlines, bool autoTeamBalance, bool joinInTheProcess,
            int  roundLimit,   int  preroundTime,  ulong  inventorySlot,   bool lockedSpectatorCamera, bool highLatencyAutokick,
            bool overtimeMode, int  maxPlayers,    int  classRestriction)
        {
            FriendlyFire          = friendlyFire;
            EnemyOutlines         = enemyOutlines;
            AutoTeamBalance       = autoTeamBalance;
            JoinInTheProcess      = joinInTheProcess;
            RoundLimit            = roundLimit;
            PreroundTime          = preroundTime;
            InventorySlot         = inventorySlot;
            LockedSpectatorCamera = lockedSpectatorCamera;
            HighLatencyAutokick   = highLatencyAutokick;
            OvertimeMode          = overtimeMode;
            MaxPlayers            = maxPlayers;
            ClassRestriction      = classRestriction;
        }

        public IEnumerable<Class> GetAllowedClasses()
        {
            foreach (int classPosition in typeof(Class).GetEnumValues())
            {
                var classValue = (int) Math.Pow(2, classPosition);
                if ((ClassRestriction & classValue) == classValue)
                    yield return (Class) classPosition;
            }
        }

        public static GameroomCustomParams ParseNode(HtmlNode customParamsNode)
        {
            bool friendlyFire          = customParamsNode.Attributes["friendly_fire"].BoolValue();
            bool enemyOutlines         = customParamsNode.Attributes["enemy_outlines"].BoolValue();
            bool autoTeamBalance       = customParamsNode.Attributes["auto_team_balance"].BoolValue();
            bool joinInTheProcess      = customParamsNode.Attributes["join_in_the_process"].BoolValue();
            int  maxPlayers            = customParamsNode.Attributes["max_players"].IntValue();
            int  roundLimit            = customParamsNode.Attributes["round_limit"].IntValue();
            int  preroundTime          = customParamsNode.Attributes["preround_time"].IntValue(); 
            ulong inventorySlot         = customParamsNode.Attributes["inventory_slot"].ULongValue();
            bool lockedSpectatorCamera = customParamsNode.Attributes["locked_spectator_camera"].BoolValue();
            bool highLatencyAutokick   = customParamsNode.Attributes["high_latency_autokick"].BoolValue();
            bool overtimeMode          = customParamsNode.Attributes["overtime_mode"].BoolValue();
            int  classRestriction      = customParamsNode.Attributes["class_restriction"].IntValue();

            return new GameroomCustomParams(
                friendlyFire, enemyOutlines, autoTeamBalance, joinInTheProcess, roundLimit, preroundTime,
                inventorySlot, lockedSpectatorCamera, highLatencyAutokick, overtimeMode, maxPlayers, classRestriction);
        }
    }
}