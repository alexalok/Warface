using System.Security;
using aDevLib.Classes;

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
            return SecurityElement.Escape(unescapedText);
        }

        public static string UnescapeXml(string escapedXml)
        {
            return string.IsNullOrEmpty(escapedXml)
                ? escapedXml
                : escapedXml.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }
    }
}