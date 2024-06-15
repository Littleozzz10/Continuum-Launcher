using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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
using GameData = XeniaLauncher.Shared.GameData;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XLCompanion;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Continuum_Launcher;

namespace XeniaLauncher
{
    public class DatabaseResult : IWindowEffects
    {
        public int devIndex, pubIndex;
        public List<GameInfo> results;
        public int resultIndex;
        public List<string> developers, publishers;
        public string tempGameTitle;
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                devIndex--;
                if (devIndex < 0)
                {
                    devIndex = developers.Count - 1;
                }
                AdjustDeveloper(game, source);
            }
            else if (buttonIndex == 1)
            {
                devIndex++;
                if (devIndex >= developers.Count)
                {
                    devIndex = 0;
                }
                AdjustDeveloper(game, source);
            }
            if (buttonIndex == 2)
            {
                pubIndex--;
                if (pubIndex < 0)
                {
                    pubIndex = publishers.Count - 1;
                }
                AdjustPublisher(game, source);
            }
            else if (buttonIndex == 3)
            {
                pubIndex++;
                if (pubIndex >= publishers.Count)
                {
                    pubIndex = 0;
                }
                AdjustPublisher(game, source);
            }
            else if (buttonIndex == 4)
            {
                game.text = new TextInputWindow(game, "Edit Game Title", tempGameTitle, Game1.State.DatabaseResult);
            }
            else if (buttonIndex == 5)
            {
                game.OpenDateEditWindow(Game1.State.ReleaseYear, Game1.State.DatabaseResult);
            }
            else if (buttonIndex == 6)
            {
                game.gameData[game.index].gameTitle = tempGameTitle;
                game.gameData[game.index].developer = developers[devIndex];
                game.gameData[game.index].publisher = publishers[pubIndex];
                game.gameData[game.index].year = game.tempYear;
                game.gameData[game.index].month = game.tempMonth;
                game.gameData[game.index].day = game.tempDay;
                game.SaveGames();
                game.state = Game1.State.GameMenu;
                game.selectSound.Play();
            }
            else if (buttonIndex == 7)
            {
                game.state = source.returnState;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            results = game.databaseGameInfo;
            resultIndex = game.databaseResultIndex;
            game.tempYear = Convert.ToInt32(results[resultIndex].Release_Date.Substring(0, 4));
            game.tempMonth = Convert.ToInt32(results[resultIndex].Release_Date.Substring(5, 2));
            game.tempDay = Convert.ToInt32(results[resultIndex].Release_Date.Substring(8, 2));
            string date = "Database Release: " + game.tempMonth + "-" + game.tempDay + "-" + game.tempYear;
            if ((bool)game.databaseGameInfo[game.databaseResultIndex].Incorrect_Date)
            {
                date = date + "*";
            }
            game.databaseResultIndex = -1;

            window.extraSprites.Add(new TextSprite(game.font, "", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Selected Developer:", 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[1].Centerize(new Vector2(960, 285));
            window.extraSprites.Add(new TextSprite(game.font, "", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Selected Publisher:", 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[3].Centerize(new Vector2(960, 425));
            window.extraSprites.Add(new TextSprite(game.font, "STFS Name: " + game.gameData[game.index].gameTitle, 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[4].Centerize(new Vector2(720, 570));
            window.extraSprites[4].tags.Add("gray");
            window.extraSprites.Add(new TextSprite(game.font, date, 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[5].Centerize(new Vector2(1205, 570));
            window.extraSprites[5].tags.Add("gray");
            developers = results[resultIndex].Developers.ToList();
            if (developers.Count == 0)
            {
                developers.Add("Unknown Developer");
            }
            publishers = results[resultIndex].Publishers.ToList();
            if (publishers.Count == 0)
            {
                publishers.Add("Unknown Publisher");
            }
            tempGameTitle = results[resultIndex].Title;
            AdjustDeveloper(game, window);
            AdjustPublisher(game, window);
        }
        private void AdjustDeveloper(Game1 game, Window source)
        {
            source.extraSprites[0].ToTextSprite().text = developers[devIndex];
            source.extraSprites[0].Centerize(new Vector2(960, 345));
        }
        private void AdjustPublisher(Game1 game, Window source)
        {
            source.extraSprites[2].ToTextSprite().text = publishers[pubIndex];
            source.extraSprites[2].Centerize(new Vector2(960, 485));
        }
    }
    public class DatabaseResultInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 1)
            {
                buttonIndex = 7;
            }
            else if (buttonIndex == 6 || buttonIndex == 7)
            {
                buttonIndex--;
            }
            else
            {
                buttonIndex -= 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 7)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex == 5 || buttonIndex == 6)
            {
                buttonIndex++;
            }
            else
            {
                buttonIndex += 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 5)
            {
                if (buttonIndex % 2 == 1)
                {
                    buttonIndex--;
                }
                else
                {
                    buttonIndex++;
                }
            }
            else
            {
                buttonIndex = 4;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 5)
            {
                if (buttonIndex % 2 == 1)
                {
                    buttonIndex--;
                }
                else
                {
                    buttonIndex++;
                }
            }
            else
            {
                buttonIndex = 5;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
    }

    public class DatabasePicker : IWindowEffects
    {
        public int gameIndex;
        public List<GameInfo> results;
        public int resultIndex;
        public List<string> games;
        public string tempGameTitle;
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                gameIndex--;
                if (gameIndex < 0)
                {
                    gameIndex = games.Count - 1;
                }
                AdjustGame(game, source);
            }
            else if (buttonIndex == 1)
            {
                gameIndex++;
                if (gameIndex >= games.Count)
                {
                    gameIndex = 0;
                }
                AdjustGame(game, source);
            }
            else if (buttonIndex == 2)
            {
                game.databaseResultIndex = gameIndex;
                game.OpenDatabaseResult(Game1.State.DatabasePicker);
            }
            else if (buttonIndex == 3)
            {
                game.state = Game1.State.GameMenu;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            results = game.databaseGameInfo;
            resultIndex = game.databaseResultIndex;
            games = new List<string>();
            foreach (GameInfo info in results)
            {
                games.Add(info.Title);
            }
            window.extraSprites.Add(new TextSprite(game.font, "", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "STFS Name: " + game.gameData[game.index].gameTitle, 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[1].Centerize(new Vector2(960, 600));
            window.extraSprites[1].tags.Add("gray");
            tempGameTitle = results[resultIndex].Title;
            AdjustGame(game, window);
        }
        private void AdjustGame(Game1 game, Window source)
        {
            source.extraSprites[0].ToTextSprite().text = games[gameIndex];
            source.extraSprites[0].Centerize(new Vector2(960, 495));
        }
    }
    public class DatabasePickerInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 1)
            {
                buttonIndex += 2;
            }
            else
            {
                buttonIndex -= 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 2)
            {
                buttonIndex -= 2;
            }
            else
            {
                buttonIndex += 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex % 2 == 1)
            {
                buttonIndex--;
            }
            else
            {
                buttonIndex++;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex % 2 == 1)
            {
                buttonIndex--;
            }
            else
            {
                buttonIndex++;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
    }
}
