using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Warface.Files;
using Warface.Files.GameItems;

namespace RandomBoxInfo
{
    public class RandomBoxInfo
    {
        const string ItemsFolderName = @"H:\z_warfacebackups\ru\pak\17-04-2019\GameData.pak\Items";
        const string LocalizationFolderName = @"H:\z_warfacebackups\ru\pak\17-04-2019\Russian.pak\Languages";
        const string CrownFolderName = "короны";
        const string WbOrKFolderName = "варбаксы и кредиты";

        Dictionary<string, GameItem> _gameItems;
        Dictionary<string, LocalizationEntry> _localizationEntries;

        public void Start()
        {
            EnsureDirsCreatedAndEmpty();
            Load();
            foreach (var gameItem in _gameItems.Where(gi => gi.Value.ItemType == GameItem.Type.RandomBox))
            {
                ProcessRandomBox(gameItem.Value);
            }
        }

        void Load()
        {
            _gameItems = LoadItems().
                GroupBy(i => i.Name). //remove duplicate names
                Select(g => g.First()). //
                ToDictionary(i => i.Name, i => i);
            _localizationEntries = LoadLocalizationEntries().
                GroupBy(e => e.Key). //grouping to remove duplicates with the same key
                Select(g => g.First()). //we can't just use distinct bc there are some entries that have the same key but different original/translation
                ToDictionary(e => e.Key, e => e);
        }

        void ProcessRandomBox(GameItem gameItem)
        {
            if (!gameItem.ItemRandomBox.HasValue)
                throw new ArgumentException();

            if (!gameItem.ItemUIStats.HasValue)
                throw new ArgumentException();

            string friendlyName;
            try
            {
                friendlyName = _localizationEntries[gameItem.ItemUIStats.Value.Name].Translation;
            }
            catch
            {
                if (gameItem.Name.Contains("_test"))
                    return;
                friendlyName = gameItem.ItemUIStats.Value.Name;
            }
            bool isCrown = gameItem.Name.Contains("_crown");
            var groupedChances = new List<List<KeyValuePair<string, double>>>();
            foreach (var group in gameItem.ItemRandomBox.Value.Groups)
            {
                var chances = new List<KeyValuePair<string, double>>();
                int totalWeights = group.Sum(i => i.Weight);
                foreach (var item in group)
                {
                    var mainItem = _gameItems[item.Name];
                    string friendlyItemName = _localizationEntries[mainItem.ItemUIStats?.Name].Translation;

                    if (item.FriendlyExpiration != null)
                    {
                        friendlyItemName += $" (на {item.FriendlyExpiration})";
                    }
                    else if (item.Amount.HasValue)
                    {
                        friendlyItemName += $" ({item.Amount.Value} штук)";
                    }
                    else
                    {
                        friendlyItemName += $" (навсегда)";
                    }
                    double chance = item.Weight / (double) totalWeights;
                    chances.Add(new KeyValuePair<string, double>(friendlyItemName, chance));
                }
                groupedChances.Add(chances);
            }
            SaveProcessedRandomBox(friendlyName, isCrown, groupedChances);
        }


        void SaveProcessedRandomBox(string friendlyName, bool isCrown, List<List<KeyValuePair<string, double>>> groupedChances, int tryCount = 0)
        {
            string folderName = isCrown ? CrownFolderName : WbOrKFolderName;
            string filePath;
            if (tryCount == 0)
                filePath = Path.Combine(folderName, $"{friendlyName}.txt");
            else
                filePath = Path.Combine(folderName, $"{friendlyName} ({tryCount}).txt");
            if (File.Exists(filePath))
            {
                SaveProcessedRandomBox($"{friendlyName}", isCrown, groupedChances, ++tryCount);
                return;
            }
            using (var sw = new StreamWriter(filePath))
            {
                var groupId = 0;
                foreach (var chances in groupedChances)
                {
                    sw.WriteLine("Группа №" + ++groupId);
                    int chancesKeyRowLength = chances.Max(c => c.Key.Length);
                    foreach (var chance in chances)
                    {
                        int whitespacesToAdd = chancesKeyRowLength - chance.Key.Length;
                        sw.WriteLine($"{chance.Key}{GenWhitespaces(whitespacesToAdd)}\t{chance.Value:P}");
                    }
                    sw.WriteLine();
                }
            }
        }

        string GenWhitespaces(int count)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(" ");
            }
            return sb.ToString();
        }

        void EnsureDirsCreatedAndEmpty()
        {
            if (!Directory.Exists(CrownFolderName))
                Directory.CreateDirectory(CrownFolderName);

            foreach (string file in Directory.GetFiles(CrownFolderName))
                File.Delete(file);

            if (!Directory.Exists(WbOrKFolderName))
                Directory.CreateDirectory(WbOrKFolderName);

            foreach (string file in Directory.GetFiles(WbOrKFolderName))
                File.Delete(file);
        }

        IEnumerable<GameItem> LoadItems()
        {
            var files = new DirectoryInfo(ItemsFolderName).GetFiles("*.xml", SearchOption.AllDirectories);
            var items = new List<GameItem>();
            foreach (var file in files)
            {
                string text = File.ReadAllText(file.FullName);
                try
                {
                    var gameItem = GameItem.ParseText(text, out bool isParsed);
                    if (isParsed)
                        items.Add(gameItem);
                }
                catch (NotSupportedException)
                {
                }
            }
            return items;
        }

        IEnumerable<LocalizationEntry> LoadLocalizationEntries()
        {
            var files = new DirectoryInfo(LocalizationFolderName).GetFiles("text_*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string text = File.ReadAllText(file.FullName);
                var fileEntries = LocalizationEntry.ParseLocalizationFile(text);
                foreach (var fileEntry in fileEntries)
                    yield return fileEntry;
            }
        }
    }
}