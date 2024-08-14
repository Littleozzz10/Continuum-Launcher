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
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D white, rectTex, logo, circ, calendar, player, logoCanary, mainLogo, topBorderTex, bottomBorderTex, compLogo, topBorderNew, bottomBorderNew;
        public SpriteFont font, bold;
        public SoundEffect selectSound, backSound, launchSound, switchSound, buttonSwitchSound, sortSound, leftFolderSound, rightFolderSound;
        public ObjectSprite xeniaCompatLogo, canaryCompatLogo, xeniaCompat, canaryCompat, topBorder, bottomBorder, topBorderBack, bottomBorderBack, jumpFade;
        public TextSprite titleSprite, subTitleSprite, sortSprite, folderSprite, xeniaUntestedText, canaryUntestedText, timeText, dateText, contNumText, controllerText, freeSpaceText, drivesText, triviaSprite, jumpToText, jumpIndexText;
        public Layer mainFadeLayer, bottomLayer, backBorderLayer, triviaMaskingLayer, topBorderLayer, jumpLayer;
        public Gradient mainFadeGradient, darkGradient, blackGradient, selectGradient, whiteGradient, buttonGradient;
        public AnimationPath mainTransitionPath, folderPath, secondFolderPath, topBorderPath, bottomBorderPath;
        public List<Ring> rings;
        public List<XGame> gameIcons;
        public List<GameData> gameData, masterData, localData;
        public Dictionary<string, Texture2D> arts, compatBars, themeThumbnails, icons;
        public Dictionary<string, string> stfsFiles; // Stores stfsFiles during the game import process
        public List<string> folders, trivia;
        public List<List<DataEntry>> dataFiles;
        public Window xexWindow, launchWindow, menuWindow, optionsWindow, graphicsWindow, compatWindow, settingsWindow, creditsWindow, dataWindow, manageWindow, deleteWindow, gameManageWindow, gameXeniaSettingsWindow, gameFilepathsWindow, gameInfoWindow, gameCategoriesWindow, gameXEXWindow, newGameWindow, databaseResultWindow, releaseWindow, databasePickerWindow, fileManageWindow, metadataWindow, dataSortWindow, dataFilterWindow;
        public MessageWindow message;
        public TextInputWindow text;
        public Color backColor, backColorAlt, fontColor, fontSelectColor, fontAltColor, fontAltLightColor, majorFontColor, sortColor, folderColor, timeDateColor, cornerStatsColor, triviaColor, topBorderColor, bottomBorderColor, ringMainColor, ringSelectColor, descColor;
        public SaveData configData;
        public DataManageStrings dataStrings;
        public SequenceFade bottomInfo;
        public DataEntry toDelete, toImport, toExtract;
        public MobyData mobyData;
        public List<GameInfo> databaseGameInfo;
        public System.Drawing.Image tempIconSTFS;
        public string xeniaPath, canaryPath, configPath, ver, compileDate, textWindowInput, newXEX, tempTitleSTFS, tempIdSTFS, tempFilepathSTFS, extractPath, newGamePath, tempGameTitle;
        public int index, ringFrames, ringDuration, folderIndex, compatWaitFrames, selectedDataIndex, compatWindowDelay, fullscreenDelay, tempCategoryIndex, databaseResultIndex, tempYear, tempMonth, tempDay, jumpLayerAlpha, jumpTriggerCooldown, jumpTriggerCooldownDefault;
        public bool right, firstLoad, firstReset, skipDraw, showRings, xeniaFullscreen, consolidateFiles, runHeadless, triggerMissingWindow, updateFreeSpace, messageYes, militaryTime, inverseDate, checkDrivesOnManage, lastActiveCheck, forceInit, newGameProcess, enableExp, hideSecretMetadata, refreshData, showResearchPrompt, windowClickExit, enterCloseTextInput, rightClickGames;
        public enum State
        {
            Main, Select, Launch, Menu, Options, Credits, Graphics, Settings, Compat, Message, Data, Manage, Delete, GameMenu, GameXeniaSettings, GameFilepaths, GameInfo, GameCategories, GameXEX, Text, NewGame, DatabaseResult, ReleaseYear, ReleaseMonth, ReleaseDay, DatabasePicker, ManageFile, Metadata, DataSort, DataFilter
        }
        public State state;
        public enum Sort
        {
            AZ, ZA, Date, Dev, Pub
        }
        public Sort sort;
        public enum Theme
        {
            Original, Green, Blue, Orange, Gray, Purple, Custom
        }
        public Theme theme;
        public enum CWSettings
        {
            Off, Untested, All
        }
        public CWSettings cwSettings;
        public enum LogLevel
        {
            Error, Warning, Info, Debug
        }
        public LogLevel logLevel;
        public enum DataSort
        {
            NameAZ, NameZA, SizeHighLow, SizeLowHigh, FileCountHighLow, FileCountLowHigh
        }
        public DataSort dataSort;
        public Shared.DataFilter dataFilter;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1728;
            _graphics.PreferredBackBufferHeight = 972;
            Window.AllowUserResizing = false;
            firstLoad = true;
            configPath = "Content\\XLConfig.txt";
            Window.Title = "Continuum Launcher";
            ver = Shared.VERSION;
            compileDate = Shared.COMPILED;
        }

        protected override void Initialize()
        {
            Logging.Initialize(this);

            Ozzz.Initialize(new Vector2((float)GraphicsDevice.Viewport.Width / 1920, (float)GraphicsDevice.Viewport.Height / 1080), 60);
            GamepadInput.AddIndex(PlayerIndex.One);
            Logging.Write(LogType.Standard, Event.InitEvent, "OzzzFramework initialized");

            gameData = new List<GameData>();
            masterData = new List<GameData>();
            localData = new List<GameData>();
            folders = new List<string>();
            trivia = new List<string>();
            dataStrings = new DataManageStrings();
            dataFiles = new List<List<DataEntry>>();
            Logging.Write(LogType.Standard, Event.InitEvent, "Data lists initalized");

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
            if (File.Exists("Apps\\Xenia\\xenia.exe"))
            {
                xeniaPath = "Apps\\Xenia\\xenia.exe";
                Logging.Write(LogType.Critical, Event.XeniaPath, "Xenia path found", "xeniaPath", xeniaPath);
            }
            if (File.Exists("Apps\\Canary\\xenia_canary.exe"))
            {
                canaryPath = "Apps\\Canary\\xenia_canary.exe";
                Logging.Write(LogType.Critical, Event.XeniaPath, "Canary path found", "canaryPath", canaryPath);
            }
            if (File.Exists("Apps\\Dump\\xenia-vfs-dump.exe"))
            {
                extractPath = "Apps\\Dump\\xenia-vfs-dump.exe";
                Logging.Write(LogType.Critical, Event.XeniaPath, "Dump path found", "extractPath", extractPath);
            }
            else
            {
                extractPath = null;
            }

            // Experimental config file
            SaveDataObject exp = read.savedData.FindData("enableExp");
            if (exp != null)
            {
                enableExp = Convert.ToBoolean(exp.data);
                Logging.Write(LogType.Critical, Event.InitEvent, "Experimental config file loaded");
            }

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
                SaveConfig();
                triggerMissingWindow = true;
            }

            // Settings file
            ReadConfig();

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
            Logging.Write(LogType.Important, Event.InitEvent, "Folders loaded");

            gameData = gameData.OrderBy(o=>o.alphaAs).ThenBy(o=>o.gameTitle).ToList();
            arts = new Dictionary<string, Texture2D>();
            compatBars = new Dictionary<string, Texture2D>();
            themeThumbnails = new Dictionary<string, Texture2D>();
            icons = new Dictionary<string, Texture2D>();
            rings = new List<Ring>();
            ringFrames = 30;
            ringDuration = 240;
            compatWindowDelay = 30;
            fullscreenDelay = -1;
            jumpLayerAlpha = 0;
            jumpTriggerCooldownDefault = 30;
            jumpTriggerCooldown = jumpTriggerCooldownDefault;
            showRings = true;
            checkDrivesOnManage = true;
            lastActiveCheck = true;
            //newGameProcess = false;
            forceInit = false;
            hideSecretMetadata = true;
            showResearchPrompt = false;
            windowClickExit = true;
            enterCloseTextInput = false;
            rightClickGames = false;

            dataSort = DataSort.NameAZ;

            textWindowInput = null;
            gameManageWindow = null;
            gameCategoriesWindow = null;

            state = State.Main;

            base.Initialize();

            Logging.Write(LogType.Critical, Event.Init, "Initialization complete");
        }

        /// <summary>
        /// Saves the config file
        /// </summary>
        /// <returns>Whether or not the config file was successfully saved</returns>
        public bool SaveConfig()
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
        /// <summary>
        /// Reads in the config file and sets variables from it
        /// </summary>
        public void ReadConfig()
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
        /// <summary>
        /// Coverts a data size to a larger unit of storage measurement in needed, and adds a suffix to the end of the string
        /// </summary>
        public string ConvertDataSize(string size)
        {
            Logging.Write(LogType.Debug, Event.DataSizeConvert, "Data size conversion: " + size);
            double num = Convert.ToDouble(size);
            // Bytes
            if (num < 1000)
            {
                return "" + num + " B";
            }
            // Kilobytes
            else if (num < (1000 * 1000))
            {
                return "" + Math.Round((num / 1024), 3 - ("" + (int)(num / 1024)).Length) + " KB";
            }
            // Megabytes
            else if (num < (1000 * 1000 * 1000))
            {
                return "" + Math.Round((num / 1024 / 1024), 3 - ("" + (int)(num / 1024 / 1024)).Length) + " MB";
            }
            // Gigabytes
            else if (num < Math.Pow(1000, 4))
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024), Math.Max(0, 3 - ("" + (int)(num / 1024 / 1024 / 1024)).Length)) + " GB";
            }
            // Terabytes
            else if (num < Math.Pow(1000, 5))
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024 / 1024), Math.Max(0, 3 - ("" + (int)(num / 1024 / 1024 / 1024 / 1024)).Length)) + " TB";
            }
            return size;
        }

        /// <summary>
        /// Returns a Color based on the given string (Ex: "255 255 255 255")
        /// </summary>
        public Color ColorFromString(string str)
        {
            Logging.Write(LogType.Debug, Event.ColorFromString, "Color from string: " + str);
            Color toReturn = Color.FromNonPremultiplied(0, 0, 0, 0);
            string[] split = str.Split(" ");
            if (split.Length >= 4)
            {
                toReturn.R = Convert.ToByte(split[0]);
                toReturn.G = Convert.ToByte(split[1]);
                toReturn.B = Convert.ToByte(split[2]);
                toReturn.A = Convert.ToByte(split[3]);
            }
            return toReturn;
        }

        /// <summary>
        /// Resets the theme, used for changing to a new theme
        /// </summary>
        /// <param name="newTheme">The new theme to change to</param>
        /// <param name="forceWindowReset">If true, forces all open Window to reconstruct (This resets and applies the theme to the Window)</param>
        public void ResetTheme(Theme newTheme, bool forceWindowReset)
        {
            Logging.Write(LogType.Debug, Event.ThemeReset, "Theme reset: " + newTheme.ToString(), new Dictionary<string, string>() { { "forceWindowReset", "" + forceWindowReset } });
            StreamReader themeReader = null;
            if (enableExp)
            {
                themeReader = new StreamReader("Content\\XLTheme.txt");
            }
            fontColor = Color.White;
            fontSelectColor = Color.White;
            fontAltColor = Color.White;
            fontAltLightColor = Ozzz.Helper.DivideColor(Color.White, 2.0f);
            majorFontColor = Color.White;
            sortColor = Color.White;
            folderColor = Color.White;
            timeDateColor = Color.White;
            cornerStatsColor = Color.White;
            triviaColor = Color.White;
            topBorderColor = Color.White;
            bottomBorderColor = Color.White;
            descColor = Color.White;
            theme = newTheme;
            switch (theme)
            {
                case Theme.Original:
                    backColor = Color.FromNonPremultiplied(133, 133, 133, 255);
                    backColorAlt = Color.FromNonPremultiplied(70, 70, 70, 255);
                    break;
                case Theme.Green:
                    backColor = Color.FromNonPremultiplied(60, 110, 9, 255);
                    backColorAlt = Color.FromNonPremultiplied(40, 70, 6, 255);
                    break;
                case Theme.Orange:
                    backColor = Color.FromNonPremultiplied(190, 80, 20, 255);
                    backColorAlt = Color.FromNonPremultiplied(133, 56, 14, 255);
                    break;
                case Theme.Blue:
                    backColor = Color.FromNonPremultiplied(60, 60, 210, 255);
                    backColorAlt = Color.FromNonPremultiplied(110, 110, 110, 255);
                    break;
                case Theme.Gray:
                    backColor = Color.FromNonPremultiplied(133, 133, 133, 255);
                    backColorAlt = Color.FromNonPremultiplied(70, 70, 70, 255);
                    break;
                case Theme.Purple:
                    backColor = Color.FromNonPremultiplied(40, 30, 40, 255);
                    backColorAlt = Color.FromNonPremultiplied(45, 0, 45, 255);
                    break;
                case Theme.Custom:
                    backColor = ColorFromString(themeReader.ReadLine());
                    backColorAlt = ColorFromString(themeReader.ReadLine());
                    majorFontColor = ColorFromString(themeReader.ReadLine());
                    cornerStatsColor = ColorFromString(themeReader.ReadLine());
                    break;
            }

            mainFadeGradient = new Gradient(majorFontColor, 20);
            mainFadeGradient.colors.Add(Color.Gray);
            mainFadeGradient.ValueUpdate(0);
            darkGradient = new Gradient(GetTransparentColor(backColor), 20);
            blackGradient = new Gradient(GetTransparentColor(backColor), 20);
            buttonGradient = new Gradient(GetTransparentColor(backColor), 20);
            selectGradient = new Gradient(GetTransparentColor(backColor), 20);
            if (bottomInfo != null)
            {
                bottomInfo.displayGradient.colors[0] = GetTransparentColor(cornerStatsColor);
                bottomInfo.displayGradient.colors[1] = cornerStatsColor;
                bottomInfo.Reset();
            }

            if (theme == Theme.Original)
            {
                darkGradient.colors.Add(Color.FromNonPremultiplied(70, 70, 70, 255));
                blackGradient.colors.Add(Color.FromNonPremultiplied(30, 30, 30, 255));
                buttonGradient.colors.Add(Color.FromNonPremultiplied(10, 10, 10, 255));
                selectGradient.colors.Add(Color.FromNonPremultiplied(152, 211, 21, 255));
            }
            else if (theme == Theme.Green)
            {
                darkGradient.colors.Add(Color.FromNonPremultiplied(40, 70, 6, 255));
                blackGradient.colors.Add(Color.FromNonPremultiplied(110, 110, 110, 255));
                buttonGradient.colors.Add(Color.FromNonPremultiplied(50, 50, 50, 255));
                selectGradient.colors.Add(Color.FromNonPremultiplied(152, 211, 21, 255));
            }
            else if (theme == Theme.Orange)
            {
                darkGradient.colors.Add(Color.FromNonPremultiplied(133, 56, 14, 255));
                blackGradient.colors.Add(Color.FromNonPremultiplied(110, 110, 110, 255));
                buttonGradient.colors.Add(Color.FromNonPremultiplied(50, 50, 50, 255));
                selectGradient.colors.Add(Color.FromNonPremultiplied(152, 211, 21, 255));
            }
            else if (theme == Theme.Blue)
            {
                darkGradient.colors.Add(Color.FromNonPremultiplied(110, 110, 110, 255));
                blackGradient.colors.Add(Color.FromNonPremultiplied(70, 70, 70, 255));
                buttonGradient.colors.Add(Color.FromNonPremultiplied(30, 30, 30, 255));
                selectGradient.colors.Add(Color.FromNonPremultiplied(152, 211, 21, 255));
            }
            else if (theme == Theme.Gray)
            {
                darkGradient.colors.Add(Color.FromNonPremultiplied(70, 70, 70, 255));
                blackGradient.colors.Add(Color.FromNonPremultiplied(30, 30, 30, 255));
                buttonGradient.colors.Add(Color.FromNonPremultiplied(10, 10, 10, 255));
                selectGradient.colors.Add(Color.FromNonPremultiplied(140, 140, 140, 255));
            }
            else if (theme == Theme.Purple)
            {
                darkGradient.colors.Add(Color.FromNonPremultiplied(45, 0, 45, 255));
                blackGradient.colors.Add(Color.FromNonPremultiplied(50, 50, 50, 255));
                buttonGradient.colors.Add(Color.FromNonPremultiplied(40, 10, 40, 255));
                selectGradient.colors.Add(Color.FromNonPremultiplied(140, 0, 140, 255));
            }
            else if (theme == Theme.Custom)
            {
                darkGradient.colors.Add(ColorFromString(themeReader.ReadLine()));
                blackGradient.colors.Add(ColorFromString(themeReader.ReadLine()));
                buttonGradient.colors.Add(ColorFromString(themeReader.ReadLine()));
                selectGradient.colors.Add(ColorFromString(themeReader.ReadLine()));
                fontColor = ColorFromString(themeReader.ReadLine());
                fontSelectColor = ColorFromString(themeReader.ReadLine());
                fontAltColor = ColorFromString(themeReader.ReadLine());
                fontAltLightColor = ColorFromString(themeReader.ReadLine());
                sortColor = ColorFromString(themeReader.ReadLine());
                folderColor = ColorFromString(themeReader.ReadLine());
                timeDateColor = ColorFromString(themeReader.ReadLine());
                triviaColor = ColorFromString(themeReader.ReadLine());
                topBorderColor = ColorFromString(themeReader.ReadLine());
                bottomBorderColor = ColorFromString(themeReader.ReadLine());
                ringMainColor = ColorFromString(themeReader.ReadLine());
                ringSelectColor = ColorFromString(themeReader.ReadLine());
                descColor = ColorFromString(themeReader.ReadLine());
            }
            if (theme != Theme.Custom)
            {
                descColor = selectGradient.colors[1];
            }
            darkGradient.ValueUpdate(0);
            blackGradient.ValueUpdate(0);
            buttonGradient.ValueUpdate(0);
            selectGradient.ValueUpdate(0);
            whiteGradient = new Gradient(GetTransparentColor(backColor), 20);
            if (theme == Theme.Custom)
            {
                whiteGradient.colors.Add(ColorFromString(themeReader.ReadLine()));
            }
            else
            {
                whiteGradient.colors.Add(Color.FromNonPremultiplied(255, 255, 255, 255));
            }
            whiteGradient.ValueUpdate(0);

            if (enableExp)
            {
                themeReader.Close();
            }

            if (forceWindowReset)
            {
                menuWindow.ResetGradients();
                menuWindow.whiteGradient.Update();
                optionsWindow.ResetGradients();
                optionsWindow.whiteGradient.Update();
            }

            // Resetting Description Box colors
            if (optionsWindow != null)
            {
                foreach (DescriptionBox desc in optionsWindow.descriptionBoxes)
                {
                    desc.color = descColor;
                    desc.textSprite.color = fontSelectColor;
                }
                foreach (DescriptionBox desc in menuWindow.descriptionBoxes)
                {
                    desc.color = descColor;
                    desc.textSprite.color = fontSelectColor;
                }
            }
        }

        /// <summary>
        /// Returns a color with the alpha set to 0
        /// </summary>
        public Color GetTransparentColor(Color color)
        {
            return Color.FromNonPremultiplied(color.R, color.G, color.B, 0);
        }

        /// <summary>
        /// Internally used to reset the positions of bottom border Layers
        /// </summary>
        public void ResetBorders()
        {
            Logging.Write(LogType.Debug, Event.BorderReset, "Resetting Borders");
            topBorderLayer.sprites.Clear();
            topBorder = new ObjectSprite(topBorderTex, new Rectangle(141, -30, 1650, 91));
            topBorderLayer.Add(topBorder);
            topBorderLayer.Add(new ObjectSprite(white, new Rectangle(491, -1, 921, 51), Color.FromNonPremultiplied(255, 0, 0, 100)));
            topBorderLayer.Add(new ObjectSprite(white, new Rectangle(61, -100, 452, 51), Color.FromNonPremultiplied(255, 0, 0, 100)));
            topBorderLayer.sprites.Last().rotation = (float)Math.PI / 14.0f;
            topBorderLayer.Add(new ObjectSprite(white, new Rectangle(1401, 0, 452, 51), Color.FromNonPremultiplied(255, 0, 0, 100)));
            topBorderLayer.sprites.Last().rotation = (float)Math.PI / -15.1f;
            backBorderLayer.sprites.Clear();
            backBorderLayer.Add(new ObjectSprite(white, new Rectangle(0, 855, 155, 400), Color.White));
            backBorderLayer.Add(new ObjectSprite(white, new Rectangle(155, 855, 355, 400), Color.White));
            backBorderLayer.sprites[1].rotation = (float)Math.PI / 4.45f;
            backBorderLayer.Add(new ObjectSprite(white, new Rectangle(1765, 855, 155, 400), Color.White));
            backBorderLayer.Add(new ObjectSprite(white, new Rectangle(1765, 855, 355, 400), Color.White));
            backBorderLayer.sprites[3].rotation = (float)Math.PI / 3.55f;
            backBorderLayer.Add(new ObjectSprite(white, new Rectangle(0, 1020, 1920, 200), Color.White));
            triviaMaskingLayer.sprites.Clear();
            triviaMaskingLayer.Add(new ObjectSprite(white, new Rectangle(0, 1020, 355, 60), backColorAlt));
            triviaMaskingLayer.Add(new ObjectSprite(white, new Rectangle(1570, 1020, 355, 60), backColorAlt));
        }

        /// <summary>
        /// Internally used to adjust displayed games, resetting positions and artwork after the AnimationPaths have finished playing
        /// </summary>
        public void ResetGameIcons()
        {
            Logging.Write(LogType.Debug, Event.GameIconReset, "Resetting game icons");
            List<int> indexes = new List<int>();
            foreach (XGame game in gameIcons)
            {
                indexes.Add(game.index);
            }
            for (int i = 0; i < indexes.Count; i++)
            {
                gameIcons[i].index = indexes[i];
                if (!newGameProcess)
                {
                    if (!firstLoad && right && !firstReset)
                    {
                        gameIcons[i].index++;
                    }
                    else if (!firstLoad && !right && !firstReset)
                    {
                        gameIcons[i].index--;
                    }
                }
                gameIcons[i].AdjustIndex(gameData.Count);
            }
            newGameProcess = false;
            indexes = new List<int>();
            foreach (XGame game in gameIcons)
            {
                indexes.Add(game.index);
            }

            gameIcons = new List<XGame>();
            gameIcons.Add(new XGame(arts[gameData[indexes[0]].gameTitle], new Rectangle(50, 306, 300, 406)));
            gameIcons.Add(new XGame(arts[gameData[indexes[1]].gameTitle], new Rectangle(380, 273, 350, 473)));
            gameIcons.Add(new XGame(arts[gameData[indexes[2]].gameTitle], new Rectangle(760, 239, 400, 541)));
            gameIcons.Add(new XGame(arts[gameData[indexes[3]].gameTitle], new Rectangle(1190, 273, 350, 473)));
            gameIcons.Add(new XGame(arts[gameData[indexes[4]].gameTitle], new Rectangle(1570, 306, 300, 406)));
            for (int i = 0; i < indexes.Count; i++)
            {
                gameIcons[i].index = indexes[i];
            }

            gameIcons[2].playerIndex = PlayerIndex.One;
            gameIcons[2].downButtons.Add(Buttons.A);
            gameIcons[2].gpButtonState = Button.GPButtonState.AnyFirst;

            gameIcons[0].rightPath = new AnimationPath(gameIcons[0].button.sprite, new Vector2(-2000, 306), 1f, 15);
            gameIcons[1].rightPath = new AnimationPath(gameIcons[1].button.sprite, new Vector2(26, 273), 0.85824f, 15);
            gameIcons[2].rightPath = new AnimationPath(gameIcons[2].button.sprite, new Vector2(356, 239), 0.875f, 15);
            gameIcons[3].rightPath = new AnimationPath(gameIcons[3].button.sprite, new Vector2(785, 277), 1.142857f, 15);
            gameIcons[4].rightPath = new AnimationPath(gameIcons[4].button.sprite, new Vector2(1215, 307), 1.166667f, 15);
            gameIcons[0].leftPath = new AnimationPath(gameIcons[0].button.sprite, new Vector2(406, 306), 1.166667f, 15);
            gameIcons[1].leftPath = new AnimationPath(gameIcons[1].button.sprite, new Vector2(785, 273), 1.142857f, 15);
            gameIcons[2].leftPath = new AnimationPath(gameIcons[2].button.sprite, new Vector2(1166, 240), 0.875f, 15);
            gameIcons[3].leftPath = new AnimationPath(gameIcons[3].button.sprite, new Vector2(1546, 277), 0.85824f, 15);
            gameIcons[4].leftPath = new AnimationPath(gameIcons[4].button.sprite, new Vector2(3620, 307), 1f, 15);
            foreach (XGame game in gameIcons)
            {
                game.rightPath.frames = 0;
                game.rightPath.paused = true;
                game.leftPath.frames = 0;
                game.leftPath.paused = true;
                //game.textures = arts.Values.ToList();
                
                try
                {
                    //game.button.sprite.ToObjectSprite().textures[0] = arts[gameData[game.index].gameTitle];
                    //game.button.sprite.ToObjectSprite().texIndex = game.index;
                }
                catch
                {
                    game.button.sprite.ToObjectSprite().textures[0] = white;
                }
            }
            if (bottomBorder != null)
            {
                bottomBorder.pos = new Vector2(-5, 850);
                timeText.pos.Y = 920;
                dateText.pos.Y = 980;
                contNumText.pos.Y = 910;
                controllerText.pos.Y = 980;
                freeSpaceText.pos.Y = 930;
                drivesText.pos.Y = 980;
                triviaSprite.pos.Y = 1025;
                bottomBorderPath = new AnimationPath(bottomLayer, new Vector2(-5, 900), 1f, 20);
                bottomBorderPath.frames = 20;
                bottomLayer.Update(false);
                topBorder.pos.Y = -30;
                topBorder.UpdatePos();
                backBorderLayer.Update(false);
                ResetBorders();
            }
        }
        /// <summary>
        /// Sets up the Launcher to use a new folder
        /// </summary>
        public void FolderReset()
        {
            Logging.Write(LogType.Debug, Event.FolderReset, "Resetting Folders", new Dictionary<string, string>() { { "folderCount", "" + folders.Count } });
            if (folders.Count >= 2)
            {
                gameData.Clear();
                index = 0;
                foreach (GameData data in masterData)
                {
                    if (data.folders.Contains(folders[folderIndex]))
                    {
                        gameData.Add(data);
                    }
                }
                gameIcons[0].index = -2;
                gameIcons[1].index = -1;
                gameIcons[2].index = 0;
                gameIcons[3].index = 1;
                gameIcons[4].index = 2;
                firstReset = true;
                ResetGameIcons();
                foreach (XGame game in gameIcons)
                {
                    game.AdjustIndex(gameData.Count);
                }
            }
        }
        /// <summary>
        /// Updates all of the Gradients used for transitioning between game selection and other Windows
        /// </summary>
        public void UpdateGradients()
        {
            mainFadeGradient.Update();
            darkGradient.Update();
            blackGradient.Update();
            selectGradient.Update();
            whiteGradient.Update();
            buttonGradient.Update();
            if (mainTransitionPath != null)
            {
                mainTransitionPath.Update();
                bottomBorderPath.Update();
                topBorderPath.Update();
            }
        }
        public void BeginMainTransition()
        {
            BeginMainTransition(false);
        }
        /// <summary>
        /// Begins the Main Transition from game selection to game launching, and vice versa
        /// </summary>
        /// <param name="skipTransition"></param>
        public void BeginMainTransition(bool skipTransition)
        {
            Logging.Write(LogType.Debug, Event.MainTransition, "Beginning main transition");
            mainFadeGradient.ValueUpdate(1);
            mainFadeGradient.Update();
            darkGradient.ValueUpdate(1);
            darkGradient.Update();
            blackGradient.ValueUpdate(1);
            blackGradient.Update();
            selectGradient.ValueUpdate(1);
            selectGradient.Update();
            whiteGradient.ValueUpdate(1);
            whiteGradient.Update();
            buttonGradient.ValueUpdate(1);
            buttonGradient.Update();
            if (!skipTransition && mainTransitionPath != null)
            {
                mainTransitionPath.ReverseAnimation();
                mainTransitionPath.frames = 20 - bottomBorderPath.frames;
                bottomBorderPath.ReverseAnimation();
                bottomBorderPath.frames = 20 - bottomBorderPath.frames;
                topBorderPath.ReverseAnimation();
                topBorderPath.frames = 20 - topBorderPath.frames;
            }
            foreach (Ring ring in rings)
            {
                int frames = ring.fade.frameCycle;
                ring.fade = new Gradient(ring.fade.GetColor(), frames);
                ring.fade.colors.Add(backColor);
                ring.fade.ValueUpdate(0);
            }
        }
        /// <summary>
        /// Used to calculate total drives and size
        /// </summary>
        public void SetDriveSpaceText()
        {
            Logging.Write(LogType.Debug, Event.DriveSpaceTextSet, "Setting drive space text");
            float tempY = 0;
            DriveInfo[] drives = DriveInfo.GetDrives();
            double space = 0;
            double freeSpace = 0;
            int driveCount = 0;
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    space += drive.TotalSize;
                    freeSpace += drive.TotalFreeSpace;
                    driveCount++;
                }
                catch (Exception e)
                {
                    Logging.Write(LogType.Important, Event.Error, "Failed to set drive space text", "exception", e.ToString());
                }
            }
            freeSpaceText.text = "" + ConvertDataSize("" + freeSpace) + "/" + ConvertDataSize("" + space);
            drivesText.text = "" + driveCount + " Drives Connected";
            tempY = freeSpaceText.pos.Y;
            freeSpaceText.Centerize(new Vector2(1790, 920));
            freeSpaceText.pos.Y = tempY;
            freeSpaceText.UpdatePos();
            drivesText.Centerize(new Vector2(freeSpaceText.GetCenterPoint().X, drivesText.GetCenterPoint().Y));
            drivesText.UpdatePos();
            updateFreeSpace = false;
        }
        /// <summary>
        /// Sets compatibility textures for the game launching menu
        /// </summary>
        public void SetCompatTextures()
        {
            Logging.Write(LogType.Debug, Event.CompatTextureSet, "Setting compatibility settings");
            if (gameData[index].xeniaCompat == GameData.XeniaCompat.Unknown || gameData[index].xeniaCompat == GameData.XeniaCompat.Broken)
            {
                xeniaCompat.textures[0] = compatBars["nothing"];
            }
            else if (gameData[index].xeniaCompat == GameData.XeniaCompat.Starts)
            {
                xeniaCompat.textures[0] = compatBars["intro"];
            }
            else if (gameData[index].xeniaCompat == GameData.XeniaCompat.Menu)
            {
                xeniaCompat.textures[0] = compatBars["menu"];
            }
            else if (gameData[index].xeniaCompat == GameData.XeniaCompat.Gameplay1)
            {
                xeniaCompat.textures[0] = compatBars["reach"];
            }
            else if (gameData[index].xeniaCompat == GameData.XeniaCompat.Gameplay2)
            {
                xeniaCompat.textures[0] = compatBars["plays"];
            }
            else if (gameData[index].xeniaCompat == GameData.XeniaCompat.Gameplay3)
            {
                xeniaCompat.textures[0] = compatBars["most"];
            }
            else if (gameData[index].xeniaCompat == GameData.XeniaCompat.Playable)
            {
                xeniaCompat.textures[0] = compatBars["perfect"];
            }

            if (gameData[index].canaryCompat == GameData.XeniaCompat.Unknown || gameData[index].canaryCompat == GameData.XeniaCompat.Broken)
            {
                canaryCompat.textures[0] = compatBars["nothing"];
            }
            else if (gameData[index].canaryCompat == GameData.XeniaCompat.Starts)
            {
                canaryCompat.textures[0] = compatBars["intro"];
            }
            else if (gameData[index].canaryCompat == GameData.XeniaCompat.Menu)
            {
                canaryCompat.textures[0] = compatBars["menu"];
            }
            else if (gameData[index].canaryCompat == GameData.XeniaCompat.Gameplay1)
            {
                canaryCompat.textures[0] = compatBars["reach"];
            }
            else if (gameData[index].canaryCompat == GameData.XeniaCompat.Gameplay2)
            {
                canaryCompat.textures[0] = compatBars["plays"];
            }
            else if (gameData[index].canaryCompat == GameData.XeniaCompat.Gameplay3)
            {
                canaryCompat.textures[0] = compatBars["most"];
            }
            else if (gameData[index].canaryCompat == GameData.XeniaCompat.Playable)
            {
                canaryCompat.textures[0] = compatBars["perfect"];
            }
        }

        public void OpenDatabaseResult(State returnState)
        {
            state = State.DatabaseResult;
            databaseResultWindow = new Window(this, new Rectangle(360, 100, 1200, 880), "Database Result (" + databaseGameInfo[0].Variants[0].TitleID + ")", "Database Name: " + databaseGameInfo[0].Title, new DatabaseResult(), new DatabaseResultInput(), new GenericStart(), returnState, true);
            databaseResultWindow.AddButton(new Rectangle(410, 300, 90, 90));
            databaseResultWindow.AddButton(new Rectangle(1420, 300, 90, 90));
            databaseResultWindow.AddButton(new Rectangle(410, 440, 90, 90));
            databaseResultWindow.AddButton(new Rectangle(1420, 440, 90, 90));
            databaseResultWindow.AddButton(new Rectangle(495, 610, 450, 100));
            databaseResultWindow.AddButton(new Rectangle(980, 610, 450, 100));
            databaseResultWindow.AddButton(new Rectangle(610, 730, 700, 100));
            databaseResultWindow.AddButton(new Rectangle(610, 850, 700, 100));
            databaseResultWindow.AddText("<");
            databaseResultWindow.AddText(">");
            databaseResultWindow.AddText("<");
            databaseResultWindow.AddText(">");
            databaseResultWindow.AddText("Edit Game Title");
            databaseResultWindow.AddText("Edit Release Date");
            databaseResultWindow.AddText("Accept Entry and Save");
            databaseResultWindow.AddText("Discard Changes");
            
            databaseResultWindow.buttonEffects.SetupEffects(this, databaseResultWindow);
        }
        public void OpenDatabasePicker()
        {
            state = State.DatabasePicker;
            databaseResultIndex = 0;
            databasePickerWindow = new Window(this, new Rectangle(210, 250, 1500, 580), "Database Picker (" + databaseGameInfo[0].Variants[0].TitleID + ")", "Multiple variants match this Title ID. Choose one to continue.", new DatabasePicker(), new DatabasePickerInput(), new GenericStart(), State.GameMenu, true);
            databasePickerWindow.AddButton(new Rectangle(260, 450, 90, 90));
            databasePickerWindow.AddButton(new Rectangle(1560, 450, 90, 90));
            databasePickerWindow.AddButton(new Rectangle(495, 680, 450, 100));
            databasePickerWindow.AddButton(new Rectangle(980, 680, 450, 100));
            databasePickerWindow.AddText("<");
            databasePickerWindow.AddText(">");
            databasePickerWindow.AddText("Select Entry");
            databasePickerWindow.AddText("Back to Menu");

            databasePickerWindow.buttonEffects.SetupEffects(this, databasePickerWindow);
        }
        public string GetFilepathString(string path)
        {
            return GetFilepathString(path, false);
        }
        public string GetFilepathString(string path, bool removeBackslash)
        {
            Logging.Write(LogType.Debug, Event.FilepathStringCleanse, "Cleaning filepath string", new Dictionary<string, string>()
            {
                { "path", path },
                { "removeBackslash", removeBackslash.ToString() }
            });
            char[] invalidNameChars = Path.GetInvalidFileNameChars();
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (removeBackslash)
            {
                List<char> charList = invalidNameChars.ToList();
                charList.Remove('\\');
                invalidNameChars = charList.ToArray();
            }
            string regexSearch = new string(new string(invalidNameChars) + new string(invalidPathChars));
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string newTitle = r.Replace(path, "");
            return newTitle;
        }
        /// <summary>
        /// Returns the parameter string to attach to the command that launches Xenia
        /// </summary>
        /// <returns></returns>
        public string GetParamString()
        {
            string param = "";
            param = param + " --draw_resolution_scale_x=" + gameData[index].resX;
            param = param + " --draw_resolution_scale_y=" + gameData[index].resY;
            param = param + " --vsync=" + gameData[index].vsync.ToString().ToLower();
            param = param + " --d3d12_readback_resolve=" + gameData[index].cpuReadback.ToString().ToLower();
            param = param + " --mount_cache=" + gameData[index].mountCache.ToString().ToLower();
            if (gameData[index].renderer == GameData.Renderer.Any)
            {
                param = param + " --gpu=any";
            }
            else if (gameData[index].renderer == GameData.Renderer.Direct3D12)
            {
                param = param + " --gpu=d3d12";
            }
            else if (gameData[index].renderer == GameData.Renderer.Vulkan)
            {
                param = param + " --gpu=vulkan";
            }
            if (gameData[index].license == GameData.LicenseMask.None)
            {
                param = param + " --license_mask=0";
            }
            else if (gameData[index].license == GameData.LicenseMask.First)
            {
                param = param + " --license_mask=1";
            }
            else if (gameData[index].license == GameData.LicenseMask.All)
            {
                param = param + " --license_mask=-1";
            }
            param = param + " --log_level=" + (int)logLevel;
            param = param + " --fullscreen=" + xeniaFullscreen.ToString().ToLower();
            param = param + " --headless=" + runHeadless.ToString().ToLower();
            Logging.Write(LogType.Critical, Event.XeniaParam, "Xenia param string: " + param);
            return param;
        }
        /// <summary>
        /// Marks the currently selected games as having been played
        /// </summary>
        public void MarkGameAsPlayed()
        {
            gameData[index].timesLaunched++;
            gameData[index].lastPlayed = DateTime.Now.ToBinary();
            SaveGames();
        }
        public void LaunchXenia(string path)
        {
            string param = GetParamString();
            launchSound.Play();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "\"" + path + "\"" + param;
            string title = gameData[index].gameTitle;
            string newTitle = GetFilepathString(title);
            startInfo.WorkingDirectory = "XData\\Xenia\\" + newTitle;
            Directory.CreateDirectory("XData\\Xenia\\" + newTitle);
            if (consolidateFiles)
            {
                try
                {
                    if (!gameData[index].preferCanary)
                    {
                        File.Copy(xeniaPath, "XData\\Xenia\\" + newTitle + "\\xenia.exe");
                        Logging.Write(LogType.Important, Event.Launch, "Copied Xenia executable", "dir", startInfo.WorkingDirectory);
                    }
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Falied to copy Xenia executable", "dir", startInfo.WorkingDirectory);
                }
                try
                {
                    File.Create("XData\\Xenia\\" + newTitle + "\\portable.txt");
                    Logging.Write(LogType.Standard, Event.Launch, "Created portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Failed to create portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                startInfo.FileName = "XData\\Xenia\\" + newTitle + "\\xenia.exe";
            }
            else
            {
                startInfo.FileName = xeniaPath;
            }
            try
            {
                Process.Start(startInfo);
                OpenCompatWindow(false, compatWindowDelay);
                MarkGameAsPlayed();
                Logging.Write(LogType.Critical, Event.Launch, "Launched Xenia", new Dictionary<string, string>()
                {
                    { "gameTitle", title },
                    { "newTitle", newTitle },
                    { "titleID", gameData[index].titleId }
                });
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Failed to launch Xenia", "dir", startInfo.WorkingDirectory);
                message = new MessageWindow(this, "Launch Error", "Unable to launch Xenia", state);
                state = State.Message;
            }
        }
        public void LaunchXenia()
        {
            LaunchXenia(gameData[index].gamePath);
        }
        public void LaunchCanary(string path)
        {
            string param = GetParamString();
            launchSound.Play();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "\"" + path + "\"" + param;
            string title = gameData[index].gameTitle;
            string newTitle = GetFilepathString(title);
            startInfo.WorkingDirectory = "XData\\Canary\\" + newTitle;
            Directory.CreateDirectory("XData\\Canary\\" + newTitle);
            if (consolidateFiles)
            {
                try
                {
                    if (!gameData[index].preferCanary)
                    {
                        File.Copy(canaryPath, "XData\\Canary\\" + newTitle + "\\xenia_canary.exe");
                        Logging.Write(LogType.Important, Event.Launch, "Copied Canary executable", "dir", startInfo.WorkingDirectory);
                    }
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Falied to copy Canary executable", "dir", startInfo.WorkingDirectory);
                }
                try
                {
                    File.Create("XData\\Canary\\" + newTitle + "\\portable.txt");
                    Logging.Write(LogType.Standard, Event.Launch, "Created portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Failed to create portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                startInfo.FileName = "XData\\Canary\\" + newTitle + "\\xenia_canary.exe";
            }
            else
            {
                startInfo.FileName = canaryPath;
            }
            try
            {
                Process.Start(startInfo);
                OpenCompatWindow(true, compatWindowDelay);
                MarkGameAsPlayed();
                Logging.Write(LogType.Critical, Event.Launch, "Launched Canary", new Dictionary<string, string>()
                {
                    { "gameTitle", title },
                    { "newTitle", newTitle },
                    { "titleID", gameData[index].titleId }
                });
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Failed to launch Canary", "dir", startInfo.WorkingDirectory);
                message = new MessageWindow(this, "Launch Error", "Unable to launch Canary", state);
                state = State.Message;
            }
        }
        public void LaunchCanary()
        {
            LaunchCanary(gameData[index].gamePath);
        }
        public void EditGame()
        {
            gameManageWindow = new Window(this, new Rectangle(560, 115, 800, 860), "Manage " + gameData[index].gameTitle, new ManageGame(), new StdInputEvent(6), new GenericStart(), State.NewGame);
            gameManageWindow.buttonEffects.SetupEffects(this, gameManageWindow);
            state = State.GameMenu;
        }
        public void OpenDateEditWindow(State initialState, State returnState)
        {
            if (initialState == State.ReleaseYear)
            {
                releaseWindow = new Window(this, new Rectangle(460, 140, 1000, 800), "Edit Release Date", "Select a Year", new EditYearEffects(), new EditYearInput(), new GenericStart(), returnState, true);
                releaseWindow.changeEffects = new EditYearChangeEffects(releaseWindow);
                // 2005-2008
                releaseWindow.AddButton(new Rectangle(515, 320, 200, 90));
                releaseWindow.AddButton(new Rectangle(745, 320, 200, 90));
                releaseWindow.AddButton(new Rectangle(975, 320, 200, 90));
                releaseWindow.AddButton(new Rectangle(1205, 320, 200, 90));
                // 2008-2012
                releaseWindow.AddButton(new Rectangle(515, 420, 200, 90));
                releaseWindow.AddButton(new Rectangle(745, 420, 200, 90));
                releaseWindow.AddButton(new Rectangle(975, 420, 200, 90));
                releaseWindow.AddButton(new Rectangle(1205, 420, 200, 90));
                // 2013-2016
                releaseWindow.AddButton(new Rectangle(515, 520, 200, 90));
                releaseWindow.AddButton(new Rectangle(745, 520, 200, 90));
                releaseWindow.AddButton(new Rectangle(975, 520, 200, 90));
                releaseWindow.AddButton(new Rectangle(1205, 520, 200, 90));
                // 2017-2018
                releaseWindow.AddButton(new Rectangle(745, 620, 200, 90));
                releaseWindow.AddButton(new Rectangle(975, 620, 200, 90));
                // Exit buttons
                releaseWindow.AddButton(new Rectangle(635, 810, 650, 100));
                for (int i = 2005; i <= 2018; i++)
                {
                    releaseWindow.AddText("" + i);
                }
                releaseWindow.AddText("Cancel");
                releaseWindow.buttonEffects.SetupEffects(this, releaseWindow);
                state = State.ReleaseYear;
            }
            else if (initialState == State.ReleaseMonth)
            {
                releaseWindow = new Window(this, new Rectangle(460, 140, 1000, 800), "Edit Release Date", "Select a Month", new EditMonthEffects(), new EditMonthInput(), new GenericStart(), returnState, true);
                releaseWindow.changeEffects = new EditMonthChangeEffects(releaseWindow);
                // Jan-Apr
                releaseWindow.AddButton(new Rectangle(510, 320, 200, 90));
                releaseWindow.AddButton(new Rectangle(740, 320, 200, 90));
                releaseWindow.AddButton(new Rectangle(970, 320, 200, 90));
                releaseWindow.AddButton(new Rectangle(1200, 320, 200, 90));
                // May-Aug
                releaseWindow.AddButton(new Rectangle(510, 420, 200, 90));
                releaseWindow.AddButton(new Rectangle(740, 420, 200, 90));
                releaseWindow.AddButton(new Rectangle(970, 420, 200, 90));
                releaseWindow.AddButton(new Rectangle(1200, 420, 200, 90));
                // Sep-Dec
                releaseWindow.AddButton(new Rectangle(510, 520, 200, 90));
                releaseWindow.AddButton(new Rectangle(740, 520, 200, 90));
                releaseWindow.AddButton(new Rectangle(970, 520, 200, 90));
                releaseWindow.AddButton(new Rectangle(1200, 520, 200, 90));
                // Exit buttons
                releaseWindow.AddButton(new Rectangle(635, 810, 650, 100));
                for (int i = 1; i <= 12; i++)
                {
                    releaseWindow.AddText("" + i);
                }
                releaseWindow.AddText("Back to Year");
                releaseWindow.buttonEffects.SetupEffects(this, releaseWindow);
                state = State.ReleaseMonth;
            }
            else if (initialState == State.ReleaseDay)
            {
                releaseWindow = new Window(this, new Rectangle(460, 140, 1000, 800), "Edit Release Date", "Select a Day", new EditDayEffects(), new EditDayInput(), new GenericStart(), returnState, true);
                releaseWindow.changeEffects = new EditDayChangeEffects(releaseWindow);
                // 01-08
                releaseWindow.AddButton(new Rectangle(560, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(660, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(760, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(860, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(960, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(1060, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(1160, 320, 90, 90));
                releaseWindow.AddButton(new Rectangle(1260, 320, 90, 90));
                // 09-16
                releaseWindow.AddButton(new Rectangle(560, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(660, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(760, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(860, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(960, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(1060, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(1160, 420, 90, 90));
                releaseWindow.AddButton(new Rectangle(1260, 420, 90, 90));
                // 17-24
                releaseWindow.AddButton(new Rectangle(560, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(660, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(760, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(860, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(960, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(1060, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(1160, 520, 90, 90));
                releaseWindow.AddButton(new Rectangle(1260, 520, 90, 90));
                // 25-31
                releaseWindow.AddButton(new Rectangle(610, 620, 90, 90));
                releaseWindow.AddButton(new Rectangle(710, 620, 90, 90));
                releaseWindow.AddButton(new Rectangle(810, 620, 90, 90));
                releaseWindow.AddButton(new Rectangle(910, 620, 90, 90));
                releaseWindow.AddButton(new Rectangle(1010, 620, 90, 90));
                releaseWindow.AddButton(new Rectangle(1110, 620, 90, 90));
                releaseWindow.AddButton(new Rectangle(1210, 620, 90, 90));
                // Exit button
                releaseWindow.AddButton(new Rectangle(635, 810, 650, 100));
                for (int i = 1; i <= 31; i++)
                {
                    releaseWindow.AddText("" + i);
                }
                releaseWindow.AddText("Back to Month");
                releaseWindow.buttonEffects.SetupEffects(this, releaseWindow);
                state = State.ReleaseDay;
            }
            else
            {
                Logging.Write(LogType.Critical, Event.HowInTheEverlastingHeckDidYouEvenGetThisErrorMessage, "Just... how?");
                message = new MessageWindow(this, "Something went wrong", "Well, it looks like Littleozzz10 ****ed up. It's a really dumb thing, too, so please report with much laughter :(", state);
                state = State.Message;
            }
        }

        /// <summary>
        /// Opens the compatibility window after Xenia has been launched
        /// </summary>
        /// <param name="canary">Whether or not Canary was launched instead of Xenia (Master)</param>
        /// <param name="delay">The number of frames to wait before opening the window (Used to ensure that the window doesn't pop back up before Xenia has been launched)</param>
        public void OpenCompatWindow(bool canary, int delay)
        {
            OpenCompatWindow(canary, delay, false);
        }
        public void OpenCompatWindow(bool canary, int delay, bool forceOpen)
        {
            if (cwSettings == CWSettings.All || (cwSettings == CWSettings.Untested && ((canary && gameData[index].canaryCompat == GameData.XeniaCompat.Unknown) || !canary && gameData[index].xeniaCompat == GameData.XeniaCompat.Unknown)) || forceOpen)
            {
                compatWindow = new Window(this, new Rectangle(360, 40, 1200, 1000), "Update Xenia Compatibility", new CompatWindowEffects(), new CompatInput(), new GenericStart(), state, false);
                compatWindow.changeEffects = new CompatIndexChange(this, compatWindow);
                compatWindow.AddButton(new Rectangle(410, 530, 545, 100), "The game does not launch at all,\nor Xenia hangs on a black screen.", DBSpawnPos.AboveRight, 0.4f);
                compatWindow.AddButton(new Rectangle(965, 530, 545, 100), "The game launches and reaches opening logos,\nbut does not go beyond\na title screen.", DBSpawnPos.AboveLeft, 0.4f);
                compatWindow.AddButton(new Rectangle(410, 640, 545, 100), "The game reaches menus and it is possible\nto navigate through them.", DBSpawnPos.AboveRight, 0.4f);
                compatWindow.AddButton(new Rectangle(965, 640, 545, 100), "The game gets to some form of gameplay,\nbut isn't very playable. May also\nfrequently crash.", DBSpawnPos.AboveLeft, 0.4f);
                compatWindow.AddButton(new Rectangle(410, 750, 545, 100), "The game may suffer from frequent framerate hitches,\nbad audio, missing textures, but can be played.", DBSpawnPos.AboveRight, 0.4f);
                compatWindow.AddButton(new Rectangle(965, 750, 545, 100), "The game is quite playable, with few issues. Crashes\nmay happen but are very rare. The game should be\nmostly completable.", DBSpawnPos.AboveLeft, 0.4f);
                compatWindow.AddButton(new Rectangle(410, 860, 545, 100), "The game has no issues at all, and can be completed.\nGameplay experience should match that of an Xbox 360,\nif not be superior.", DBSpawnPos.AboveRight, 0.4f);
                compatWindow.AddButton(new Rectangle(965, 860, 545, 100), "Unknown compatibililty, game remains untested on this system.", DBSpawnPos.AboveLeft, 0.4f);
                compatWindow.AddText("Doesn't Launch");
                compatWindow.AddText("Starts, No Menu");
                compatWindow.AddText("Gets to Menu");
                compatWindow.AddText("Reaches Gameplay");
                compatWindow.AddText("Somewhat Playable");
                compatWindow.AddText("Mostly Playable");
                compatWindow.AddText("Perfect/Near Perfect");
                compatWindow.AddText("Untested/Unknown");

                // Extra Sprites for icons
                compatWindow.extraSprites.Add(new ObjectSprite(logo, new Rectangle(-1000, 0, 200, 200)));
                compatWindow.extraSprites.Last().Centerize(new Vector2(710, 260));
                compatWindow.extraSprites.Add(new ObjectSprite(logo, new Rectangle(-1000, 0, 200, 200)));
                compatWindow.extraSprites.Last().Centerize(new Vector2(1210, 260));
                compatWindow.extraSprites.Add(new ObjectSprite(xeniaCompat.textures[0], new Rectangle(-1000, 0, 250, 30)));
                compatWindow.extraSprites.Last().Centerize(new Vector2(710, 380));
                compatWindow.extraSprites.Add(new ObjectSprite(xeniaCompat.textures[0], new Rectangle(-1000, 0, 250, 30)));
                compatWindow.extraSprites.Last().Centerize(new Vector2(1210, 380));
                compatWindow.extraSprites.Add(new TextSprite(font, "Untested", 0.64f, new Vector2(-1000, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                compatWindow.extraSprites.Last().Centerize(new Vector2(710, 380));
                compatWindow.extraSprites.Add(new TextSprite(font, "Untested", 0.64f, new Vector2(-1000, 0), Color.FromNonPremultiplied(0, 0, 0, 0)));
                compatWindow.extraSprites.Last().Centerize(new Vector2(1210, 380));
                if (gameData[index].xeniaCompat == GameData.XeniaCompat.Unknown)
                {
                    compatWindow.extraSprites[2].visible = false;
                    compatWindow.extraSprites[3].visible = false;
                }
                else
                {
                    compatWindow.extraSprites[4].visible = false;
                    compatWindow.extraSprites[5].visible = false;
                }

                if (canary)
                {
                    compatWindow.titleSprite.text = "Update Canary Compatibility";
                    compatWindow.extraSprites[0].ToObjectSprite().textures[0] = logoCanary;
                    compatWindow.extraSprites[1].ToObjectSprite().textures[0] = logoCanary;
                    if (gameData[index].canaryCompat == GameData.XeniaCompat.Unknown)
                    {
                        compatWindow.extraSprites[2].visible = false;
                        compatWindow.extraSprites[3].visible = false;
                        compatWindow.extraSprites[4].visible = true;
                        compatWindow.extraSprites[5].visible = true;
                    }
                    else
                    {
                        compatWindow.extraSprites[2].visible = true;
                        compatWindow.extraSprites[3].visible = true;
                        compatWindow.extraSprites[4].visible = false;
                        compatWindow.extraSprites[5].visible = false;
                        compatWindow.extraSprites[2].ToObjectSprite().textures[0] = canaryCompat.textures[0];
                        compatWindow.extraSprites[3].ToObjectSprite().textures[0] = canaryCompat.textures[0];
                    }
                    compatWindow.changeEffects = new CompatIndexChange(this, compatWindow, gameData[index].canaryCompat);
                }
                state = State.Compat;
                compatWaitFrames = delay;
            }
        }
        private void JumpToIndexHandler(int gameIndex, string character)
        {
            index = gameIndex;
            int offset = 1;
            if (right)
            {
                offset = -1;
            }

            for (int i = 0; i < gameIcons.Count; i++)
            {
                gameIcons[i].index = index + (i - 2 + offset);
            }
            ResetGameIcons();
            switchSound.Play();
            jumpLayerAlpha = 480;
            jumpToText.text = character;
            jumpToText.Centerize(new Vector2(960, 140));
            jumpIndexText.text = "" + (gameIndex + 1) + " of " + gameData.Count;
            jumpIndexText.Centerize(new Vector2(960, 820));
            Logging.Write(LogType.Standard, Event.XGameChange, "Index jump", "newIndex", "" + index);
        }
        public void JumpTo(string character, bool indexChange)
        {
            if (indexChange && state == State.Main)
            {
                switch (sort)
                {
                    case Sort.Date:
                        int year = 2005;
                        switch (character)
                        {
                            case "0":
                                year = 2010;
                                break;
                            case "1":
                                year = 2011;
                                break;
                            case "2":
                                year = 2012;
                                break;
                            case "3":
                                year = 2013;
                                break;
                            case "4":
                                year = 2014;
                                break;
                            case "5":
                                year = 2005;
                                break;
                            case "6":
                                year = 2006;
                                break;
                            case "7":
                                year = 2007;
                                break;
                            case "8":
                                year = 2008;
                                break;
                            case "9":
                                year = 2009;
                                break;
                        }
                        if (gameData[index].year == year && year < 2010)
                        {
                            year += 10;
                        }
                        else if (gameData[index].year == year && year >= 2015 && year <= 2018)
                        {
                            year -= 10;
                        }
                        for (int i = 0; i < gameData.Count; i++)
                        {
                            if (gameData[i].year == year)
                            {
                                JumpToIndexHandler(i, "" + year);
                                break;
                            }
                        }
                        break;
                    case Sort.Dev:
                        for (int i = 0; i < gameData.Count; i++)
                        {
                            if (gameData[i].developer.Substring(0, character.Length).ToLower() == character.ToLower())
                            {
                                JumpToIndexHandler(i, character);
                                break;
                            }
                        }
                        break;
                    case Sort.Pub:
                        for (int i = 0; i < gameData.Count; i++)
                        {
                            if (gameData[i].publisher.Substring(0, character.Length).ToLower() == character.ToLower())
                            {
                                JumpToIndexHandler(i, character);
                                break;
                            }
                        }
                        break;
                    default:
                        for (int i = 0; i < gameData.Count; i++)
                        {
                            if (gameData[i].alphaAs.Substring(0, character.Length).ToLower() == character.ToLower())
                            {
                                JumpToIndexHandler(i, character);
                                break;
                            }
                        }
                        break;
                }
            }
        }

        public void SaveGames()
        {
            try
            {
                SaveData save = new SaveData(configPath);
                save.AddSaveObject(new SaveDataObject("xenia", xeniaPath, SaveData.DataType.String));
                save.AddSaveObject(new SaveDataObject("canary", canaryPath, SaveData.DataType.String));
                save.AddSaveObject(new SaveDataObject("enableExp", "" + enableExp, SaveData.DataType.Boolean));
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

        public void LoadArts()
        {
            int missingArts = 0;
            int missingIcons = 0;
            int failedArts = 0;
            int failedIcons = 0;
            arts = new Dictionary<string, Texture2D>();
            icons = new Dictionary<string, Texture2D>();
            foreach (GameData data in gameData)
            {
                // New cover checking
                if (data.artPath == "NULL")
                {
                    if (Directory.Exists(data.gamePath + "\\..\\..\\_covers") && File.Exists(data.gamePath + "\\..\\..\\_covers\\cover0.jpg"))
                    {
                        data.artPath = data.gamePath + "\\..\\..\\_covers\\cover0.jpg";
                        Logging.Write(LogType.Important, Event.NewCoverDetect, "cover0 found", new Dictionary<string, string>()
                        {
                            { "gameTitle", data.gameTitle },
                            { "artPath", data.artPath }
                        });
                    }
                }

                if (File.Exists(data.artPath))
                {
                    try
                    {
                        arts.Add(data.gameTitle, Texture2D.FromFile(GraphicsDevice, data.artPath));
                        Logging.Write(LogType.Important, Event.CoverLoaded, "Cover art loaded", new Dictionary<string, string>()
                        {
                            { "gameTitle", data.gameTitle },
                            { "artPath", data.artPath }
                        });
                    }
                    catch
                    {
                        arts.Add(data.gameTitle, white);
                        Logging.Write(LogType.Critical, Event.Error, "Artwork failed to load", new Dictionary<string, string>()
                        {
                            { "gameTitle", data.gameTitle },
                            { "artPath", data.artPath }
                        });
                        failedArts++;
                    }
                }
                else
                {
                    arts.Add(data.gameTitle, white);
                    Logging.Write(LogType.Important, Event.Error, "Art path does not exist", new Dictionary<string, string>()
                    {
                        { "gameTitle", data.gameTitle },
                        { "artPath", data.artPath }
                    });
                    missingArts++;
                }
                if (File.Exists(data.iconPath))
                {
                    try
                    {
                        icons.Add(data.gameTitle, Texture2D.FromFile(GraphicsDevice, data.iconPath));
                        Logging.Write(LogType.Important, Event.IconLoaded, "Icon loaded", new Dictionary<string, string>()
                        {
                            { "gameTitle", data.gameTitle },
                            { "iconPath", data.iconPath }
                        });
                    }
                    catch
                    {
                        icons.Add(data.gameTitle, white);
                        Logging.Write(LogType.Critical, Event.Error, "Icon failed to load", new Dictionary<string, string>()
                        {
                            { "gameTitle", data.gameTitle },
                            { "iconPath", data.iconPath }
                        });
                        failedIcons++;
                    }
                }
                else
                {
                    icons.Add(data.gameTitle, white);
                    Logging.Write(LogType.Important, Event.Error, "Icon path does not exist", new Dictionary<string, string>()
                    {
                        { "gameTitle", data.gameTitle },
                        { "iconPath", data.iconPath }
                    });
                    missingIcons++;
                }
            }
            icons.Add("Continuum Launcher", mainLogo);
            icons.Add("Continuum Companion", compLogo);
            icons.Add("Xenia Content and Temporary Data", mainLogo);
            icons.Add("Artwork and Icons", mainLogo);
            icons.Add("All Games and Data", mainLogo);
            icons.Add("All Launcher Data", mainLogo);
            Logging.Write(LogType.Critical, Event.ArtLoadComplete, "Artwork loading complete", new Dictionary<string, string>()
            {
                { "missingArts", "" + missingArts },
                { "missingIcons", "" + missingIcons },
                { "failedArts", "" + failedArts },
                { "failedIcons", "" + failedIcons }
            });
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
                    if (arts.ContainsKey(gameData[index].gameTitle))
                    {
                        arts.Add(tempGameTitle, arts[gameData[index].gameTitle]);
                        arts.Remove(gameData[index].gameTitle);
                    }
                    else
                    {
                        arts.Add(tempGameTitle, white);
                    }
                    if (icons.ContainsKey(gameData[index].gameTitle))
                    {
                        icons.Add(tempGameTitle, icons[gameData[index].gameTitle]);
                        icons.Remove(gameData[index].gameTitle);
                    }
                    else
                    {
                        icons.Add(tempGameTitle, white);
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
        /// Returns true if the current State indicates the Launcher is in the Select menu
        /// </summary>
        public bool CheckStateSelectMenu()
        {
            return (state == State.Select || state == State.Launch || state == State.Compat || state == State.GameMenu || state == State.DatabasePicker || state == State.DatabaseResult || state == State.GameInfo || state == State.ReleaseYear || state == State.ReleaseMonth || state == State.ReleaseDay || state == State.GameFilepaths || state == State.GameXeniaSettings || state == State.GameCategories || state == State.GameXEX);
        }

        public void SetResolution(Vector2 resolution)
        {
            _graphics.PreferredBackBufferWidth = (int)resolution.X;
            _graphics.PreferredBackBufferHeight = (int)resolution.Y;
            _graphics.ApplyChanges();
            Ozzz.scale = resolution / new Vector2(1920, 1080);
            Logging.Write(LogType.Important, Event.ResolutionSet, "Set resolution", new Dictionary<string, string>()
            {
                { "resolution", "(" + resolution.X + ", " + resolution.Y + ")" },
                { "scale", "(" + Ozzz.scale.X + ", " + Ozzz.scale.Y + ")" }
            });
        }
        public Vector2 GetResolution()
        {
            return new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }
        public bool ToggleFullscreen()
        {
            if (_graphics.IsFullScreen)
            {
                _graphics.IsFullScreen = false;
            }
            else
            {
                _graphics.IsFullScreen = true;
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }
            _graphics.ApplyChanges();
            Ozzz.scale = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) / new Vector2(1920, 1080);
            Logging.Write(LogType.Important, Event.ResolutionSet, "Fullscreen toggle", new Dictionary<string, string>()
            {
                { "isFullScreen", _graphics.IsFullScreen.ToString() },
                { "resolution", "(" + _graphics.PreferredBackBufferWidth + ", " + _graphics.PreferredBackBufferHeight + ")" },
                { "scale", "(" + Ozzz.scale.X + ", " + Ozzz.scale.Y + ")" }
            });
            return _graphics.IsFullScreen;
        }
        public bool GetFullscreen()
        {
            return _graphics.IsFullScreen;
        }
        public bool ToggleVSync()
        {
            _graphics.SynchronizeWithVerticalRetrace = !_graphics.SynchronizeWithVerticalRetrace;
            Logging.Write(LogType.Important, Event.VSyncToggle, "VSync toggle");
            return _graphics.SynchronizeWithVerticalRetrace;
        }
        public bool GetVSync()
        {
            return _graphics.SynchronizeWithVerticalRetrace;
        }

        /// <summary>
        /// Adjusts the cover of the selected game, if multiple covers are available in the game's directory
        /// </summary>
        public void AdjustCover()
        {
            // Only enabled if experimental settings are active
            if (enableExp)
            {
                Logging.Write(LogType.Standard, Event.CoverAdjust, "Adjusting cover");
                GameData data = gameData[index];
                if (File.Exists(data.artPath))
                {
                    string defaultCoverPath = data.gamePath + "\\..\\..\\_covers";
                    string[] split = data.artPath.Split("\\");
                    if (Directory.Exists(defaultCoverPath))
                    {
                        string[] covers = Directory.GetFiles(defaultCoverPath);
                        int coverIndex = -1;
                        foreach (string cover in covers)
                        {
                            if (data.artPath == cover)
                            {
                                coverIndex = Convert.ToInt32(split.Last().Substring(5, 1));
                            }
                        }
                        // Setting new cover, if possible
                        if (coverIndex + 1 >= covers.Length)
                        {
                            coverIndex = 0;
                        }
                        else
                        {
                            coverIndex++;
                        }
                        // Making new cover path
                        string newCoverPath = defaultCoverPath + "\\cover" + coverIndex + ".jpg";
                        if (File.Exists(newCoverPath) && newCoverPath != data.artPath)
                        {
                            data.artPath = newCoverPath;
                            LoadArts();
                            newGameProcess = true;
                            ResetGameIcons();
                            sortSound.Play();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads in the MobyGames data from local JSON files
        /// </summary>
        public void LoadMobyData()
        {
            // Initializing global data
            mobyData = new MobyData();
            // Loading data
            for (int i = 2005; i <= 2018; i++)
            {
                try
                {
                    StreamReader dataReader = new StreamReader("Content\\Database\\games" + i + ".json");
                    string data = dataReader.ReadToEnd();
                    MobyData yearData = JsonConvert.DeserializeObject<MobyData>(data);
                    // Fixing release dates
                    foreach (GameInfo info in yearData.Data)
                    {
                        info.Incorrect_Date = false;
                        // Fixing missing data
                        if (info.Release_Date.Length == 4)
                        {
                            info.Release_Date = info.Release_Date + "-01-01";
                            info.Incorrect_Date = true;
                            Logging.Write(LogType.Debug, Event.MobyDataReleaseFix, "Data only had a release year", "game", info.Title);
                        }
                        else if (info.Release_Date.Length == 7)
                        {
                            info.Release_Date = info.Release_Date + "-01";
                            info.Incorrect_Date = true;
                            Logging.Write(LogType.Debug, Event.MobyDataReleaseFix, "Data only had a release year/month", "game", info.Title);
                        }
                        // Fixing incorrect dates
                        if (info.Release_Date.Substring(0, 4) != i.ToString())
                        {
                            info.Release_Date = "" + i + "-01-01";
                            info.Incorrect_Date = true;
                            Logging.Write(LogType.Debug, Event.MobyDataReleaseFix, "Data has original release year, not 360 year", "game", info.Title);
                        }
                    }
                    // Adding data to global MobyData
                    if (mobyData.Data == null)
                    {
                        mobyData.Data = yearData.Data;
                    }
                    else
                    {
                        mobyData.Data = (GameInfo[])mobyData.Data.Concat(yearData.Data).ToArray();
                    }
                    // Logging
                    Logging.Write(LogType.Standard, Event.MobyDataLoad, "games" + i + ".json loaded");
                }
                catch (FileNotFoundException e)
                {
                    Logging.Write(LogType.Critical, Event.Error, "Failed to load games" + i + ".json", "exception", e.ToString());
                }
            }
            // Adjusting release dates
            foreach (GameInfo info in mobyData.Data)
            {
                DateTime date = new DateTime(Convert.ToInt32(info.Release_Date.Substring(0, 4)), Convert.ToInt32(info.Release_Date.Substring(5, 2)), Convert.ToInt32(info.Release_Date.Substring(8, 2)));
                // Fixing releases before the Xbox 360's actual launch day
                if (date.Year == 2005 && ((date.Month == 11 && date.Day < 22) || date.Month <= 10))
                {
                    info.Release_Date = "2005-11-22";
                }
                Logging.Write(LogType.Debug, Event.MobyDataReleaseFix, "Data has release date before 360 launched", "game", info.Title);
            }
            Logging.Write(LogType.Important, Event.MobyDataLoadComplete, "Database load complete");
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

        /// <summary>
        /// Loads all of the Launcher's content (Textures, Fonts, data, etc)
        /// </summary>
        protected override void LoadContent()
        {
            // Creating Content directories if needed
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

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            white = Content.Load<Texture2D>("Textures/white");
            rectTex = Content.Load<Texture2D>("Textures/roundrect");
            logo = Content.Load<Texture2D>("Textures/xenia");
            logoCanary = Content.Load<Texture2D>("Textures/canary");
            circ = Content.Load<Texture2D>("Textures/circ outline");
            calendar = Content.Load<Texture2D>("Textures/calendar");
            player = Content.Load<Texture2D>("Textures/user");
            mainLogo = Content.Load<Texture2D>("Textures/continuum_small");
            topBorderTex = Content.Load<Texture2D>("Textures/xl_border_top");
            bottomBorderTex = Content.Load<Texture2D>("Textures/xl_border_bottom");
            compLogo = Content.Load<Texture2D>("Textures/continuum comp");
            bottomBorderNew = Content.Load<Texture2D>("Textures/xl_border_bottom_new");

            font = Content.Load<SpriteFont>("Fonts/Font");
            bold = Content.Load<SpriteFont>("Fonts/Bold");

            selectSound = Content.Load<SoundEffect>("Audio/btn_Select");
            backSound = Content.Load<SoundEffect>("Audio/btn_Back");
            launchSound = Content.Load<SoundEffect>("Audio/dl_complete");
            switchSound = Content.Load<SoundEffect>("Audio/btn_InactiveSelect");
            buttonSwitchSound = Content.Load<SoundEffect>("Audio/btn_backG");
            sortSound = Content.Load<SoundEffect>("Audio/tab_Switch");
            leftFolderSound = Content.Load<SoundEffect>("Audio/snd_panelunfold");
            rightFolderSound = Content.Load<SoundEffect>("Audio/snd_panelfold");

            compatBars.Add("nothing", Content.Load<Texture2D>("Textures/compat-nothing"));
            compatBars.Add("intro", Content.Load<Texture2D>("Textures/compat-intro"));
            compatBars.Add("menu", Content.Load<Texture2D>("Textures/compat-menu"));
            compatBars.Add("reach", Content.Load<Texture2D>("Textures/compat-reach"));
            compatBars.Add("plays", Content.Load<Texture2D>("Textures/compat-plays"));
            compatBars.Add("most", Content.Load<Texture2D>("Textures/compat-most"));
            compatBars.Add("perfect", Content.Load<Texture2D>("Textures/compat-perfect"));

            themeThumbnails.Add("original", Content.Load<Texture2D>("Textures/Themes/theme-original"));
            themeThumbnails.Add("green", Content.Load<Texture2D>("Textures/Themes/theme-green"));
            themeThumbnails.Add("orange", Content.Load<Texture2D>("Textures/Themes/theme-orange"));
            themeThumbnails.Add("blue", Content.Load<Texture2D>("Textures/Themes/theme-blue"));
            themeThumbnails.Add("gray", Content.Load<Texture2D>("Textures/Themes/theme-gray"));
            themeThumbnails.Add("purple", Content.Load<Texture2D>("Textures/Themes/theme-purple"));
            if (enableExp)
            {
                themeThumbnails.Add("custom", white);
            }
            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Internal assets loaded");

            LoadArts();

            // Loading trivia
            if (File.Exists("Content\\Trivia.txt"))
            {
                StreamReader triviaFile = new StreamReader("Content\\Trivia.txt");
                while (!triviaFile.EndOfStream)
                {
                    trivia.Add(triviaFile.ReadLine());
                    Logging.Write(LogType.Debug, Event.TriviaLoad, "Loaded new trivia", "triviaString", trivia.Last());
                }
            }
            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Trivia loaded");

            gameIcons = new List<XGame>();
            gameIcons.Add(new XGame(white, new Rectangle(50, 336, 300, 406)));
            gameIcons.Add(new XGame(white, new Rectangle(380, 303, 350, 473)));
            gameIcons.Add(new XGame(white, new Rectangle(760, 269, 400, 541)));
            gameIcons.Add(new XGame(white, new Rectangle(1190, 303, 350, 473)));
            gameIcons.Add(new XGame(white, new Rectangle(1570, 336, 300, 406)));
            gameIcons[0].index = -2;
            gameIcons[1].index = -1;
            gameIcons[2].index = 0;
            gameIcons[3].index = 1;
            gameIcons[4].index = 2;

            ResetGameIcons();

            foreach (XGame game in gameIcons)
            {
                game.AdjustIndex(gameData.Count);
            }
            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Game icons initialized");

            xeniaCompatLogo = new ObjectSprite(logo, new Rectangle(220, 400, 125, 125));
            canaryCompatLogo = new ObjectSprite(logoCanary, new Rectangle(530, 400, 125, 125));
            xeniaCompat = new ObjectSprite(compatBars["nothing"], new Rectangle(204, 535, 156, 19));
            canaryCompat = new ObjectSprite(compatBars["nothing"], new Rectangle(514, 535, 156, 19));
            xeniaUntestedText = new TextSprite(font, "Untested", 0.4f, new Vector2(222, 525), Color.FromNonPremultiplied(0, 0, 0, 0));
            canaryUntestedText = new TextSprite(font, "Untested", 0.4f, new Vector2(532, 525), Color.FromNonPremultiplied(0, 0, 0, 0));
            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Compat window elements initialized");

            titleSprite = new TextSprite(bold, "");
            subTitleSprite = new TextSprite(font, "", 0.6f);
            sortSprite = new TextSprite(font, "", 0.8f);
            sortSprite.pos.Y = 70;
            folderSprite = new TextSprite(font, "", 1f);
            folderPath = new AnimationPath(folderSprite, Vector2.Zero, 1f, 30);
            folderPath.frames = 0;

            timeText = new TextSprite(bold, "", 0.55f, new Vector2(0, 0), Color.White);
            timeText.Centerize(new Vector2(110, 920));
            dateText = new TextSprite(font, "", 0.3f, new Vector2(120, 980), Color.White);
            contNumText = new TextSprite(bold, "0", 0.7f, new Vector2(1770, 910), Color.White);
            controllerText = new TextSprite(font, "Controllers Connected", 0.3f, new Vector2(1665, 980), Color.White);
            freeSpaceText = new TextSprite(bold, "", 0.4f, new Vector2(0, 930), Color.White);
            freeSpaceText.visible = false;
            drivesText = new TextSprite(font, "0 Drives Connected", 0.3f, new Vector2(1660, 980), Color.White);
            drivesText.visible = false;
            SetDriveSpaceText();
            bottomBorder = new ObjectSprite(bottomBorderTex, new Rectangle(-5, 850, 1930, 171), Color.FromNonPremultiplied(255, 255, 255, 255));
            Layer infoLayer = new Layer(white, new Rectangle());
            infoLayer.Add(contNumText);
            infoLayer.Add(controllerText);
            Layer driveLayer = new Layer(white, new Rectangle());
            driveLayer.Add(freeSpaceText);
            driveLayer.Add(drivesText);
            bottomInfo = new SequenceFade(infoLayer, GetTransparentColor(cornerStatsColor), cornerStatsColor, 300, 150);
            bottomInfo.currentIndex = 0;
            bottomInfo.AddLayer(driveLayer);
            triviaSprite = new TextSprite(font, "", 0.5f, new Vector2(600, 1025), Color.White);
            triviaSprite.velocity = new Vector2(-3, 0);
            triviaSprite.skipLayerDraw = true;

            backBorderLayer = new Layer(white, new Rectangle());
            topBorderLayer = new Layer(white, new Rectangle());

            bottomLayer = new Layer(white, new Rectangle(-5, 850, 1930, 230));
            bottomLayer.Add(backBorderLayer);
            backBorderLayer.skipLayerDraw = true;

            triviaMaskingLayer = new Layer(white, new Rectangle());
            ResetBorders();
            bottomLayer.Add(triviaMaskingLayer);

            bottomLayer.Add(bottomBorder);
            bottomLayer.Add(timeText);
            bottomLayer.Add(dateText);
            bottomLayer.Add(contNumText);
            bottomLayer.Add(controllerText);
            bottomLayer.Add(freeSpaceText);
            bottomLayer.Add(drivesText);
            bottomLayer.Add(triviaSprite);
            bottomBorderPath = new AnimationPath(bottomLayer, new Vector2(-5, 900), 1f, 20);

            topBorderPath = new AnimationPath(topBorderLayer, new Vector2(135, -91), 1f, 20);

            mainFadeLayer = new Layer(white, new Rectangle());
            mainFadeLayer.Add(titleSprite);
            mainFadeLayer.Add(subTitleSprite);

            jumpFade = new ObjectSprite(white, new Rectangle(0, 0, 1920, 1080), Color.FromNonPremultiplied(0, 0, 0, 0));
            jumpToText = new TextSprite(bold, "", 2.0f, Vector2.Zero, Color.FromNonPremultiplied(255, 255, 255, 0));
            jumpIndexText = new TextSprite(font, "", 0.4f, Vector2.Zero, Color.FromNonPremultiplied(255, 255, 255, 0));
            jumpLayer = new Layer(white, new Rectangle(), Color.FromNonPremultiplied(0, 0, 0, 0));
            jumpLayer.Add(jumpFade);
            jumpLayer.Add(jumpToText);
            jumpLayer.Add(jumpIndexText);

            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Dashboard elements initialized");

            LoadMobyData();

            //ReadConfig();

            Logging.Write(LogType.Critical, Event.ContentLoad, "Content Load complete");
        }

        /// <summary>
        /// This Update() method is called once every frame
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (forceInit && state != State.GameMenu && state != State.GameXeniaSettings && state != State.GameInfo && state != State.GameFilepaths && state != State.GameCategories && state != State.GameXEX && state != State.Message && state != State.Text)
            {
                FolderReset();
                folderIndex = 0;
                Initialize();
                LoadArts();
            }

            // User Research prompt
            if (showResearchPrompt)
            {
                message = new MessageWindow(this, "User Research Note", "This is a User Research build of Continuum Launcher. As such, features may be in an early state and may not represent the final version.", State.Main);
                state = State.Message;
                showResearchPrompt = false;
                Logging.Write(LogType.Standard, Event.ResearchPrompt, "User Research prompt shown");
            }

            GamepadInput.Update();
            MouseInput.Update();
            KeyboardInput.Update();
            if (GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Back, true) || GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.B, true) || KeyboardInput.keys["Escape"].IsFirstDown() || MouseInput.IsRightFirstDown() || KeyboardInput.keys["Backspace"].IsFirstDown())
            {
                if (state == State.Main && IsActive)
                {
                    state = State.Menu;
                    menuWindow = new Window(this, new Rectangle(560, 115, 800, 860), "Menu", new Menu(), new StdInputEvent(6), new GenericStart(), State.Main);
                    menuWindow.AddButton(new Rectangle(610, 265, 700, 100), "Go back to the game selection menu.", DBSpawnPos.CenterLeftBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 375, 700, 100), "Add a game to Continuum.", DBSpawnPos.CenterRightBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 485, 700, 100), "Change preferences, Continuum settings,\nglobal Xenia settings, and more.", DBSpawnPos.CenterLeftBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 595, 700, 100), "Manage imported game data,\ninstall add-on content,\ndelete temporary data, and more.", DBSpawnPos.CenterRightTop, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 705, 700, 100), "View Continuum's Credits.", DBSpawnPos.CenterLeftTop, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 815, 700, 100), "Close Continuum.", DBSpawnPos.CenterRightTop, 0.4f);
                    menuWindow.AddText("Return to Dashboard");
                    menuWindow.AddText("Add a Game");
                    menuWindow.AddText("Launcher Options");
                    menuWindow.AddText("Manage Data");
                    menuWindow.AddText("About/Credits");
                    menuWindow.AddText("Exit Launcher");
                    foreach (TextSprite sprite in menuWindow.sprites)
                    {
                        sprite.scale = 0.6f;
                    }
                    menuWindow.skipMainStateTransition = true;
                }
            }

            if (fullscreenDelay > 0)
            {
                fullscreenDelay--;
                if (fullscreenDelay == 0)
                {
                    _graphics.IsFullScreen = true;
                    _graphics.ApplyChanges();
                }
            }

            // Missing/Corrupt Settings file notification
            if (triggerMissingWindow)
            {
                message = new MessageWindow(this, "Note", "Settings file missing or corrupted; Default created", State.Main);
                state = State.Message;
                triggerMissingWindow = false;
            }

            // Jump trigger cooldown
            if (jumpTriggerCooldown > 0)
            {
                jumpTriggerCooldown--;
            }
            if (GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.RightTrigger) <= 0.65f && GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftTrigger) <= 0.65f)
            {
                jumpTriggerCooldown = 0;
            }

            // Moving game icons
            bool indexChange = true;
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadRight, true) || GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) > 0.3 || KeyboardInput.keys["Right"].IsDown() || gameIcons[3].CheckMouse(true) || gameIcons[4].CheckMouse(true)) && state == State.Main && !firstReset && IsActive)
            {
                foreach (XGame game in gameIcons)
                {
                    if (game.leftPath.frames <= 0 && game.rightPath.frames <= 0)
                    {
                        game.rightPath.frames = 15;
                        game.rightPath.paused = false;
                        right = true;
                        if (indexChange)
                        {
                            index++;
                            if (index >= gameData.Count)
                            {
                                index = 0;
                            }
                            indexChange = false;
                            switchSound.Play();
                            Logging.Write(LogType.Standard, Event.XGameChange, "Index change +1", "newIndex", "" + index);
                        }
                    }
                }
            }
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadLeft, true) || GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) < -0.3 || KeyboardInput.keys["Left"].IsDown() || gameIcons[0].CheckMouse(true) || gameIcons[1].CheckMouse(true)) && state == State.Main && !firstReset && IsActive)
            {
                foreach (XGame game in gameIcons)
                {
                    if (game.leftPath.frames <= 0 && game.rightPath.frames <= 0)
                    {
                        game.leftPath.frames = 15;
                        game.leftPath.paused = false;
                        right = false;
                        if (indexChange)
                        {
                            index--;
                            if (index < 0)
                            {
                                index = gameData.Count - 1;
                            }
                            indexChange = false;
                            switchSound.Play();
                            Logging.Write(LogType.Standard, Event.XGameChange, "Index change -1", "newIndex", "" + index);
                        }
                    }
                }
            }
            else if (jumpTriggerCooldown == 0 && GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.RightTrigger) >= 0.65f)
            {
                int newIndex = index + 1;
                string currChar = gameData[index].alphaAs.Substring(0, 1).ToUpper();
                string newChar = currChar;
                while (newChar == currChar && newIndex != index)
                {
                    if (newIndex >= gameData.Count)
                    {
                        newIndex = 0;
                    }
                    else
                    {
                        newChar = gameData[newIndex].alphaAs.Substring(0, 1).ToUpper();
                        newIndex++;
                    }
                }
                if (newChar != currChar)
                {
                    JumpTo(newChar, indexChange);
                    indexChange = false;
                    jumpTriggerCooldown = jumpTriggerCooldownDefault;
                }
            }
            else if (jumpTriggerCooldown == 0 && GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftTrigger) >= 0.65f)
            {
                int newIndex = index - 1;
                string currChar = gameData[index].alphaAs.Substring(0, 1).ToUpper();
                string newChar = currChar;
                while (newChar == currChar && newIndex != index)
                {
                    if (newIndex <= 0)
                    {
                        newIndex = gameData.Count - 1;
                    }
                    else
                    {
                        newChar = gameData[newIndex].alphaAs.Substring(0, 1).ToUpper();
                        newIndex--;
                    }
                }
                if (newChar != currChar)
                {
                    JumpTo(newChar, indexChange);
                    indexChange = false;
                    jumpTriggerCooldown = jumpTriggerCooldownDefault;
                }
            }
            foreach (string key in TextInputWindow.keys)
            {
                if (KeyboardInput.keys[key].IsFirstDown())
                {
                    JumpTo(key, indexChange);
                    indexChange = false;
                }
            }
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.A, true) || KeyboardInput.keys["Enter"].IsFirstDown() || KeyboardInput.keys["Space"].IsFirstDown() || (gameIcons[2].CheckMouse(true) && MouseInput.IsLeftFirstDown())) && state == State.Main && IsActive)
            {
                Logging.Write(LogType.Standard, Event.GameSelected, "Game selected", "index", "" + index);
                state = State.Select;
                mainFadeGradient.ValueUpdate(0);
                mainFadeGradient.Update();
                darkGradient.ValueUpdate(0);
                darkGradient.Update();
                blackGradient.ValueUpdate(0);
                blackGradient.Update();
                selectGradient.ValueUpdate(0);
                selectGradient.Update();
                whiteGradient.ValueUpdate(0);
                whiteGradient.Update();
                buttonGradient.ValueUpdate(0);
                buttonGradient.Update();
                mainTransitionPath = new AnimationPath(gameIcons[2].button.sprite, new Vector2(1000, 260), 1.2f, 20);
                mainTransitionPath.frames = 20;
                bottomBorderPath = new AnimationPath(bottomLayer, new Vector2(-5, 900), 1f, 20);
                bottomBorderPath.frames = 20;
                topBorderPath = new AnimationPath(topBorderLayer, new Vector2(topBorderPath.sprite.pos.X, -145), 1f, 20);
                topBorderPath.frames = 20;
                foreach (Ring ring in rings)
                {
                    int frames = ring.fade.frameCycle;
                    ring.fade = new Gradient(ring.fade.GetColor(), frames);
                    ring.fade.colors.Add(darkGradient.colors[1]);
                    ring.fade.ValueUpdate(0);
                }

                launchWindow = new Window(this, new Rectangle(165, 180, 550, 700), "", new GameLaunch(), new SelectInput(), new GenericStart(), State.Main);
                launchWindow.AddButton(new Rectangle(195, 570, 490, 80));
                launchWindow.AddButton(new Rectangle(195, 660, 490, 80));
                launchWindow.AddButton(new Rectangle(195, 750, 490, 80));
                launchWindow.AddButton(xeniaCompatLogo.rect);
                launchWindow.AddButton(canaryCompatLogo.rect);
                launchWindow.AddText("Launch With Xenia");
                launchWindow.AddText("Launch With Canary");
                launchWindow.AddText("Manage Game");
                launchWindow.useFade = false;

                SetCompatTextures();
                selectSound.Play();

                bottomInfo.ForceSwitch(0);
                bottomInfo.Reset();

                if (gameData[index].kinect == GameData.KinectCompat.Required)
                {
                    Logging.Write(LogType.Standard, Event.KinectWarning, "Kinect game selected");
                    message = new MessageWindow(this, "Note", "Xenia currently does not support the Kinect peripheral. This game may not work as intended.", State.Select);
                    state = State.Message;
                }
            }
            else if (xexWindow != null && state == State.Launch)
            {
                xexWindow.Update();
            }
            else if (launchWindow != null && state == State.Select)
            {
                launchWindow.Update();
            }

            // hahaha
            if (state != State.Message && Keyboard.GetState().IsKeyDown(Keys.C) && Keyboard.GetState().IsKeyDown(Keys.H) && Keyboard.GetState().IsKeyDown(Keys.U) && Keyboard.GetState().IsKeyDown(Keys.K))
            {
                message = new MessageWindow(this, "Et tu, Brute?", "Dedicated to Chuck", state);
                state = State.Message;
            }

            // Sorting
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Y, true) || KeyboardInput.keys["RShift"].IsFirstDown() || (sortSprite.CheckMouse(true) && MouseInput.IsLeftFirstDown())) && state == State.Main && IsActive)
            {
                if (sort == Sort.AZ)
                {
                    sort = Sort.ZA;
                    gameData = gameData.OrderByDescending(o => o.alphaAs).ThenByDescending(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.ZA)
                {
                    sort = Sort.Date;
                    gameData = gameData.OrderBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Date)
                {
                    sort = Sort.Dev;
                    gameData = gameData.OrderBy(o => o.developer).ThenBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Dev)
                {
                    sort = Sort.Pub;
                    gameData = gameData.OrderBy(o => o.publisher).ThenBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Pub)
                {
                    sort = Sort.AZ;
                    gameData = gameData.OrderBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                firstReset = true;
                ResetGameIcons();
                sortSound.Play();
                Logging.Write(LogType.Standard, Event.DashSort, "Games have been sorted", "newSort", sort.ToString());
            }
            // Switching folders
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder, true) || KeyboardInput.keys["RCtrl"].IsFirstDown() || (folderSprite.CheckMouse(true) && MouseInput.IsLeftFirstDown())) && state == State.Main && IsActive)
            {
                // Folder indexes
                if (folders.Count == 2)
                {
                    if (folderIndex == 0)
                    {
                        folderIndex = 1;
                    }
                    else
                    {
                        folderIndex = 0;
                    }
                }
                else if (folders.Count >= 3)
                {
                    folderIndex++;
                    if (folderIndex >= folders.Count)
                    {
                        folderIndex = 0;
                    }
                }
                FolderReset();
                rightFolderSound.Play();
                folderPath = new AnimationPath(folderSprite, new Vector2(200 - folderSprite.font.MeasureString(folderSprite.text).X, 60), 1f, 15);
                secondFolderPath = new AnimationPath(folderSprite, new Vector2(10000, 10000), 1f, 15);
                Logging.Write(LogType.Standard, Event.DashFolderSwitch, "Folder index change +1", "newFolderIndex", "" + folderIndex);
            }
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.LeftShoulder, true) || KeyboardInput.keys["LCtrl"].IsFirstDown()) && state == State.Main && IsActive)
            {
                // Folder indexes
                if (folders.Count == 2)
                {
                    if (folderIndex == 0)
                    {
                        folderIndex = 1;
                    }
                    else
                    {
                        folderIndex = 0;
                    }
                }
                else if (folders.Count >= 3)
                {
                    folderIndex--;
                    if (folderIndex < 0)
                    {
                        folderIndex = folders.Count - 1;
                    }
                }
                FolderReset();
                leftFolderSound.Play();
                folderPath = new AnimationPath(folderSprite, new Vector2(1860, 60), 1f, 15);
                secondFolderPath = new AnimationPath(folderSprite, new Vector2(-1, -1), 1f, 15);
                Logging.Write(LogType.Standard, Event.DashFolderSwitch, "Folder index change -1", "newFolderIndex", "" + folderIndex);
            }
            // Switching covers
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.X, true) || KeyboardInput.keys["Tab"].IsFirstDown()) && state == State.Main && IsActive)
            {
                AdjustCover();
            }

            // Updating
            foreach (XGame game in gameIcons)
            {
                game.rightPath.Update();
                if (game.button.sprite.pos.X <= -1016)
                {
                    game.button.sprite.pos.X = 2526;
                }
                game.leftPath.Update();
                if (game.button.sprite.pos.X >= 2536)
                {
                    game.button.sprite.pos.X = -906;
                }
                game.Update();
            }
            if (gameIcons[0].rightPath.frames == 0 && !gameIcons[0].rightPath.paused)
            {
                ResetGameIcons();
            }
            else if (gameIcons[0].leftPath.frames == 0 && !gameIcons[0].leftPath.paused)
            {
                ResetGameIcons();
            }

            // File deletion check
            if (messageYes && toDelete != null)
            {
                string fileTitle = GetFilepathString(localData[selectedDataIndex].gameTitle);
                try
                {
                    if (toDelete.name.Contains("Xenia {Temporary Copy}"))
                    {
                        if (File.Exists("XData\\Xenia\\" + fileTitle + "\\xenia.exe"))
                        {
                            File.Delete("XData\\Xenia\\" + fileTitle + "\\xenia.exe");
                        }
                        if (File.Exists("XData\\Xenia\\" + fileTitle + "\\xenia.log"))
                        {
                            File.Delete("XData\\Xenia\\" + fileTitle + "\\xenia.log");
                        }
                        if (File.Exists("XData\\Xenia\\" + fileTitle + "\\xenia.config.toml"))
                        {
                            File.Delete("XData\\Xenia\\" + fileTitle + "\\xenia.config.toml");
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Xenia temp copy", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Xenia Canary {Temporary Copy}"))
                    {
                        if (File.Exists("XData\\Canary\\" + fileTitle + "\\xenia_canary.exe"))
                        {
                            File.Delete("XData\\Canary\\" + fileTitle + "\\xenia_canary.exe");
                        }
                        if (File.Exists("XData\\Canary\\" + fileTitle + "\\xenia.log"))
                        {
                            File.Delete("XData\\Canary\\" + fileTitle + "\\xenia.log");
                        }
                        if (File.Exists("XData\\Canary\\" + fileTitle + "\\xenia-canary.config.toml"))
                        {
                            File.Delete("XData\\Canary\\" + fileTitle + "\\xenia-canary.config.toml");
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Canary temp copy", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Save Data (Xenia)"))
                    {
                        if (Directory.Exists("XData\\Xenia\\" + fileTitle + "\\content"))
                        {
                            foreach (string filepath in Directory.GetFiles("XData\\Xenia\\" + fileTitle + "\\content", "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete("XData\\Xenia\\" + fileTitle + "\\content", true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Xenia save data", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Save Data (Canary)"))
                    {
                        string dir = "XData\\Canary\\" + fileTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\profile";
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Canary save data", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Installed DLC"))
                    {
                        string dir = "XData\\Canary\\" + localData[selectedDataIndex].gameTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\00000002";
                        dir = GetFilepathString(dir, true);
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted installed DLC", "dir", dir);
                    }
                    else if (toDelete.name.Contains("Installed Title Update"))
                    {
                        string dir = "XData\\Canary\\" + localData[selectedDataIndex].gameTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\000B0000";
                        dir = GetFilepathString(dir, true);
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted installed Title Update", "dir", dir);
                    }
                    else if (toDelete.name.Contains("Extract"))
                    {
                        string[] split = localData[selectedDataIndex].gamePath.Split("\\");
                        string currentDir = "";
                        for (int i = 0; i < split.Length - 2; i++)
                        {
                            currentDir += split[i] + "\\";
                        }
                        currentDir += "_EXTRACT";
                        //currentDir = GetFilepathString(currentDir, true).Insert(1, ":");
                        //currentDir += "\\" + contentId;
                        //currentDir += "\\" + dataFiles[selectedDataIndex][manageWindow.stringIndex].name;
                        //currentDir = GetFilepathString(currentDir, true).Insert(1, ":");

                        string dir = currentDir;
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Extract", "dir", dir);
                    }
                    refreshData = true;
                    message = new MessageWindow(this, "File Deleted", "The file was successfully deleted", State.Data);
                    state = State.Message;
                }
                catch
                {
                    message = new MessageWindow(this, "Error", "Unable to delete file. It may currently be in use", State.Data);
                    state = State.Message;
                    Logging.Write(LogType.Critical, Event.Error, "Error when deleting file", "fileTitle", fileTitle);
                }
                messageYes = false;
                toDelete = null;
            }
            // File import check (DLC and Title Updates)
            else if (messageYes && toImport != null)
            {
                if (File.Exists(toImport.filepath))
                {
                    // Making first folder (Content ID, such as 00000002 for DLC)
                    string contentId = "";
                    if (toImport.subTitle == "Downloadable Content")
                    {
                        contentId = "00000002";
                    }
                    else if (toImport.subTitle == "Title Update")
                    {
                        contentId = "000B0000";
                    }
                    // Ensuring a content folder is present
                    string currentDir = "XData\\Canary\\";
                    currentDir += masterData[selectedDataIndex].gameTitle + "\\";
                    currentDir = GetFilepathString(currentDir, true);
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += "content" + "\\";
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += masterData[selectedDataIndex].titleId + "\\";
                    currentDir = GetFilepathString(currentDir, true);
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += contentId + "\\";
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += toImport.filepath.Split("\\").Last();
                    currentDir = GetFilepathString(currentDir, true);
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    // Deleting any existing data
                    else
                    {
                        foreach (string filepath in Directory.GetFiles(currentDir))
                        {
                            File.Delete(filepath);
                        }
                    }
                    // Extracting content to folder
                    string args = "\"" + toImport.filepath + "\" \"" + Directory.GetCurrentDirectory() + "\\" + currentDir + "\"";
                    ProcessStartInfo startInfo = new ProcessStartInfo(extractPath, args);
                    startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    Process.Start(startInfo);
                    Logging.Write(LogType.Critical, Event.CanaryImport, "Launched dump tool", "args", args);
                }
                else
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error when importing content", "filepath", toImport.filepath);
                    message = new MessageWindow(this, "Error", "Import filepath does not exist", State.Manage);
                    state = State.Message;
                }
                messageYes = false;
                toImport = null;
            }
            // File extraction check
            else if (messageYes && toExtract != null)
            {
                if (File.Exists(toExtract.filepath))
                {
                    // Making first folder (Content ID, such as 00000002 for DLC)
                    string contentId = "";
                    if (toExtract.subTitle == "Downloadable Content")
                    {
                        contentId = "00000002";
                    }
                    else if (toExtract.subTitle == "Title Update")
                    {
                        contentId = "000B0000";
                    }
                    else if (toExtract.subTitle == "Installed Xbox 360 Game")
                    {
                        contentId = "00004000";
                    }
                    else if (toExtract.subTitle == "Installed Game on Demand")
                    {
                        contentId = "00007000";
                    }
                    else if (toExtract.subTitle == "Video")
                    {
                        contentId = "00090000";
                    }
                    else if (toExtract.subTitle == "Game Trailer")
                    {
                        contentId = "000C0000";
                    }
                    else if (toExtract.subTitle == "Xbox Live Arcade Title")
                    {
                        contentId = "000D0000";
                    }
                    else if (toExtract.subTitle == "Gamer Picture")
                    {
                        contentId = "00020000";
                    }
                    else if (toExtract.subTitle == "Xbox 360 Theme")
                    {
                        contentId = "00030000";
                    }
                    // Ensuring a content folder is present
                    string[] split = localData[selectedDataIndex].gamePath.Split("\\");
                    string currentDir = "";
                    for (int i = 0; i < split.Length - 2; i++)
                    {
                        currentDir += split[i] + "\\";
                    }
                    currentDir += "_EXTRACT";
                    currentDir = GetFilepathString(currentDir, true).Insert(1, ":");
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += "\\" + contentId;
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += "\\" + dataFiles[selectedDataIndex][manageWindow.stringIndex].name;
                    currentDir = GetFilepathString(currentDir, true).Insert(1, ":");
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    // Deleting any existing data
                    else
                    {
                        foreach (string filepath in Directory.GetFiles(currentDir))
                        {
                            File.Delete(filepath);
                        }
                    }
                    // Extracting content to folder
                    string args = "\"" + toExtract.filepath + "\" \"" + currentDir + "\"";
                    ProcessStartInfo startInfo = new ProcessStartInfo(extractPath, args);
                    startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    Process.Start(startInfo);
                    Logging.Write(LogType.Critical, Event.Extract, "Launched dump tool", "args", args);
                }
                else
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error when extracting content", "filepath", toExtract.filepath);
                    message = new MessageWindow(this, "Error", "Extract filepath does not exist", State.Manage);
                    state = State.Message;
                }
                messageYes = false;
                toExtract = null;
            }

            // Updating windows
            if (state == State.Menu)
            {
                menuWindow.Update();
            }
            else if (state == State.GameMenu)
            {
                gameManageWindow.Update();
                // Checking for database searches
                if (databaseGameInfo != null && databaseGameInfo.Count > 0)
                {
                    if (databaseGameInfo.Count == 1) // Skipping game selection
                    {
                        databaseResultIndex = 0;
                        OpenDatabaseResult(State.GameMenu);
                    }
                    else
                    {
                        OpenDatabasePicker();
                    }
                }
            }
            else if (state == State.GameXeniaSettings)
            {
                gameXeniaSettingsWindow.Update();
            }
            else if (state == State.DatabasePicker)
            {
                databasePickerWindow.Update();
            }
            else if (state == State.DatabaseResult)
            {
                databaseResultWindow.Update();
                if (textWindowInput != null)
                {
                    tempGameTitle = textWindowInput;
                    textWindowInput = null;
                }
            }
            else if (state == State.GameInfo)
            {
                gameInfoWindow.Update();
                if (textWindowInput != null)
                {
                    if (gameInfoWindow.buttonIndex == 0 && gameData[index].gameTitle != textWindowInput)
                    {
                        tempGameTitle = textWindowInput;
                        RenameGame();
                        gameInfoWindow.titleSprite.text = "Info for " + tempGameTitle;
                        gameManageWindow.titleSprite.text = "Manage " + tempGameTitle;
                    }
                    else if (gameInfoWindow.buttonIndex == 1)
                    {
                        gameData[index].developer = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameInfoWindow.buttonIndex == 2)
                    {
                        gameData[index].publisher = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameInfoWindow.buttonIndex == 3)
                    {
                        gameData[index].titleId = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameInfoWindow.buttonIndex == 4)
                    {
                        gameData[index].alphaAs = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    textWindowInput = null;
                }
            }
            else if (state == State.ReleaseYear || state == State.ReleaseMonth || state == State.ReleaseDay)
            {
                releaseWindow.Update();
            }
            else if (state == State.GameFilepaths)
            {
                gameFilepathsWindow.Update();
                if (textWindowInput != null)
                {
                    if (gameFilepathsWindow.buttonIndex == 0)
                    {
                        gameData[index].gamePath = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameFilepathsWindow.buttonIndex == 1)
                    {
                        try
                        {
                            arts[gameData[index].gameTitle] = Texture2D.FromFile(_graphics.GraphicsDevice, textWindowInput);
                            gameData[index].artPath = textWindowInput;
                        }
                        catch
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid image path", "filepath", textWindowInput);
                            message = new MessageWindow(this, "Error", "Invalid image path", State.GameFilepaths);
                            state = State.Message;
                        }
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameFilepathsWindow.buttonIndex == 2)
                    {
                        try
                        {
                            icons[gameData[index].gameTitle] = Texture2D.FromFile(_graphics.GraphicsDevice, textWindowInput);
                            gameData[index].iconPath = textWindowInput;
                        }
                        catch
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid icon path", "filepath", textWindowInput);
                            message = new MessageWindow(this, "Error", "Invalid icon path", State.GameFilepaths);
                            state = State.Message;
                        }
                        SaveGames();
                        textWindowInput = null;
                    }
                }
            }
            else if (state == State.GameCategories)
            {
                gameCategoriesWindow.Update();
                if (textWindowInput != null)
                {
                    // Create Category
                    if (gameCategoriesWindow.buttonIndex == 3)
                    {
                        if (!folders.Contains(textWindowInput))
                        {
                            folders.Add(textWindowInput);
                            gameData[index].folders.Add(textWindowInput);
                            SaveGames();
                            Logging.Write(LogType.Important, Event.FolderAdd, "Added a new folder", "name", textWindowInput);
                        }
                        else
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Duplicate folder name", "name", textWindowInput);
                            message = new MessageWindow(this, "Error", "Cannot have duplicate Category names", State.GameCategories);
                            state = State.Message;
                        }
                        textWindowInput = null;
                    }
                    // Rename Category
                    else if (gameCategoriesWindow.buttonIndex == 4)
                    {
                        if (!folders.Contains(textWindowInput))
                        {
                            string oldName = folders[tempCategoryIndex];
                            folders.Add(textWindowInput);
                            foreach (GameData game in masterData)
                            {
                                if (game.folders.Contains(oldName))
                                {
                                    game.folders.Remove(oldName);
                                    game.folders.Add(textWindowInput);
                                }
                            }
                            SaveGames();
                            FolderReset();
                            Initialize();
                            Logging.Write(LogType.Important, Event.FolderRename, "Renamed folder", new Dictionary<string, string>()
                            {
                                { "oldName", oldName },
                                { "newName", textWindowInput }
                            });
                        }
                        else
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Duplicate folder name", "name", textWindowInput);
                            message = new MessageWindow(this, "Error", "Cannot have duplicate Category names", State.GameCategories);
                            state = State.Message;
                        }
                        textWindowInput = null;
                    }
                }
                if (messageYes)
                {
                    // Deleting Category
                    if (gameCategoriesWindow.buttonIndex == 5)
                    {
                        string folderName = folders[tempCategoryIndex];
                        foreach (GameData game in masterData)
                        {
                            game.folders.Remove(folders[tempCategoryIndex]);
                        }
                        folders.Remove(folders[tempCategoryIndex]);
                        SaveGames();
                        FolderReset();
                        Initialize();
                        Logging.Write(LogType.Important, Event.FolderDelete, "Deleted folder", "name", folderName);
                    }
                    messageYes = false;
                }
            }
            else if (state == State.GameXEX)
            {
                gameXEXWindow.Update();

                if (textWindowInput != null)
                {
                    if (gameXEXWindow.buttonIndex == 2)
                    {
                        if (String.IsNullOrEmpty(newXEX))
                        {
                            newXEX = textWindowInput;
                            text = new TextInputWindow(this, "Edit XEX Filepath", "", State.GameXEX);
                        }
                        else
                        {
                            gameData[index].xexNames.Add(newXEX);
                            gameData[index].xexPaths.Add(textWindowInput);
                            Logging.Write(LogType.Important, Event.AddXEX, "Added new XEX", new Dictionary<string, string>()
                            {
                                { "xexName", newXEX },
                                { "xexPath", textWindowInput }
                            });
                            newXEX = null;
                            SaveGames();
                        }
                        textWindowInput = null;
                    }
                    else if (gameXEXWindow.buttonIndex == 3)
                    {
                        gameData[index].xexNames[tempCategoryIndex] = textWindowInput;
                        gameXEXWindow.extraSprites[0].ToTextSprite().text = textWindowInput;
                        SaveGames();
                        Logging.Write(LogType.Important, Event.RenameXEX, "Renamed XEX", "name", textWindowInput);
                        textWindowInput = null;
                    }
                    else if (gameXEXWindow.buttonIndex == 4)
                    {
                        gameData[index].xexPaths[tempCategoryIndex] = textWindowInput;
                        SaveGames();
                        Logging.Write(LogType.Important, Event.RepathXEX, "Changed XEX path", "path", textWindowInput);
                        textWindowInput = null;
                    }
                }
                if (messageYes)
                {
                    if (gameXEXWindow.buttonIndex == 5)
                    {
                        if (tempCategoryIndex == -1)
                        {
                            Logging.Write(LogType.Critical, Event.DeleteGame, "Deleting game", "gameTitle", gameData[index].gameTitle);
                            int masterIndex = -1;
                            int count = 0;
                            foreach (GameData game in masterData)
                            {
                                if (game.Equals(gameData[index]))
                                {
                                    masterIndex = count;
                                    break;
                                }
                                count++;
                            }
                            gameData.RemoveAt(index);
                            masterData.RemoveAt(masterIndex);
                            SaveGames();
                            newGameProcess = true;
                            Initialize();
                            LoadArts();
                            index = 0;
                            BeginMainTransition();
                            FolderReset();
                            state = State.Main;
                        }
                        else
                        {
                            Logging.Write(LogType.Critical, Event.DeleteXEX, "Deleted XEX", "name", gameData[index].xexNames[tempCategoryIndex]);
                            gameData[index].xexNames.RemoveAt(tempCategoryIndex);
                            gameData[index].xexPaths.RemoveAt(tempCategoryIndex);
                            SaveGames();
                        }
                        messageYes = false;
                    }
                }
            }
            else if (state == State.Options)
            {
                optionsWindow.Update();
            }
            else if (state == State.Graphics)
            {
                graphicsWindow.Update();
            }
            else if (state == State.Settings)
            {
                settingsWindow.Update();
            }
            else if (state == State.NewGame)
            {
                if (stfsFiles == null)
                {
                    stfsFiles = new Dictionary<string, string>();
                }
                newGameWindow.Update();

                if (newGameProcess)
                {
                    state = State.Main;
                    Initialize();
                    FolderReset();
                    index = 0;
                }
                if (textWindowInput != null)
                {
                    if (newGameWindow.buttonIndex == 0)
                    {
                        // New advanced import
                        try
                        {
                            // Finding the directory
                            tempFilepathSTFS = textWindowInput;
                            newGamePath = tempFilepathSTFS;
                            DirectoryInfo dirInfo = new DirectoryInfo(tempFilepathSTFS);
                            textWindowInput = null;
                            if (dirInfo.Exists)
                            {
                                string mainGame = ""; // The name of the STFS file that will be the main executable

                                // Searching for STFS files
                                string[] contentTypes = ["00004000", "00007000", "000D0000", "00080000"];
                                for (int i = 0; i < contentTypes.Length; i++)
                                {
                                    string newMain = FindSTFSGames(dirInfo.FullName, contentTypes[i], stfsFiles);
                                    // Getting main game
                                    if (mainGame == "")
                                    {
                                        mainGame = newMain;
                                    }
                                }

                                // If a game was found
                                if (mainGame != "")
                                {
                                    STFS24 newSTFS = new STFS24(stfsFiles[mainGame]);
                                    tempTitleSTFS = mainGame;
                                    tempIdSTFS = STFS24.GetByteArrayAsHex(newSTFS.ReturnMetadata().GetTitleID());
                                    tempFilepathSTFS = stfsFiles[mainGame];
                                    newSTFS.CloseStream();
                                    // Icon code (My new STFS24 library doesn't support icon extraction yet)
                                    STFS stfs = new STFS(stfsFiles[mainGame]);
                                    tempIconSTFS = stfs.icon;
                                    stfsFiles.Remove(mainGame);
                                    message = new MessageWindow(this, "STFS Results", "Title: " + tempTitleSTFS + ", Title ID: " + tempIdSTFS, State.NewGame, MessageWindow.MessagePrompts.OKCancel);
                                    state = State.Message;
                                }
                            }
                            else // Directory doesn't exist
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception e)
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid game directory", "dir", textWindowInput);
                            message = new MessageWindow(this, "Error", "Invalid Directory\n" + e.ToString(), State.NewGame);
                            textWindowInput = null;
                            state = State.Message;
                        }
                    }
                    else if (newGameWindow.buttonIndex == 1)
                    {
                        if (String.IsNullOrEmpty(newXEX))
                        {
                            newXEX = textWindowInput;
                            text = new TextInputWindow(this, "Filepath for " + newXEX, "", State.NewGame);
                        }
                        else
                        {
                            masterData.Add(new GameData());
                            masterData.Last().gameTitle = newXEX;
                            masterData.Last().gamePath = textWindowInput;
                            gameData.Add(masterData.Last());
                            SaveGames();
                            textWindowInput = null;
                            newGameProcess = true;
                            newXEX = "";
                            index = gameData.Count - 1;
                            EditGame();
                        }
                    }
                    else if (newGameWindow.buttonIndex == 0 || newGameWindow.buttonIndex == 2)
                    {
                        // Importing from STFS
                        try
                        {
                            STFS stfs = new STFS(textWindowInput);
                            tempFilepathSTFS = textWindowInput;
                            textWindowInput = null;
                            tempTitleSTFS = stfs.data.displayName;
                            tempIdSTFS = stfs.titleID;
                            tempIconSTFS = stfs.icon;
                            message = new MessageWindow(this, "STFS Results", "Title: " + tempTitleSTFS + ", Title ID: " + tempIdSTFS, State.NewGame, MessageWindow.MessagePrompts.OKCancel);
                            state = State.Message;
                        }
                        catch (Exception e)
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid STFS/SVOD file", "exception", e.ToString());
                            message = new MessageWindow(this, "Error", "Invalid STFS/SVOD file\n" + e.ToString(), State.NewGame);
                            state = State.Message;
                        }
                    }
                }
                if (messageYes)
                {
                    if (newGameWindow.buttonIndex == 0)
                    {
                        Logging.Write(LogType.Important, Event.NewGameStartSTFS, "Starting game add process", "tempIdSTFS", tempIdSTFS);
                        // Saving icon
                        if (!Directory.Exists("IconData"))
                        {
                            Directory.CreateDirectory("IconData");
                            Logging.Write(LogType.Critical, Event.DirCreate, "Created IconData directory");
                        }
                        tempIconSTFS.Save("IconData\\" + tempIdSTFS + ".png");
                        Logging.Write(LogType.Critical, Event.IconSave, "Saved icon", "path", "IconData\\" + tempIdSTFS + ".png");
                        // Creating new game
                        masterData.Add(new GameData());
                        // Checking if a game with this name is already imported
                        int duplicateNum = 1;
                        for (int i = 0; i <  masterData.Count; i++)
                        {
                            string titleCheck = tempTitleSTFS;
                            if (duplicateNum >= 2)
                            {
                                titleCheck = titleCheck + " (" + duplicateNum + ")";
                            }
                            if (titleCheck == masterData[i].gameTitle)
                            {
                                duplicateNum++;
                                i = 0;
                            }
                        }
                        // Changing name to avoid duplicates, if needed
                        if (duplicateNum >= 2)
                        {
                            tempTitleSTFS = tempTitleSTFS + " (" + duplicateNum + ")";
                            Logging.Write(LogType.Standard, Event.NewGameDuplicate, "New game has duplicate name, fixing name", "newName", tempTitleSTFS);
                        }
                        // Grabbing a cover for the game
                        if (Directory.Exists(newGamePath + "\\_covers"))
                        {
                            if (File.Exists(newGamePath + "\\_covers\\cover0.jpg"))
                            {
                                masterData.Last().artPath = newGamePath + "\\_covers\\cover0.jpg";
                                arts[masterData.Last().gameTitle] = Texture2D.FromFile(_graphics.GraphicsDevice, masterData.Last().artPath);
                                masterData.Last().hasCoverArt = true;
                                Logging.Write(LogType.Debug, Event.NewGameCoverFound, "cover0.jpg found");
                            }
                        }
                        // Adding game to masterData
                        masterData.Last().gameTitle = tempTitleSTFS;
                        masterData.Last().gamePath = tempFilepathSTFS;
                        masterData.Last().titleId = tempIdSTFS;
                        masterData.Last().iconPath = "IconData\\" + tempIdSTFS + ".png";
                        masterData.Last().xexNames = stfsFiles.Keys.ToList();
                        Logging.Write(LogType.Critical, Event.NewGameProcessed, "New game added", new Dictionary<string, string>()
                        {
                            { "gameTitle", tempTitleSTFS },
                            { "gamePath", tempFilepathSTFS },
                            { "titleID", tempIdSTFS },
                            { "iconPath", masterData.Last().iconPath },
                            { "xexCount", "" + masterData.Last().xexNames.Count }
                        });
                        for (int i = 0; i < stfsFiles.Keys.Count; i++)
                        {
                            masterData.Last().xexPaths.Add(stfsFiles[masterData.Last().xexNames[i]]);
                        }
                        stfsFiles = null;
                        gameData.Add(masterData.Last());
                        SaveGames();
                        newGameProcess = true;
                        messageYes = false;
                        index = gameData.Count - 1;
                        EditGame();
                    }
                }
            }
            else if (state == State.Data)
            {
                if (refreshData)
                {
                    DataWindowEffects.RefreshData(this, dataWindow);
                    refreshData = false;
                    Logging.Write(LogType.Debug, Event.ManageDataRefresh, "Data refresh");
                }
                dataWindow.Update();
            }
            else if (state == State.DataSort)
            {
                dataSortWindow.Update();
            }
            else if (state == State.DataFilter)
            {
                dataFilterWindow.Update();
            }
            else if (state == State.Compat && IsActive && compatWaitFrames <= 0)
            {
                compatWindow.Update();
                if (!lastActiveCheck)
                {
                    if (GetFullscreen())
                    {
                        if (_graphics.IsFullScreen)
                        {
                            _graphics.IsFullScreen = false;
                            _graphics.ApplyChanges();
                            fullscreenDelay = 30;
                        }
                    }
                }
            }
            else if (state == State.Manage)
            {
                manageWindow.Update();
                if (Convert.ToInt32(manageWindow.tags[0]) != manageWindow.stringIndex)
                {
                    DataWindowEffects.UpdateText(this, manageWindow, dataWindow.stringIndex);
                }
            }
            else if (state == State.ManageFile)
            {
                fileManageWindow.Update();
            }
            else if (state == State.Metadata)
            {
                metadataWindow.Update();
                if (KeyboardInput.keys["S"].IsFirstDown())
                {
                    hideSecretMetadata = !hideSecretMetadata;
                    fileManageWindow.buttonEffects.ActivateButton(this, fileManageWindow, fileManageWindow.buttons[fileManageWindow.stringIndex], fileManageWindow.stringIndex);
                }
            }
            else if (state == State.Credits)
            {
                creditsWindow.Update();
                if (KeyboardInput.keys["V"].IsFirstDown())
                {
#if DEBUG
                    string verType = "DEBUG";
#else
                    string verType = "RELEASE";
#endif
                    if (enableExp)
                    {
                        verType += " + EXP";
                    }
                    message = new MessageWindow(this, "Version Info", "VERNUM " + Shared.VERNUM + ". " + verType, State.Credits);
                    state = State.Message;
                }
            }
            else if (compatWaitFrames > 0)
            {
                compatWaitFrames--;
            }
            if (state == State.Message)
            {
                message.Update();
            }
            else if (state == State.Text)
            {
                text.Update();
            }

            // Updating text
            titleSprite.text = gameData[index].gameTitle;
            titleSprite.pos = titleSprite.GetCenterCoords();
            titleSprite.pos.Y += 330;
            titleSprite.scale = 0.9f;
            if (sort == Sort.Pub)
            {
                subTitleSprite.text = gameData[index].publisher + " - ";
            }
            else
            {
                subTitleSprite.text = gameData[index].developer + " - ";
            }
            if (sort == Sort.Date)
            {
                if (inverseDate)
                {
                    subTitleSprite.text += gameData[index].day + "/" + gameData[index].month + "/" + gameData[index].year;
                }
                else
                {
                    subTitleSprite.text += gameData[index].month + "/" + gameData[index].day + "/" + gameData[index].year;
                }
            }
            else
            {
                subTitleSprite.text += gameData[index].year;
            }
            subTitleSprite.pos = subTitleSprite.GetCenterCoords();
            subTitleSprite.pos.Y += 400;
            // Sort text
            if (sort == Sort.AZ)
            {
                sortSprite.text = "Sort: A-Z";
            }
            else if (sort == Sort.ZA)
            {
                sortSprite.text = "Sort: Z-A";
            }
            else if (sort == Sort.Date)
            {
                sortSprite.text = "Sort: Release";
            }
            else if (sort == Sort.Dev)
            {
                sortSprite.text = "Sort: Developer";
            }
            else if (sort == Sort.Pub)
            {
                sortSprite.text = "Sort: Publisher";
            }
            sortSprite.pos.X = 1840 - sortSprite.font.MeasureString(sortSprite.text).X * 0.8f;
            sortSprite.color = sortColor;

            // Folder text animation
            if (folderPath.frames <= 0)
            {
                if (secondFolderPath != null && secondFolderPath.offset.X < 0)
                {
                    folderSprite.text = folders[folderIndex];
                    float fontX = folderSprite.font.MeasureString(folderSprite.text).X / 2;
                    folderSprite.pos.X = 205 - fontX;
                    folderPath = new AnimationPath(folderSprite, new Vector2(960 - fontX, 60), 1f, 15);
                    secondFolderPath.offset.X = 0;
                }
                else if (secondFolderPath != null && secondFolderPath.offset.X > 0)
                {
                    folderSprite.text = folders[folderIndex];
                    float fontX = folderSprite.font.MeasureString(folderSprite.text).X / 2;
                    folderSprite.pos.X = 1405 + fontX;
                    folderPath = new AnimationPath(folderSprite, new Vector2(960 - fontX, 60), 1f, 15);
                    secondFolderPath.offset.X = 0;
                }
                else
                {
                    folderSprite.text = folders[folderIndex];
                    folderSprite.pos = folderSprite.GetCenterCoords();
                    folderSprite.pos.Y = 60;
                    folderSprite.color = folderColor;
                }
            }
            else
            {
                folderPath.Update();
            }
            Color altColor = Ozzz.Helper.NewColorAlpha(fontAltColor, (int)whiteGradient.values[3]);
            xeniaUntestedText.color = altColor;
            canaryUntestedText.color = altColor;

            // Updating fades
            if (state == State.Select)
            {
                if (mainFadeGradient.frameCycle != 20)
                {
                    UpdateGradients();
                }
                foreach (Sprite s in mainFadeLayer.sprites)
                {
                    s.color = mainFadeGradient.GetColor();
                }
            }
            else if (state == State.Main || state == State.Menu)
            {
                if (mainFadeGradient.frameCycle != 20)
                {
                    UpdateGradients();
                    firstReset = true;
                }
                else if (firstReset)
                {
                    ResetGameIcons();
                    firstReset = false;
                }
                foreach (Sprite s in mainFadeLayer.sprites)
                {
                    s.color = mainFadeGradient.GetColor();
                }
            }

            // Updating Jump Layer
            jumpLayer.Update();
            if (jumpLayerAlpha > 0)
            {
                //jumpToText.color = fontColor;
                //jumpIndexText.color = fontColor;

                jumpLayerAlpha -= 16;
                if (jumpLayerAlpha > 240)
                {
                    jumpFade.color.A = 120;
                    jumpToText.color = Color.White;
                    jumpIndexText.color = Color.White;
                }
                else
                {
                    jumpFade.color.A = (byte)(jumpLayerAlpha / 2);
                    jumpToText.color = Color.FromNonPremultiplied(jumpLayerAlpha, jumpLayerAlpha, jumpLayerAlpha, jumpLayerAlpha);
                    jumpIndexText.color = Color.FromNonPremultiplied(jumpLayerAlpha, jumpLayerAlpha, jumpLayerAlpha, jumpLayerAlpha);
                }
            }

            // Updating ObjectSprites
            xeniaCompatLogo.UpdatePos();
            xeniaCompatLogo.color = whiteGradient.GetColor();
            canaryCompatLogo.UpdatePos();
            canaryCompatLogo.color = whiteGradient.GetColor();
            xeniaCompat.UpdatePos();
            xeniaCompat.color = whiteGradient.GetColor();
            canaryCompat.UpdatePos();
            canaryCompat.color = whiteGradient.GetColor();
            bottomBorder.UpdatePos();
            topBorderLayer.Update();
            backBorderLayer.Update();
            triviaMaskingLayer.Update();
            foreach (Sprite sprite in backBorderLayer.sprites)
            {
                sprite.color = backColorAlt;
            }
            foreach (Sprite sprite in triviaMaskingLayer.sprites)
            {
                sprite.color = backColorAlt;
            }
            foreach (Sprite sprite in topBorderLayer.sprites)
            {
                sprite.color = backColorAlt;
            }
            topBorder.color = topBorderColor;
            bottomBorder.color = bottomBorderColor;

            // Updating rings
            for (int i = 0; i < rings.Count; i++)
            {
                rings[i].Update();
                if (rings[i].color == rings[i].fade.colors[1])
                {
                    rings.RemoveAt(i);
                    i--;
                }
            }
            if (ringFrames <= 0)
            {
                ringFrames = 45;
                if (CheckStateSelectMenu())
                {
                    rings.Add(new Ring(circ, new Rectangle(560, 1080, 800, 800), ringDuration, Ring.RingType.Gray, this));
                }
                else
                {
                    rings.Add(new Ring(circ, new Rectangle(560, 1080, 800, 800), ringDuration, Ring.RingType.LightGreen, this));
                }
            }
            else
            {
                ringFrames--;
            }

            // Updating corner text
            if (firstLoad)
            {
                bottomInfo.displayGradient.colors[0] = backColorAlt;
                bottomInfo.Reset();
            }

            // Time
            timeText.color = timeDateColor;
            if (timeText.CheckMouse(true) && MouseInput.IsLeftFirstDown() && (state == State.Main || state == State.Select))
            {
                militaryTime = !militaryTime;
                buttonSwitchSound.Play();
                SaveConfig();
            }
            if (militaryTime)
            {
                timeText.text = "" + DateTime.Now.Hour + ":" + Ozzz.Helper.IntToString(DateTime.Now.Minute, 2) + ":" + Ozzz.Helper.IntToString(DateTime.Now.Second, 2);
            }
            else
            {
                string suffix = "AM";
                int hour = DateTime.Now.Hour;
                if (hour >= 13)
                {
                    suffix = "PM";
                    hour -= 12;
                }
                timeText.text = "" + hour + ":" + Ozzz.Helper.IntToString(DateTime.Now.Minute, 2) + ":" + Ozzz.Helper.IntToString(DateTime.Now.Second, 2) + " " + suffix;
            }
            float tempY = timeText.pos.Y;
            timeText.Centerize(new Vector2(120, 970));
            timeText.pos.Y = tempY;
            timeText.UpdatePos();

            // Date
            dateText.color = timeDateColor;
            if (dateText.CheckMouse(true) && MouseInput.IsLeftFirstDown() && (state == State.Main || state == State.Select))
            {
                inverseDate = !inverseDate;
                buttonSwitchSound.Play();
                SaveConfig();
            }
            if (inverseDate)
            {
                dateText.text = "" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            }
            else
            {
                dateText.text = "" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year;
            }
            tempY = dateText.pos.Y;
            dateText.Centerize(new Vector2(timeText.GetCenterPoint().X, 1000));
            dateText.pos.Y = tempY;

            // Controllers
            int oldControllers = Convert.ToInt32(contNumText.text);
            int connectedControllers = 0;
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                GamePadState state = GamePad.GetState(i);
                if (state.IsConnected)
                {
                    connectedControllers++;
                }
            }
            if (oldControllers != connectedControllers)
            {
                Logging.Write(LogType.Important, Event.ControllerCountChange, "Controller(s) have been dis/connected", "change", "" + (connectedControllers - oldControllers));
            }
            contNumText.text = "" + connectedControllers;

            // Other corner stuff
            if ((drivesText.CheckMouse(true) || freeSpaceText.CheckMouse(true)) && MouseInput.IsLeftFirstDown() && state == State.Main)
            {
                if (bottomInfo.currentIndex == 1)
                {
                    checkDrivesOnManage = !checkDrivesOnManage;
                    string enabledText = "enabled";
                    if (!checkDrivesOnManage)
                    {
                        enabledText = "disabled";
                    }
                    message = new MessageWindow(this, "Info", "Drive checking on Manage Data " + enabledText, state);
                    state = State.Message;
                    SaveConfig();
                }
            }
            if (state != State.Select && state != State.Launch && (state != State.Message || (message.returnState != State.Select && message.returnState != State.Launch)))
            {
                bottomInfo.Update();
            }
            if (updateFreeSpace)
            {
                SetDriveSpaceText();
            }

            // Updating trivia
            triviaSprite.UpdatePos();
            if (triviaSprite.pos.X + triviaSprite.GetSize().X < 330)
            {
                string nextTrivia = triviaSprite.text;
                while (nextTrivia == triviaSprite.text)
                {
                    nextTrivia = trivia[new Random().Next(0, trivia.Count)];
                }
                triviaSprite.text = nextTrivia;
                triviaSprite.pos.X = 1600;
                Logging.Write(LogType.Debug, Event.TriviaChange, "New trivia displayed", "trivia", nextTrivia);
            }
            triviaSprite.color = triviaColor;

            // Unloading windows
            if (state == State.Main)
            {
                launchWindow = null;
                menuWindow = null;
                gameXEXWindow = null;
                newGameWindow = null;
            }
            else if (state == State.Select)
            {
                xexWindow = null;
                compatWindow = null;
                gameManageWindow = null;
            }
            else if (state == State.GameMenu)
            {
                gameXeniaSettingsWindow = null;
                databaseResultWindow = null;
                databasePickerWindow = null;
                databaseGameInfo = null;
                gameInfoWindow = null;
                releaseWindow = null;
                gameFilepathsWindow = null;
                gameCategoriesWindow = null;
                gameXEXWindow = null;
            }
            else if (state == State.DatabaseResult)
            {
                releaseWindow = null;
            }
            else if (state == State.DatabasePicker)
            {
                databaseResultWindow = null;
            }
            else if (state == State.GameInfo)
            {
                if (releaseWindow != null)
                {
                    gameData[index].year = tempYear;
                    gameData[index].month = tempMonth;
                    gameData[index].day = tempDay;
                }
                releaseWindow = null;
                databasePickerWindow = null;
            }
            else if (state == State.Menu)
            {
                optionsWindow = null;
                dataWindow = null;
                creditsWindow = null;
                newGameWindow = null;
            }
            else if (state == State.NewGame)
            {
                gameManageWindow = null;
            }
            else if (state == State.Options)
            {
                graphicsWindow = null;
                settingsWindow = null;
            }
            else if (state == State.Launch)
            {
                compatWindow = null;
            }
            else if (state == State.Data)
            {
                manageWindow = null;
                fileManageWindow = null;
                dataSortWindow = null;
                dataFilterWindow = null;
            }
            else if (state == State.Manage)
            {
                fileManageWindow = null;
            }
            else if (state == State.ManageFile)
            {
                metadataWindow = null;
            }
            if (state != State.Message)
            {
                message = null;
            }
            if (state != State.Text)
            {
                text = null;
            }

            firstLoad = false;
            lastActiveCheck = IsActive;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backColor);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (showRings)
            {
                foreach (Ring ring in rings)
                {
                    ring.Draw(_spriteBatch);
                }
            }
            
            if (!skipDraw)
            {
                foreach (XGame game in gameIcons)
                {
                    game.Draw(_spriteBatch);
                }
            }
            else
            {
                skipDraw = false;
            }
            titleSprite.Draw(_spriteBatch);
            subTitleSprite.Draw(_spriteBatch);
            folderSprite.Draw(_spriteBatch);
            _spriteBatch.Draw(white, Ozzz.Scaling.RectangleScaled(0, 0, 210, 210), backColor);
            _spriteBatch.Draw(white, Ozzz.Scaling.RectangleScaled(1400, 0, 700, 210), backColor);
            sortSprite.Draw(_spriteBatch);
            _spriteBatch.Draw(mainLogo, Ozzz.Scaling.RectangleScaled(50, 40, 150, 150), Color.White);
            jumpLayer.Draw(_spriteBatch);
            backBorderLayer.Draw(_spriteBatch);
            triviaSprite.Draw(_spriteBatch);
            timeText.Draw(_spriteBatch);
            bottomLayer.Draw(_spriteBatch);
            bottomInfo.Draw(_spriteBatch);

            // Select menu
            _spriteBatch.Draw(white, new Rectangle(0, 0, Ozzz.Scaling.ScaleIntX(1920), Ozzz.Scaling.ScaleIntY(1080)), darkGradient.GetColor());
            if (CheckStateSelectMenu() && showRings)
            {
                foreach (Ring ring in rings)
                {
                    ring.Draw(_spriteBatch);
                }
            }
            if (mainTransitionPath != null && (mainTransitionPath.frames > 0))
            {
                backBorderLayer.Draw(_spriteBatch);
                triviaSprite.Draw(_spriteBatch);
            }
            if (launchWindow != null)
            {
                launchWindow.Draw(_spriteBatch);
            }
            bottomLayer.Draw(_spriteBatch);
            topBorderLayer.Draw(_spriteBatch);
            Color altColor = Ozzz.Helper.NewColorAlpha(fontAltColor, (int)whiteGradient.values[3]);
            _spriteBatch.DrawString(font, "Developer: " + gameData[index].developer, Ozzz.Scaling.ScaleVector2(new Vector2(200, 200)), altColor, 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "Publisher: " + gameData[index].publisher, Ozzz.Scaling.ScaleVector2(new Vector2(200, 240)), altColor, 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(calendar, Ozzz.Scaling.RectangleScaled(200, 290, 40, 40), altColor);
            if (inverseDate)
            {
                _spriteBatch.DrawString(font, "" + gameData[index].day + "/" + gameData[index].month + "/" + gameData[index].year, Ozzz.Scaling.ScaleVector2(new Vector2(250, 290)), altColor, 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            }
            else
            {
                _spriteBatch.DrawString(font, "" + gameData[index].month + "/" + gameData[index].day + "/" + gameData[index].year, Ozzz.Scaling.ScaleVector2(new Vector2(250, 290)), altColor, 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            }
            //_spriteBatch.DrawString(font, "Title ID: " + gameData[index].titleId, Ozzz.Scaling.ScaleVector2(new Vector2(200, 320)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(player, Ozzz.Scaling.RectangleScaled(200, 340, 40, 40), altColor);
            _spriteBatch.DrawString(font, "" + gameData[index].minPlayers + "-" + gameData[index].maxPlayers, Ozzz.Scaling.ScaleVector2(new Vector2(250, 340)), altColor, 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.DrawString(bold, gameData[index].gameTitle, Ozzz.Scaling.ScaleVector2(new Vector2(30, 25)), Ozzz.Helper.NewColorAlpha(majorFontColor, (int)whiteGradient.values[3]), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(1f), SpriteEffects.None, 0f);
            gameIcons[2].Draw(_spriteBatch);
            xeniaCompatLogo.Draw(_spriteBatch);
            canaryCompatLogo.Draw(_spriteBatch);
            if (gameData[index].xeniaCompat == GameData.XeniaCompat.Unknown)
            {
                xeniaUntestedText.Draw(_spriteBatch);
            }
            else
            {
                xeniaCompat.Draw(_spriteBatch);
            }
            if (gameData[index].canaryCompat == GameData.XeniaCompat.Unknown)
            {
                canaryUntestedText.Draw(_spriteBatch);
            }
            else
            {
                canaryCompat.Draw(_spriteBatch);
            }
            if (xexWindow != null)
            {
                xexWindow.Draw(_spriteBatch);
            }
            if (menuWindow != null)
            {
                menuWindow.Draw(_spriteBatch);
            }
            if (newGameWindow != null)
            {
                newGameWindow.Draw(_spriteBatch);
            }
            if (gameManageWindow != null)
            {
                gameManageWindow.Draw(_spriteBatch);
            }
            if (gameXeniaSettingsWindow != null)
            {
                gameXeniaSettingsWindow.Draw(_spriteBatch);
            }
            if (databasePickerWindow != null)
            {
                databasePickerWindow.Draw(_spriteBatch);
            }
            if (databaseResultWindow  != null)
            {
                databaseResultWindow.Draw(_spriteBatch);
            }
            if (gameInfoWindow != null)
            {
                gameInfoWindow.Draw(_spriteBatch);
            }
            if (releaseWindow != null) 
            {
                releaseWindow.Draw(_spriteBatch);
            }
            if (gameFilepathsWindow != null)
            {
                gameFilepathsWindow.Draw(_spriteBatch);
            }
            if (gameCategoriesWindow != null)
            {
                gameCategoriesWindow.Draw(_spriteBatch);
            }
            if (gameXEXWindow != null)
            {
                gameXEXWindow.Draw(_spriteBatch);
            }
            if (creditsWindow != null)
            {
                creditsWindow.Draw(_spriteBatch);
            }
            if (optionsWindow != null)
            {
                optionsWindow.Draw(_spriteBatch);
            }
            if (graphicsWindow != null)
            {
                graphicsWindow.Draw(_spriteBatch);
            }
            if (compatWindow != null)
            {
                compatWindow.Draw(_spriteBatch);
            }
            if (settingsWindow != null)
            {
                settingsWindow.Draw(_spriteBatch);
            }
            if (dataWindow != null)
            {
                dataWindow.Draw(_spriteBatch);
            }
            if (dataSortWindow != null)
            {
                dataSortWindow.Draw(_spriteBatch);
            }
            if (dataFilterWindow != null)
            {
                dataFilterWindow.Draw(_spriteBatch);
            }
            if (manageWindow != null)
            {
                manageWindow.Draw(_spriteBatch);
            }
            if (fileManageWindow != null)
            {
                fileManageWindow.Draw(_spriteBatch);
            }
            if (metadataWindow != null)
            {
                metadataWindow.Draw(_spriteBatch);
            }
            if (message != null)
            {
                message.Draw(_spriteBatch);
            }
            if (text != null)
            {
                text.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}