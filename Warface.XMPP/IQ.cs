using System;
using System.Diagnostics;
using System.Linq;
using aDevLib;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Warface.Enums;

namespace Warface.XMPP
{
    public class IQ
    {
        public const string IdCryPrefix = "cry";

        public HtmlNode Node { get; }

        [CanBeNull]
        public string From
        {
            get => Node.Attributes.Contains("from") ? Node.Attributes["from"].Value : null;
            set
            {
                if (value == null)
                {
                    Node.Attributes.Remove("from");
                    return;
                }
                Node.Attributes["from"].Value = value;
            }
        }

        [CanBeNull]
        public string To
        {
            get => Node.Attributes.Contains("to") ? Node.Attributes["to"].Value : null;
            set
            {
                if (value == null)
                {
                    Node.Attributes.Remove("to");
                    return;
                }
                Node.Attributes["to"].Value = value;
            }
        }

        public IQType Type
        {
            get => (IQType) Enum.Parse(typeof(IQType), Node.Attributes["type"].Value, true);
            set => Node.Attributes["type"].Value = value.ToString().ToLowerInvariant();
        }

        public string ID
        {
            get => Node.Attributes["id"].Value;
            set => Node.Attributes["id"].Value = value;
        }

        public bool IsCryId => ID.StartsWith(IdCryPrefix);

        public IQError? Error    { get; }
        public bool     HasError => Error != null;

        public bool IsEmpty => !Node.HasChildNodes;

        public IQ(HtmlNode iqNode)
        {
            Node = iqNode;
            var errorNode = iqNode.ChildNodes.FirstOrDefault(n => n.Name == "error");
            if (errorNode != null)
            {
                var code       = errorNode.Attributes["code"].IntValue();
                var customCode = errorNode.Attributes["custom_code"]?.IntValue();
                Error = new IQError(code, customCode);
            }
        }

        public static IQ CreateIq([CanBeNull] string from, [CanBeNull] string to, [CanBeNull] string id, IQType type, [CanBeNull] string content)
        {
            //<iq to='k01.warface' id='uid00000003' type='get' from='250716918@warface/GameClient' xmlns='jabber:client'>

            if (id == null) //to and from can be null when we're binding, etc
                id = GetRandomID();

            var iqNode = HtmlNode.CreateNode("<iq/>");
            iqNode.Name = "iq";

            //add attributes
            if (to != null)
                iqNode.Attributes.Add("to", to);
            if (from != null)
                iqNode.Attributes.Add("from", from);
            iqNode.Attributes.Add("id",    id);
            iqNode.Attributes.Add("type",  type.ToString().ToLowerInvariant());
            iqNode.Attributes.Add("xmlns", "jabber:client");

            //add inner content
            if (content != null)
                iqNode.InnerHtml = content;

            return new IQ(iqNode);
        }

        public static IQ CreateIq(string from, SpecialStaticJID to, [CanBeNull] string id, IQType type, string content)
        {
            if (id == null)
                id = GetRandomID();
            return CreateIq(from, ResolveSpecialJID(to), id, type, content);
        }

        public static IQ CreateIq(string from, SpecialStaticJID to, [CanBeNull] string id, IQType type, Query innerQuery)
        {
            if (id == null)
                id = GetRandomID();
            return CreateIq(from, ResolveSpecialJID(to), id, type, innerQuery.GetCompressedNodeCopy().OuterHtml);
        }

        public static IQ CreateIq([CanBeNull] string from, [CanBeNull] string to, [CanBeNull] string id, IQType type, Query innerQuery)
        {
            if (id == null)
                id = GetRandomID();
            return CreateIq(from, to, id, type, innerQuery.GetCompressedNodeCopy().OuterHtml);
        }

        static string ResolveSpecialJID(SpecialStaticJID specialStaticJID)
        {
            switch (specialStaticJID)
            {
                case SpecialStaticJID.Warface:
                    return "warface";
                case SpecialStaticJID.MsWarface:
                    return "ms.warface";
                case SpecialStaticJID.K01:
                    return "k01.warface";
                default:
                    throw new ArgumentOutOfRangeException(nameof(specialStaticJID), specialStaticJID, null);
            }
        }

        static string GetRandomID()
        {
            return "uid" + SRandom.Next(0, 99999999).ToString("00000000");
        }

        public override string ToString() => Node.OuterHtml;
    }
}