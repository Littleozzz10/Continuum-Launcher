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

namespace XeniaLauncher
{
    public class GameCategories : IWindowEffects
    {
        public int folderIndex;
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                folderIndex--;
                if (folderIndex < 0)
                {
                    folderIndex = game.folders.Count - 1;
                }
                AdjustCategory(game, source);
            }
            else if (buttonIndex == 1)
            {
                folderIndex++;
                if (folderIndex >= game.folders.Count)
                {
                    folderIndex = 0;
                }
                AdjustCategory(game, source);
            }
            else if (buttonIndex == 2)
            {
                if (folderIndex == 0)
                {
                    game.message = new MessageWindow(game, "Oops!", "Cannot add or remove games from All Games", Game1.State.GameCategories);
                    game.state = Game1.State.Message;
                }
                else
                {
                    if (game.gameData[game.index].folders.Contains(game.folders[folderIndex]))
                    {
                        game.gameData[game.index].folders.Remove(game.folders[folderIndex]);
                    }
                    else
                    {
                        game.gameData[game.index].folders.Add(game.folders[folderIndex]);
                    }
                    AdjustButtonName(game, source);
                    game.SaveGames();
                    game.forceInit = true;
                }
            }
            else if (buttonIndex == 3)
            {
                game.text = new TextInputWindow(game, "Create New Category", "", Game1.State.GameCategories);
            }
            else if (buttonIndex == 4)
            {
                if (folderIndex == 0)
                {
                    game.message = new MessageWindow(game, "Oops!", "Cannot rename the All Games category", Game1.State.GameCategories);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.tempCategoryIndex = folderIndex;
                    game.text = new TextInputWindow(game, "Rename Category", "", Game1.State.GameCategories);
                }
            }
            else if (buttonIndex == 5)
            {
                if (folderIndex == 0)
                {
                    game.message = new MessageWindow(game, "Oops!", "Cannot delete the All Games category", Game1.State.GameCategories);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.tempCategoryIndex = folderIndex;
                    game.message = new MessageWindow(game, "Warning", "Are you sure you want to delete this Category?", Game1.State.GameCategories, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
            }
            else if (buttonIndex == 6)
            {
                game.state = Game1.State.GameMenu;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            window.extraSprites.Add(new TextSprite(game.bold, "", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Game: " + game.gameData[game.index].gameTitle, 0.4f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites[1].Centerize(new Vector2(960, 450));
            AdjustCategory(game, window);
        }
        private void AdjustCategory(Game1 game, Window source)
        {
            source.extraSprites[0].ToTextSprite().text = game.folders[folderIndex];
            source.extraSprites[0].Centerize(new Vector2(960, 365));
            AdjustButtonName(game, source);
        }
        private void AdjustButtonName(Game1 game, Window source)
        {
            source.sprites[2].text = "Add Game to Cat.";
            if (game.gameData[game.index].folders.Contains(game.folders[folderIndex]) || folderIndex == 0)
            {
                source.sprites[2].text = "Remove Game From Cat.";
            }
        }
    }
    public class GameCategoriesInput : IButtonInputEvent
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
