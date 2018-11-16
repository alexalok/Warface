using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Warface.Entities.Sessions
{
    public partial class BroadcastSessionResult
    {
        public List<PlayerResult> PlayerResults { get; }

        public BroadcastSessionResult(IEnumerable<PlayerResult> playerResults)
        {
            PlayerResults = playerResults.ToList();
        }

        public static BroadcastSessionResult ParseNode(HtmlNode broadcastSessionResultNode)
        {
            var playerResultNodes = broadcastSessionResultNode.SelectNodes("./player_result");
            var playerResults     = playerResultNodes.Select(PlayerResult.ParseNode);

            return new BroadcastSessionResult(playerResults);
        }

        /*
         * <broadcast_session_result bcast_receivers='415943875@warface/GameClient...'>
			<player_result ...
         */
    }
}