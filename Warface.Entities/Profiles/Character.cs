using System;
using System.Threading.Tasks;
using Warface.Entities.Profiles.Common;
using Warface.Enums;

namespace Warface.Entities.Profiles
{
    public class Character
    {
        readonly TaskCompletionSource<bool> _characterInfoFetchedTcs = new TaskCompletionSource<bool>();
        bool                                _isCharacterInfoFetched;

        #region BASIC

        public string       Nickname             { get; set; }
        public PlayerStatus Status               { get; set; } = PlayerStatus.Online;
        public Class        CurrentClass         { get; set; }
        public string       PrimaryWeapon        { get; set; } = "ar01_shop";  //TODO default value needed?
        public string       PrimaryWeaponSkin    { get; set; } = string.Empty; //TODO default value needed?
        public int          Experience           { get; set; }
        public Rank         Rank                 { get; set; }
        public string       BannerBadge          { get; set; }
        public string       BannerMark           { get; set; }
        public string       BannerStripe         { get; set; }
        public int          PvpRatingPoints      { get; set; }
        public int          ItemsUnlocked        { get; set; }
        public int          ChallengesCompleted  { get; set; } = 228; //default value in case we don't fetch achievements
        public int          MissionsCompleted    { get; set; }
        public int          PvpWins              { get; set; }
        public int          PvpLoses             { get; set; }
        public int          PvpKills             { get; set; }
        public int          PvpDeaths            { get; set; }
        public int          PlaytimeSeconds      { get; set; }
        public float        LeavingsPercentage   { get; set; }
        public int          CoopClimbsPerformed  { get; set; }
        public int          CoopAssistsPerformed { get; set; }
        public Class        FavoritePvpClass     { get; set; }
        public Class        FavoritePveClass     { get; set; }

        public bool     IsInClan         => !string.IsNullOrWhiteSpace(ClanName);
        public string   ClanName         { get; set; } = string.Empty; //some queries use empty string as no clan indicator
        public ClanRole ClanRole         { get; set; }                 //TODO add clan info
        public int      ClanPosition     { get; set; }
        public int      ClanPoints       { get; set; }
        public string   ClanMemeberSince { get; set; }

        public bool IsCharacterInfoFetched
        {
            get => _isCharacterInfoFetched;
            set
            {
                if (!value)
                    throw new ArgumentException("Unfetching character info is not supported. Only 'true' value can be assigned.");
                if (_isCharacterInfoFetched)
                    throw new InvalidOperationException("This property can only be assigned once.");
                _isCharacterInfoFetched = value;
                _characterInfoFetchedTcs.SetResult(value);
            }
        }

        public Task WaitForCharacterInfoFetched() => _characterInfoFetchedTcs.Task;

        #endregion

        #region STATUS

        public string PlaceToken { get; set; }
        //{
        //get
        //{
        //    if (Status.Has(PlayerStatus.Lobby))
        //        return "@ui_playerinfo_inlobby";
        //    if (Gameroom != null)
        //        return Gameroom.Mission?.Mode == "pve" ? "@ui_playerinfo_pveroom" : "@ui_playerinfo_pvproom";
        //    return string.Empty;
        //}
        //}

        public string PlaceInfoToken { get; set; }
        //{
        //    get
        //    {
        //        if (Gameroom?.Mission?.Type != null)
        //            return "@" + Gameroom.Mission.Type;
        //        else
        //            return "@ui_playerinfo_location";
        //    }
        //}

        public string ModeInfoToken    { get; set; } //=> Gameroom?.Mission?.ModeName ?? string.Empty;
        public string MissionInfoToken { get; set; } //=> Gameroom?.Mission?.Name ?? string.Empty; TODO

        #endregion

        #region MONEY

        public int GameMoney  { get; set; }
        public int CryMoney   { get; set; }
        public int CrownMoney { get; set; }

        #endregion
    }
}