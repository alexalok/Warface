using System.Net;
using aDevLib;
using HtmlAgilityPack;

namespace Warface.Entities
{
    public struct Session
    {
        public string Server { get; }
        public IPAddress Hostname { get; }

        public int Port { get; }

        public bool Local { get; }

        public string SessionId { get; }

        Session(string server, IPAddress hostname, int port, bool local, string sessionId)
        {
            Server = server;
            Hostname = hostname;
            Port = port;
            Local = local;
            SessionId = sessionId;
        }

        public static Session ParseNode(HtmlNode sessionJoinNode)
        {
            //<session_join room_id='10871592193693321097' server='ded3-lv-eq-tr_64000' hostname='212.68.32.90' 
            //port='64000' local='0' session_id='146236211925286815'/>

            string server = sessionJoinNode.Attributes["server"].Value;
            var hostname = IPAddress.Parse(sessionJoinNode.Attributes["hostname"].Value);
            int port = sessionJoinNode.Attributes["port"].IntValue();
            bool local = sessionJoinNode.Attributes["local"].BoolValue();
            string sessionId = sessionJoinNode.Attributes["session_id"].Value;

            return new Session(server, hostname, port, local, sessionId);
        }
    }
}