using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Ozzz = XeniaLauncher.OzzzFramework;
using Sprite = XeniaLauncher.OzzzFramework.Sprite;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;
using TextSprite = XeniaLauncher.OzzzFramework.TextSprite;
using Layer = XeniaLauncher.OzzzFramework.SpriteGroup.Layer;
using Button = XeniaLauncher.OzzzFramework.Button;
using Gradient = XeniaLauncher.OzzzFramework.Gradient;
using AnimationPath = XeniaLauncher.OzzzFramework.AnimationPath;
using MouseInput = XeniaLauncher.OzzzFramework.MouseInput;
using KeyboardInput = XeniaLauncher.OzzzFramework.KeyboardInput;
using Key = XeniaLauncher.OzzzFramework.KeyboardInput.Key;
using GamepadInput = XeniaLauncher.OzzzFramework.GamepadInput;
using AnalogPad = XeniaLauncher.OzzzFramework.GamepadInput.AnalogPad;
using DigitalPad = XeniaLauncher.OzzzFramework.GamepadInput.DigitalPad;
using GameData = XeniaLauncher.Shared.GameData;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XLCompanion;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Data;

namespace XeniaLauncher
{
    public class Menu : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                game.state = Game1.State.Main;
                game.backSound.Play();
            }
            else if (buttonIndex == 1)
            {
                game.newGameWindow = new Window(game, new Rectangle(560, 220, 800, 640), "Add a Game", new NewGame(), new StdInputEvent(4), new GenericStart(), Game1.State.Menu);
                game.state = Game1.State.NewGame;
                game.newGameWindow.AddButton(new Rectangle(610, 365, 700, 100));
                game.newGameWindow.AddButton(new Rectangle(610, 475, 700, 100));
                game.newGameWindow.AddButton(new Rectangle(610, 585, 700, 100));
                game.newGameWindow.AddButton(new Rectangle(610, 695, 700, 100));
                game.newGameWindow.AddText("Folder + Database Import");
                game.newGameWindow.AddText("Manual Import (OLD)");
                game.newGameWindow.AddText("Import From STFS (OLD)");
                game.newGameWindow.AddText("Back to Menu");
                game.newGameWindow.buttonEffects.SetupEffects(game, source);
            }
            // Options window
            else if (buttonIndex == 2)
            {
                game.optionsWindow = new Window(game, new Rectangle(460, 140, 1000, 840), "Launcher Options", "Global Continuum Options", new OptionsEffects(), new OptionsInput(), new OptionsStart(), Game1.State.Menu, true);
                // Volume buttons
                game.optionsWindow.AddButton(new Rectangle(910, 305, 90, 90));
                game.optionsWindow.AddText("<");
                game.optionsWindow.AddButton(new Rectangle(1310, 305, 90, 90));
                game.optionsWindow.AddText(">");
                // Theme buttons
                game.optionsWindow.AddButton(new Rectangle(910, 405, 90, 190));
                game.optionsWindow.AddText("<");
                game.optionsWindow.AddButton(new Rectangle(1310, 405, 90, 190));
                game.optionsWindow.AddText(">");
                // Compat prompt buttons
                game.optionsWindow.AddButton(new Rectangle(910, 605, 90, 90));
                game.optionsWindow.AddText("<");
                game.optionsWindow.AddButton(new Rectangle(1310, 605, 90, 90));
                game.optionsWindow.AddText(">");
                // Other option window buttons
                game.optionsWindow.AddButton(new Rectangle(500, 715, 450, 100));
                game.optionsWindow.AddText("Graphics Settings");
                game.optionsWindow.AddButton(new Rectangle(970, 715, 450, 100));
                game.optionsWindow.AddText("Xenia Settings");

                game.optionsWindow.AddButton(new Rectangle(660, 840, 600, 100));
                game.optionsWindow.AddText("Back To Menu");

                game.optionsWindow.extraSprites.Add(new TextSprite(game.font, "Sound Volume:", 0.6f, new Vector2(500, 310), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.optionsWindow.extraSprites.Add(new TextSprite(game.bold, "8", 0.8f, new Vector2(1140, 295), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.optionsWindow.extraSprites.Add(new TextSprite(game.font, "UI Theme:", 0.6f, new Vector2(500, 460), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.optionsWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(1005, 405, 290, 180), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.optionsWindow.extraSprites.Add(new TextSprite(game.font, "Show Compat Prompt:", 0.5f, new Vector2(500, 620), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.optionsWindow.extraSprites.Add(new TextSprite(game.bold, "Untested Only", 0.5f, new Vector2(1140, 625), Color.FromNonPremultiplied(0, 0, 0, 0)));

                game.state = Game1.State.Options;
            }
            // Manage Data
            else if (buttonIndex == 3)
            {
                if (game.checkDrivesOnManage)
                {
                    game.SetDriveSpaceText();
                }
                game.dataWindow = new Window(game, new Rectangle(-200, 0, 2320, 1080), "Manage Data", new DataWindowEffects(), new DataWindowInput(), new GenericStart(), Game1.State.Menu);
                game.dataWindow.changeEffects = new DataWindowChangeEffects(game, game.dataWindow);
                game.dataWindow.AddButton(new Rectangle(20, 150, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 280, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 410, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 540, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 670, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 800, 1880, 120));

                game.dataStrings.Clear();
                game.dataFiles.Clear();
                game.localData.Clear();
                foreach (GameData data in game.masterData)
                {
                    game.dataFiles.Add(new List<DataEntry>());
                }
                float totalSize = 0;
                float artSize = 0;
                float tempDataSize = 0;
                int index = 0;
                List<string> missingList = new List<string>();
                Dictionary<string, int> linkedDataStrings = new Dictionary<string, int>();
                try
                {
                    foreach (GameData data in game.masterData)
                    {
                        // Game data
                        if (File.Exists(data.gamePath))
                        {
                            game.dataWindow.AddText("");
                            game.dataStrings.dataStringList.Add(data.gameTitle);
                            DirectoryInfo directory = new DirectoryInfo(new FileInfo(data.gamePath).DirectoryName);
                            float size = 0;
                            DirectoryInfo directoryInfo = new DirectoryInfo(data.gamePath).Parent;
                            // Checking which data retrieval mode to use
                            if (Shared.contentTypes.Keys.Contains(directoryInfo.Name))
                            {
                                DirectoryInfo parent = directoryInfo.Parent;
                                foreach (DirectoryInfo dir in parent.GetDirectories("*", SearchOption.TopDirectoryOnly))
                                {
                                    if (Shared.contentTypes.Keys.Contains(dir.Name))
                                    {
                                        // Handling files in a extract folder
                                        if (dir.Name == "_EXTRACT")
                                        {
                                            float localSize = 0;
                                            foreach (FileInfo file in dir.GetFiles("*", SearchOption.TopDirectoryOnly))
                                            {
                                                localSize += file.Length;
                                            }
                                            size += localSize;
                                            if (localSize > 0)
                                            {
                                                game.dataFiles[index].Add(new DataEntry("Micellaneous Extracts", Shared.contentTypes["_EXTRACT"], game.ConvertDataSize("" + localSize), null, game.icons[data.gameTitle]));
                                                game.dataFiles[index].Last().fileSize = localSize;
                                            }
                                            // Extracts in proper folder structure
                                            foreach (DirectoryInfo extractDir in dir.GetDirectories())
                                            {
                                                if (Shared.filteredContentTypes[game.dataFilter].Contains(extractDir.Name))
                                                {
                                                    float extractSize = 0;
                                                    foreach (FileInfo file in extractDir.GetFiles("*", SearchOption.AllDirectories))
                                                    {
                                                        extractSize += file.Length;
                                                    }
                                                    size += extractSize;
                                                    game.dataFiles[index].Add(new DataEntry("Extracted " + Shared.contentTypes[extractDir.Name], Shared.contentTypes["_EXTRACT"], game.ConvertDataSize("" + extractSize), null, game.icons[data.gameTitle]));
                                                    game.dataFiles[index].Last().fileSize = extractSize;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (FileInfo file in dir.GetFiles("*", SearchOption.TopDirectoryOnly))
                                            {
                                                if (Shared.filteredContentTypes[game.dataFilter].Contains(file.Directory.Name))
                                                {
                                                    float localSize = file.Length;
                                                    if (Directory.Exists(dir.FullName + "\\" + file.Name + ".data"))
                                                    {
                                                        DirectoryInfo dataDirectory = new DirectoryInfo(dir.FullName + "\\" + file.Name + ".data");
                                                        foreach (FileInfo dataFile in dataDirectory.GetFiles("*", SearchOption.AllDirectories))
                                                        {
                                                            localSize += dataFile.Length;
                                                        }
                                                    }
                                                    size += localSize;
                                                    // Adding data
                                                    game.dataFiles[index].Add(new DataEntry("Blam!", Shared.contentTypes[dir.Name], game.ConvertDataSize("" + localSize), file.FullName, game.white));
                                                    game.dataFiles[index].Last().fileSize = localSize;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (FileInfo file in directory.GetFiles("*", SearchOption.AllDirectories))
                                {
                                    size += file.Length;
                                }
                                game.dataFiles[index].Add(new DataEntry(data.gameTitle, "Installed Xbox 360 Game", game.ConvertDataSize("" + size), null, game.icons[data.gameTitle]));
                            }
                            // Saves and Xenia data
                            if (Directory.Exists("XData") && game.dataFilter == Shared.DataFilter.All || game.dataFilter == Shared.DataFilter.TempContent)
                            {
                                if (Directory.Exists("XData\\Xenia"))
                                {
                                    foreach (string dir in Directory.GetDirectories("XData\\Xenia"))
                                    {
                                        string title = data.gameTitle;
                                        string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                                        Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                                        string newTitle = r.Replace(title, "");
                                        if (dir.Split("\\").Last() == newTitle)
                                        {
                                            // Xenia data
                                            float dataSize = 0;
                                            if (File.Exists(dir + "\\xenia.exe"))
                                            {
                                                dataSize += new FileInfo(dir + "\\xenia.exe").Length;
                                            }
                                            if (File.Exists(dir + "\\xenia.log"))
                                            {
                                                dataSize += new FileInfo(dir + "\\xenia.log").Length;
                                            }
                                            if (File.Exists(dir + "\\xenia.config.toml"))
                                            {
                                                dataSize += new FileInfo(dir + "\\xenia.config.toml").Length;
                                            }
                                            if (dataSize > 0)
                                            {
                                                game.dataFiles[index].Add(new DataEntry("Xenia {Temporary Copy}", "Localized Xenia Data", game.ConvertDataSize("" + dataSize), null, game.logo));
                                                game.dataFiles[index].Last().fileSize = dataSize;
                                                tempDataSize += dataSize;
                                            }
                                            size += dataSize;
                                            // Save data
                                            if (Directory.Exists(dir + "\\content\\" + data.titleId + "\\profile"))
                                            {
                                                float saveSize = 0;
                                                foreach (string filename in Directory.GetFiles(dir + "\\content\\" + data.titleId + "\\profile", "", SearchOption.AllDirectories))
                                                {
                                                    FileInfo file = new FileInfo(filename);
                                                    saveSize += file.Length;
                                                }
                                                if (saveSize > 0)
                                                {
                                                    game.dataFiles[index].Add(new DataEntry("Save Data (Xenia)", "Xenia Game Save", game.ConvertDataSize("" + saveSize), null, game.icons[data.gameTitle]));
                                                    game.dataFiles[index].Last().fileSize = saveSize;
                                                    tempDataSize += saveSize;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (Directory.Exists("XData\\Canary"))
                                {
                                    foreach (string dir in Directory.GetDirectories("XData\\Canary"))
                                    {
                                        string title = data.gameTitle;
                                        string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                                        Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                                        string newTitle = r.Replace(title, "");
                                        if (dir.Split("\\").Last() == newTitle)
                                        {
                                            // Xenia data
                                            float dataSize = 0;
                                            if (File.Exists(dir + "\\xenia_canary.exe"))
                                            {
                                                dataSize += new FileInfo(dir + "\\xenia_canary.exe").Length;
                                            }
                                            if (File.Exists(dir + "\\xenia.log"))
                                            {
                                                dataSize += new FileInfo(dir + "\\xenia.log").Length;
                                            }
                                            if (File.Exists(dir + "\\xenia-canary.config.toml"))
                                            {
                                                dataSize += new FileInfo(dir + "\\xenia-canary.config.toml").Length;
                                            }
                                            if (dataSize > 0)
                                            {
                                                game.dataFiles[index].Add(new DataEntry("Xenia Canary {Temporary Copy}", "Localized Xenia Data", game.ConvertDataSize("" + dataSize), null, game.logoCanary));
                                                game.dataFiles[index].Last().fileSize = dataSize;
                                                tempDataSize += dataSize;
                                            }
                                            size += dataSize;
                                            // Save data
                                            if (Directory.Exists(dir + "\\content\\" + data.titleId + "\\profile"))
                                            {
                                                float saveSize = 0;
                                                foreach (string filename in Directory.GetFiles(dir + "\\content\\" + data.titleId + "\\profile", "", SearchOption.AllDirectories))
                                                {
                                                    FileInfo file = new FileInfo(filename);
                                                    saveSize += file.Length;
                                                }
                                                if (saveSize > 0)
                                                {
                                                    game.dataFiles[index].Add(new DataEntry("Save Data (Canary)", "Xenia Game Save", game.ConvertDataSize("" + saveSize), null, game.icons[data.gameTitle]));
                                                    game.dataFiles[index].Last().fileSize = saveSize;
                                                    tempDataSize += saveSize;
                                                }
                                                size += saveSize;
                                            }
                                            // Imported DLC
                                            if (Directory.Exists(dir + "\\content\\" + data.titleId + "\\00000002"))
                                            {
                                                float saveSize = 0;
                                                foreach (string filename in Directory.GetFiles(dir + "\\content\\" + data.titleId + "\\00000002", "", SearchOption.AllDirectories))
                                                {
                                                    FileInfo file = new FileInfo(filename);
                                                    saveSize += file.Length;
                                                }
                                                if (saveSize > 0)
                                                {
                                                    game.dataFiles[index].Add(new DataEntry("Installed DLC", "Xenia Installed Content", game.ConvertDataSize("" + saveSize), null, game.icons[data.gameTitle]));
                                                    game.dataFiles[index].Last().fileSize = saveSize;
                                                    tempDataSize += saveSize;
                                                }
                                                size += saveSize;
                                            }
                                            // Imported Title Updates
                                            if (Directory.Exists(dir + "\\content\\" + data.titleId + "\\000B0000"))
                                            {
                                                float saveSize = 0;
                                                foreach (string filename in Directory.GetFiles(dir + "\\content\\" + data.titleId + "\\000B0000", "", SearchOption.AllDirectories))
                                                {
                                                    FileInfo file = new FileInfo(filename);
                                                    saveSize += file.Length;
                                                }
                                                if (saveSize > 0)
                                                {
                                                    game.dataFiles[index].Add(new DataEntry("Installed Title Update", "Xenia Installed Content", game.ConvertDataSize("" + saveSize), null, game.icons[data.gameTitle]));
                                                    game.dataFiles[index].Last().fileSize = saveSize;
                                                    tempDataSize += saveSize;
                                                }
                                                size += saveSize;
                                            }
                                        }
                                    }
                                }
                            }
                            totalSize += size;
                            // Art data
                            float tempArtSize = 0;
                            string defaultCoverPath = data.gamePath + "\\..\\..\\_covers";
                            if (Directory.Exists(defaultCoverPath)) // New cover art code
                            {
                                string[] covers = Directory.GetFiles(defaultCoverPath, "*.jpg", SearchOption.TopDirectoryOnly);
                                foreach (string cover in covers)
                                {
                                    tempArtSize += new FileInfo(cover).Length;
                                }
                            }
                            else if (File.Exists(data.artPath)) // Falling back on old code if needed
                            {
                                tempArtSize += new FileInfo(data.artPath).Length;
                            }
                            if (File.Exists(data.iconPath)) // Icon
                            {
                                tempArtSize += new FileInfo(data.iconPath).Length;
                            }
                            game.dataFiles[index].Add(new DataEntry("Artwork and Icon", "Resources", game.ConvertDataSize("" + tempArtSize), null, game.mainLogo));
                            game.dataFiles[index].Last().fileSize = tempArtSize;
                            artSize += tempArtSize;
                            switch (game.dataSort)
                            {
                                case Game1.DataSort.NameAZ:
                                    game.dataFiles[index] = game.dataFiles[index].OrderBy(o => o.name).ToList(); break;
                                case Game1.DataSort.NameZA:
                                    game.dataFiles[index] = game.dataFiles[index].OrderByDescending(o => o.name).ToList(); break;
                                case Game1.DataSort.SizeHighLow:
                                    game.dataFiles[index] = game.dataFiles[index].OrderByDescending(o => o.fileSize).ThenBy(o => o.name).ToList(); break;
                                case Game1.DataSort.SizeLowHigh:
                                    game.dataFiles[index] = game.dataFiles[index].OrderBy(o => o.fileSize).ThenBy(o => o.name).ToList(); break;
                                case Game1.DataSort.FileCountHighLow:
                                    game.dataFiles[index] = game.dataFiles[index].OrderByDescending(o => o.fileSize).ThenBy(o => o.name).ToList(); break;
                                case Game1.DataSort.FileCountLowHigh:
                                    game.dataFiles[index] = game.dataFiles[index].OrderByDescending(o => o.fileSize).ThenBy(o => o.name).ToList(); break;
                            }
                            game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + size));
                            game.dataStrings.dataIdList.Add(data.titleId.Replace("0x", ""));
                            linkedDataStrings.Add(data.gameTitle, game.dataStrings.dataSizeList.Count - 1);
                            data.fileSize = size;
                            data.fileCount = game.dataFiles[index].Count;
                            index++;

                            game.localData.Add(data); // Adding data to separate list of data
                        }
                        else
                        {
                            missingList.Add(data.gameTitle);
                        }
                    }

                    // Sorting games before adding the Launcher to the list
                    switch (game.dataSort)
                    {
                        case Game1.DataSort.NameAZ:
                            game.localData = game.localData.OrderBy(o => o.gameTitle).ToList(); break;
                        case Game1.DataSort.NameZA:
                            game.localData = game.localData.OrderByDescending(o => o.gameTitle).ToList(); break;
                        case Game1.DataSort.SizeHighLow:
                            game.localData = game.localData.OrderByDescending(o => o.fileSize).ThenBy(o => o.gameTitle).ToList(); break;
                        case Game1.DataSort.SizeLowHigh:
                            game.localData = game.localData.OrderBy(o => o.fileSize).ThenBy(o => o.gameTitle).ToList(); break;
                        case Game1.DataSort.FileCountHighLow:
                            game.localData = game.localData.OrderByDescending(o => o.fileCount).ThenByDescending(o => o.fileSize).ThenBy(o => o.gameTitle).ToList(); break;
                        case Game1.DataSort.FileCountLowHigh:
                            game.localData = game.localData.OrderBy(o => o.fileCount).ThenByDescending(o => o.fileSize).ThenBy(o => o.gameTitle).ToList(); break;
                    }
                    DataManageStrings newStrings = new DataManageStrings();
                    foreach (GameData gameData in game.localData)
                    {
                        newStrings.dataStringList.Add(game.dataStrings.dataStringList[linkedDataStrings[gameData.gameTitle]]);
                        newStrings.dataSizeList.Add(game.dataStrings.dataSizeList[linkedDataStrings[gameData.gameTitle]]);
                        newStrings.dataIdList.Add(game.dataStrings.dataIdList[linkedDataStrings[gameData.gameTitle]]);
                    }
                    game.dataStrings = newStrings;

                    // New code to add Continuum Launcher as a 'game' instead of extra strings
                    game.dataFiles.Add(new List<DataEntry>());
                    GameData cont = new GameData();
                    cont.gameTitle = "Continuum Launcher Data";
                    game.localData.Add(cont);
                    // Calculating size of main Launcher files
                    float contSize = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\", "*", SearchOption.TopDirectoryOnly))
                    {
                        FileInfo file = new FileInfo(filepath);
                        contSize += file.Length;
                    }
                    float contAudioSize = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\Content\\Audio", "*", SearchOption.AllDirectories))
                    {
                        FileInfo file = new FileInfo(filepath);
                        contAudioSize += file.Length;
                    }
                    float contDatabaseSize = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\Content\\Database", "*", SearchOption.AllDirectories))
                    {
                        FileInfo file = new FileInfo(filepath);
                        contDatabaseSize += file.Length;
                    }
                    float contFontsSize = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\Content\\Fonts", "*", SearchOption.AllDirectories))
                    {
                        FileInfo file = new FileInfo(filepath);
                        contFontsSize += file.Length;
                    }
                    float contTextureSize = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\Content\\Textures", "*", SearchOption.AllDirectories))
                    {
                        FileInfo file = new FileInfo(filepath);
                        contTextureSize += file.Length;
                    }
                    float contRuntimeSize = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\runtimes", "*", SearchOption.AllDirectories))
                    {
                        if (!filepath.Contains("win-x64"))
                        {
                            FileInfo file = new FileInfo(filepath);
                            contRuntimeSize += file.Length;
                        }
                    }
                    float win64Size = 0;
                    foreach (string filepath in Directory.GetFiles(Environment.CurrentDirectory + "\\runtimes\\win-x64", "*", SearchOption.AllDirectories))
                    {
                        FileInfo file = new FileInfo(filepath);
                        win64Size += file.Length;
                    }
                    float[] appsSize = { 0.0f, 0.0f, 0.0f, 0.0f };
                    if (File.Exists(Environment.CurrentDirectory + "\\Apps\\Xenia\\xenia.exe"))
                    {
                        appsSize[0] = new FileInfo(Environment.CurrentDirectory + "\\Apps\\Xenia\\xenia.exe").Length;
                    }
                    if (File.Exists(Environment.CurrentDirectory + "\\Apps\\Canary\\xenia_canary.exe"))
                    {
                        appsSize[1] = new FileInfo(Environment.CurrentDirectory + "\\Apps\\Canary\\xenia_canary.exe").Length;
                    }
                    if (File.Exists(Environment.CurrentDirectory + "\\Apps\\Dump\\xenia-vfs-dump.exe"))
                    {
                        appsSize[2] = new FileInfo(Environment.CurrentDirectory + "\\Apps\\Dump\\xenia-vfs-dump.exe").Length;
                    }
                    appsSize[3] = appsSize[0] + appsSize[1] + appsSize[2];
                    float contTotalSize = contSize + contAudioSize + contDatabaseSize + contFontsSize + contTextureSize + win64Size;
                    // Adding Launcher files to file manager
                    game.dataFiles[index].Add(new DataEntry("Continuum Launcher", "Internal", game.ConvertDataSize("" + contTotalSize), Environment.CurrentDirectory + "\\", game.mainLogo));
                    game.dataFiles[index].Last().fileSize = contTotalSize;
                    game.dataFiles[index].Add(new DataEntry("Xenia", "Internal (Xenia)", game.ConvertDataSize("" + appsSize[0]), Environment.CurrentDirectory + "\\Apps\\Xenia\\", game.logo));
                    game.dataFiles[index].Last().fileSize = appsSize[0];
                    game.dataFiles[index].Add(new DataEntry("Xenia Canary", "Internal (Xenia)", game.ConvertDataSize("" + appsSize[1]), Environment.CurrentDirectory + "\\Apps\\Canary\\", game.logoCanary));
                    game.dataFiles[index].Last().fileSize = appsSize[1];
                    game.dataFiles[index].Add(new DataEntry("Xenia VFS Dump Tool", "Internal (Xenia)", game.ConvertDataSize("" + appsSize[2]), Environment.CurrentDirectory + "\\Apps\\Dump\\", game.logo));
                    game.dataFiles[index].Last().fileSize = appsSize[2];
                    game.dataFiles[index].Add(new DataEntry("Optional Additional Compatibility Runtimes", "Internal", game.ConvertDataSize("" + contRuntimeSize), Environment.CurrentDirectory + "\\runtimes\\", game.mainLogo));
                    game.dataFiles[index].Last().fileSize = contRuntimeSize;
                    game.dataFiles[index].Add(new DataEntry("Total: Artwork and Icons", "User Data Metric", game.ConvertDataSize("" + artSize), Environment.CurrentDirectory + "\\IconData\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = -1;
                    game.dataFiles[index].Add(new DataEntry("Total: Xenia Content and Temporary Data", "User Data Metric", game.ConvertDataSize("" + tempDataSize), Environment.CurrentDirectory + "\\XData\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = -2;
                    game.dataFiles[index].Add(new DataEntry("Total: All Storage Use", "User Data Metric", game.ConvertDataSize("" + (contTotalSize + contRuntimeSize + win64Size + totalSize + artSize)), null, game.compLogo));
                    game.dataFiles[index].Last().fileSize = -3;
#if DEBUG
                    game.dataFiles[index].Add(new DataEntry("XeniaLauncher: Debug Build Root Folder", "Internal (Non-Indexed)", game.ConvertDataSize("" + contSize), Environment.CurrentDirectory + "\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = contSize;
                    game.dataFiles[index].Add(new DataEntry("Content: Compiled XNB Audio Files", "Internal (Non-Indexed)", game.ConvertDataSize("" + contAudioSize), Environment.CurrentDirectory + "\\Content\\Audio\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = contAudioSize;
                    game.dataFiles[index].Add(new DataEntry("Content: X360 Game Database", "Internal (Non-Indexed)", game.ConvertDataSize("" + contDatabaseSize), Environment.CurrentDirectory + "\\Content\\Database\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = contDatabaseSize;
                    game.dataFiles[index].Add(new DataEntry("Content: Compiled XNB Font Files & TTF Fonts", "Internal (Non-Indexed)", game.ConvertDataSize("" + contFontsSize), Environment.CurrentDirectory + "\\Content\\Fonts\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = contFontsSize;
                    game.dataFiles[index].Add(new DataEntry("Content: Compiled XNB Textures", "Internal (Non-Indexed)", game.ConvertDataSize("" + contTextureSize), Environment.CurrentDirectory + "\\Content\\Textures\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = contTextureSize;
                    game.dataFiles[index].Add(new DataEntry("Apps: Xenia + Canary + Dump", "Internal (Non-Indexed)", game.ConvertDataSize("" + appsSize[3]), Environment.CurrentDirectory + "\\Content\\Apps\\", game.logoCanary));
                    game.dataFiles[index].Last().fileSize = appsSize[3];
                    game.dataFiles[index].Add(new DataEntry("XeniaLauncher: Win-x64 Runtime", "Internal (Non-Indexed)", game.ConvertDataSize("" + win64Size), Environment.CurrentDirectory + "\\runtimes\\win-x64\\", game.compLogo));
                    game.dataFiles[index].Last().fileSize = win64Size;
#endif

                    // Adding Launcher to games
                    game.dataStrings.dataStringList.Add("Continuum Launcher");
                    game.dataStrings.dataSizeList.Add(game.ConvertDataSize("" + (contTotalSize + win64Size + contRuntimeSize)));
                    game.dataStrings.dataIdList.Add("Internal");
                    //game.dataStrings.dataStringList.Add("Artwork and Icons");
                    //game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + artSize));
                    //game.dataStrings.dataIdList.Add("Internal");
                    //game.dataStrings.dataStringList.Add("Xenia Content and Temporary Data");
                    //game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + tempDataSize));
                    //game.dataStrings.dataIdList.Add("Internal");
                    //game.dataStrings.dataStringList.Add("All Games and Data");
                    //game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + totalSize));
                    //game.dataStrings.dataIdList.Add("Internal");
                    //game.dataStrings.dataStringList.Add("All Launcher Data");
                    //game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + (totalSize + artSize)));
                    //game.dataStrings.dataIdList.Add("Internal");
                    game.dataWindow.strings.Add("");

                    // Extra buttons
                    game.dataWindow.AddText("Sort Entries");
                    game.dataWindow.AddText("Filter Entries");
                    game.dataWindow.AddText("Refresh Data");
                    game.dataWindow.AddText("Back to Menu");
                    foreach (TextSprite sprite in game.dataWindow.sprites)
                    {
                        sprite.scale = 0.6f;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        game.dataStrings.dataStringList.Add("");
                        game.dataStrings.dataSizeList.Add("");
                        game.dataStrings.dataIdList.Add("");
                    }

                    // TextSprites for the window
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataStringList[0], 0.6f, new Vector2(140, 160), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataStringList[1], 0.6f, new Vector2(140, 290), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataStringList[2], 0.6f, new Vector2(140, 420), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataStringList[3], 0.6f, new Vector2(140, 550), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataStringList[4], 0.6f, new Vector2(140, 680), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataStringList[5], 0.6f, new Vector2(140, 810), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[0], 0.6f, new Vector2(1140, 160), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[1], 0.6f, new Vector2(1140, 290), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[2], 0.6f, new Vector2(1140, 420), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[3], 0.6f, new Vector2(1140, 550), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[4], 0.6f, new Vector2(1140, 680), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[5], 0.6f, new Vector2(1140, 810), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataIdList[0], 0.4f, new Vector2(140, 220), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataIdList[1], 0.4f, new Vector2(140, 350), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataIdList[2], 0.4f, new Vector2(140, 480), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataIdList[3], 0.4f, new Vector2(140, 610), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataIdList[4], 0.4f, new Vector2(140, 740), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataIdList[5], 0.4f, new Vector2(140, 870), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(32, 162, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(32, 292, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(32, 422, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(32, 552, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(32, 682, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.dataWindow.extraSprites.Add(new ObjectSprite(game.white, new Rectangle(32, 812, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    // Bottom strings for counts
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, "1 of " + (game.localData.Count + 4), 0.6f, new Vector2(60, 965), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.dataWindow.extraSprites.Add(new TextSprite(game.font, game.ConvertDataSize("" + totalSize) + " Total", 0.6f, new Vector2(1600, 965), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.dataWindow.inputEvents.UpButton(game, game.dataWindow, 0);
                    game.dataWindow.inputEvents.DownButton(game, game.dataWindow, 5);
                    for (int i = 12; i < 18; i++)
                    {
                        game.dataWindow.extraSprites[i].tags.Add("gray");
                    }

                    game.state = Game1.State.Data;

                    // Showing message if game files are missing
                    if (missingList.Count > 0)
                    {
                        string message = "";
                        if (missingList.Count == 1)
                        {
                            message = "A game could not be found: " + missingList[0];
                        }
                        else if (missingList.Count == 2)
                        {
                            message = "" + missingList.Count + " games could not be found, including " + missingList[0] + " and " + missingList[1];
                        }
                        else
                        {
                            message = "" + missingList.Count + " games could not be found, including " + missingList[0] + ", " + missingList[1] + " and " + (missingList.Count - 2) + " more";
                        }
                        game.message = new MessageWindow(game, "Perhaps the archives are incomplete...", message, Game1.State.Data);
                        game.state = Game1.State.Message;
                    }
                }
                catch (FileNotFoundException e)
                {
                    //game.message = new MessageWindow(game, "Error", "Unknown error while searching for game files", Game1.State.Menu);
                    game.message = new MessageWindow(game, "Error", e.ToString().Split("\n")[0], Game1.State.Menu);
                    game.state = Game1.State.Message;
                }
                
            }
            // About/Credits
            else if (buttonIndex == 4)
            {
                game.creditsWindow = new Window(game, new Rectangle(150, 50, 1620, 980), "About Continuum Launcher", "Version " + Shared.VERSION + ", compiled on " + Shared.COMPILED, new MessageButtonEffects(), new SingleButtonEvent(), new GenericStart(), Game1.State.Menu, true);
                game.creditsWindow.AddButton(new Rectangle(460, 900, 1000, 100));
                game.creditsWindow.AddText("Back to Menu");

                game.creditsWindow.extraSprites.Add(new ObjectSprite(game.mainLogo, new Rectangle(0, 0, 175, 175), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 300));
                game.creditsWindow.extraSprites.Add(new TextSprite(game.font, "Continuum Launcher created by William Jones (a.k.a. Littleozzz10)", 0.6f, new Vector2(0, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 460));
                game.creditsWindow.extraSprites.Add(new TextSprite(game.font, "Xbox 360 sounds and Convection font made by Microsoft.", 0.6f, new Vector2(0, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 540));
                game.creditsWindow.extraSprites.Add(new TextSprite(game.font, "Disclaimer: Continuum Launcher has no affiliation with the Xenia Project.", 0.6f, new Vector2(0, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 620));
                game.creditsWindow.extraSprites.Add(new TextSprite(game.font, "Xenia and Xenia Canary logos made by the Xenia Project.", 0.6f, new Vector2(0, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 680));
                game.creditsWindow.extraSprites.Add(new TextSprite(game.font, "Game information data provided by MobyGames.", 0.6f, new Vector2(0, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 750));
                game.creditsWindow.extraSprites.Add(new TextSprite(game.font, "Available on GitHub: Littleozzz10\\Continuum Launcher", 0.6f, new Vector2(0, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                game.creditsWindow.extraSprites.Last().Centerize(new Vector2(960, 820));

                game.state = Game1.State.Credits;
            }
            // Exit
            else if (buttonIndex == 5)
            {
                game.Exit();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {

        }
    }
}
