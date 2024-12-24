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
        public void InitializeOzzzFrameworkSystems()
        {
            Ozzz.Initialize(new Vector2((float)GraphicsDevice.Viewport.Width / 1920, (float)GraphicsDevice.Viewport.Height / 1080), 60);
            GamepadInput.AddIndex(PlayerIndex.One);
            Logging.Write(LogType.Standard, Event.InitEvent, "OzzzFramework initialized");
        }

        public void InitializeDataLists()
        {
            gameData = new List<GameData>();
            masterData = new List<GameData>();
            localData = new List<GameData>();
            folders = new List<string>();
            trivia = new List<string>();
            dataStrings = new DataManageStrings();
            dataFiles = new List<List<DataEntry>>();
            Logging.Write(LogType.Standard, Event.InitEvent, "Data lists initalized");
        }

        public void InitializeInput()
        {
            KeyboardInput.keys.Add("Right", new Key(Keys.Right));
            KeyboardInput.keys.Add("Left", new Key(Keys.Left));
            KeyboardInput.keys.Add("Up", new Key(Keys.Up));
            KeyboardInput.keys.Add("Down", new Key(Keys.Down));
            KeyboardInput.keys.Add("RShift", new Key(Keys.RightShift));
            KeyboardInput.keys.Add("LShift", new Key(Keys.LeftShift));
            KeyboardInput.keys.Add("RCtrl", new Key(Keys.RightControl));
            KeyboardInput.keys.Add("LCtrl", new Key(Keys.LeftControl));
            KeyboardInput.keys.Add("Caps", new Key(Keys.CapsLock));
            KeyboardInput.keys.Add("Enter", new Key(Keys.Enter));
            KeyboardInput.keys.Add("Space", new Key(Keys.Space));
            KeyboardInput.keys.Add("Backspace", new Key(Keys.Back));
            KeyboardInput.keys.Add("Tab", new Key(Keys.Tab));
            KeyboardInput.keys.Add("Escape", new Key(Keys.Escape));
            KeyboardInput.keys.Add("1", new Key(Keys.D1));
            KeyboardInput.keys.Add("2", new Key(Keys.D2));
            KeyboardInput.keys.Add("3", new Key(Keys.D3));
            KeyboardInput.keys.Add("4", new Key(Keys.D4));
            KeyboardInput.keys.Add("5", new Key(Keys.D5));
            KeyboardInput.keys.Add("6", new Key(Keys.D6));
            KeyboardInput.keys.Add("7", new Key(Keys.D7));
            KeyboardInput.keys.Add("8", new Key(Keys.D8));
            KeyboardInput.keys.Add("9", new Key(Keys.D9));
            KeyboardInput.keys.Add("0", new Key(Keys.D0));
            KeyboardInput.keys.Add("-", new Key(Keys.OemMinus));
            KeyboardInput.keys.Add("=", new Key(Keys.OemPlus));
            KeyboardInput.keys.Add("Q", new Key(Keys.Q));
            KeyboardInput.keys.Add("W", new Key(Keys.W));
            KeyboardInput.keys.Add("E", new Key(Keys.E));
            KeyboardInput.keys.Add("R", new Key(Keys.R));
            KeyboardInput.keys.Add("T", new Key(Keys.T));
            KeyboardInput.keys.Add("Y", new Key(Keys.Y));
            KeyboardInput.keys.Add("U", new Key(Keys.U));
            KeyboardInput.keys.Add("I", new Key(Keys.I));
            KeyboardInput.keys.Add("O", new Key(Keys.O));
            KeyboardInput.keys.Add("P", new Key(Keys.P));
            KeyboardInput.keys.Add("[", new Key(Keys.OemOpenBrackets));
            KeyboardInput.keys.Add("]", new Key(Keys.OemCloseBrackets));
            KeyboardInput.keys.Add("\\", new Key(Keys.OemBackslash));
            KeyboardInput.keys.Add("A", new Key(Keys.A));
            KeyboardInput.keys.Add("S", new Key(Keys.S));
            KeyboardInput.keys.Add("D", new Key(Keys.D));
            KeyboardInput.keys.Add("F", new Key(Keys.F));
            KeyboardInput.keys.Add("G", new Key(Keys.G));
            KeyboardInput.keys.Add("H", new Key(Keys.H));
            KeyboardInput.keys.Add("J", new Key(Keys.J));
            KeyboardInput.keys.Add("K", new Key(Keys.K));
            KeyboardInput.keys.Add("L", new Key(Keys.L));
            KeyboardInput.keys.Add(";", new Key(Keys.OemSemicolon));
            KeyboardInput.keys.Add("'", new Key(Keys.OemQuotes));
            KeyboardInput.keys.Add("Z", new Key(Keys.Z));
            KeyboardInput.keys.Add("X", new Key(Keys.X));
            KeyboardInput.keys.Add("C", new Key(Keys.C));
            KeyboardInput.keys.Add("V", new Key(Keys.V));
            KeyboardInput.keys.Add("B", new Key(Keys.B));
            KeyboardInput.keys.Add("N", new Key(Keys.N));
            KeyboardInput.keys.Add("M", new Key(Keys.M));
            KeyboardInput.keys.Add(",", new Key(Keys.OemComma));
            KeyboardInput.keys.Add(".", new Key(Keys.OemPeriod));
            KeyboardInput.keys.Add("/", new Key(Keys.OemQuestion));
            MouseInput.posCapacity = 2;
            Logging.Write(LogType.Standard, Event.InitEvent, "Input systems initialized");
        }

        public void ReadAndInitializeConfig()
        {
            // Config file
            folders.Add("All Games");
            SaveData read = new SaveData(configPath);
            read.ReadFile();

            // Xenia paths
            //xeniaPath = read.savedData.FindData("xenia").data;
            //if (!File.Exists(xeniaPath))
            //{
            //    if (File.Exists("Apps\\Xenia\\xenia.exe"))
            //    {
            //        xeniaPath = "Apps\\Xenia\\xenia.exe";
            //    }
            //}
            //canaryPath = read.savedData.FindData("canary").data;
            //if (!File.Exists(canaryPath))
            //{
            //    if (File.Exists("Apps\\Canary\\xenia_canary.exe"))
            //    {
            //        xeniaPath = "Apps\\Canary\\xenia_canary.exe";
            //    }
            //}
            FindXenia();

            // Experimental config file
            SaveDataObject exp = read.savedData.FindData("enableExp");
            if (exp != null)
            {
                enableExp = Convert.ToBoolean(exp.data);
                Logging.Write(LogType.Critical, Event.InitEvent, "Experimental config file loaded");
            }

            // Vernum
            SaveDataObject ver = read.savedData.FindData("vernum");
            if (ver != null)
            {
                configVernum = Convert.ToInt32(ver.data);
                Logging.Write(LogType.Critical, Event.InitEvent, "Vernum read from config", "vernum", "" + configVernum);
                if (configVernum == Shared.VERNUM)
                {
                    welcomeShown = true;
                }
            }
            else
            {
                configVernum = Shared.VERNUM - 1;
            }

            // Game data
            SaveDataChunk games = read.savedData.FindData("games").GetChunk();
            foreach (SaveDataChunk game in games.saveDataObjects)
            {
                GameData data = new GameData();
                data.Read(game);
                masterData.Add(data);
                // Logging the game
                if (Logging.logType == LogType.Important)
                {
                    Logging.Write(LogType.Important, Event.GameLoad, "Game loaded from config file", new Dictionary<string, string>()
                    {
                        { "gameTitle", data.gameTitle },
                        { "gamePath", data.gamePath },
                        { "titleID", data.titleId }
                    });
                }
                else if (Logging.logType == LogType.Standard)
                {
                    Logging.Write(LogType.Important, Event.GameLoad, "Game loaded from config file", new Dictionary<string, string>()
                    {
                        { "gameTitle", data.gameTitle },
                        { "gamePath", data.gamePath },
                        { "titleID", data.titleId },
                        { "artPath", data.artPath },
                        { "iconPath", data.iconPath }
                    });
                }
                else if (Logging.logType == LogType.Debug)
                {
                    Dictionary<string, string> config = new Dictionary<string, string>()
                    {
                        { "alphaAs", data.alphaAs },
                        { "artPath", data.artPath },
                        { "canaryCompat", data.canaryCompat.ToString() },
                        { "cpuReadback", data.cpuReadback.ToString() },
                        { "day", "" + data.day },
                        { "developer", data.developer },
                        { "fileCount", "" + data.fileCount },
                        { "fileSize", "" + data.fileSize },
                        { "gamePath", data.gamePath },
                        { "gameTitle", data.gameTitle },
                        { "hasCoverArt", data.hasCoverArt.ToString() },
                        { "iconPath", data.iconPath },
                        { "kinect", data.kinect.ToString() },
                        { "lastPlayed", "" + data.lastPlayed },
                        { "license", data.license.ToString() },
                        { "maxPlayers", "" + data.maxPlayers },
                        { "minPlayers", "" + data.minPlayers },
                        { "month", "" + data.month },
                        { "mountCache", data.mountCache.ToString() },
                        { "preferCanary", data.preferCanary.ToString() },
                        { "publisher", data.publisher },
                        { "renderer", data.renderer.ToString() },
                        { "resX", "" + data.resX },
                        { "resY", "" + data.resY },
                        { "timesLaunched", "" + data.timesLaunched },
                        { "titleID", data.titleId },
                        { "vsync", data.vsync.ToString() },
                        { "xeniaCompat", data.xeniaCompat.ToString() },
                        { "year", "" + data.year }
                    };
                    for (int i = 0; i < data.folders.Count; i++)
                    {
                        config.Add("folder" + i, data.folders[i]);
                    }
                    for (int i = 0; i < data.xexNames.Count; i++)
                    {
                        config.Add("xexName" + i, data.xexNames[i]);
                    }
                    for (int i = 0; i < data.xexPaths.Count; i++)
                    {
                        config.Add("xexPath" + i, data.xexPaths[i]);
                    }
                    Logging.Write(LogType.Important, Event.GameLoad, "Game loaded from config file", config);
                }
            }
            Logging.Write(LogType.Critical, Event.GameLoadComplete, "Config load complete, " + masterData.Count + " games found");
        }

        public void SetDefaultSettings()
        {
            // Making new settings file if not already present
            SoundEffect.MasterVolume = 0.7f;
            ResetTheme(Theme.Original, false);
            cwSettings = CWSettings.Untested;
            xeniaFullscreen = false;
            logLevel = LogLevel.Info;
            consolidateFiles = true;
            runHeadless = false;
            if (!File.Exists("XLSettings.txt"))
            {
                Logging.Write(LogType.Critical, Event.Error, "Settings file missing, creating default");
                configData = new SaveData("XLSettings.txt");
                SaveSettings();
                triggerMissingWindow = true;
            }
        }

        /// <summary>
        /// Reads in the config file and sets variables from it
        /// </summary>
        public void ReadAndInitializeSettings()
        {
            // Reading in settings
            try
            {
                configData = new SaveData("XLSettings.txt");
                configData.ReadFile();
                SoundEffect.MasterVolume = (float)Convert.ToDecimal(configData.savedData.FindChunkedData("masterVolume", false).data);
                ResetTheme((Theme)Convert.ToInt32(configData.savedData.FindChunkedData("menuTheme", false).data), false);
                cwSettings = (CWSettings)Convert.ToInt32(configData.savedData.FindChunkedData("cwSettings", false).data);
                SetResolution(new Vector2((float)Convert.ToDecimal(configData.savedData.FindChunkedData("resX", false).data), (float)Convert.ToDecimal(configData.savedData.FindChunkedData("resY", false).data)));
                if (Convert.ToBoolean(configData.savedData.FindChunkedData("fullscreen", false).data))
                {
                    ToggleFullscreen();
                }
                if (Convert.ToBoolean(configData.savedData.FindChunkedData("vsync", false).data))
                {
                    ToggleVSync();
                }
                showRings = Convert.ToBoolean(configData.savedData.FindChunkedData("showRings", false).data);
                logLevel = (LogLevel)Convert.ToInt32(configData.savedData.FindChunkedData("logLevel", false).data);
                xeniaFullscreen = Convert.ToBoolean(configData.savedData.FindChunkedData("runFullscreen", false).data);
                consolidateFiles = Convert.ToBoolean(configData.savedData.FindChunkedData("consolidate", false).data);
                runHeadless = Convert.ToBoolean(configData.savedData.FindChunkedData("headless", false).data);
                militaryTime = Convert.ToBoolean(configData.savedData.FindChunkedData("militaryTime", false).data);
                inverseDate = Convert.ToBoolean(configData.savedData.FindChunkedData("inverseDate", false).data);
                checkDrivesOnManage = Convert.ToBoolean(configData.savedData.FindChunkedData("checkDrivesOnManage", false).data);
                // Logging
                if (Logging.logType >= LogType.Standard)
                {
                    Logging.Write(LogType.Standard, Event.SettingsRead, "Settings loaded", new Dictionary<string, string>()
                    {
                        { "masterVolume", "" + SoundEffect.MasterVolume },
                        { "theme", theme.ToString() },
                        { "cwSettings", cwSettings.ToString() },
                        { "resolutionWidth", "" + _graphics.PreferredBackBufferWidth },
                        { "resolutionHeight", "" + _graphics.PreferredBackBufferHeight },
                        { "fullscreen", "" + GetFullscreen() },
                        { "showRings", showRings.ToString() },
                        { "logLevel", logLevel.ToString() },
                        { "xeniaFullscreen", xeniaFullscreen.ToString() },
                        { "consolidateFiles", consolidateFiles.ToString() },
                        { "runHeadless", runHeadless.ToString() },
                        { "militaryTime", militaryTime.ToString() },
                        { "inverseDate", inverseDate.ToString() },
                        { "checkDrivesOnManage", checkDrivesOnManage.ToString() },
                    });
                }
                else
                {
                    Logging.Write(LogType.Important, Event.SettingsRead, "Settings loaded");
                }
            }
            catch (Exception e)
            {
                SoundEffect.MasterVolume = 0.8f;
                ResetTheme(Theme.Original, false);
                cwSettings = CWSettings.Untested;
                xeniaFullscreen = false;
                logLevel = LogLevel.Info;
                consolidateFiles = true;
                runHeadless = false;
                checkDrivesOnManage = true;
                Logging.Write(LogType.Critical, Event.Error, "Reverting to default settings", "exception", e.ToString());
            }
        }

        public void InitializeGameFolders()
        {
            // Folders
            foreach (GameData data in masterData)
            {
                data.folders.Add("All Games");
                gameData.Add(data);
                foreach (string folder in data.folders)
                {
                    if (!folders.Contains(folder))
                    {
                        folders.Add(folder);
                        Logging.Write(LogType.Standard, Event.FolderCreate, "New Folder created: " + folder, new Dictionary<string, string>() { { "originGameTitle", data.gameTitle } });
                    }
                }
            }
            folders.Sort();
            folderIndex = folders.IndexOf("All Games");
            Logging.Write(LogType.Important, Event.InitEvent, "Folders loaded");
        }
    }
}
