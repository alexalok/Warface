using System;
using System.Linq;
using System.Net;
using HtmlAgilityExtended;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Warface.Entities.Profiles
{
    public struct ProfileInfo
    {
        [CanBeNull] readonly string          _onlineId;
        readonly             long            _profileId;
        readonly             long            _userId;
        readonly             int             _rank;
        [CanBeNull] readonly IPAddress       _ipAddress;
        [CanBeNull] readonly DateTimeOffset? _loginTime;

        public string Nickname { get; }

        [CanBeNull]
        public string OnlineId
        {
            get
            {
                if (!IsOnline)
                    throw new InvalidOperationException("This property is unavailable for an offline profile");
                return _onlineId;
            }
        }

        public PlayerStatus Status { get; }

        public long ProfileId
        {
            get
            {
                if (!IsOnline)
                    throw new InvalidOperationException("This property is unavailable for an offline profile");
                return _profileId;
            }
        }

        public long UserId
        {
            get
            {
                if (!IsOnline)
                    throw new InvalidOperationException("This property is unavailable for an offline profile");
                return _userId;
            }
        }

        public int Rank
        {
            get
            {
                if (!IsOnline)
                    throw new InvalidOperationException("This property is unavailable for an offline profile");
                return _rank;
            }
        }

        [CanBeNull]
        public IPAddress IpAddress //not available in newest game versions
        {
            get
            {
                if (!IsOnline)
                    throw new InvalidOperationException("This property is unavailable for an offline profile");
                return _ipAddress;
            }
        }

        [CanBeNull]
        public DateTimeOffset? LoginTime //not available in newest game versions
        {
            get
            {
                if (!IsOnline)
                    throw new InvalidOperationException("This property is unavailable for an offline profile");
                return _loginTime;
            }
        }

        public bool IsOnline => Status != PlayerStatus.Offline;

        ProfileInfo(string nickname) //user is offline
        {
            Nickname = nickname;
            Status   = PlayerStatus.Offline;

            _onlineId  = null;
            _profileId = 0;
            _userId    = 0;
            _rank      = 0;
            _ipAddress = null;
            _loginTime = null;
        }

        ProfileInfo(string nickname, string onlineId, PlayerStatus playerStatus, long profileId, long userId, int rank)
        {
            //user is online, patched version (ip and login time are unavailable)

            Nickname = nickname;
            Status   = playerStatus;

            _onlineId  = onlineId;
            _profileId = profileId;
            _userId    = userId;
            _rank      = rank;
            _ipAddress = null;
            _loginTime = null;
        }

        ProfileInfo(string nickname, string onlineId, PlayerStatus playerStatus, long profileId, long userId, int rank, IPAddress ipAddress, DateTimeOffset loginTime)
        {
            //user is online, old version (ip and login time are available)

            Nickname = nickname;
            Status   = playerStatus;

            _onlineId  = onlineId;
            _profileId = profileId;
            _userId    = userId;
            _rank      = rank;
            _ipAddress = ipAddress;
            _loginTime = loginTime;
        }

        /// <summary>
        /// Get profile info of a player (common info like jid/uid, rank, status, ip, etc)
        /// </summary>
        /// <param name="profileInfoGetStatusNode"></param>
        /// <returns>ProfileInfo struct with info about a player. If player is offline, only Nickname and Status are guaranteed to be correct.</returns>
        public static ProfileInfo ParseProfileInfoNode(HtmlNode profileInfoGetStatusNode)
        {
            //<profile_info_get_status nickname='GingerLeprechaun'><profile_info><info nickname='GingerLeprechaun' 
            //online_id ='21531320@warface/GameClient' status='13' profile_id='1922463' user_id='21531320' rank='68' tags='' 
            //ip_address ='51.37.11.149' login_time='1501091066'/></profile_info></profile_info_get_status></query></iq>

            //<profile_info_get_status nickname='иван_храпенко'><profile_info/></profile_info_get_status></query></iq>

            var infoNode = profileInfoGetStatusNode.SelectSingleNode("./profile_info/info");
            if (infoNode == null)
            {
                string nick        = profileInfoGetStatusNode.Attributes["nickname"].Value;
                var    profileInfo = new ProfileInfo(nick);
                return profileInfo;
            }

            string nickname  = infoNode.Attributes["nickname"].Value;
            string onlineId  = infoNode.Attributes["online_id"].Value;
            var    status    = (PlayerStatus) infoNode.Attributes["status"].IntValue();
            long   profileId = infoNode.Attributes["profile_id"].LongValue();
            long   userId    = infoNode.Attributes["user_id"].LongValue();
            int    rank      = infoNode.Attributes["rank"].IntValue();

            if (infoNode.Attributes.Contains("ip_address") && infoNode.Attributes.Contains("login_time"))
            {
                IPAddress.TryParse(infoNode.Attributes["ip_address"].Value, out var ipAddress);
                long unixTime  = Convert.ToInt64(new string(infoNode.Attributes["login_time"].Value.Take(10).ToArray()));
                var  loginTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
                return new ProfileInfo(nickname, onlineId, status, profileId, userId, rank, ipAddress, loginTime);
            }
            return new ProfileInfo(nickname, onlineId, status, profileId, userId, rank);
        }
    }
}