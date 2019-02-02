using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.Sessions
{
    public partial class BroadcastSessionResult
    {
        public partial class PlayerResult
        {
            public class PvpRatingOutcome
            {
                public int    Rank           { get; }
                public bool   GameResult     { get; }
                public string GamesHistory   { get; }
                public bool   WinStreakBonus { get; }

                public PvpRatingOutcome(int rank, bool gameResult, string gamesHistory, bool winStreakBonus)
                {
                    Rank           = rank;
                    GameResult     = gameResult;
                    GamesHistory   = gamesHistory;
                    WinStreakBonus = winStreakBonus;
                }

                public static PvpRatingOutcome ParseNode(HtmlNode pvpRatingNode)
                {
                    var rank           = pvpRatingNode.Attributes["rank"].IntValue();
                    var gameResult     = pvpRatingNode.Attributes["game_result"].BoolValue();
                    var gamesHistory   = pvpRatingNode.Attributes["games_history"].Value;
                    var winStreakBonus = pvpRatingNode.Attributes["win_streak_bonus"].BoolValue();

                    return new PvpRatingOutcome(rank, gameResult, gamesHistory, winStreakBonus);
                }

                //<pvp_rating_outcome rank='8' game_result='0' games_history='fwf' win_streak_bonus='0'/>
            }
        }
    }
}