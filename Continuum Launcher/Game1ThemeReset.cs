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
        /// Resets the theme, used for changing to a new theme
        /// </summary>
        /// <param name="newTheme">The new theme to change to</param>
        /// <param name="forceWindowReset">If true, forces all open Window to reconstruct (This resets and applies the theme to the Window)</param>
        public void ResetTheme(Theme newTheme, bool forceWindowReset)
        {
            Logging.Write(LogType.Debug, Event.ThemeReset, "Theme reset: " + newTheme.ToString(), new Dictionary<string, string>() { { "forceWindowReset", "" + forceWindowReset } });
            StreamReader themeReader = null;
            if (File.Exists("Content\\XLTheme.txt"))
            {
                themeReader = new StreamReader("Content\\XLTheme.txt");
                try
                {
                    int v = Convert.ToInt32(themeReader.ReadLine());
                    if (v < 2016)
                    {
                        themeReader.Close();
                        Logging.Write(LogType.Critical, Event.Error, "Custom theme is outdated", "oldVernum", "" + v);
                    }
                }
                catch (Exception e)
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error reading from XLTheme (0)", "exception", e.ToString());
                    newTheme = theme;
                    if (!firstLoad)
                    {
                        message = new MessageWindow(this, "This is outrageous! It's unfair!", "Error reading from custom Theme file.", state);
                        state = State.Message;
                    }
                }
            }
            else
            {
                Logging.Write(LogType.Critical, Event.Error, "XLTheme not found");
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
                    try
                    {
                        backColor = ColorFromString(themeReader.ReadLine());
                        backColorAlt = ColorFromString(themeReader.ReadLine());
                        majorFontColor = ColorFromString(themeReader.ReadLine());
                        cornerStatsColor = ColorFromString(themeReader.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Logging.Write(LogType.Critical, Event.Error, "Error reading from XLTheme (1)", "exception", e.ToString());
                        if (!firstLoad)
                        {
                            message = new MessageWindow(this, "This is outrageous! It's unfair!", "Error reading from custom Theme file.", state);
                            state = State.Message;
                        }
                    }
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
                try
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
                    dataTitleColor = ColorFromString(themeReader.ReadLine());
                }
                catch (Exception e)
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error reading from XLTheme (2)", "exception", e.ToString());
                    if (!firstLoad)
                    {
                        message = new MessageWindow(this, "This is outrageous! It's unfair!", "Error reading from custom Theme file.", state);
                        state = State.Message;
                    }
                }
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
                try
                {
                    whiteGradient.colors.Add(Color.FromNonPremultiplied(255, 255, 255, 255));
                }
                catch (Exception e)
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error reading from XLTheme (3)", "exception", e.ToString());
                    if (!firstLoad)
                    {
                        message = new MessageWindow(this, "This is outrageous! It's unfair!", "Error reading from custom Theme file.", state);
                        state = State.Message;
                    }
                }
            }
            whiteGradient.ValueUpdate(0);

            if (themeReader != null && themeReader.EndOfStream && newTheme == Theme.Custom)
            {
                try
                {
                    if (File.Exists("Content\\theme.png"))
                    {
                        customThemeImage = Texture2D.FromFile(_graphics.GraphicsDevice, "Content\\theme.png");
                    }
                    else
                    {
                        customThemeImage = white;
                    }
                    if (themeThumbnails != null)
                    {
                        themeThumbnails["custom"] = customThemeImage;
                    }
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Unable to load theme.png");
                    if (themeThumbnails != null)
                    {
                        themeThumbnails["custom"] = white;
                    }
                }
            }

            if (themeReader != null)
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
    }
}
