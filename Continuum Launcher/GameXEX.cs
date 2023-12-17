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
    public class GameXEX : IWindowEffects
    {
        public List<string> xexList;
        public int xexIndex;
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                xexIndex--;
                if (xexIndex < 0)
                {
                    xexIndex = game.gameData[game.index].xexNames.Count;
                }
                AdjustXEX(game, source);
            }
            else if (buttonIndex == 1)
            {
                xexIndex++;
                if (xexIndex > game.gameData[game.index].xexNames.Count)
                {
                    xexIndex = 0;
                }
                AdjustXEX(game, source);
            }
            else if (buttonIndex == 2)
            {
                game.text = new TextInputWindow(game, "Add New XEX", "", Game1.State.GameXEX);
            }
            else if (buttonIndex == 3)
            {
                if (xexIndex == 0)
                {
                    game.message = new MessageWindow(game, "Oops!", "Cannot rename placeholder XEX", Game1.State.GameXEX);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.tempCategoryIndex = xexIndex - 1;
                    game.text = new TextInputWindow(game, "Rename XEX", game.gameData[game.index].xexNames[xexIndex - 1], Game1.State.GameXEX);
                }
            }
            else if (buttonIndex == 4)
            {
                if (xexIndex == 0)
                {
                    game.message = new MessageWindow(game, "Oops!", "Cannot edit placeholder XEX", Game1.State.GameXEX);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.tempCategoryIndex = xexIndex - 1;
                    game.text = new TextInputWindow(game, "Edit XEX Filepath", game.gameData[game.index].xexPaths[xexIndex - 1], Game1.State.GameXEX);
                }
            }
            else if (buttonIndex == 5)
            {
                if (xexIndex == 0)
                {
                    game.tempCategoryIndex = -1;
                    game.message = new MessageWindow(game, "Warning", "Are you sure you want to delete this game? This CANNOT be undone", Game1.State.GameXEX, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.tempCategoryIndex = xexIndex - 1;
                    xexIndex = 0;
                    game.message = new MessageWindow(game, "Warning", "Are you sure you want to delete this executable?", Game1.State.GameXEX, MessageWindow.MessagePrompts.YesNo);
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
            AdjustXEX(game, window);
        }
        private void AdjustXEX(Game1 game, Window source)
        {
            SetupXEXList(game);
            source.extraSprites[0].ToTextSprite().text = xexList[xexIndex];
            source.extraSprites[0].Centerize(new Vector2(960, 365));
        }
        private void SetupXEXList(Game1 game)
        {
            xexList = new List<string>();
            xexList.Add(game.gameData[game.index].gameTitle);
            foreach (string xex in game.gameData[game.index].xexNames)
            {
                xexList.Add(xex);
            }
        }
    }
}
