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
using Continuum_Launcher;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeniaLauncher
{
    public class ManageGame : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                game.state = Game1.State.GameXeniaSettings;
                game.gameXeniaSettingsWindow = new Window(game, new Rectangle(260, 185, 1400, 750), "Xenia Settings for " + game.gameData[game.index].gameTitle, "Note: These settings only apply to this game", new XeniaGameSettings(), new XeniaGameInput(), new GenericStart(), Game1.State.GameMenu, true);
                // First set
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(285, 375, 90, 90), "For XBLA games, the license to use:\n  - (0) : No License, Trial Mode\n  - (1) : First License, Full Game Mode\n  - (-1) : All Licenses, (Can be buggy)", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(865, 375, 90, 90), "For XBLA games, the license to use:\n  - (0) : No License, Trial Mode\n  - (1) : First License, Full Game Mode\n  - (-1) : All Licenses, (Can be buggy)", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(285, 475, 90, 90), "When enabled, Continuum will not copy Xenia\nexecutables to the game save directory.\nThis means you must provide a Xenia/Xenia Canary\nexecutable yourself.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(865, 475, 90, 90), "When enabled, Continuum will not copy Xenia\nexecutables to the game save directory.\nThis means you must provide a Xenia/Xenia Canary\nexecutable yourself.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(285, 575, 90, 90), "Enables Xenia's cache mount for this game. Usually\nmore resource intensive. Some games require this,\nsuch as the Forza titles.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(865, 575, 90, 90), "Enables Xenia's cache mount for this game. Usually\nmore resource intensive. Some games require this,\nsuch as the Forza titles.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(285, 675, 90, 90), "When on, Xenia will read back CPU shader memory\nexports during emulation. Highly resource intensive, but can\nimprove emulation quality. Some games require this to be\nenabled, but it can also adversely affect emulation in others.", Ozzz.DescriptionBox.SpawnPositions.AboveRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(865, 675, 90, 90), "When on, Xenia will read back CPU shader memory\nexports during emulation. Highly resource intensive, but can\nimprove emulation quality. Some games require this to be\nenabled, but it can also adversely affect emulation in others.", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                game.gameXeniaSettingsWindow.AddText("<");
                game.gameXeniaSettingsWindow.AddText(">");
                // Second set
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(975, 375, 90, 90), "Adjusts Xenia's internal resolution scale on the\nX-axis. Does not change window size or monitor\nresolution. Reduces performance and may break\nsome games.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1545, 375, 90, 90), "Adjusts Xenia's internal resolution scale on the\nX-axis. Does not change window size or monitor\nresolution. Reduces performance and may break\nsome games.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(975, 475, 90, 90), "Adjusts Xenia's internal resolution scale on the\nY-axis. Does not change window size or monitor\nresolution. Reduces performance and may break\nsome games.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1545, 475, 90, 90), "Adjusts Xenia's internal resolution scale on the\nY-axis. Does not change window size or monitor\nresolution. Reduces performance and may break\nsome games.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(975, 575, 90, 90), "When enabled, V-Sync limits framerate to the monitor's\nrefresh rate. Enable to reduce screen tearing. Will not\nimprove framerate. Does not work with some games,\nsuch as the Halo titles.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1545, 575, 90, 90), "When enabled, V-Sync limits framerate to the monitor's\nrefresh rate. Enable to reduce screen tearing. Will not\nimprove framerate. Does not work with some games,\nsuch as the Halo titles.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(975, 675, 90, 90), "The graphics renderer for Xenia to use:\n  - Any: Let Xenia choose the renderer (Likely D3D12)\n  - D3D12: Default, works best in most games\n  - Vulkan: Experimental, can sometimes work best", Ozzz.DescriptionBox.SpawnPositions.AboveRight, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(1545, 675, 90, 90), "The graphics renderer for Xenia to use:\n  - Any: Let Xenia choose the renderer (Likely D3D12)\n  - D3D12: Default, works best in most games\n  - Vulkan: Experimental, can sometimes work best", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.gameXeniaSettingsWindow.AddButton(new Rectangle(605, 795, 700, 100));
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
                game.databaseGameInfo = new List<GameInfo>(); // Clearing out old data
                foreach (GameInfo info in game.mobyData.Data)
                {
                    if (info.Variants != null && (info.Variants[0].TitleID == game.gameData[game.index].titleId))
                    {
                        game.databaseGameInfo.Add(info);
                    }
                }
                if (game.databaseGameInfo.Count == 0)
                {
                    if (game.gameData[game.index].titleId == "0x00000000")
                    {
                        game.message = new MessageWindow(game, "Nothing to See Here...", "This game's Title ID is either null or 0x00000000. A game must have a valid Title ID to perform a database lookup.", Game1.State.GameMenu);
                    }
                    else
                    {
                        game.message = new MessageWindow(game, "Aw, Shucks!", "No Title ID matches found in the database.", Game1.State.GameMenu);
                    }
                    game.state = Game1.State.Message;
                }
            }
            else if (buttonIndex == 2)
            {
                game.state = Game1.State.GameInfo;
                game.gameInfoWindow = new Window(game, new Rectangle(260, 185, 1400, 750), "Info for " + game.gameData[game.index].gameTitle, new GameInfoWindow(), new GameInfoInput(), new GenericStart(), Game1.State.GameMenu);

                game.gameInfoWindow.AddButton(new Rectangle(290, 335, 650, 90), "Edit the game's display name.", Ozzz.DescriptionBox.SpawnPositions.CenterRightBottom, 0.4f);
                game.gameInfoWindow.AddButton(new Rectangle(290, 435, 650, 90), "Edit the game's developer.", Ozzz.DescriptionBox.SpawnPositions.CenterRightBottom, 0.4f);
                game.gameInfoWindow.AddButton(new Rectangle(290, 535, 650, 90), "Edit the game's publisher.", Ozzz.DescriptionBox.SpawnPositions.CenterRightBottom, 0.4f);
                game.gameInfoWindow.AddButton(new Rectangle(290, 635, 650, 90), "Edit the game's title ID. This is used for database\nlookups and storing save data.", Ozzz.DescriptionBox.SpawnPositions.CenterRightTop, 0.4f);
                game.gameInfoWindow.AddButton(new Rectangle(970, 335, 660, 90), "Edit the game's alphabetization value. This name is not\ndisplayed, but is used when sorting alphabetically.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftBottom, 0.4f);
                game.gameInfoWindow.AddButton(new Rectangle(970, 435, 660, 90), "Edit the game's release date.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftBottom, 0.4f);
                game.gameInfoWindow.AddButton(new Rectangle(970, 535, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(1540, 535, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(970, 635, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(1540, 635, 90, 90));
                game.gameInfoWindow.AddButton(new Rectangle(610, 755, 700, 100));
                game.gameInfoWindow.AddText("Edit Name");
                game.gameInfoWindow.AddText("Edit Developer");
                game.gameInfoWindow.AddText("Edit Publisher");
                game.gameInfoWindow.AddText("Edit Title ID");
                game.gameInfoWindow.AddText("Edit Alphabetization");
                game.gameInfoWindow.AddText("Edit Release Date");
                game.gameInfoWindow.AddText("<");
                game.gameInfoWindow.AddText(">");
                game.gameInfoWindow.AddText("<");
                game.gameInfoWindow.AddText(">");
                game.gameInfoWindow.AddText("Back to Manage Window");
                game.gameInfoWindow.skipMainStateTransition = true;
                game.gameInfoWindow.buttonEffects.SetupEffects(game, game.gameInfoWindow);
            }
            else if (buttonIndex == 3)
            {
                game.state = Game1.State.GameFilepaths;
                game.gameFilepathsWindow = new Window(game, new Rectangle(560, 170, 800, 740), "Filepaths for " + game.gameData[game.index].gameTitle, new GameFilepaths(), new StdInputEvent(4), new GenericStart(), Game1.State.GameMenu);

                game.gameFilepathsWindow.AddButton(new Rectangle(610, 320, 700, 100), "Edit the direct filepath to the game's\nfile, either in GoD or XEX format.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftBottom, 0.4f);
                game.gameFilepathsWindow.AddButton(new Rectangle(610, 430, 700, 100), "Edit the direct filepath to the game's\ncover art file, in JPG format.", Ozzz.DescriptionBox.SpawnPositions.CenterRightBottom, 0.4f);
                game.gameFilepathsWindow.AddButton(new Rectangle(610, 540, 700, 100), "Edit the direct filepath to the game's\nicon. This is usually done automatically\nin an STFS Folder Import.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftBottom, 0.4f);
                game.gameFilepathsWindow.AddButton(new Rectangle(610, 760, 700, 100));
                game.gameFilepathsWindow.AddText("Edit Game Filepath");
                game.gameFilepathsWindow.AddText("Edit Cover Art Filepath");
                game.gameFilepathsWindow.AddText("Edit Icon Filepath");
                game.gameFilepathsWindow.AddText("Back to Manage Window");
                game.gameFilepathsWindow.buttonEffects.SetupEffects(game, game.gameFilepathsWindow);
            }
            else if (buttonIndex == 4)
            {
                game.state = Game1.State.GameCategories;
                game.gameCategoriesWindow = new Window(game, new Rectangle(360, 170, 1200, 740), "Manage Categories", new GameCategories(), new GameCategoriesInput(), new GenericStart(), Game1.State.GameMenu);

                game.gameCategoriesWindow.AddButton(new Rectangle(410, 320, 90, 90), "Previous Category", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.gameCategoriesWindow.AddButton(new Rectangle(1420, 320, 90, 90), "Next Category", Ozzz.DescriptionBox.SpawnPositions.AboveRight, 0.4f);
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
            else if (buttonIndex == 5)
            {
                game.state = Game1.State.GameXEX;
                game.gameXEXWindow = new Window(game, new Rectangle(360, 170, 1200, 740), "Manage Executables", "Tip: Delete the main XEX to delete the game", new GameXEX(), new GameCategoriesInput(), new GenericStart(), Game1.State.GameMenu, true);

                game.gameXEXWindow.AddButton(new Rectangle(410, 320, 90, 90), "Previous Executable", Ozzz.DescriptionBox.SpawnPositions.AboveLeft, 0.4f);
                game.gameXEXWindow.AddButton(new Rectangle(1420, 320, 90, 90), "Next Executable", Ozzz.DescriptionBox.SpawnPositions.AboveRight, 0.4f);
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
            game.gameManageWindow.AddButton(new Rectangle(610, 265, 700, 100), "Edit options to pass to Xenia when\nlaunching this game.", Ozzz.DescriptionBox.SpawnPositions.CenterRightBottom, 0.4f);
            game.gameManageWindow.AddButton(new Rectangle(610, 375, 700, 100), "Use Continuum's Xbox 360 game database\nto fill in this game's title, developer, and\npublisher. Requires a valid title ID.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftBottom, 0.4f);
            game.gameManageWindow.AddButton(new Rectangle(610, 485, 700, 100), "Edit information about this game,\nlike it's title, developer, title ID,\nalphabetization, and more.", Ozzz.DescriptionBox.SpawnPositions.CenterRightBottom, 0.4f);
            game.gameManageWindow.AddButton(new Rectangle(610, 595, 700, 100), "Edit the filepaths to the game, it's\ncover art, and it's icon.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftTop, 0.4f);
            game.gameManageWindow.AddButton(new Rectangle(610, 705, 700, 100), "Edit the Categories this game is in,\nor add new Categories.", Ozzz.DescriptionBox.SpawnPositions.CenterRightTop, 0.4f);
            game.gameManageWindow.AddButton(new Rectangle(610, 815, 700, 100), "Edit existing Executables for this game,\nor add new ones.", Ozzz.DescriptionBox.SpawnPositions.CenterLeftTop, 0.4f);
            game.gameManageWindow.AddText("Edit Launch Settings");
            game.gameManageWindow.AddText("Database Lookup");
            game.gameManageWindow.AddText("Edit Game Info");
            game.gameManageWindow.AddText("Change Filepaths");
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
