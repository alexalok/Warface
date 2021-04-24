using System;
using System.Linq;
using aDevLib;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Warface.Entities.Channels
{
    public class Channel
    {
        [NotNull]
        public string Name { get; }

        public int PlayersCount { get; }

        [NotNull]
        public string JID => $"masterserver@warface/{Name}";

        [NotNull]
        public string Conference => $"global.{Name}";

        public ChannelType Type      => Name.StartsWith("pve_") ? ChannelType.Pve : ChannelType.Pvp;
        public string      ShortName => string.Join("_", Name.Split('_').Take(Type == ChannelType.Pve ? 1 : 2)); //pve pvp_pro
        public bool        IsJoined;
        public bool        IsInConference;

        public Channel([NotNull] string name, int playersCount = 0)
        {
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            PlayersCount = playersCount;

            if (name.Contains("@") || name.Contains("/") || !(name.StartsWith("pvp_") || name.StartsWith("pve_") || name.StartsWith("dal_")))
                throw new ArgumentException($"Argument must be a valid channel name.", nameof(name));
        }

        public static Channel ParseNode(HtmlNode serverNode)
        {
            /*
                <server resource='pvp_pro_006_r1' server_id='306' channel='pvp_pro' rank_group='all' load='0.386905' online='441' min_rank='26' max_rank='90' bootstrap=''>
					<load_stats>
						<load_stat type='quick_play' value='196'/>
						<load_stat type='survival' value='255'/>
						<load_stat type='pve' value='255'/>
					</load_stats>
				</server>
             */

            string name         = serverNode.Attributes["resource"].Value;
            int    playersCount = serverNode.Attributes["online"].IntValue();

            return new Channel(name, playersCount);
        }

        public static Channel FromJid(string jid)
        {
            //masterserver@warface/pvp_pro_006_r1
            var splitJid = jid.Split('/');
            if (splitJid[0] != "masterserver@warface")
                throw new ArgumentException("Invalid JID format.", nameof(jid));
            string name = splitJid[1];
            return new Channel(name);
        }

        public static bool operator ==(Channel channel1, Channel channel2)
        {
            if (ReferenceEquals(channel1, channel2))
                return true;
            if (ReferenceEquals(channel1, null) ||
                ReferenceEquals(channel2, null))
                return false;
            return channel1.Name == channel2.Name;
        }

        public static bool operator !=(Channel channel1, Channel channel2)
        {
            return !(channel1 == channel2);
        }
    }
}