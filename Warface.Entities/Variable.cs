using HtmlAgilityPack;

namespace Warface.Entities
{
    public struct Variable
    {
        public readonly string Key;
        public readonly string Value;
        public readonly HtmlNode Node;

        public Variable(string key, string value, HtmlNode node)
        {
            Key   = key;
            Value = value;
            Node = node;
        }
    }
}