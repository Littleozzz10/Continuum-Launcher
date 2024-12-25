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

        public void CheckForceReInit()
        {
            if (forceInit && state != State.GameMenu && state != State.GameXeniaSettings && state != State.GameInfo && state != State.GameFilepaths && state != State.GameCategories && state != State.GameXEX && state != State.Message && state != State.Text)
            {
                FolderReset();
                folderIndex = folders.IndexOf("All Games");
                Initialize();
                LoadArts();
            }
        }

        public void CheckUserResearchPrompt()
        {
            if (showResearchPrompt)
            {
                message = new MessageWindow(this, "User Research Note", "This is a User Research build of Continuum Launcher. As such, features may be in an early state and may not represent the final version.", State.Main);
                state = State.Message;
                showResearchPrompt = false;
                Logging.Write(LogType.Standard, Event.ResearchPrompt, "User Research prompt shown");
            }
        }

        public void FullscreenTickUpdate()
        {
            if (fullscreenDelay > 0)
            {
                fullscreenDelay--;
                if (fullscreenDelay == 0)
                {
                    _graphics.IsFullScreen = true;
                    _graphics.ApplyChanges();
                }
            }
        }

        public void CheckMissingConfigMessage()
        {
            if (triggerMissingWindow)
            {
                message = new MessageWindow(this, "Note", "Settings file missing or corrupted; Default created", State.Main);
                state = State.Message;
                triggerMissingWindow = false;
            }
        }

        public void CheckJumpToCooldown()
        {
            if (jumpTriggerCooldown > 0)
            {
                jumpTriggerCooldown--;
            }
            if (GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.RightTrigger) <= 0.65f && GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftTrigger) <= 0.65f)
            {
                jumpTriggerCooldown = 0;
            }
        }

        public void UpdateGameIcons()
        {
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
        }

        public void UpdateXeniaUntestedColor()
        {
            Color altColor = Ozzz.Helper.NewColorAlpha(fontAltColor, (int)whiteGradient.values[3]);
            xeniaUntestedText.color = altColor;
            canaryUntestedText.color = altColor;
        }

        public void UpdateObjectSprites()
        {
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
        }

        public void UpdateBackgroundRings()
        {
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
        }

        public void UpdateTutorial()
        {
            if (tutorial != null)
            {
                tutorial.Update();
            }
        }

        public void CheckWelcome()
        {
            if (!welcomeShown)
            {
                welcomeWindow = new Window(this, new Rectangle(50, 50, 1820, 980), "Welcome to Continuum Launcher!", "You've just been updated. Here's what's new.", new WelcomeEffects(), new StdInputEvent(2), new GenericStart(), State.Main, true);
                welcomeWindow.AddButton(new Rectangle(310, 900, 600, 100));
                welcomeWindow.AddButton(new Rectangle(1010, 900, 600, 100));
                welcomeWindow.AddText("Let's Go!");
                welcomeWindow.AddText("View Tutorials");
                welcomeWindow.extraSprites.Add(new ObjectSprite(mainLogo, new Rectangle(300, 360, 300, 300)));
                // Updated window
                if (configVernum > 0 && configVernum < Shared.VERNUM)
                {
                    TextSprite header = new TextSprite(font, "Continuum just got better. The all-new 1.2 version brings\nmany new features, including the following:", 0.5f, new Vector2(760, 260), Color.White);
                    header.splitDraw = true;
                    welcomeWindow.extraSprites.Add(header);

                    TextSprite notes = new TextSprite(font,
                        "- New game database for matching games with their information\n" +
                        "- Interactive Tutorials and tooltips to guide you through Continuum\n" +
                        "- New import system to make importing games even easier\n" +
                        "- Overhauled and improved Data Management\n" +
                        "- Custom Theme support (Advanced feature)\n" +
                        "- Dozens of bug fixes and changes from v1.1\n" +
                        "... And even more!", 0.4f, new Vector2(860, 370), Color.White);
                    notes.splitDraw = true;
                    welcomeWindow.extraSprites.Add(notes);

                    TextSprite footnote = new TextSprite(font, "View complete patch notes and a full usage guide on GitHub.\nUntil then, it's time to Jump In and play some games!\n                                                                                - Littleozzz10", 0.5f, new Vector2(760, 660), Color.White);
                    footnote.splitDraw = true;
                    welcomeWindow.extraSprites.Add(footnote);
                }
                else if (configVernum <= 0)
                {
                    TextSprite header = new TextSprite(font, "This is Continuum Launcher, an application for managing and\nplaying your Xbox 360 library with Xenia. If it's your first time\nhere, check out the Tutorials menu or the GitHub guide to get\nstarted.", 0.5f, new Vector2(760, 260), Color.White);
                    header.splitDraw = true;
                    welcomeWindow.extraSprites.Add(header);

                    TextSprite notes = new TextSprite(font,
                        "Note: Continuum is not designed to enable piracy, and piracy is not condoned.\n" +
                        "Additionally, Continuum is free, open-source software. If you paid for any part\n" +
                        "of this software, get a refund immedietely and report the seller.", 0.4f, new Vector2(760, 500), Color.White);
                    notes.splitDraw = true;
                    welcomeWindow.extraSprites.Add(notes);

                    TextSprite footnote = new TextSprite(font, "Continuum is in active development, so please report any bugs\non GitHub. Now, it's time to Jump In and play some games!\n                                                                                - Littleozzz10", 0.5f, new Vector2(760, 660), Color.White);
                    footnote.splitDraw = true;
                    welcomeWindow.extraSprites.Add(footnote);
                    welcomeWindow.descSprite.text = "The Xenia launcher we've all been waiting for.";
                }
                // First run window
                welcomeWindow.skipMainStateTransition = true;
                state = State.Welcome;
                welcomeShown = true;
                SaveGames();
            }
        }
    }
}
