using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            else if (game.gameData[game.index].xexNames.Count == 0)
            {
                if (buttonIndex == 0)
                {
                    game.LaunchXenia();
                }
                else if (buttonIndex == 1)
                {
                    game.LaunchCanary();
                }
                else if (buttonIndex == 2)
                {
                    game.DefaultQuickstart();
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
