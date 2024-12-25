using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Convert = System.Convert;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
using SaveData = XeniaLauncher.Shared.SaveData;
using SaveDataObject = XeniaLauncher.Shared.SaveData.SaveDataObject;
using SaveDataChunk = XeniaLauncher.Shared.SaveData.SaveDataChunk;
using SequenceFade = XeniaLauncher.OzzzFramework.SequenceFade;
using GameData = XeniaLauncher.Shared.GameData;
using DescriptionBox = XeniaLauncher.OzzzFramework.DescriptionBox;
using DBSpawnPos = XeniaLauncher.OzzzFramework.DescriptionBox.SpawnPositions;
using LogType = Continuum_Launcher.Logging.LogType;
using Event = Continuum_Launcher.Logging.LogEvent;
using SharpDX.MediaFoundation;
using SharpFont;
using XLCompanion;
using STFS;
using Newtonsoft.Json;
using Continuum_Launcher;
using Assimp;
using static XeniaLauncher.OzzzFramework.KeyboardInput;

namespace XeniaLauncher
{
    public partial class Game1
    {
        /// <summary>
        /// Saves the settings file
        /// </summary>
        /// <returns>Whether or not the config file was successfully saved</returns>
        public bool SaveSettings()
        {
            try
            {
                configData.savedData.Clear();
                // Version
                configData.AddSaveObject(new SaveDataObject("vernum", "" + Shared.VERNUM, SaveData.DataType.Number));
                // Launcher options
                SaveDataChunk launcherChunk = new SaveDataChunk("launcherOptions");
                launcherChunk.AddData("masterVolume", "" + SoundEffect.MasterVolume, SaveData.DataType.Decimal);
                launcherChunk.AddData("menuTheme", "" + (int)theme, SaveData.DataType.Number);
                launcherChunk.AddData("cwSettings", "" + (int)cwSettings, SaveData.DataType.Number);
                configData.AddSaveChunk(launcherChunk);
                // Graphics settings
                SaveDataChunk graphicsChunk = new SaveDataChunk("graphicsSettings");
                graphicsChunk.AddData("resX", "" + _graphics.PreferredBackBufferWidth, SaveData.DataType.Number);
                graphicsChunk.AddData("resY", "" + _graphics.PreferredBackBufferHeight, SaveData.DataType.Number);
                graphicsChunk.AddData("fullscreen", "" + _graphics.IsFullScreen, SaveData.DataType.Boolean);
                graphicsChunk.AddData("vsync", "" + _graphics.SynchronizeWithVerticalRetrace, SaveData.DataType.Boolean);
                graphicsChunk.AddData("showRings", "" + showRings, SaveData.DataType.Boolean);
                configData.AddSaveChunk(graphicsChunk);
                // Xenia settings
                SaveDataChunk xeniaChunk = new SaveDataChunk("xeniaSettings");
                xeniaChunk.AddData("logLevel", "" + (int)logLevel, SaveData.DataType.Number);
                xeniaChunk.AddData("runFullscreen", "" + xeniaFullscreen, SaveData.DataType.Boolean);
                xeniaChunk.AddData("consolidate", "" + consolidateFiles, SaveData.DataType.Boolean);
                xeniaChunk.AddData("headless", "" + runHeadless, SaveData.DataType.Boolean);
                configData.AddSaveChunk(xeniaChunk);
                // Extra settings
                SaveDataChunk extraChunk = new SaveDataChunk("extraSettings");
                extraChunk.AddData("militaryTime", "" + militaryTime, SaveData.DataType.Boolean);
                extraChunk.AddData("inverseDate", "" + inverseDate, SaveData.DataType.Boolean);
                extraChunk.AddData("checkDrivesOnManage", "" + checkDrivesOnManage, SaveData.DataType.Boolean);
                configData.AddSaveChunk(extraChunk);
                configData.SaveToFile();
                Logging.Write(LogType.Critical, Event.SettingsSave, "XLSettings saved");
                return true;
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Unable to save settings file", "exception", e.ToString());
                message = new MessageWindow(this, "Error", "Unable to save settings file", state);
                state = State.Message;
            }
            return false;
        }

        public void SaveGames()
        {
            try
            {
                SaveData save = new SaveData(configPath);
                save.AddSaveObject(new SaveDataObject("xenia", xeniaPath, SaveData.DataType.String));
                save.AddSaveObject(new SaveDataObject("canary", canaryPath, SaveData.DataType.String));
                save.AddSaveObject(new SaveDataObject("enableExp", "" + enableExp, SaveData.DataType.Boolean));
                save.AddSaveObject(new SaveDataObject("vernum", "" + Shared.VERNUM, SaveData.DataType.Number));
                SaveDataChunk chunk = new SaveDataChunk("games");
                masterData = masterData.OrderBy(o => o.gameTitle).ToList();
                foreach (GameData game in masterData)
                {
                    chunk.AddChunk(game.Save());
                    Logging.Write(LogType.Debug, Event.GameSave, "Game saved", "title", game.gameTitle);
                }
                save.AddSaveChunk(chunk);
                save.SaveToFile();
                Logging.Write(LogType.Important, Event.GameSaveComplete, "Config file saved");
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Unable to save config file", "exception", e.ToString());
                message = new MessageWindow(this, "Unable to Save", e.ToString().Split("\n")[0], State.Menu);
                state = State.Message;
            }
        }

