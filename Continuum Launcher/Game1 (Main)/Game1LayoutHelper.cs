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
                resort = true;
                ResetGameIcons();
                foreach (XGame game in gameIcons)
                {
                    game.AdjustIndex(gameData.Count);
                }
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

        public void ExitTutorial()
        {
            tutorialExitPrompt = false;
            messageYes = false;
            tutorial = null;
            tutorialLock = false;
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

        public void UpdateTitleText()
        {
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
        }

        public void UpdateSortText()
        {
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
        }

        public void UpdateFolderTextAnimation()
        {
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
        }

        public void UpdateFades()
        {
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
        }

        public void UpdateJumpLayer()
        {
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
        }

        public void UpdateCornerText()
        {
            if (firstLoad)
            {
                bottomInfo.displayGradient.colors[0] = backColorAlt;
                bottomInfo.Reset();
            }
        }

        public void UpdateTutorialFade()
        {
            tutorialFade.UpdatePos();
            if (tutorialLock && tutorialFade.color.A < 80)
            {
                tutorialFade.color.A += 4;
            }
            else if (!tutorialLock && tutorialFade.color.A > 0)
            {
                tutorialFade.color.A -= 4;
            }
        }

        public void UpdateDateTimeDisplay()
        {
            // Time
            timeText.color = timeDateColor;
            if (timeText.CheckMouse(true) && MouseInput.IsLeftFirstDown() && (state == State.Main || state == State.Select) && IsActive)
            {
                militaryTime = !militaryTime;
                buttonSwitchSound.Play();
                SaveSettings();
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
            if (dateText.CheckMouse(true) && MouseInput.IsLeftFirstDown() && (state == State.Main || state == State.Select) && IsActive)
            {
                inverseDate = !inverseDate;
                buttonSwitchSound.Play();
                SaveSettings();
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
        }

        public void UpdateControllerDisplay()
        {
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
        }

        public void FinishCornerUpdate()
        {
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
                    SaveSettings();
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
        }

        public void UpdateTrivia()
        {
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
        }
    }
}
