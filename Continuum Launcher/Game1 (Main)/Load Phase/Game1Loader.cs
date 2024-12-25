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

        public void LoadInternalAssets()
        {
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
            if (customThemeImage == null)
            {
                customThemeImage = white;
            }
            themeThumbnails.Add("custom", customThemeImage);
            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Internal assets loaded");
        }

        public void LoadLanguages()
        {
            languages = new List<LanguageStrings>();
            languageIndex = 0;

            // Loading language
            string index = Ozzz.Helper.IntToString(languageIndex + 1, 2);
            string dir = "Content\\Language\\" + index + "\\";
            languages.Add(new LanguageStrings());
            if (Directory.Exists(dir))
            {
                for (int i = 1; File.Exists(dir + "str" + Ozzz.Helper.IntToString(i, 4) + ".clf"); i++)
                {
                    languages[0].ImportFile(dir + "str" + Ozzz.Helper.IntToString(i, 4) + ".clf");
                }
            }
        }

        public void LoadTrivia()
        {
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
        }

        public void CreateGameIcons()
        {
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
        }

        public void CreateCompatibilityWindowSprites()
        {
            xeniaCompatLogo = new ObjectSprite(logo, new Rectangle(220, 400, 125, 125));
            canaryCompatLogo = new ObjectSprite(logoCanary, new Rectangle(530, 400, 125, 125));
            xeniaCompat = new ObjectSprite(compatBars["nothing"], new Rectangle(204, 535, 156, 19));
            canaryCompat = new ObjectSprite(compatBars["nothing"], new Rectangle(514, 535, 156, 19));
            xeniaUntestedText = new TextSprite(font, "Untested", 0.4f, new Vector2(222, 525), Color.FromNonPremultiplied(0, 0, 0, 0));
            canaryUntestedText = new TextSprite(font, "Untested", 0.4f, new Vector2(532, 525), Color.FromNonPremultiplied(0, 0, 0, 0));
            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Compat window elements initialized");
        }

        public void CreateDashboardElements()
        {
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

            tutorialFade = new ObjectSprite(white, new Rectangle(0, 0, 1920, 1080), Color.FromNonPremultiplied(0, 0, 0, 0));

            Logging.Write(LogType.Critical, Event.ContentLoadEvent, "Dashboard elements initialized");
        }
    }
}
