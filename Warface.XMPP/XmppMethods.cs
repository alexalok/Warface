using System.Security;
using System.Text;
using aDevLib;

namespace Warface.XMPP
{
    public static class XmppMethods
    {
        public static string GetRandomIqUid(bool cryPrefix = false)
        {
            return (cryPrefix ? IQ.IdCryPrefix : "uid") + SRandom.Next(0, 99999999).ToString("00000000");
        }

        public static string EscapeXml(string unescapedText)
        {
            var sb = new StringBuilder(unescapedText);
            EscapeXml(sb);
            return sb.ToString();
        }

        public static void EscapeXml(StringBuilder unescapedText)
        {
            unescapedText.
                Replace("&", "&amp;"). //amp must come first or else it will break all replacements before it
                Replace("'", "&apos;").
                Replace("\"", "&quot;").
                Replace(">",  "&gt;").
                Replace("<",  "&lt;");
        }

        public static string UnescapeXml(string escapedXml)
        {
            return string.IsNullOrEmpty(escapedXml)
                ? escapedXml
                : escapedXml.
                    Replace("&apos;", "'").
                    Replace("&quot;", "\"").
                    Replace("&gt;",   ">").
                    Replace("&lt;",   "<").
                    Replace("&amp;",  "&");
        }
    }
}