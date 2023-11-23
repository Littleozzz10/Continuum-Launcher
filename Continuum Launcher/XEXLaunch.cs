using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class XEXLaunch : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            // First button is the normal game
            if (buttonIndex == 0)
            {
                if (game.launchWindow.buttonIndex == 0)
                {
                    GameLaunch.SafeLaunchXenia(game);
                }
                else if (game.launchWindow.buttonIndex == 1)
                {
                    GameLaunch.SafeLaunchCanary(game);
                }
                else if (game.launchWindow.buttonIndex == 2)
                {
                    GameLaunch.SafeQuickstart(game);
                }
            }
            // Last button is the exit button
            else if (buttonIndex == source.strings.Count - 1)
            {
                game.state = Game1.State.Select;
                game.backSound.Play();
            }
            // Any other button in between is an XEX button
            else
            {
                if (game.launchWindow.buttonIndex == 0)
                {
                    GameLaunch.SafeLaunchXenia(game, game.gameData[game.index].xexPaths[buttonIndex - 1]);
                }
                else if (game.launchWindow.buttonIndex == 1)
                {
                    GameLaunch.SafeLaunchCanary(game, game.gameData[game.index].xexPaths[buttonIndex - 1]);
                }
                else if (game.launchWindow.buttonIndex == 2)
                {
                    GameLaunch.SafeQuickstart(game, game.gameData[game.index].xexPaths[buttonIndex - 1]);
                }
            }
        }
        public void SetupEffects(Game1 game, Window source)
        {

        }
    }
}
