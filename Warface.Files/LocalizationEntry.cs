using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;

namespace Warface.Files
{
    public struct LocalizationEntry
    {
        public string Key { get; }

        public string Original { get; }

        [CanBeNull]
        public string Translation { get; }

        public LocalizationEntry(string key, string original, string translation)
        {
            Key         = key;
            Original    = original;
            Translation = translation;
        }

        public static bool ParseNode(XmlNode entryNode, out LocalizationEntry localizationEntry)
        {
            localizationEntry = default;

            string key = entryNode.Attributes["key"]?.Value;

            string originalValue = entryNode.SelectSingleNode("./original").Attributes["value"]?.Value;
            if (string.IsNullOrWhiteSpace(originalValue))
                return false;

            string translationValue = entryNode.SelectSingleNode("./translation").Attributes["value"]?.Value;

            localizationEntry = new LocalizationEntry(key, originalValue, translationValue);
            return true;
        }

        public static IEnumerable<LocalizationEntry> ParseLocalizationFile(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var entryNodes = xmlDoc.SelectNodes("/localization/entry");
            foreach (XmlNode entryNode in entryNodes)
            {
                if (ParseNode(entryNode, out var localizationEntry))
                    yield return localizationEntry;
            }
        }

        public static bool operator ==(LocalizationEntry item1, LocalizationEntry item2)
        {
            return
                item1.Key         == item2.Key      &&
                item1.Original    == item2.Original &&
                item1.Translation == item2.Translation;
        }

        public static bool operator !=(LocalizationEntry item1, LocalizationEntry item2)
        {
            return !(item1 == item2);
        }

        /*
         * <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <localization>
	            <entry key="ui_armor_shared_pants_01">
		            <original value="Default pants"/>
		            <translation value="Стандартные штаны"/>
	            </entry>
            ...
         *
         */
    }
}