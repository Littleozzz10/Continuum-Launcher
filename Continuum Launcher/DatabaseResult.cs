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
            else if (buttonIndex == 6)
            {
                game.gameData[game.index].gameTitle = tempGameTitle;
                game.gameData[game.index].developer = developers[devIndex];
                game.gameData[game.index].publisher = publishers[pubIndex];
                game.SaveGames();
                game.state = Game1.State.GameMenu;
                game.selectSound.Play();
            }
            else if (buttonIndex == 7)
            {
                game.state = Game1.State.GameMenu;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            window.extraSprites.Add(new TextSprite(game.bold, "", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Selected Developer:", 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[1].Centerize(new Vector2(960, 355));
            window.extraSprites.Add(new TextSprite(game.bold, "", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Selected Publisher:", 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[3].Centerize(new Vector2(960, 495));
            results = game.databaseGameInfo;
            resultIndex = game.databaseResultIndex;
            game.databaseResultIndex = -1;
            developers = results[resultIndex].Developers.ToList();
            publishers = results[resultIndex].Publishers.ToList();
            tempGameTitle = results[resultIndex].Title;
            AdjustDeveloper(game, window);
            AdjustPublisher(game, window);
        }
        private void AdjustDeveloper(Game1 game, Window source)
        {
            source.extraSprites[0].ToTextSprite().text = developers[devIndex];
            source.extraSprites[0].Centerize(new Vector2(960, 415));
        }
        private void AdjustPublisher(Game1 game, Window source)
        {
            source.extraSprites[2].ToTextSprite().text = publishers[pubIndex];
            source.extraSprites[2].Centerize(new Vector2(960, 555));
        }
    }
    public class DatabaseResultInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 1)
            {
                buttonIndex = 6;
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
            if (buttonIndex == 6)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex == 5)
            {
                buttonIndex = 6;
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
            if (buttonIndex != 6)
            {
                if (buttonIndex % 2 == 1)
                {
                    buttonIndex++;
                }
                else
                {
                    buttonIndex--;
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
            if (buttonIndex != 6)
            {
                if (buttonIndex % 2 == 0)
                {
                    buttonIndex++;
                }
                else
                {
                    buttonIndex--;
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
}
