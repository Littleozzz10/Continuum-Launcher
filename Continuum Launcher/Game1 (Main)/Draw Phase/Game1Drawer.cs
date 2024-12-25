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
        public void DrawRings()
        {
            foreach (Ring ring in rings)
            {
                ring.Draw(_spriteBatch);
            }
        }

        public void DrawGames()
        {
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
        }

        public void DrawDashboardElements()
        {
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
        }

        public void DrawSelectMenuLayer()
        {
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
        }

        public void DrawAdditionalWindows()
        {
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
            if (databaseResultWindow != null)
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
            if (welcomeWindow != null)
            {
                welcomeWindow.Draw(_spriteBatch);
            }
            if (tutorialWindow != null)
            {
                tutorialWindow.Draw(_spriteBatch);
            }
            if (text != null)
            {
                text.Draw(_spriteBatch);
            }

            tutorialFade.Draw(_spriteBatch);
            if (tutorial != null)
            {
                tutorial.Draw(_spriteBatch);
            }

            if (message != null)
            {
                message.Draw(_spriteBatch);
            }
        }
    }
}
