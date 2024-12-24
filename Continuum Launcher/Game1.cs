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
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D white, rectTex, logo, circ, calendar, player, logoCanary, mainLogo, topBorderTex, bottomBorderTex, compLogo, topBorderNew, bottomBorderNew, customThemeImage;
        public SpriteFont font, bold;
        public SoundEffect selectSound, backSound, launchSound, switchSound, buttonSwitchSound, sortSound, leftFolderSound, rightFolderSound;
        public ObjectSprite xeniaCompatLogo, canaryCompatLogo, xeniaCompat, canaryCompat, topBorder, bottomBorder, topBorderBack, bottomBorderBack, jumpFade, tutorialFade;
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
        public Window xexWindow, launchWindow, menuWindow, optionsWindow, graphicsWindow, compatWindow, settingsWindow, creditsWindow, dataWindow, manageWindow, deleteWindow, gameManageWindow, gameXeniaSettingsWindow, gameFilepathsWindow, gameInfoWindow, gameCategoriesWindow, gameXEXWindow, newGameWindow, databaseResultWindow, releaseWindow, databasePickerWindow, fileManageWindow, metadataWindow, dataSortWindow, dataFilterWindow, tutorialWindow, welcomeWindow;
        public MessageWindow message;
        public TextInputWindow text;
        public Color backColor, backColorAlt, fontColor, fontSelectColor, fontAltColor, fontAltLightColor, majorFontColor, sortColor, folderColor, timeDateColor, cornerStatsColor, triviaColor, topBorderColor, bottomBorderColor, ringMainColor, ringSelectColor, descColor, dataTitleColor;
        public SaveData configData;
        public DataManageStrings dataStrings;
        public SequenceFade bottomInfo;
        public DataEntry toDelete, toImport, toExtract;
        public Tutorial tutorial;
        public MobyData mobyData;
        public List<GameInfo> databaseGameInfo;
        public System.Drawing.Image tempIconSTFS;
        public string xeniaPath, canaryPath, configPath, ver, compileDate, textWindowInput, newXEX, tempTitleSTFS, tempIdSTFS, tempFilepathSTFS, extractPath, newGamePath, tempGameTitle;
        public int index, ringFrames, ringDuration, folderIndex, compatWaitFrames, selectedDataIndex, compatWindowDelay, fullscreenDelay, tempCategoryIndex, databaseResultIndex, tempYear, tempMonth, tempDay, jumpLayerAlpha, jumpTriggerCooldown, jumpTriggerCooldownDefault, configVernum, kinectGameAdded;
        public bool right, firstLoad, firstReset, skipDraw, showRings, xeniaFullscreen, consolidateFiles, runHeadless, triggerMissingWindow, updateFreeSpace, messageYes, militaryTime, inverseDate, checkDrivesOnManage, lastActiveCheck, forceInit, newGameProcess, enableExp, hideSecretMetadata, refreshData, showResearchPrompt, windowClickExit, enterCloseTextInput, rightClickGames, tutorialLock, tutorialExitPrompt, welcomeShown, resort;
        public enum State
        {
            None, Any, Main, Select, Launch, Menu, Options, Credits, Graphics, Settings, Compat, Message, Data, Manage, Delete, GameMenu, GameXeniaSettings, GameFilepaths, GameInfo, GameCategories, GameXEX, Text, NewGame, DatabaseResult, ReleaseYear, ReleaseMonth, ReleaseDay, DatabasePicker, ManageFile, Metadata, DataSort, DataFilter, TutorialSelect, Welcome
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
            _graphics.PreferredBackBufferWidth = 1536;
            _graphics.PreferredBackBufferHeight = 864;
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

            InitializeOzzzFrameworkSystems();

            InitializeDataLists();

            InitializeInput();

            ReadAndInitializeConfig();

            SetDefaultSettings();

            // Settings file
            ReadAndInitializeSettings();

            InitializeGameFolders();

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
        /// Loads all of the Launcher's content (Textures, Fonts, data, etc)
        /// </summary>
        protected override void LoadContent()
        {
            // Creating Content directories if needed
            CheckAndCreateContentDirectories();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loading assets
            LoadInternalAssets();

            // Loading cover art
            LoadArts();

            // Loading trivia
            LoadTrivia();

            // Creating game icon Sprites
            CreateGameIcons();

            // Creating compatibility window Sprites
            CreateCompatibilityWindowSprites();

            // Creating dashboard elements
            CreateDashboardElements();

            // Loading database data
            LoadMobyData();

            Logging.Write(LogType.Critical, Event.ContentLoad, "Content Load complete");
        }

        /// <summary>
        /// This Update() method is called once every frame
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Checking if some other action requires Continuum to reinitialize
            CheckForceReInit();

            // User Research prompt
            CheckUserResearchPrompt();

            GamepadInput.Update();
            MouseInput.Update();
            KeyboardInput.Update();

            // Checks to see if the user has opened the Menu while on the main Dashboard
            CheckMenuOpened();

            // Ticks forward the fullscreen delay, if needed
            FullscreenTickUpdate();

            // Missing/Corrupt Settings file notification
            CheckMissingConfigMessage();

            // Jump trigger cooldown
            CheckJumpToCooldown();

            // Moving game icons
            CheckSelectedGameChange();

            // Game selection
            CheckGameSelected();

            // hahaha
            CheckDedicationMessageOpen();

            // Switching folders
            CheckFolderSwitch();

            // Sorting
            CheckSortChangeAndCoverChange();

            // Updating game icons
            UpdateGameIcons();

            // Check if the user has taken an action that requires deleting, importing, or extracting files
            CheckFileDeletionImportExtraction();

            // --- Updating windows ---

            UpdateMenu();
            UpdateGameMenu();
            UpdateGameXeniaSettings();
            UpdateDatabasePicker();
            UpdateDatabaseResult();
            UpdateGameInfo();
            UpdateReleaseWindow();
            UpdateGameFilepaths();
            UpdateGameCategories();
            UpdateGameXEX();
            UpdateOptionsWindow();
            UpdateGraphicsWindow();
            UpdateSettingsWindow();
            UpdateNewGame();
            UpdateDataWindow();
            UpdateDataSort();
            UpdateCompatWindow();
            UpdateManageWindow();
            UpdateManageFileWindow();
            UpdateMetadataWindow();
            UpdateTutorialSelect();
            UpdateCredits();
            UpdateWelcome();

            TutorialExitCheck();

            UpdateMessageWindow();
            UpdateTextInputWindow();

            // Updating text
            UpdateTitleText();

            // Sort text
            UpdateSortText();

            // Folder text animation
            UpdateFolderTextAnimation();

            // Compat window stuff
            UpdateXeniaUntestedColor();

            // Updating fades
            UpdateFades();

            // Updating Jump Layer
            UpdateJumpLayer();

            // Updating ObjectSprites
            UpdateObjectSprites();

            // Updating rings
            UpdateBackgroundRings();

            // Updating corner text
            UpdateCornerText();

            // Updating tutorial fade
            UpdateTutorialFade();

            // Date and Time display
            UpdateDateTimeDisplay();

            // Controllers
            UpdateControllerDisplay();

            // Other corner stuff
            FinishCornerUpdate();

            // Updating trivia
            UpdateTrivia();

            // Updating tutorial
            UpdateTutorial();

            // Unloading windows
            UnloadWindows();

            CheckWelcome();

            firstLoad = false;
            lastActiveCheck = IsActive;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backColor);

            _spriteBatch.Begin();

            // Drawing first Ring layer
            if (showRings)
            {
                DrawRings();
            }

            DrawGames();

            DrawDashboardElements();

            // Select menu
            DrawSelectMenuLayer();

            // Other windows
            DrawAdditionalWindows();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}