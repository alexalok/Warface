using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Ionic.Zlib;
using JetBrains.Annotations;

namespace Warface.XMPP
{
    public class Query
    {
        const int MaxUncompressedStanzaLength = 1024;

        public HtmlNode Node { get; }

        [CanBeNull]
        public HtmlNode InnerNode => Node.HasChildNodes ? Node.FirstChild : null;

        [CanBeNull]
        public string InnerNodeName => InnerNode?.Name;

        public bool IsEmpty => InnerNode == null;

        public IQ ParentIQ { get; }

        Query(HtmlNode queryNode, IQ parentIQ = null)
        {
            DecompressIfNeeded(queryNode);
            Node     = queryNode;
            ParentIQ = parentIQ;
        }

        public static Query FromIQ(IQ iq)
        {
            var queryNode = iq.Node.FirstChild;
            if (queryNode.Name != "query")
                throw new InvalidOperationException();
            return new Query(queryNode, iq);
        }

        public static Query Create(HtmlNode queryInnerContent)
        {
            var queryContentClone = queryInnerContent.Clone();
            var queryNode         = HtmlNode.CreateNode("<query xmlns='urn:cryonline:k01'/>");
            queryNode.AppendChild(queryContentClone);
            return new Query(queryNode);
        }

        public HtmlNode GetCompressedNodeCopy()
        {
            var nodeClone = Node.Clone();
            Compress(nodeClone);
            return nodeClone;
        }

        public bool Compress()
        {
            return Compress(Node);
        }

        bool Compress(HtmlNode queryNode)
        {
            var compressExclusions = new[] {"account", "get_master_server"};

            if (queryNode.OuterLength < MaxUncompressedStanzaLength || IsEmpty || compressExclusions.Contains(InnerNodeName))
                return false;

            var clonedQuery = new Query(queryNode.Clone());
            queryNode.RemoveAllChildren();

            //<query xmlns='urn:cryonline:k01'>
            //<data query_name='shop_get_offers' code='3' from='1000' to='1115' hash='20180710110503' compressedData='...' originalSize='43159'
            var dataNode = HtmlNode.CreateNode("<data/>");

            string queryName      = clonedQuery.InnerNodeName;
            int    originalSize   = Encoding.UTF8.GetBytes(clonedQuery.InnerNode.OuterHtml).Length;
            string compressedData = Zip(clonedQuery.InnerNode.OuterHtml);

            dataNode.Attributes.Add("query_name",     queryName);
            dataNode.Attributes.Add("compressedData", compressedData);
            dataNode.Attributes.Add("originalSize",   originalSize.ToString());

            if (queryName == "join_channel") //test - remove
                ;

            foreach (var contentAttribute in clonedQuery.InnerNode.Attributes.Reverse())
                dataNode.Attributes.Insert(1, contentAttribute);

            queryNode.AppendChild(dataNode);
            return true;
        }

        public override string ToString() => GetCompressedNodeCopy().OuterHtml;

        static void DecompressIfNeeded(HtmlNode queryNode)
        {
            //<query xmlns='urn:cryonline:k01'><data query_name='join_channel' compressedData='...' originalSize='12365'/></query>

            var dataNode = queryNode.SelectSingleNode("./data");
            if (dataNode == null)
                return;

            string queryName   = dataNode.Attributes["query_name"].Value;
            string dataContent = Unzip(dataNode.Attributes["compressedData"].Value);

            queryNode.InnerHtml = dataContent;
        }

        static string Unzip(string compressed)
        {
            using (var input = new MemoryStream(Convert.FromBase64String(compressed)))
            using (var gzip = new ZlibStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return Encoding.UTF8.GetString(output.ToArray());
            }
        }

        static string Zip(string decompressed)
        {
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes(decompressed)))
            using (var gzip = new ZlibStream(input, CompressionMode.Compress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return Convert.ToBase64String(output.ToArray());
            }
        }
    }
}