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
using STFS;
using XLCompanion;
using SharpFont;
using System.Reflection;
using SharpFont.PostScript;
using Continuum_Launcher;

namespace XeniaLauncher
{
    public class ManageDataEffects : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            game.fileManageWindow = new Window(game, new Rectangle(1205, 180, 550, 700), "", new ManageFileEffects(), new StdInputEvent(4), new GenericStart(), Game1.State.Manage);
            game.state = Game1.State.ManageFile;
            // NOTE: Buttons are added in reverse order but text strings are not
            if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Localized Xenia Data" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Xenia Installed Content" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Xenia Game Save" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Extracted Content")
            {
                game.fileManageWindow.AddButton(new Rectangle(1235, 750, 490, 80));
                game.fileManageWindow.AddButton(new Rectangle(1235, 660, 490, 80));
                game.fileManageWindow.AddText(Shared.FileManageStrings["explorer"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["delete"]);
                game.fileManageWindow.inputEvents = new StdInputEvent(2);
            }
            else if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Downloadable Content" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Title Update")
            {
                game.fileManageWindow.AddButton(new Rectangle(1235, 750, 490, 80), "Install this content for use in Xenia Canary.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 660, 490, 80), "Extract this content into an _EXTRACT folder\nin this game's directory.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 570, 490, 80), "Open the STFS Metadata Viewer.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 480, 490, 80));
                game.fileManageWindow.AddText(Shared.FileManageStrings["explorer"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["metadata"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["extract"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["install"]);
            }
            else if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Video" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Game Trailer")
            {
                game.fileManageWindow.AddButton(new Rectangle(1235, 750, 490, 80), "Play this video in Windows' default video player.\nMust first extract the STFS container.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 660, 490, 80), "Extract this video into an _EXTRACT folder\nin this game's directory.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 570, 490, 80), "Open the STFS Metadata Viewer.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 480, 490, 80));
                game.fileManageWindow.AddText(Shared.FileManageStrings["explorer"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["metadata"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["extract"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["video"]);
            }
            else if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Installed Game on Demand" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Installed Disc Game" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Xbox Live Arcade Title" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Gamer Picture" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Xbox 360 Theme" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Game Demo" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Avatar Item")
            {
                game.fileManageWindow.AddButton(new Rectangle(1235, 750, 490, 80), "Extract this content into an _EXTRACT folder\nin this game's directory.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 660, 490, 80), "Open the STFS Metadata Viewer.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 570, 490, 80));
                game.fileManageWindow.AddText(Shared.FileManageStrings["explorer"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["metadata"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["extract"]);
                game.fileManageWindow.inputEvents = new StdInputEvent(3);
            }
            else if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Configuration Data")
            {
                game.fileManageWindow.AddButton(new Rectangle(1235, 750, 490, 80), "Create a backup of the user config file,\nwhich stores info for imported games.\n(\\Continuum Launcher\\Content\\XLConfig.txt)", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.fileManageWindow.AddButton(new Rectangle(1235, 660, 490, 80));
                game.fileManageWindow.AddText(Shared.FileManageStrings["explorer"]);
                game.fileManageWindow.AddText(Shared.FileManageStrings["backup"]);
                game.fileManageWindow.inputEvents = new StdInputEvent(2);
            }
            else
            {
                game.fileManageWindow.AddButton(new Rectangle(1235, 750, 490, 80));
                game.fileManageWindow.AddText(Shared.FileManageStrings["explorer"]);
                game.fileManageWindow.inputEvents = new SingleButtonEvent();
            }
            game.fileManageWindow.buttons.Reverse();
            //game.fileManageWindow.strings.Reverse();
            game.fileManageWindow.descriptionBoxes.Reverse();
            game.fileManageWindow.buttonEffects.SetupEffects(game, game.fileManageWindow);
        }
        public void SetupEffects(Game1 game, Window source)
        {
            int buttonIndex = source.stringIndex;

            // Reading STFS data
            if (buttonIndex < game.localData.Count)
            {
                // Purging any data that's about to be read again
                for (int i = 0; i < game.dataFiles[buttonIndex].Count; i++)
                {
                    DataEntry entry = game.dataFiles[buttonIndex][i];
                    // Excluding Xenia data, since it will already have been read prior to this
                    if (entry.subTitle != "Localized Xenia Data" && entry.subTitle != "Xenia Game Save" && entry.subTitle != "Xenia Installed Content" && entry.subTitle != "Resources" && entry.subTitle != "Extracted Content" && entry.subTitle != "Internal" && entry.subTitle != "Internal (Non-Indexed)" && entry.subTitle != "Internal (Xenia)" && entry.subTitle != "User Data Metric" && entry.subTitle != "Configuration Data" && entry.subTitle != "Program Usage Data")
                    {
                        game.dataFiles[buttonIndex].RemoveAt(i);
                        i--;
                    }
                }

                // Getting the data of the selected game
                GameData data = game.localData[buttonIndex];
                float size = 0;
                DirectoryInfo directoryInfo = new DirectoryInfo(data.gamePath).Parent;
                DirectoryInfo parent = directoryInfo.Parent;
                // Getting all directories inside the game's content folder (00004000, 000D0000, 00000002, etc)
                foreach (DirectoryInfo dir in parent.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    // Making sure the directory matches with an X360 content type ID
                    if (Shared.contentTypes.Keys.Contains(dir.Name) && Shared.filteredContentTypes[game.dataFilter].Contains(dir.Name))
                    {
                        // Looping through all X360 content files
                        foreach (FileInfo file in dir.GetFiles("*", SearchOption.TopDirectoryOnly))
                        {
                            STFS iconStfs = new STFS(file.FullName);
                            // Getting the total size in bytes of the content file
                            float localSize = file.Length;
                            if (Directory.Exists(dir.FullName + "\\" + file.Name + ".data"))
                            {
                                // Taking any extra data files into account (For disc/GoD games)
                                DirectoryInfo dataDirectory = new DirectoryInfo(dir.FullName + "\\" + file.Name + ".data");
                                foreach (FileInfo dataFile in dataDirectory.GetFiles("*", SearchOption.AllDirectories))
                                {
                                    localSize += dataFile.Length;
                                }
                            }
                            size += localSize;
                            // Reading icon from STFS file into memory (not saved to storage)
                            Texture2D localTexture = game.icons[data.gameTitle];
                            MemoryStream memory = new MemoryStream();
                            if (iconStfs.icon != null)
                            {
                                iconStfs.icon.Save(memory, iconStfs.icon.RawFormat);
                            }
                            try
                            {
                                localTexture = Texture2D.FromStream(game.GraphicsDevice, memory);
                            }
                            catch (Exception e)
                            {
                                // Using the default blank texture as a backup
                                localTexture = game.white;
                                Logging.Write(Logging.LogType.Critical, Logging.LogEvent.Error, "Icon unable to save", "exception", e.ToString());
                            }
                            STFS24 stfs = new STFS24(file.FullName);
                            XMetadata stfsMeta = stfs.ReturnMetadata();
                            string newTitle = stfsMeta.GetDisplayName()[0];
                            game.dataFiles[buttonIndex].Add(new DataEntry(newTitle, Shared.contentTypes[dir.Name], game.ConvertDataSize("" + localSize), file.FullName, localTexture));
                            game.dataFiles[buttonIndex].Last().fileSize = localSize;
                            Logging.Write(Logging.LogType.Standard, Logging.LogEvent.GameFileFound, "Game file found", new Dictionary<string, string>()
                            {
                                { "filepath", file.FullName },
                                { "newTitle", newTitle },
                                { "localSize", "" + localSize }
                            });
                        }
                    }
                }
                // Sorting content files
                game.dataFiles[buttonIndex] = game.dataFiles[buttonIndex].OrderByDescending(o => o.fileSize).ThenBy(o => o.name).ToList();

                // Final window setup
                for (int i = 0; i < game.dataFiles[buttonIndex].Count; i++)
                {
                    if (i < 6)
                    {
                        game.manageWindow.AddButton(new Rectangle(20, 150 + i * 130, 1880, 120));
                        game.manageWindow.extraSprites.Add(new TextSprite(game.font, game.dataFiles[buttonIndex][i].name, 0.6f, new Vector2(140, 160 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0)));
                        TextSprite sizeText = new TextSprite(game.font, game.dataFiles[buttonIndex][i].size, 0.6f, new Vector2(1140, 160 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0));
                        sizeText.JustifyRight(new Vector2(1820, 175 + i * 130));
                        game.manageWindow.extraSprites.Add(sizeText);
                        TextSprite subTitleSprite = new TextSprite(game.font, game.dataFiles[buttonIndex][i].subTitle, 0.4f, new Vector2(140, 220 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0));
                        subTitleSprite.tags.Add("gray");
                        game.manageWindow.extraSprites.Add(subTitleSprite);
                        game.manageWindow.extraSprites.Add(new ObjectSprite(game.dataFiles[buttonIndex][i].icon, new Rectangle(32, 162 + i * 130, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    }
                    game.manageWindow.AddText("");
                }
                game.manageWindow.extraSprites.Add(new TextSprite(game.font, "1 of " + game.dataFiles[game.selectedDataIndex].Count, 0.6f, new Vector2(60, 965), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.manageWindow.extraSprites.Add(new TextSprite(game.font, game.dataStrings.dataSizeList[game.selectedDataIndex] + " Total", 0.6f, new Vector2(1600, 965), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.manageWindow.tags.Add("0");
            }
        }
    }
}
