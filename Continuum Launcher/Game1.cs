using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Convert = System.Convert;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using SharpDX.MediaFoundation;
using SharpFont;
using XLCompanion;

namespace XeniaLauncher
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D white, rectTex, logo, circ, calendar, player, logoCanary, mainLogo, topBorderTex, bottomBorderTex, compLogo, topBorderNew, bottomBorderNew;
        public SpriteFont font, bold;
        public SoundEffect selectSound, backSound, launchSound, switchSound, buttonSwitchSound, sortSound, leftFolderSound, rightFolderSound;
        public ObjectSprite xeniaCompatLogo, canaryCompatLogo, xeniaCompat, canaryCompat, topBorder, bottomBorder, topBorderBack, bottomBorderBack;
        public TextSprite titleSprite, subTitleSprite, sortSprite, folderSprite, xeniaUntestedText, canaryUntestedText, timeText, dateText, contNumText, controllerText, freeSpaceText, drivesText, triviaSprite;
        public Layer mainFadeLayer, bottomLayer, backBorderLayer, triviaMaskingLayer, topBorderLayer;
        public Gradient mainFadeGradient, darkGradient, blackGradient, selectGradient, whiteGradient, buttonGradient;
        public AnimationPath mainTransitionPath, folderPath, secondFolderPath, topBorderPath, bottomBorderPath;
        public List<Ring> rings;
        public List<XGame> gameIcons;
        public List<GameData> gameData, masterData;
        public Dictionary<string, Texture2D> arts, compatBars, themeThumbnails, icons;
        public List<string> folders, trivia;
        public List<List<DataEntry>> dataFiles;
        public Window xexWindow, launchWindow, menuWindow, optionsWindow, graphicsWindow, compatWindow, settingsWindow, creditsWindow, dataWindow, manageWindow, deleteWindow, gameManageWindow, gameXeniaSettingsWindow, gameFilepathsWindow, gameInfoWindow, gameCategoriesWindow, gameXEXWindow, newGameWindow;
        public MessageWindow message;
        public TextInputWindow text;
        public Color backColor, backColorAlt;
        public SaveData configData;
        public DataManageStrings dataStrings;
        public SequenceFade bottomInfo;
        public DataEntry toDelete, toImport;
        public System.Drawing.Image tempIconSTFS;
        public string xeniaPath, canaryPath, configPath, ver, compileDate, textWindowInput, newXEX, tempTitleSTFS, tempIdSTFS, tempFilepathSTFS, extractPath;
        public int index, ringFrames, ringDuration, folderIndex, compatWaitFrames, selectedDataIndex, compatWindowDelay, fullscreenDelay, tempCategoryIndex;
        public bool right, firstLoad, firstReset, skipDraw, showRings, xeniaFullscreen, consolidateFiles, runHeadless, triggerMissingWindow, updateFreeSpace, messageYes, militaryTime, inverseDate, checkDrivesOnManage, lastActiveCheck, forceInit, newGameProcess;
        public enum State
        {
            Main, Select, Launch, Menu, Options, Credits, Graphics, Settings, Compat, Message, Data, Manage, Delete, GameMenu, GameXeniaSettings, GameFilepaths, GameInfo, GameCategories, GameXEX, Text, NewGame
        }
        public State state;
        public enum Sort
        {
            AZ, ZA, Date, Dev, Pub
        }
        public Sort sort;
        public enum Theme
        {
            Original, Green, Blue, Orange, Gray, Purple
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
            Ozzz.Initialize(new Vector2((float)GraphicsDevice.Viewport.Width / 1920, (float)GraphicsDevice.Viewport.Height / 1080), 60);
            GamepadInput.AddIndex(PlayerIndex.One);

            gameData = new List<GameData>();
            masterData = new List<GameData>();
            folders = new List<string>();
            trivia = new List<string>();
            dataStrings = new DataManageStrings();
            dataFiles = new List<List<DataEntry>>();

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

            folders.Add("All Games");
            SaveData read = new SaveData(configPath);
            read.ReadFile();
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
            }
            if (File.Exists("Apps\\Canary\\xenia_canary.exe"))
            {
                canaryPath = "Apps\\Canary\\xenia_canary.exe";
            }
            if (File.Exists("Apps\\Dump\\xenia-vfs-dump.exe"))
            {
                extractPath = "Apps\\Dump\\xenia-vfs-dump.exe";
            }
            else
            {
                extractPath = null;
            }
            SaveDataChunk games = read.savedData.FindData("games").GetChunk();
            foreach (SaveDataChunk game in games.saveDataObjects)
            {
                GameData data = new GameData();
                data.Read(game);
                masterData.Add(data);
            }
            foreach (GameData data in masterData)
            {
                data.folders.Add("All Games");
                gameData.Add(data);
                foreach (string folder in data.folders)
                {
                    if (!folders.Contains(folder))
                    {
                        folders.Add(folder);
                    }
                }
            }
            folders.Sort();
            gameData = gameData.OrderBy(o=>o.gameTitle).ToList();
            arts = new Dictionary<string, Texture2D>();
            compatBars = new Dictionary<string, Texture2D>();
            themeThumbnails = new Dictionary<string, Texture2D>();
            icons = new Dictionary<string, Texture2D>();
            rings = new List<Ring>();
            ringFrames = 30;
            ringDuration = 240;
            compatWindowDelay = 30;
            fullscreenDelay = -1;
            showRings = true;
            checkDrivesOnManage = true;
            lastActiveCheck = true;
            newGameProcess = false;
            forceInit = false;

            textWindowInput = null;
            gameManageWindow = null;
            gameCategoriesWindow = null;

            // Making new config settings file if not already present
            SoundEffect.MasterVolume = 0.7f;
            ResetTheme(Theme.Original, false);
            cwSettings = CWSettings.Untested;
            xeniaFullscreen = false;
            logLevel = LogLevel.Info;
            consolidateFiles = true;
            runHeadless = false;
            if (!File.Exists("XLSettings.txt"))
            {
                configData = new SaveData("XLSettings.txt");
                SaveConfig();
                triggerMissingWindow = true;
            }

            state = State.Main;

            base.Initialize();
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
                return true;
            }
            catch
            {
                message = new MessageWindow(this, "Error", "Unable to save to settings file", state);
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
            }
            catch
            {
                SoundEffect.MasterVolume = 0.8f;
                ResetTheme(Theme.Original, false);
                cwSettings = CWSettings.Untested;
                xeniaFullscreen = false;
                logLevel = LogLevel.Info;
                consolidateFiles = true;
                runHeadless = false;
                checkDrivesOnManage = true;
            }
        }
        /// <summary>
        /// Coverts a data size to a larger unit of storage measurement in needed, and adds a suffix to the end of the string
        /// </summary>
        public string ConvertDataSize(string size)
        {
            double num = Convert.ToDouble(size);
            // Bytes
            if (num < 1024)
            {
                return "" + num + " B";
            }
            // Kilobytes
            else if (num < (1024 * 1024))
            {
                return "" + Math.Round((num / 1024), 3 - ("" + (int)(num / 1024)).Length) + " KB";
            }
            // Megabytes
            else if (num < (1024 * 1024 * 1024))
            {
                return "" + Math.Round((num / 1024 / 1024), 3 - ("" + (int)(num / 1024 / 1024)).Length) + " MB";
            }
            // Gigabytes
            else if (num < Math.Pow(1024, 4))
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024), Math.Max(0, 3 - ("" + (int)(num / 1024 / 1024 / 1024)).Length)) + " GB";
            }
            // Terabytes
            else if (num < Math.Pow(1024, 5))
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024 / 1024), Math.Max(0, 3 - ("" + (int)(num / 1024 / 1024 / 1024 / 1024)).Length)) + " TB";
            }
            return size;
        }

        /// <summary>
        /// Resets the theme, used for changing to a new theme
        /// </summary>
        /// <param name="newTheme">The new theme to change to</param>
        /// <param name="forceWindowReset">If true, forces all open Window to reconstruct (This resets and applies the theme to the Window)</param>
        public void ResetTheme(Theme newTheme, bool forceWindowReset)
        {
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
            }

            mainFadeGradient = new Gradient(Color.White, 20);
            mainFadeGradient.colors.Add(Color.Gray);
            mainFadeGradient.ValueUpdate(0);
            darkGradient = new Gradient(GetTransparentColor(backColor), 20);
            blackGradient = new Gradient(GetTransparentColor(backColor), 20);
            buttonGradient = new Gradient(GetTransparentColor(backColor), 20);
            selectGradient = new Gradient(GetTransparentColor(backColor), 20);
            if (bottomInfo != null && !firstLoad)
            {
                bottomInfo.displayGradient.colors[0] = backColor;
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
            darkGradient.ValueUpdate(0);
            blackGradient.ValueUpdate(0);
            buttonGradient.ValueUpdate(0);
            selectGradient.ValueUpdate(0);
            whiteGradient = new Gradient(GetTransparentColor(backColor), 20);
            whiteGradient.colors.Add(Color.FromNonPremultiplied(255, 255, 255, 255));
            whiteGradient.ValueUpdate(0);

            if (forceWindowReset)
            {
                menuWindow.ResetGradients();
                menuWindow.whiteGradient.Update();
                optionsWindow.ResetGradients();
                optionsWindow.whiteGradient.Update();
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
            List<int> indexes = new List<int>();
            foreach (XGame game in gameIcons)
            {
                indexes.Add(game.index);
            }
            for (int i = 0; i < indexes.Count; i++)
            {
                gameIcons[i].index = indexes[i];
                if (!firstLoad && right && !firstReset)
                {
                    gameIcons[i].index++;
                }
                else if (!firstLoad && !right && !firstReset)
                {
                    gameIcons[i].index--;
                }
                gameIcons[i].AdjustIndex(gameData.Count);
            }
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
                ring.fade.colors.Add(darkGradient.colors[0]);
                ring.fade.ValueUpdate(0);
            }
        }
        /// <summary>
        /// Used to calculate total drives and size
        /// </summary>
        public void SetDriveSpaceText()
        {
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
                catch { }
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
        public string GetFilepathString(string path)
        {
            return GetFilepathString(path, false);
        }
        public string GetFilepathString(string path, bool removeBackslash)
        {
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
            return param;
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
                    File.Copy(xeniaPath, "XData\\Xenia\\" + newTitle + "\\xenia.exe");
                }
                catch { }
                try
                {
                    File.Create("XData\\Xenia\\" + newTitle + "\\portable.txt");
                }
                catch { }
                startInfo.FileName = "XData\\Xenia\\" + newTitle + "\\xenia.exe";
            }
            else
            {
                startInfo.FileName = xeniaPath;
            }
            Process.Start(startInfo);

            OpenCompatWindow(false, compatWindowDelay);
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
                    File.Copy(canaryPath, "XData\\Canary\\" + newTitle + "\\xenia_canary.exe");
                }
                catch { }
                try
                {
                    File.Create("XData\\Canary\\" + newTitle + "\\portable.txt");
                }
                catch { }
                startInfo.FileName = "XData\\Canary\\" + newTitle + "\\xenia_canary.exe";
            }
            else
            {
                startInfo.FileName = canaryPath;
            }
            Process.Start(startInfo);

            OpenCompatWindow(true, compatWindowDelay);
        }
        public void LaunchCanary()
        {
            LaunchCanary(gameData[index].gamePath);
        }
        public void DefaultQuickstart(string path)
        {
            launchSound.Play();
            if (gameData[index].preferCanary)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Arguments = "\"" + path + "\"";
                string title = gameData[index].gameTitle;
                string newTitle = GetFilepathString(title);
                startInfo.WorkingDirectory = "XData\\Canary\\" + newTitle;
                Directory.CreateDirectory("XData\\Canary\\" + newTitle);
                if (consolidateFiles)
                {
                    try
                    {
                        File.Copy(canaryPath, "XData\\Canary\\" + newTitle + "\\xenia_canary.exe");
                    }
                    catch { }
                    try
                    {
                        File.Create("XData\\Canary\\" + newTitle + "\\portable.txt");
                    }
                    catch { }
                    startInfo.FileName = "XData\\Canary\\" + newTitle + "\\xenia_canary.exe";
                }
                else
                {
                    startInfo.FileName = canaryPath;
                }
                Process.Start(startInfo);
                OpenCompatWindow(true, compatWindowDelay);
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Arguments = "\"" + path + "\"";
                string title = gameData[index].gameTitle;
                string newTitle = GetFilepathString(title);
                startInfo.WorkingDirectory = "XData\\Xenia\\" + newTitle;
                Directory.CreateDirectory("XData\\Xenia\\" + newTitle);
                if (consolidateFiles)
                {
                    try
                    {
                        File.Copy(xeniaPath, "XData\\Xenia\\" + newTitle + "\\xenia.exe");
                    }
                    catch { }
                    try
                    {
                        File.Create("XData\\Xenia\\" + newTitle + "\\portable.txt");
                    }
                    catch { }
                    startInfo.FileName = "XData\\Xenia\\" + newTitle + "\\xenia.exe";
                }
                else
                {
                    startInfo.FileName = xeniaPath;
                }
                Process.Start(startInfo);

                OpenCompatWindow(false, compatWindowDelay);
            }
        }
        public void DefaultQuickstart()
        {
            DefaultQuickstart(gameData[index].gamePath);
        }
        public void EditGame()
        {
            gameManageWindow = new Window(this, new Rectangle(560, 170, 800, 750), "Manage " + gameData[index].gameTitle, new ManageGame(), new StdInputEvent(5), new GenericStart(), State.NewGame);
            gameManageWindow.buttonEffects.SetupEffects(this, gameManageWindow);
            state = State.GameMenu;
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
                compatWindow.AddButton(new Rectangle(410, 530, 545, 100));
                compatWindow.AddButton(new Rectangle(965, 530, 545, 100));
                compatWindow.AddButton(new Rectangle(410, 640, 545, 100));
                compatWindow.AddButton(new Rectangle(965, 640, 545, 100));
                compatWindow.AddButton(new Rectangle(410, 750, 545, 100));
                compatWindow.AddButton(new Rectangle(965, 750, 545, 100));
                compatWindow.AddButton(new Rectangle(410, 860, 545, 100));
                compatWindow.AddButton(new Rectangle(965, 860, 545, 100));
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

        public void SaveGames()
        {
            try
            {
                SaveData save = new SaveData(configPath);
                save.AddSaveObject(new SaveDataObject("xenia", xeniaPath, SaveData.DataType.String));
                save.AddSaveObject(new SaveDataObject("canary", canaryPath, SaveData.DataType.String));
                SaveDataChunk chunk = new SaveDataChunk("games");
                masterData = masterData.OrderBy(o => o.gameTitle).ToList();
                foreach (GameData game in masterData)
                {
                    chunk.AddChunk(game.Save());
                }
                save.AddSaveChunk(chunk);
                save.SaveToFile();
            }
            catch (Exception e)
            {
                message = new MessageWindow(this, "Unable to Save", e.ToString().Split("\n")[0], State.Menu);
                state = State.Message;
            }
        }

        public void LoadArts()
        {
            arts = new Dictionary<string, Texture2D>();
            icons = new Dictionary<string, Texture2D>();
            foreach (GameData data in gameData)
            {
                if (File.Exists(data.artPath))
                {
                    try
                    {
                        arts.Add(data.gameTitle, Texture2D.FromFile(GraphicsDevice, data.artPath));
                    }
                    catch
                    {
                        arts.Add(data.gameTitle, white);
                    }
                }
                else
                {
                    arts.Add(data.gameTitle, white);
                }
                if (File.Exists(data.iconPath))
                {
                    try
                    {
                        icons.Add(data.gameTitle, Texture2D.FromFile(GraphicsDevice, data.iconPath));
                    }
                    catch
                    {
                        icons.Add(data.gameTitle, white);
                    }
                }
                else
                {
                    icons.Add(data.gameTitle, white);
                }
            }
            icons.Add("Continuum Launcher", mainLogo);
            icons.Add("Continuum Companion", compLogo);
            icons.Add("Xenia Content and Temporary Data", mainLogo);
            icons.Add("Artwork and Icons", mainLogo);
            icons.Add("All Games and Data", mainLogo);
            icons.Add("All Launcher Data", mainLogo);
        }

        public void SetResolution(Vector2 resolution)
        {
            _graphics.PreferredBackBufferWidth = (int)resolution.X;
            _graphics.PreferredBackBufferHeight = (int)resolution.Y;
            _graphics.ApplyChanges();
            Ozzz.scale = resolution / new Vector2(1920, 1080);
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
            return _graphics.IsFullScreen;
        }
        public bool GetFullscreen()
        {
            return _graphics.IsFullScreen;
        }
        public bool ToggleVSync()
        {
            _graphics.SynchronizeWithVerticalRetrace = !_graphics.SynchronizeWithVerticalRetrace;
            return _graphics.SynchronizeWithVerticalRetrace;
        }
        public bool GetVSync()
        {
            return _graphics.SynchronizeWithVerticalRetrace;
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
            }
            if (!Directory.Exists("Apps\\Xenia"))
            {
                Directory.CreateDirectory("Apps\\Xenia");
            }
            if (!Directory.Exists("Apps\\Canary"))
            {
                Directory.CreateDirectory("Apps\\Canary");
            }
            if (!Directory.Exists("Apps\\Dump"))
            {
                Directory.CreateDirectory("Apps\\Dump");
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

            LoadArts();

            // Loading trivia
            if (File.Exists("Content\\Trivia.txt"))
            {
                StreamReader triviaFile = new StreamReader("Content\\Trivia.txt");
                while (!triviaFile.EndOfStream)
                {
                    trivia.Add(triviaFile.ReadLine());
                }
            }

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

            xeniaCompatLogo = new ObjectSprite(logo, new Rectangle(220, 400, 125, 125));
            canaryCompatLogo = new ObjectSprite(logoCanary, new Rectangle(530, 400, 125, 125));
            xeniaCompat = new ObjectSprite(compatBars["nothing"], new Rectangle(204, 535, 156, 19));
            canaryCompat = new ObjectSprite(compatBars["nothing"], new Rectangle(514, 535, 156, 19));
            xeniaUntestedText = new TextSprite(font, "Untested", 0.4f, new Vector2(222, 525), Color.FromNonPremultiplied(0, 0, 0, 0));
            canaryUntestedText = new TextSprite(font, "Untested", 0.4f, new Vector2(532, 525), Color.FromNonPremultiplied(0, 0, 0, 0));

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
            bottomInfo = new SequenceFade(infoLayer, Color.FromNonPremultiplied(255, 255, 255, 0), Color.White, 300, 150);
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

            ReadConfig();
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

            GamepadInput.Update();
            MouseInput.Update();
            KeyboardInput.Update();
            if (GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Back, true) || GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.B, true) || KeyboardInput.keys["Escape"].IsFirstDown() || MouseInput.IsRightFirstDown() || KeyboardInput.keys["Backspace"].IsFirstDown())
            {
                if (state == State.Main && IsActive)
                {
                    state = State.Menu;
                    menuWindow = new Window(this, new Rectangle(560, 115, 800, 860), "Menu", new Menu(), new StdInputEvent(6), new GenericStart(), State.Main);
                    menuWindow.AddButton(new Rectangle(610, 265, 700, 100));
                    menuWindow.AddButton(new Rectangle(610, 375, 700, 100));
                    menuWindow.AddButton(new Rectangle(610, 485, 700, 100));
                    menuWindow.AddButton(new Rectangle(610, 595, 700, 100));
                    menuWindow.AddButton(new Rectangle(610, 705, 700, 100));
                    menuWindow.AddButton(new Rectangle(610, 815, 700, 100));
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
                        }
                    }
                }
            }
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.A, true) || KeyboardInput.keys["Enter"].IsFirstDown() || KeyboardInput.keys["Space"].IsFirstDown() || (gameIcons[2].CheckMouse(true) && MouseInput.IsLeftFirstDown())) && state == State.Main && IsActive)
            {
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
                    gameData = gameData.OrderByDescending(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.ZA)
                {
                    sort = Sort.Date;
                    gameData = gameData.OrderBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Date)
                {
                    sort = Sort.Dev;
                    gameData = gameData.OrderBy(o => o.developer).ThenBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Dev)
                {
                    sort = Sort.Pub;
                    gameData = gameData.OrderBy(o => o.publisher).ThenBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Pub)
                {
                    sort = Sort.AZ;
                    gameData = gameData.OrderBy(o => o.gameTitle).ToList();
                }
                firstReset = true;
                ResetGameIcons();
                sortSound.Play();
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
                string fileTitle = GetFilepathString(masterData[selectedDataIndex].gameTitle);
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
                }
                else if (toDelete.name.Contains("Save Data (Canary)"))
                {
                    string dir = "XData\\Canary\\" + fileTitle + "\\content\\" + masterData[selectedDataIndex].titleId + "\\profile";
                    if (Directory.Exists(dir))
                    {
                        foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                        {
                            File.Delete(filepath);
                        }
                        Directory.Delete(dir, true);
                    }
                }
                else if (toDelete.name.Contains("Installed DLC"))
                {
                    string dir = "XData\\Canary\\" + masterData[selectedDataIndex].gameTitle + "\\content\\" + masterData[selectedDataIndex].titleId + "\\00000002";
                    dir = GetFilepathString(dir, true);
                    if (Directory.Exists(dir))
                    {
                        foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                        {
                            File.Delete(filepath);
                        }
                        Directory.Delete(dir, true);
                    }
                }
                else if (toDelete.name.Contains("Installed Title Update"))
                {
                    string dir = "XData\\Canary\\" + masterData[selectedDataIndex].gameTitle + "\\content\\" + masterData[selectedDataIndex].titleId + "\\000B0000";
                    dir = GetFilepathString(dir, true);
                    if (Directory.Exists(dir))
                    {
                        foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                        {
                            File.Delete(filepath);
                        }
                        Directory.Delete(dir, true);
                    }
                }
                message = new MessageWindow(this, "File Deleted", "The file was successfully deleted", State.Data);
                state = State.Message;
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
                }
                else
                {
                    message = new MessageWindow(this, "Error", "Import filepath does not exist", State.Manage);
                    state = State.Message;
                }
                messageYes = false;
                toImport = null;
            }

            // Updating windows
            if (state == State.Menu)
            {
                menuWindow.Update();
            }
            else if (state == State.GameMenu)
            {
                gameManageWindow.Update();
            }
            else if (state == State.GameXeniaSettings)
            {
                gameXeniaSettingsWindow.Update();
            }
            else if (state == State.GameInfo)
            {
                gameInfoWindow.Update();
                if (textWindowInput != null)
                {
                    if (gameInfoWindow.buttonIndex == 0 && gameData[index].gameTitle != textWindowInput)
                    {
                        if (arts.ContainsKey(gameData[index].gameTitle))
                        {
                            arts.Add(textWindowInput, arts[gameData[index].gameTitle]);
                            arts.Remove(gameData[index].gameTitle);
                        }
                        else
                        {
                            arts.Add(textWindowInput, white);
                        }
                        gameData[index].gameTitle = textWindowInput;
                        gameInfoWindow.titleSprite.text = "Info for " + textWindowInput;
                        gameManageWindow.titleSprite.text = "Manage " + textWindowInput;
                        SaveGames();
                        textWindowInput = null;
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
                        string[] dateString = textWindowInput.Split("-");
                        try
                        {
                            DateOnly date = new DateOnly(Convert.ToInt32(dateString[0]), Convert.ToInt32(dateString[1]), Convert.ToInt32(dateString[2]));
                            gameData[index].year = date.Year;
                            gameData[index].month = date.Month;
                            gameData[index].day = date.Day;
                            SaveGames();
                        }
                        catch
                        {
                            message = new MessageWindow(this, "Invalid String", "Invalid Date", State.GameInfo);
                            state = State.Message;
                        }
                        textWindowInput = null;
                    }
                }
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
                            message = new MessageWindow(this, "Error", "Invalid image path", State.GameFilepaths);
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
                        }
                        else
                        {
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
                        }
                        else
                        {
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
                        foreach (GameData game in masterData)
                        {
                            game.folders.Remove(folders[tempCategoryIndex]);
                        }
                        folders.Remove(folders[tempCategoryIndex]);
                        SaveGames();
                        FolderReset();
                        Initialize();
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
                        textWindowInput = null;
                    }
                    else if (gameXEXWindow.buttonIndex == 4)
                    {
                        gameData[index].xexPaths[tempCategoryIndex] = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                }
                if (messageYes)
                {
                    if (gameXEXWindow.buttonIndex == 5)
                    {
                        if (tempCategoryIndex == -1)
                        {
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
                            Initialize();
                            LoadArts();
                            index = 0;
                            BeginMainTransition();
                            FolderReset();
                            state = State.Main;
                        }
                        else
                        {
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
                newGameWindow.Update();

                if (newGameProcess)
                {
                    newGameProcess = false;
                    state = State.Main;
                    Initialize();
                    FolderReset();
                }
                if (textWindowInput != null)
                {
                    if (newGameWindow.buttonIndex == 0)
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
                    else if (newGameWindow.buttonIndex == 1)
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
                            message = new MessageWindow(this, "Error", "Invalid STFS/SVOD file\n" + e.ToString(), State.NewGame);
                            state = State.Message;
                        }
                    }
                }
                if (messageYes)
                {
                    if (newGameWindow.buttonIndex == 1)
                    {
                        // Saving icon
                        if (!Directory.Exists("IconData"))
                        {
                            Directory.CreateDirectory("IconData");
                        }
                        tempIconSTFS.Save("IconData\\" + tempIdSTFS + ".png");
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
                        }
                        // Adding game to masterData
                        masterData.Last().gameTitle = tempTitleSTFS;
                        masterData.Last().gamePath = tempFilepathSTFS;
                        masterData.Last().titleId = tempIdSTFS;
                        masterData.Last().iconPath = "IconData\\" + tempIdSTFS + ".png";
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
                dataWindow.Update();
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
            else if (state == State.Credits)
            {
                creditsWindow.Update();
                if (KeyboardInput.keys["V"].IsFirstDown())
                {
                    message = new MessageWindow(this, "Version Info", "Continuum Launcher version " + ver + ", compiled " + compileDate, State.Credits);
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
            subTitleSprite.text = gameData[index].developer + " - " + gameData[index].year;
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
                }
            }
            else
            {
                folderPath.Update();
            }
            xeniaUntestedText.color = whiteGradient.GetColor();
            canaryUntestedText.color = whiteGradient.GetColor();

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
            topBorder.color = Color.White;

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
                if (state == State.Select || state == State.Launch)
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
            int connectedControllers = 0;
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                GamePadState state = GamePad.GetState(i);
                if (state.IsConnected)
                {
                    connectedControllers++;
                }
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
            }

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
                gameInfoWindow = null;
                gameFilepathsWindow = null;
                gameCategoriesWindow = null;
                gameXEXWindow = null;
            }
            else if (state == State.Menu)
            {
                optionsWindow = null;
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
            backBorderLayer.Draw(_spriteBatch);
            triviaSprite.Draw(_spriteBatch);
            
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
            timeText.Draw(_spriteBatch);
            bottomLayer.Draw(_spriteBatch);
            bottomInfo.Draw(_spriteBatch);
            // Select menu
            _spriteBatch.Draw(white, new Rectangle(0, 0, Ozzz.Scaling.ScaleIntX(1920), Ozzz.Scaling.ScaleIntY(1080)), darkGradient.GetColor());
            if ((state == State.Select || state == State.Launch || state == State.Compat || state == State.GameMenu || state == State.GameInfo || state == State.GameFilepaths || state == State.GameXeniaSettings || state == State.GameCategories || state == State.GameXEX) && showRings)
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
            _spriteBatch.DrawString(font, "Developer: " + gameData[index].developer, Ozzz.Scaling.ScaleVector2(new Vector2(200, 200)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "Publisher: " + gameData[index].publisher, Ozzz.Scaling.ScaleVector2(new Vector2(200, 240)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(calendar, Ozzz.Scaling.RectangleScaled(200, 290, 40, 40), whiteGradient.GetColor());
            _spriteBatch.DrawString(font, "" + gameData[index].month + "/" + gameData[index].day + "/" + gameData[index].year, Ozzz.Scaling.ScaleVector2(new Vector2(250, 290)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            //_spriteBatch.DrawString(font, "Title ID: " + gameData[index].titleId, Ozzz.Scaling.ScaleVector2(new Vector2(200, 320)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(player, Ozzz.Scaling.RectangleScaled(200, 340, 40, 40), whiteGradient.GetColor());
            _spriteBatch.DrawString(font, "" + gameData[index].minPlayers + "-" + gameData[index].maxPlayers, Ozzz.Scaling.ScaleVector2(new Vector2(250, 340)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(0.375f), SpriteEffects.None, 0f);
            _spriteBatch.DrawString(bold, gameData[index].gameTitle, Ozzz.Scaling.ScaleVector2(new Vector2(30, 25)), whiteGradient.GetColor(), 0f, Vector2.Zero, Ozzz.Scaling.ScaleFloatX(1f), SpriteEffects.None, 0f);
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
            if (gameInfoWindow != null)
            {
                gameInfoWindow.Draw(_spriteBatch);
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
            if (manageWindow != null)
            {
                manageWindow.Draw(_spriteBatch);
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