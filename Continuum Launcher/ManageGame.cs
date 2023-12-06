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
    public class ManageGame : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                game.state = Game1.State.GameXeniaSettings;
                game.gameXeniaSettingsWindow = new Window(game, new Rectangle(260, 185, 1400, 750), "Xenia Settings for " + game.gameData[game.index].gameTitle, new XeniaGameSettings(), new StdInputEvent(17), new GenericStart(), Game1.State.GameMenu);
                // First set
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(280, 335, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(860, 335, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(280, 435, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(860, 435, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(280, 535, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(860, 535, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(280, 635, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(860, 635, 90, 90));
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                // Second set
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(970, 335, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1540, 335, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(970, 435, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1540, 435, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(970, 535, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1540, 535, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(970, 635, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1540, 635, 90, 90));
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(610, 755, 700, 100));
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("Back to Manage Window");
                game.gameXeniaSettingsWindow.skipMainStateTransition = true;
                game.gameXeniaSettingsWindow.buttonEffects.SetupEffects(game, game.gameXeniaSettingsWindow);
            }
            else if (buttonIndex == 1)
            {
                game.state = Game1.State.GameInfo;
                game.gameInfoWindow = new Window(game, new Rectangle(260, 185, 1400, 750), "Info for " + game.gameData[game.index].gameTitle, new GameInfoWindow(), new StdInputEvent(5), new GenericStart(), Game1.State.GameMenu);

                game.gameInfoWindow.AddButton(new Rectangle(280, 335, 500, 90));
                game.gameInfoWindow.AddButton(new Rectangle(280, 435, 500, 90));
                game.gameInfoWindow.AddButton(new Rectangle(280, 535, 500, 90));
                game.gameInfoWindow.AddButton(new Rectangle(280, 635, 500, 90));
                game.gameInfoWindow.AddButton(new Rectangle(970, 535, 500, 90));
                game.gameInfoWindow.AddButton(new Rectangle(970, 635, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(1540, 635, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(610, 755, 700, 100));
                game.gameInfoWindow.AddText("Edit Name");
                game.gameInfoWindow.AddText("Edit Developer");
                game.gameInfoWindow.AddText("Edit Publisher");
                game.gameInfoWindow.AddText("Edit Title ID");
                game.gameInfoWindow.AddText("Edit Release Date");
                game.gameInfoWindow.AddText("<");
                game.gameInfoWindow.AddText(">");
                game.gameInfoWindow.AddText("Back to Manage Window");
                game.gameInfoWindow.skipMainStateTransition = true;
                game.gameInfoWindow.buttonEffects.SetupEffects(game, game.gameInfoWindow);
            }
            else if (buttonIndex == 2)
            {
                
            }
            else if (buttonIndex == 3)
            {
                
            }
            else if (buttonIndex == 4)
            {
                game.Exit();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {

        }
    }
}
