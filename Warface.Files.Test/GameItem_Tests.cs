using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Warface.Files.GameItems;
using Xunit;

namespace Warface.Files.Test
{
    public class GameItem_Tests
    {
        [Theory]
        [MemberData(nameof(GetAllItemFilenamesAsIEnumerable))]
        public void GameItem_ParseText_AllItems(string filename)
        {
            string xml = File.ReadAllText(filename);
            GameItem.ParseText(xml, out _);
        }

        [Theory]
        [InlineData(@"Items\Ammo\SmokeGrenade07_c.xml")]
        [InlineData(@"Items\Weapons\mk03.xml")]
        [InlineData(@"Items\Skins\engineer_fbs_nano_01.xml")]
        [InlineData(@"Items\Armor\sniper_helmet_02.xml")]
        [InlineData(@"Items\ShopItems\random_box_82.xml")]
        [InlineData(@"Items\Bonus\set_comp_02_bonus03.xml")]
        public void GameItem_ParseText_Success(string filePath)
        {
            string xml      = File.ReadAllText(filePath);
            var    gameItem = GameItem.ParseText(xml, out bool isParsed);
            Assert.True(isParsed);
        }

        [Theory]
        [InlineData(@"Items\DefaultActions.xml")]
        public void GameItem_ParseText_Failure(string filePath)
        {
            string xml      = File.ReadAllText(filePath);
            var    gameItem = GameItem.ParseText(xml, out bool isParsed);
            Assert.False(isParsed);
        }


        [Theory]
        [MemberData(nameof(GameItem_Tests_DataGenerator.GetDataForEachType), MemberType = typeof(GameItem_Tests_DataGenerator))]
        void GameItem_Type(string xmlFilePath, GameItem.Type expectedType)
        {
            string xml      = File.ReadAllText(xmlFilePath);
            var    gameItem = GameItem.ParseText(xml, out _);
            Assert.True(gameItem.ItemType == expectedType);
        }


        static IEnumerable<string> GetAllItemFilenames()
        {
            var dirInfo = new DirectoryInfo("./Items");
            var files   = dirInfo.GetFiles("*.xml", SearchOption.AllDirectories);
            Assert.True(files.Length > 0);
            return files.Select(f => f.FullName).ToArray();
        }

        public static IEnumerable<object[]> GetAllItemFilenamesAsIEnumerable()
        {
            return GetAllItemFilenames().Select(allItemFilename => new[] {allItemFilename});
        }

        IEnumerable<string> GetValidItemFilenames()
        {
            var allFiles            = GetAllItemFilenames();
            var filesWithValidItems = new List<string>();
            foreach (string file in allFiles)
            {
                string xml = File.ReadAllText(file);
                GameItem.ParseText(xml, out bool isParsed);
                if (isParsed)
                    filesWithValidItems.Add(file);
            }
            Assert.True(filesWithValidItems.Count > 0);
            return filesWithValidItems;
        }
    }

    public static class GameItem_Tests_DataGenerator
    {
        public static IEnumerable<object[]> GetDataForEachType()
        {
            foreach (GameItem.Type type in Enum.GetValues(typeof(GameItem.Type)))
            {
                switch (type)
                {
                    case GameItem.Type.Unspecified:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Ammo\ammo_pack.xml");
                        yield return ReturnTypeWithXmlFilePath(@"Items\Weapons\shg02.xml");
                        yield return ReturnTypeWithXmlFilePath(@"Items\Weapons\sg31.xml");
                        break;
                    case GameItem.Type.Armor:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Armor\sniper_helmet_02.xml");
                        break;
                    case GameItem.Type.Armorkit:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Weapons\ak01.xml");
                        break;
                    case GameItem.Type.Attachment:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Accessories\ar01_is_d.xml");
                        break;
                    case GameItem.Type.Bonus:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Bonus\set_comp_02_bonus03.xml");
                        break;
                    case GameItem.Type.Grenade:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Ammo\SmokeGrenade07_c.xml");
                        break;
                    case GameItem.Type.Medkit:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Weapons\mk03.xml");
                        break;
                    case GameItem.Type.RandomBox:
                        yield return ReturnTypeWithXmlFilePath(@"Items\ShopItems\random_box_82.xml");
                        break;
                    case GameItem.Type.Weapon:
                        yield return ReturnTypeWithXmlFilePath(@"Items\Weapons\shg03_camo02_shop.xml");
                        break;
                }


                object[] ReturnTypeWithXmlFilePath(string xmlFilePath)
                {
                    return new object[] {xmlFilePath, type};
                }
            }
        }
    }
}