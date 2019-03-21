using System;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using Warface.Enums;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomSession
    {
        public string ID { get; }

        public SessionStatus Status { get; }

        public float GameProgress { get; }

        public DateTimeOffset? StartTime { get; }

        public int Team1StartScore { get; }

        public int Team2StartScore { get; }

        public int Revision { get; }

        public bool HasStarted => (int) Status >= 2;

        public GameroomSession(string id,              SessionStatus status,          float gameProgress, DateTimeOffset? startTime,
                               int    team1StartScore, int           team2StartScore, int   revision)
        {
            ID              = id;
            Status          = status;
            GameProgress    = gameProgress;
            StartTime       = startTime;
            Team1StartScore = team1StartScore;
            Team2StartScore = team2StartScore;
            Revision        = revision;
        }

        public HtmlNode GetAsNode()
        {
            var node = HtmlNode.CreateNode(
                $"<session id='{ID}' status='{(int) Status}' game_progress='{GameProgress}' start_time='{StartTime?.ToUnixTimeSeconds() ?? 0}' " +
                $"team1_start_score='{Team1StartScore}' team2_start_score='{Team2StartScore}' revision='{Revision}'/>");
            return node;
        }

        public static GameroomSession ParseNode(HtmlNode sessionNode)
        {
            //<session id='3461684861154490475' status='2' game_progress='0' start_time='1500903531' team1_start_score='0' 
            //team2_start_score ='0' revision='914'/>

            string          id           = sessionNode.Attributes["id"]?.Value; //sometimes there is no id
            var             status       = (SessionStatus) sessionNode.Attributes["status"].IntValue();
            float           gameProgress = sessionNode.Attributes["game_progress"].FloatValue(); //can be 2.4E-05
            DateTimeOffset? startTime    = null;
            if (sessionNode.Attributes["start_time"].TryLongValue(out long startTimeTimestamp)) //can be start_time='18446744011573954816'
                startTime = DateTimeOffset.FromUnixTimeSeconds(startTimeTimestamp);
            int team1StartScore = sessionNode.Attributes["team1_start_score"].IntValue();
            int team2StartScore = sessionNode.Attributes["team2_start_score"].IntValue();
            int revision        = sessionNode.Attributes["revision"].IntValue();
            return new GameroomSession(id, status, gameProgress, startTime, team1StartScore, team2StartScore, revision);
        }
    }
}