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
using SharpFont;
using System.Reflection;
using SharpFont.PostScript;

namespace XeniaLauncher
{
    public class ManageDataEffects : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, OzzzFramework.ObjectSprite origin, int buttonIndex)
        {
            if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Localized Xenia Data" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Xenia Installed Content" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Xenia Game Save")
            {
                game.toDelete = game.dataFiles[game.selectedDataIndex][buttonIndex];
                game.message = new MessageWindow(game, "Delete File", "Are you sure you want to delete " + game.dataFiles[game.selectedDataIndex][buttonIndex].name + "?", Game1.State.Manage, MessageWindow.MessagePrompts.YesNo);
                game.state = Game1.State.Message;
            }
            else if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Downloadable Content" || game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Title Update")
            {
                if (!String.IsNullOrEmpty(game.extractPath))
                {
                    game.toImport = game.dataFiles[game.selectedDataIndex][buttonIndex];
                    game.message = new MessageWindow(game, "Import DLC", "Do you want to import " + game.dataFiles[game.selectedDataIndex][buttonIndex].name + "?", Game1.State.Manage, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.message = new MessageWindow(game, "Import DLC", "Error: extraction tool not found or bad extract path", Game1.State.Data);
                    game.state = Game1.State.Message;
                }
            }
            else if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle == "Resources")
            {
                game.message = new MessageWindow(game, "Error", "Unable to delete resources currently loaded into Continuum.", Game1.State.Manage);
                game.state = Game1.State.Message;
            }
            else
            {
                game.message = new MessageWindow(game, "Error", "While Continuum is in a beta state, as a precaution, it cannot delete game files.", Game1.State.Manage);
                game.state = Game1.State.Message;
            }
        }
        public void SetupEffects(Game1 game, Window source)
        {
            int buttonIndex = source.stringIndex;

            // Reading STFS data
            if (buttonIndex < game.gameData.Count)
            {
                // Purging any data that's about to be read again
                for (int i = 0; i < game.dataFiles[buttonIndex].Count; i++)
                {
                    DataEntry entry = game.dataFiles[buttonIndex][i];
                    // Excluding Xenia data, since it will already have been read prior to this
                    if (entry.subTitle != "Localized Xenia Data" && entry.subTitle != "Xenia Game Save" && entry.subTitle != "Xenia Installed Content")
                    {
                        game.dataFiles[buttonIndex].RemoveAt(i);
                        i--;
                    }
                }

                // Getting the data of the selected game
                GameData data = game.gameData[buttonIndex];
                float size = 0;
                DirectoryInfo directoryInfo = new DirectoryInfo(data.gamePath).Parent;
                DirectoryInfo parent = directoryInfo.Parent;
                // Getting all directories inside the game's content folder (00004000, 000D0000, 00000002, etc)
                foreach (DirectoryInfo dir in parent.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    // Making sure the directory matches with an X360 content type ID
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
                            game.dataFiles[buttonIndex].Add(new DataEntry("Extract", Shared.contentTypes["EXTRACT"], game.ConvertDataSize("" + localSize), null, game.icons[data.gameTitle]));
                            game.dataFiles[buttonIndex].Last().fileSize = localSize;
                        }
                        // Handling all other folders
                        else
                        {
                            // Looping through all X360 content files
                            foreach (FileInfo file in dir.GetFiles("*", SearchOption.TopDirectoryOnly))
                            {
                                STFS stfs = new STFS(file.FullName);
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
                                stfs.icon.Save(memory, stfs.icon.RawFormat);
                                try
                                {
                                    localTexture = Texture2D.FromStream(game.GraphicsDevice, memory);
                                }
                                catch
                                {
                                    // Using the default blank texture as a backup
                                    localTexture = game.white;
                                }
                                // Handling garbage data at the start of the title name (Yes, there is a problem with the STFS code that causes this
                                //   but the problem has not yet been identified)
                                string newTitle = stfs.data.titleName;
                                if (dir.Name == "00004000" || dir.Name == "00007000" || dir.Name == "000D0000") // Disc Game, GoD, and XBLA Title, respectively
                                {
                                    while (newTitle[0] != stfs.data.displayName[0])
                                    {
                                        newTitle = newTitle.Substring(1);
                                    }
                                }
                                // Adding data
                                else if (dir.Name == "00000001") // 00000001: X360 saved game
                                {
                                    newTitle = newTitle.Substring(5); // Yes, this is spaghetti garbage
                                }
                                game.dataFiles[buttonIndex].Add(new DataEntry(newTitle, Shared.contentTypes[dir.Name], game.ConvertDataSize("" + localSize), file.FullName, localTexture));
                                game.dataFiles[buttonIndex].Last().fileSize = localSize;
                            }
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
                    game.manageWindow.AddText("" + i);
                }
                game.manageWindow.tags.Add("0");
            }
        }
    }
}
