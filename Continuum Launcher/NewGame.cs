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

namespace XeniaLauncher
{
    public class NewGame : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                game.text = new TextInputWindow(game, "Folder Filepath for New Game", "", Game1.State.NewGame);
            }
            else if (buttonIndex == 1)
            {
                game.text = new TextInputWindow(game, "New Game Title", "", Game1.State.NewGame);
            }
            else if (buttonIndex == 2)
            {
                game.text = new TextInputWindow(game, "STFS Filepath for New Game", "", Game1.State.NewGame);
            }
            else if (buttonIndex == 3)
            {
                game.state = Game1.State.Menu;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            foreach (TextSprite sprite in game.newGameWindow.sprites)
            {
                sprite.scale = 0.55f;
            }
        }
    }
}
