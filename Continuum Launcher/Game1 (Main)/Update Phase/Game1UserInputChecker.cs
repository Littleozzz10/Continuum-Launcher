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
        public void CheckMenuOpened()
        {
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Back, true) || GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Start, true) || GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.B, true) || KeyboardInput.keys["Escape"].IsFirstDown() || MouseInput.IsRightFirstDown() || KeyboardInput.keys["Backspace"].IsFirstDown()) && !tutorialLock)
            {
                if (state == State.Main && IsActive)
                {
                    state = State.Menu;
                    menuWindow = new Window(this, new Rectangle(560, 60, 800, 970), languages[0].GetText("menu", "title"), new Menu(), new StdInputEvent(7), new GenericStart(), State.Main);
                    menuWindow.AddButton(new Rectangle(610, 210, 700, 100), languages[0].GetText("menu", "returnDesc"), DBSpawnPos.CenterLeftBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 320, 700, 100), languages[0].GetText("menu", "addDesc"), DBSpawnPos.CenterRightBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 430, 700, 100), languages[0].GetText("menu", "optionsDesc"), DBSpawnPos.CenterLeftBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 540, 700, 100), languages[0].GetText("menu", "manageDesc"), DBSpawnPos.CenterRightBottom, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 650, 700, 100), languages[0].GetText("menu", "tutorialsDesc"), DBSpawnPos.CenterLeftTop, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 760, 700, 100), languages[0].GetText("menu", "aboutDesc"), DBSpawnPos.CenterRightTop, 0.4f);
                    menuWindow.AddButton(new Rectangle(610, 870, 700, 100), languages[0].GetText("menu", "closeDesc"), DBSpawnPos.CenterLeftTop, 0.4f);
                    menuWindow.AddText(languages[0].GetText("menu", "return"));
                    menuWindow.AddText(languages[0].GetText("menu", "add"));
                    menuWindow.AddText(languages[0].GetText("menu", "options"));
                    menuWindow.AddText(languages[0].GetText("menu", "manage"));
                    menuWindow.AddText(languages[0].GetText("menu", "tutorials"));
                    menuWindow.AddText(languages[0].GetText("menu", "about"));
                    menuWindow.AddText(languages[0].GetText("menu", "exit"));
                    foreach (TextSprite sprite in menuWindow.sprites)
                    {
                        sprite.scale = 0.6f;
                    }
                    menuWindow.skipMainStateTransition = true;
                }
            }
        }

        public void CheckSelectedGameChange()
        {
            bool indexChange = true;
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadRight, false) || GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) > 0.3 || KeyboardInput.keys["Right"].IsDown() || gameIcons[3].CheckMouse(true) || gameIcons[4].CheckMouse(true)) && state == State.Main && !firstReset && IsActive && !tutorialLock)
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
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadLeft, false) || GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) < -0.3 || KeyboardInput.keys["Left"].IsDown() || gameIcons[0].CheckMouse(true) || gameIcons[1].CheckMouse(true)) && state == State.Main && !firstReset && IsActive && !tutorialLock)
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
            else if (jumpTriggerCooldown == 0 && GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.RightTrigger) >= 0.65f && !tutorialLock)
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
            else if (jumpTriggerCooldown == 0 && GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftTrigger) >= 0.65f && !tutorialLock)
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
                if (KeyboardInput.keys[key].IsFirstDown() && !tutorialLock)
                {
                    JumpTo(key, indexChange);
                    indexChange = false;
                }
            }
        }

        public void CheckGameSelected()
        {
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.A, true) || KeyboardInput.keys["Enter"].IsFirstDown() || KeyboardInput.keys["Space"].IsFirstDown() || (gameIcons[2].CheckMouse(true) && MouseInput.IsLeftFirstDown())) && state == State.Main && IsActive && !tutorialLock)
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
                launchWindow.AddText("");
                launchWindow.AddText("");
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
            UpdateExtraLaunchWindows();
        }

        public void CheckDedicationMessageOpen()
        {
            if (state != State.Message && Keyboard.GetState().IsKeyDown(Keys.C) && Keyboard.GetState().IsKeyDown(Keys.H) && Keyboard.GetState().IsKeyDown(Keys.U) && Keyboard.GetState().IsKeyDown(Keys.K) && IsActive && !tutorialLock)
            {
                message = new MessageWindow(this, "Et tu, Brute?", "Dedicated to Chuck", state);
                state = State.Message;
            }
        }

        public void CheckFolderSwitch()
        {
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.RightShoulder, true) || KeyboardInput.keys["RCtrl"].IsFirstDown() || (folderSprite.CheckMouse(true) && MouseInput.IsLeftFirstDown())) && state == State.Main && IsActive && !tutorialLock)
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
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.LeftShoulder, true) || KeyboardInput.keys["LCtrl"].IsFirstDown()) && state == State.Main && IsActive && !tutorialLock)
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
        }

        public void CheckSortChangeAndCoverChange()
        {
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Y, true) || KeyboardInput.keys["RShift"].IsFirstDown() || (sortSprite.CheckMouse(true) && MouseInput.IsLeftFirstDown()) || resort) && state == State.Main && IsActive && !tutorialLock)
            {
                if (!resort)
                {
                    if (sort == Sort.AZ)
                    {
                        sort = Sort.ZA;
                    }
                    else if (sort == Sort.ZA)
                    {
                        sort = Sort.Date;
                    }
                    else if (sort == Sort.Date)
                    {
                        sort = Sort.Dev;
                    }
                    else if (sort == Sort.Dev)
                    {
                        sort = Sort.Pub;
                    }
                    else if (sort == Sort.Pub)
                    {
                        sort = Sort.AZ;
                    }
                }
                if (sort == Sort.AZ)
                {
                    gameData = gameData.OrderBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.ZA)
                {
                    gameData = gameData.OrderByDescending(o => o.alphaAs).ThenByDescending(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Date)
                {
                    gameData = gameData.OrderBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Dev)
                {
                    gameData = gameData.OrderBy(o => o.developer).ThenBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                else if (sort == Sort.Pub)
                {
                    gameData = gameData.OrderBy(o => o.publisher).ThenBy(o => o.year).ThenBy(o => o.month).ThenBy(o => o.day).ThenBy(o => o.alphaAs).ThenBy(o => o.gameTitle).ToList();
                }
                if (!resort)
                {
                    sortSound.Play();
                }
                firstReset = true;
                resort = false;
                ResetGameIcons();
                Logging.Write(LogType.Standard, Event.DashSort, "Games have been sorted", "newSort", sort.ToString());
            }
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.X, true) || KeyboardInput.keys["Tab"].IsFirstDown()) && state == State.Main && IsActive && !tutorialLock)
            {
                AdjustCover();
            }
        }
    }
}
