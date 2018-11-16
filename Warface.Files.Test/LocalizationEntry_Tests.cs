using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Xunit;

namespace Warface.Files.Test
{
    public class LocalizationEntry_Tests
    {
        [Fact]
        void LocalizationEntry_ParseLocalizationFile()
        {
            var files = GetAllLocalizationFiles();
            foreach (string file in files)
            {
                string xml = File.ReadAllText(file);
                Assert.True(LocalizationEntry.ParseLocalizationFile(xml).Any());
            }
        }


        [Fact]
        void LocalizationEntry_ParseNode_True()
        {
            var validXmls = GetValidXmls();
            foreach (string xml in validXmls)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                Assert.True(LocalizationEntry.ParseNode(xmlDoc.LastChild, out var localizationEntry));
                Assert.False(localizationEntry == default);
            }
        }

        [Theory]
        [InlineData("<entry><original/><translation/></entry>")]
        void LocalizationEntry_ParseNode_False(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            Assert.False(LocalizationEntry.ParseNode(xmlDoc.LastChild, out var localizationEntry));
            Assert.True(localizationEntry == default);
        }

        [Fact]
        void LocalizationEntry_Compare_True()
        {
            var validXmls = GetValidXmls();
            foreach (string xml in validXmls)
            {
                var xmlDoc1 = new XmlDocument();
                xmlDoc1.LoadXml(xml);
                Assert.True(LocalizationEntry.ParseNode(xmlDoc1.LastChild, out var localizationEntry1));
                Assert.False(localizationEntry1 == default);

                var xmlDoc2 = new XmlDocument();
                xmlDoc2.LoadXml(xml);
                Assert.True(LocalizationEntry.ParseNode(xmlDoc2.LastChild, out var localizationEntry2));
                Assert.False(localizationEntry2 == default);

                Assert.True(localizationEntry1 == localizationEntry2);
            }
        }

        IEnumerable<string> GetAllLocalizationFiles()
        {
            var dir   = new DirectoryInfo("Languages");
            var files = dir.GetFiles("text_*.xml", SearchOption.AllDirectories);
            return files.Select(f => f.FullName);
        }

        IEnumerable<string> GetValidXmls()
        {
            return new[]
            {
                "<entry key=\"ui_armor_shared_shoes_comp_18_01_name\"><original value=\"New Generation shoes\"/><translation value=\"Ботинки K.I.W.I.\"/></entry>",

                "<entry key=\"ui_weapons_ar01\"><original value=\"&lt;font size=\'14\' color=\'#dddddd\'&gt;&lt;b&gt;Attachment\'s Slots:&lt;/b&gt;&lt;/font&gt; &lt;font size=\'14\' color=\'#42EBF8\'&gt; Rail, Scope, Muzzle&lt;/font&gt;&lt;/b&gt; \\n &lt;font size=\'14\' color=\'#DDDDDD\'&gt;The Tavor TAR-21 is an Israeli bullpup assault rifle chambered for 5.56x45mm NATO ammunition with a selective fire system. It is the standard issued weapon of the Israeli infantry.&lt;/font&gt;\"/><translation value=\"&lt;font size=\'14\' color=\'#dddddd\'&gt;&lt;b&gt;Совместимые модули:&lt;/b&gt;&lt;/font&gt; &lt;font size=\'14\' color=\'#42EBF8\'&gt; ствол, цевье, прицел&lt;/font&gt;&lt;/b&gt;. &lt;b&gt;Отдача:&lt;/b&gt;&lt;font size=\'14\' color=\'#42EBF8\'&gt; слабая&lt;/font&gt;\\n&lt;font size=\'14\' color=\'#DDDDDD\'&gt;Израильская штурмовая винтовка калибра &lt;font color=\'#dddddd\'&gt;5.56x45 мм&lt;/font&gt;, выполненная по схеме булл-пап. Корпус оружия изготовлен из легких сплавов и высокопрочных полимеров.\"/></entry>"
            };
        }
    }
}