        public void RenameGame()
        {
            if (gameData[index].gameTitle != tempGameTitle)
            {
                Logging.Write(LogType.Standard, Event.GameRename, "Renaming game", new Dictionary<string, string>()
                {
                    { "oldName", gameData[index].gameTitle },
                    { "newName", tempGameTitle }
                });
                bool continueRename = true;
                try
                {
                    // Renaming the game's content folders
                    if (Directory.Exists("XData\\Xenia\\" + gameData[index].gameTitle))
                    {
                        Directory.Move("XData\\Xenia\\" + gameData[index].gameTitle, "XData\\Xenia\\" + tempGameTitle);
                    }
                    if (Directory.Exists("XData\\Canary\\" + gameData[index].gameTitle))
                    {
                        Directory.Move("XData\\Canary\\" + gameData[index].gameTitle, "XData\\Canary\\" + tempGameTitle);
                    }
                    Logging.Write(LogType.Standard, Event.XeniaContentFolderRename, "Content folders renamed");
                }
                catch (Exception e)
                {
                    continueRename = false;
                    message = new MessageWindow(this, "Error", "File IO Error: Unable to rename content folders", State.GameInfo);
                    state = State.Message;
                    Logging.Write(LogType.Critical, Event.Error, "Failed to rename Xenia content folders, aborting game rename", "exception", e.ToString());
                }
                // Continuing if folder renaming was successful
                if (continueRename)
                {
                    if (arts.ContainsKey(gameData[index].gameTitle) && !arts.ContainsKey(tempGameTitle))
                    {
                        arts.Add(tempGameTitle, arts[gameData[index].gameTitle]);
                        arts.Remove(gameData[index].gameTitle);
                        if (icons.ContainsKey(gameData[index].gameTitle) && !icons.ContainsKey(tempGameTitle))
                        {
                            icons.Add(tempGameTitle, icons[gameData[index].gameTitle]);
                            icons.Remove(gameData[index].gameTitle);
                        }
                        else if (icons.ContainsKey(tempGameTitle))
                        {
                            tempGameTitle = tempGameTitle + " (2)";
                            RenameGame();
                        }
                    }
                    else if (arts.ContainsKey(tempGameTitle))
                    {
                        tempGameTitle = tempGameTitle + " (2)";
                        RenameGame();
                    }
                    // Changing Alpha As value if it's the same as the title
                    if (gameData[index].gameTitle == gameData[index].alphaAs)
                    {
                        gameData[index].alphaAs = tempGameTitle;
                        Logging.Write(LogType.Debug, Event.AlphaAsFix, "Updated game's alphabetization");
                    }
                    gameData[index].gameTitle = tempGameTitle;
                    SaveGames();
                    Logging.Write(LogType.Important, Event.GameRename, "Game rename successful", new Dictionary<string, string>()
                    {
                        { "oldName", gameData[index].gameTitle },
                        { "newName", tempGameTitle }
                    });
                }
            }
        }

        /// <summary>
        /// Helper method for finding games' STFS files.
        /// </summary>
        /// <param name="contentType">Example: 00004000</param>
        /// <param name="stfsFiles">Dictionary of STFS files</param>
        /// <returns>The key for Dictionary stfsFiles that should be the main executable</returns>
        public string FindSTFSGames(string dirName, string contentType, Dictionary<string, string> stfsFiles)
        {
            Logging.Write(LogType.Standard, Event.FindingSTFS, "Finding STFS game files", new Dictionary<string, string>()
            {
                { "dirName", dirName },
                { "contentType", contentType }
            });
            string main = "";
            if (Directory.Exists(dirName + "\\" + contentType))
            {
                string[] filepaths = Directory.GetFiles(dirName + "\\" + contentType, "*", SearchOption.TopDirectoryOnly);
                // Searching files
                for (int i = 0; i < filepaths.Count(); i++)
                {
                    STFS24 stfs = null;
                    try
                    {
                        stfs = new STFS24(filepaths[i]);
                        XMetadata metadata = stfs.ReturnMetadata();
                        if (STFS24.GetByteArrayAsHex(metadata.GetContentTypeAsBytes()) == contentType)
                        {
                            string title = TextSprite.GetASCII(metadata.GetDisplayName()[0], font);
                            stfsFiles.Add(title, filepaths[i]);
                            Logging.Write(LogType.Debug, Event.AddingSTFS, "Found STFS file match", "stfsTitle", metadata.GetDisplayName()[0]);
                            if (main == "" && ((metadata.GetDiscNumber() > 1 && metadata.GetDiscInSet() == 1) || metadata.GetDiscNumber() <= 1 || filepaths.Count() == 1))
                            {
                                main = title;
                                Logging.Write(LogType.Debug, Event.AddingMainSTFS, "STFS file match will be main file");
                            }
                        }
                        stfs.CloseStream();
                    }
                    catch (Exception e)
                    {
                        if (stfs != null)
                        {
                            stfs.CloseStream();
                        }
                        Logging.Write(LogType.Critical, Event.Error, "Exception while reading from STFS file", new Dictionary<string, string>()
                        {
                            { "filepath", filepaths[i] },
                            { "exception", e.ToString() }
                        });
                    };
                }
            }
            return main;
        }

        public void CheckAndCreateContentDirectories()
        {
            if (!Directory.Exists("Apps"))
            {
                Directory.CreateDirectory("Apps");
                Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Created Apps directory");
            }
            if (!Directory.Exists("Apps\\Xenia"))
            {
                Directory.CreateDirectory("Apps\\Xenia");
                Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Created Apps\\Xenia directory");
            }
            if (!Directory.Exists("Apps\\Canary"))
            {
                Directory.CreateDirectory("Apps\\Canary");
                Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Created Apps\\Canary directory");
            }
            if (!Directory.Exists("Apps\\Dump"))
            {
                Directory.CreateDirectory("Apps\\Dump");
                Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Created Apps\\Dump directory");
            }
        }
    }
}
