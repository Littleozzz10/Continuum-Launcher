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

namespace XeniaLauncher
{
    public class GameLaunch : IWindowEffects
    {
        /// <summary>
        /// Attemps to launch Xenia, throwing an error message if it can't
        /// </summary>
        public static void SafeLaunchXenia(Game1 game)
        {
            SafeLaunchXenia(game, "");
        }
        public static void SafeLaunchXenia(Game1 game, string path)
        {
            if (File.Exists(game.xeniaPath))
            {
                if (String.IsNullOrEmpty(path))
                {
                    game.LaunchXenia();
                }
                else
                {
                    game.LaunchXenia(path);
                }
            }
            else
            {
                game.message = new MessageWindow(game, "These aren't the droids you're looking for...", "Provided filepath to Xenia does not exist", Game1.State.Select);
                game.state = Game1.State.Message;
            }
        }
        /// <summary>
        /// Attemps to launch Canary, throwing an error message if it can't
        /// </summary>
        public static void SafeLaunchCanary(Game1 game)
        {
            SafeLaunchCanary(game, "");
        }
        public static void SafeLaunchCanary(Game1 game, string path)
        {
            if (File.Exists(game.xeniaPath))
            {
                if (String.IsNullOrEmpty(path))
                {
                    game.LaunchCanary();
                }
                else
                {
                    game.LaunchCanary(path);
                }
            }
            else
            {
                game.message = new MessageWindow(game, "These aren't the droids you're looking for...", "Provided filepath to Xenia Canary does not exist", Game1.State.Select);
                game.state = Game1.State.Message;
            }
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 3)
            {
                game.OpenCompatWindow(false, 0, true);
            }
            else if (buttonIndex == 4)
            {
                game.OpenCompatWindow(true, 0, true);
            }
            else if (buttonIndex == 2)
            {
                game.state = Game1.State.GameMenu;
                game.gameManageWindow = new Window(game, new Rectangle(560, 115, 800, 860), "Manage " + game.gameData[game.index].gameTitle, new ManageGame(), new StdInputEvent(6), new GenericStart(), Game1.State.Select);
                game.gameManageWindow.buttonEffects.SetupEffects(game, source);
            }
            else if (game.gameData[game.index].xexNames.Count == 0)
            {
                if (buttonIndex == 0)
                {
                    SafeLaunchXenia(game);
                }
                else if (buttonIndex == 1)
                {
                    SafeLaunchCanary(game);
                }
            }
            else
            {
                game.xexWindow = new Window(game, new Rectangle(555, 290, 800, 500), "Select Executable", new XEXLaunch(), new StdInputEvent(3), new GenericStart(), Game1.State.Select);
                game.xexWindow.AddButton(new Rectangle(620, 460, 690, 80));
                game.xexWindow.AddButton(new Rectangle(620, 550, 690, 80));
                game.xexWindow.AddButton(new Rectangle(620, 640, 690, 80));
                game.xexWindow.AddText("Launch " + game.gameData[game.index].gameTitle);
                foreach (string xex in game.gameData[game.index].xexNames)
                {
                    game.xexWindow.AddText("Launch " + xex);
                }
                game.xexWindow.AddText("Cancel Launch");
                game.state = Game1.State.Launch;
            }
        }
        public void SetupEffects(Game1 game, Window source)
        {

        }
    }
}
