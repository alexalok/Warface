using System;
using JetBrains.Annotations;
using Warface.Entities.PeerStatusUpdates;

namespace Warface.Entities.Profiles
{
    public class Profile
    {
        public string Nickname  { get; }
        public int    ProfileID { get; }

        [CanBeNull]
        public string JID //empty if status is 0
        {
            get => _jid;
            protected set => _jid = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        [CanBeNull] string _jid;

        public PlayerStatus Status { get; protected set; }

        public int Experience { get; protected set; }

        [CanBeNull]
        public string PlaceToken { get; private set; }

        [CanBeNull]
        public string PlaceInfoToken { get; private set; }

        [CanBeNull]
        public string ModeInfoToken { get; private set; }

        [CanBeNull]
        public string MissionInfoToken { get; private set; }

        protected Profile(string nickname, int profileID, string jid, PlayerStatus status, int experience)
        {
            Nickname   = nickname;
            ProfileID  = profileID;
            JID        = jid;
            Status     = status;
            Experience = experience;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="jid">JID of this profile. Needed because it may be null if profile was offline at the time of creation.</param>
        public void Update(PeerStatusUpdate update, string jid)
        {
            JID              = jid;
            Status           = update.Status;
            Experience       = update.Experience;
            PlaceToken       = update.PlaceToken;
            PlaceInfoToken   = update.PlaceInfoToken;
            ModeInfoToken    = update.ModeInfoToken;
            MissionInfoToken = update.MissionInfoToken;
        }

        public void Update(Profile newProfile)
        {
            JID        = newProfile.JID;
            Status     = newProfile.Status;
            Experience = newProfile.Experience;
        }

        public void SetOffline()
        {
            JID              = null;
            Status           = PlayerStatus.Offline;
            Experience       = 0;
            PlaceToken       = null;
            PlaceInfoToken   = null;
            ModeInfoToken    = null;
            MissionInfoToken = null;
        }
    }
}