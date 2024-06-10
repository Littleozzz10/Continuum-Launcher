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
                game.dataWindow.AddButton(new Rectangle(20, 150, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 280, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 410, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 540, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 670, 1880, 120));
                game.dataWindow.AddButton(new Rectangle(20, 800, 1880, 120));
                foreach (GameData data in game.masterData)
                {
                    game.dataWindow.AddText("");
                }

                game.dataStrings.Clear();
                game.dataFiles.Clear();
                foreach (GameData data in game.masterData)
                {
                    game.dataFiles.Add(new List<DataEntry>());
                }
                float totalSize = 0;
                float artSize = 0;
                float tempDataSize = 0;
                int index = 0;
                try
                {
                    foreach (GameData data in game.masterData)
                    {
                        game.dataStrings.dataStringList.Add(data.gameTitle);
                        // Game data
                        if (File.Exists(data.gamePath))
                        {
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
                                        if (dir.Name == "EXTRACT")
                                        {
                                            float localSize = 0;
                                            foreach (FileInfo file in dir.GetFiles("*", SearchOption.AllDirectories))
                                            {
                                                localSize += file.Length;
                                            }
                                            size += localSize;
                                            game.dataFiles[index].Add(new DataEntry("Extract", Shared.contentTypes["EXTRACT"], game.ConvertDataSize("" + localSize), null, game.icons[data.gameTitle]));
                                            game.dataFiles[index].Last().fileSize = localSize;
                                        }
                                        else
                                        {
                                            foreach (FileInfo file in dir.GetFiles("*", SearchOption.TopDirectoryOnly))
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
                            else
                            {
                                foreach (FileInfo file in directory.GetFiles("*", SearchOption.AllDirectories))
                                {
                                    size += file.Length;
                                }
                                game.dataFiles[index].Add(new DataEntry(data.gameTitle, "Installed Xbox 360 Game", game.ConvertDataSize("" + size), null, game.icons[data.gameTitle]));
                            }
                            // Saves and Xenia data
                            if (Directory.Exists("XData"))
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
                            if (File.Exists(data.artPath))
                            {
                                tempArtSize += new FileInfo(data.artPath).Length;
                            }
                            if (File.Exists(data.iconPath))
                            {
                                tempArtSize += new FileInfo(data.iconPath).Length;
                            }
                            game.dataFiles[index].Add(new DataEntry("Artwork and Icon", "Resources", game.ConvertDataSize("" + tempArtSize), null, game.mainLogo));
                            game.dataFiles[index].Last().fileSize = tempArtSize;
                            artSize += tempArtSize;
                            game.dataFiles[index] = game.dataFiles[index].OrderByDescending(o => o.fileSize).ThenBy(o => o.name).ToList();
                            game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + size));
                            game.dataStrings.dataIdList.Add(data.titleId.Replace("0x", ""));
                            index++;
                        }
                    }
                    game.dataStrings.dataStringList.Add("Continuum Launcher");
                    game.dataStrings.dataSizeList.Add("Unknown");
                    game.dataStrings.dataIdList.Add("Internal");
                    game.dataStrings.dataStringList.Add("Artwork and Icons");
                    game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + artSize));
                    game.dataStrings.dataIdList.Add("Internal");
                    game.dataStrings.dataStringList.Add("Xenia Content and Temporary Data");
                    game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + tempDataSize));
                    game.dataStrings.dataIdList.Add("Internal");
                    game.dataStrings.dataStringList.Add("All Games and Data");
                    game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + totalSize));
                    game.dataStrings.dataIdList.Add("Internal");
                    game.dataStrings.dataStringList.Add("All Launcher Data");
                    game.dataStrings.dataSizeList.Add("" + game.ConvertDataSize("" + (totalSize + artSize)));
                    game.dataStrings.dataIdList.Add("Internal");
                    for (int i = 0; i < 5; i++)
                    {
                        game.dataWindow.strings.Add("");
                    }
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
                    game.dataWindow.inputEvents.UpButton(game, game.dataWindow, 0);
                    game.dataWindow.inputEvents.DownButton(game, game.dataWindow, 5);
                    for (int i = 12; i < 18; i++)
                    {
                        game.dataWindow.extraSprites[i].tags.Add("gray");
                    }

                    game.state = Game1.State.Data;
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
                game.creditsWindow = new Window(game, new Rectangle(150, 50, 1620, 980), "About Continuum Launcher", new MessageButtonEffects(), new SingleButtonEvent(), new GenericStart(), Game1.State.Menu);
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
