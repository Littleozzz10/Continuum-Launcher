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
    }
}
