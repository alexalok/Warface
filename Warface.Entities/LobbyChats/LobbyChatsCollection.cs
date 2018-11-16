using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Warface.Entities.LobbyChats
{
    public class LobbyChatsCollection
    {
        LobbyChat _global;
        LobbyChat _room;
        LobbyChat _team;
        LobbyChat _clan;

        public void SetLobbyChat(LobbyChat lobbyChat)
        {
            switch (lobbyChat.Type)
            {
                case LobbyChatType.Global:
                    _global = lobbyChat;
                    break;
                case LobbyChatType.Room:
                    _room = lobbyChat;
                    break;
                case LobbyChatType.Team:
                    _team = lobbyChat;
                    break;
                case LobbyChatType.Clan:
                    _clan = lobbyChat;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerable<LobbyChat> GetLobbyChats()
        {
            var res = new List<LobbyChat>();

            if (_global != null)
                res.Add(_global);
            if (_room != null)
                res.Add(_room);
            if (_team != null)
                res.Add(_team);
            if (_clan != null)
                res.Add(_clan);

            return res;
        }

        [CanBeNull]
        public LobbyChat GetLobbyChat(LobbyChatType type)
        {
            switch (type)
            {
                case LobbyChatType.Global:
                    return _global;
                case LobbyChatType.Room:
                    return _room;
                case LobbyChatType.Team:
                    return _team;
                case LobbyChatType.Clan:
                    return _clan;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ClearRoomChats() //to be called when leaving room
        {
            _room = null;
            _team = null;
        }

        public void ClearClanChat() //to be called when leaving clan 
        {
            _clan = null;
        }

        void ClearChannelChat() //game client must always have a channel chat
        {
            throw new NotSupportedException();
        }
    }
}