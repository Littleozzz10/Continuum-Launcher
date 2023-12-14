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

                game.gameInfoWindow.AddButton(new Rectangle(280, 335, 650, 90));
                game.gameInfoWindow.AddButton(new Rectangle(280, 435, 650, 90));
                game.gameInfoWindow.AddButton(new Rectangle(280, 535, 650, 90));
                game.gameInfoWindow.AddButton(new Rectangle(280, 635, 650, 90));
                game.gameInfoWindow.AddButton(new Rectangle(970, 335, 650, 90));
                game.gameInfoWindow.AddButton(new Rectangle(970, 435, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(1540, 435, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(970, 535, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(1540, 535, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(610, 755, 700, 100));
                game.gameInfoWindow.AddText("Edit Name");
                game.gameInfoWindow.AddText("Edit Developer");
                game.gameInfoWindow.AddText("Edit Publisher");
                game.gameInfoWindow.AddText("Edit Title ID");
                game.gameInfoWindow.AddText("Edit Release Date");
                game.gameInfoWindow.AddText("<");
                game.gameInfoWindow.AddText(">");
                game.gameInfoWindow.AddText("<");
                game.gameInfoWindow.AddText(">");
                game.gameInfoWindow.AddText("Back to Manage Window");
                game.gameInfoWindow.skipMainStateTransition = true;
                game.gameInfoWindow.buttonEffects.SetupEffects(game, game.gameInfoWindow);
            }
            else if (buttonIndex == 2)
            {
                game.state = Game1.State.GameFilepaths;
                game.gameFilepathsWindow = new Window(game, new Rectangle(560, 170, 800, 740), "Filepaths for " + game.gameData[game.index].gameTitle, new GameFilepaths(), new StdInputEvent(4), new GenericStart(), Game1.State.GameMenu);

                game.gameFilepathsWindow.AddButton(new Rectangle(610, 320, 700, 100));
                game.gameFilepathsWindow.AddButton(new Rectangle(610, 430, 700, 100));
                game.gameFilepathsWindow.AddButton(new Rectangle(610, 540, 700, 100));
                game.gameFilepathsWindow.AddButton(new Rectangle(610, 760, 700, 100));
                game.gameFilepathsWindow.AddText("Edit Game Filepath");
                game.gameFilepathsWindow.AddText("Edit Cover Art Filepath");
                game.gameFilepathsWindow.AddText("Edit Icon Filepath");
                game.gameFilepathsWindow.AddText("Back to Manage Window");
                game.gameFilepathsWindow.buttonEffects.SetupEffects(game, game.gameFilepathsWindow);
            }
            else if (buttonIndex == 3)
            {
                game.state = Game1.State.GameCategories;
                game.gameCategoriesWindow = new Window(game, new Rectangle(360, 170, 1200, 740), "Manage Categories", new GameCategories(), new StdInputEvent(7), new GenericStart(), Game1.State.GameMenu);

                game.gameCategoriesWindow.AddButton(new Rectangle(410, 320, 90, 90));
                game.gameCategoriesWindow.AddButton(new Rectangle(1420, 320, 90, 90));
                game.gameCategoriesWindow.AddButton(new Rectangle(495, 520, 450, 100));
                game.gameCategoriesWindow.AddButton(new Rectangle(980, 520, 450, 100));
                game.gameCategoriesWindow.AddButton(new Rectangle(495, 640, 450, 100));
                game.gameCategoriesWindow.AddButton(new Rectangle(980, 640, 450, 100));
                game.gameCategoriesWindow.AddButton(new Rectangle(610, 760, 700, 100));
                game.gameCategoriesWindow.AddText("<");
                game.gameCategoriesWindow.AddText(">");
                game.gameCategoriesWindow.AddText("Add Game to Cat.");
                game.gameCategoriesWindow.AddText("Create Category");
                game.gameCategoriesWindow.AddText("Rename Category");
                game.gameCategoriesWindow.AddText("Delete Category");
                game.gameCategoriesWindow.AddText("Back to Manage Window");
                game.gameCategoriesWindow.buttonEffects.SetupEffects(game, game.gameCategoriesWindow);
            }
            else if (buttonIndex == 4)
            {
                game.state = Game1.State.GameXEX;
                game.gameXEXWindow = new Window(game, new Rectangle(360, 170, 1200, 740), "Manage Executables", new GameXEX(), new StdInputEvent(7), new GenericStart(), Game1.State.GameMenu);

                game.gameXEXWindow.AddButton(new Rectangle(410, 320, 90, 90));
                game.gameXEXWindow.AddButton(new Rectangle(1420, 320, 90, 90));
                game.gameXEXWindow.AddButton(new Rectangle(495, 520, 450, 100));
                game.gameXEXWindow.AddButton(new Rectangle(980, 520, 450, 100));
                game.gameXEXWindow.AddButton(new Rectangle(495, 640, 450, 100));
                game.gameXEXWindow.AddButton(new Rectangle(980, 640, 450, 100));
                game.gameXEXWindow.AddButton(new Rectangle(610, 760, 700, 100));
                game.gameXEXWindow.AddText("<");
                game.gameXEXWindow.AddText(">");
                game.gameXEXWindow.AddText("Add New XEX");
                game.gameXEXWindow.AddText("Rename XEX");
                game.gameXEXWindow.AddText("Change XEX Path");
                game.gameXEXWindow.AddText("Delete XEX");
                game.gameXEXWindow.AddText("Back to Manage Window");
                game.gameXEXWindow.buttonEffects.SetupEffects(game, game.gameXEXWindow);
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            game.gameManageWindow.AddButton(new Rectangle(610, 320, 700, 100));
            game.gameManageWindow.AddButton(new Rectangle(610, 430, 700, 100));
            game.gameManageWindow.AddButton(new Rectangle(610, 540, 700, 100));
            game.gameManageWindow.AddButton(new Rectangle(610, 650, 700, 100));
            game.gameManageWindow.AddButton(new Rectangle(610, 760, 700, 100));
            game.gameManageWindow.AddText("Edit Launch Settings");
            game.gameManageWindow.AddText("Edit Game Info");
            game.gameManageWindow.AddText("Edit Filepaths");
            game.gameManageWindow.AddText("Manage Categories");
            game.gameManageWindow.AddText("Manage Executables");
            foreach (TextSprite sprite in game.gameManageWindow.sprites)
            {
                sprite.scale = 0.6f;
            }
            game.gameManageWindow.skipMainStateTransition = true;

            if (game.gameManageWindow.titleSprite.GetSize().X + 60 > game.gameManageWindow.rect.Width)
            {
                game.gameManageWindow.rect.Width = (int)(game.gameManageWindow.titleSprite.GetSize().X + 60);
                game.gameManageWindow.pos.X = 960 - game.gameManageWindow.rect.Width / 2;
            }
        }
    }
}
