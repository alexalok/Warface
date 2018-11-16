using HtmlAgilityExtended;
using HtmlAgilityPack;

namespace Warface.Entities.Gamerooms
{
    public struct GameroomAutostart
    {
        /*<auto_start auto_start_timeout='0' auto_start_timeout_left='0' can_manual_start='0' joined_intermission_timeout='0' revision='17' />*/

        public int  AutoStartTimeout          { get; }
        public int  AutoStartTimeoutLeft      { get; }
        public bool CanManualStart            { get; }
        public int  JoinedIntermissionTimeout { get; }
        public int  Revision                  { get; }

        public GameroomAutostart(int autoStartTimeout, int autoStartTimeoutLeft, bool canManualStart, int joinedIntermissionTimeout, int revision)
        {
            AutoStartTimeout          = autoStartTimeout;
            AutoStartTimeoutLeft      = autoStartTimeoutLeft;
            CanManualStart            = canManualStart;
            JoinedIntermissionTimeout = joinedIntermissionTimeout;
            Revision                  = revision;
        }

        public HtmlNode GetAsNode()
        {
            var node = HtmlNode.CreateNode(
                $"<auto_start auto_start_timeout='{AutoStartTimeout}' auto_start_timeout_left='{AutoStartTimeoutLeft}' can_manual_start='{(CanManualStart ? 1 : 0)}' joined_intermission_timeout='{JoinedIntermissionTimeout}' revision='{Revision}'>");
            return node;
        }

        public static GameroomAutostart ParseNode(HtmlNode autostartNode)
        {
            int  autoStartTimeout          = autostartNode.Attributes["auto_start_timeout"].IntValue();
            int  autoStartTimeoutLeft      = autostartNode.Attributes["auto_start_timeout_left"].IntValue();
            bool canManualStart            = autostartNode.Attributes["can_manual_start"].BoolValue();
            int  joinedIntermissionTimeout = autostartNode.Attributes["joined_intermission_timeout"].IntValue();
            int  revision                  = autostartNode.Attributes["revision"].IntValue();
            return new GameroomAutostart(autoStartTimeout, autoStartTimeoutLeft, canManualStart, joinedIntermissionTimeout, revision);
        }
    }
}