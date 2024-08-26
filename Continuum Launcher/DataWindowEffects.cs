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
using DescriptionBox = XeniaLauncher.OzzzFramework.DescriptionBox;
using DBSpawnPos = XeniaLauncher.OzzzFramework.DescriptionBox.SpawnPositions;
using System.Globalization;
using SharpDX.MediaFoundation;
using CppNet;

namespace XeniaLauncher
{
    public class DataWindowEffects : IWindowEffects
    {
        public static void RefreshData(Game1 game, Window source)
        {
            source = null;
            game.menuWindow.buttonEffects.ActivateButton(game, game.menuWindow, game.menuWindow.buttons[3], 3);
            game.state = Game1.State.Data;
        }
        public void SetupEffects(Game1 game, Window source)
        {

        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex < game.localData.Count)
            {
                game.selectedDataIndex = buttonIndex;
                game.manageWindow = new Window(game, game.dataWindow.rect, game.localData[buttonIndex].gameTitle, new ManageDataEffects(), new StdInputEvent(Math.Min(game.dataFiles[buttonIndex].Count, 6)), new GenericStart(), Game1.State.Data);
                game.manageWindow.changeEffects = new DataWindowChangeEffects(game, game.manageWindow);
                game.manageWindow.buttonEffects.SetupEffects(game, source);
                game.state = Game1.State.Manage;
            }
            else if (buttonIndex == game.localData.Count)
            {
                game.dataSortWindow = new Window(game, new Rectangle(560, 115, 800, 860), "Sort Data", new DataSortEffects(), new StdInputEvent(6), new GenericStart(), Game1.State.Data);
                game.dataSortWindow.AddButton(new Rectangle(610, 265, 700, 100));
                game.dataSortWindow.AddButton(new Rectangle(610, 375, 700, 100));
                game.dataSortWindow.AddButton(new Rectangle(610, 485, 700, 100));
                game.dataSortWindow.AddButton(new Rectangle(610, 595, 700, 100));
                game.dataSortWindow.AddButton(new Rectangle(610, 705, 700, 100));
                game.dataSortWindow.AddButton(new Rectangle(610, 815, 700, 100));
                game.dataSortWindow.AddText("Alphabetical A-Z");
                game.dataSortWindow.AddText("Alphabetical Z-A");
                game.dataSortWindow.AddText("File Size High-Low");
                game.dataSortWindow.AddText("File Size Low-High");
                game.dataSortWindow.AddText("Item Count High-Low");
                game.dataSortWindow.AddText("Item Count Low-High");
                foreach (TextSprite sprite in game.dataSortWindow.sprites)
                {
                    sprite.scale = 0.6f;
                }
                game.state = Game1.State.DataSort;
            }
            else if (buttonIndex == game.localData.Count + 1)
            {
                game.dataFilterWindow = new Window(game, new Rectangle(560, 170, 800, 750), "Filter Data", new DataFilterEffects(), new StdInputEvent(5), new GenericStart(), Game1.State.Data);
                game.dataFilterWindow.AddButton(new Rectangle(610, 320, 700, 100), "Includes all game data.", DBSpawnPos.CenterRightBottom, 0.4f);
                game.dataFilterWindow.AddButton(new Rectangle(610, 430, 700, 100), "Includes:\n  - Games on Demand\n  - XBLA Titles\n  - Installed Disc Games", DBSpawnPos.CenterLeftBottom, 0.4f);
                game.dataFilterWindow.AddButton(new Rectangle(610, 540, 700, 100), "Includes:\n  - Title Updates\n  - Downloadable Content", DBSpawnPos.CenterRightBottom, 0.4f);
                game.dataFilterWindow.AddButton(new Rectangle(610, 650, 700, 100), "Includes:\n  - Xenia Copies\n  - Extracted Content\n  - Game Saves\n  - Installed Xenia Content", DBSpawnPos.CenterLeftTop, 0.4f);
                game.dataFilterWindow.AddButton(new Rectangle(610, 760, 700, 100), "Includes:\n  - Game Trailers\n  - Videos", DBSpawnPos.CenterRightTop, 0.4f);
                game.dataFilterWindow.AddText("All Data");
                game.dataFilterWindow.AddText("Games Only");
                game.dataFilterWindow.AddText("Installable Content");
                game.dataFilterWindow.AddText("Temporary Data");
                game.dataFilterWindow.AddText("Videos/Trailers");
                foreach (TextSprite sprite in game.dataFilterWindow.sprites)
                {
                    sprite.scale = 0.6f;
                }
                game.state = Game1.State.DataFilter;
            }
            else if (buttonIndex == game.localData.Count + 2)
            {
                RefreshData(game, source);
            }
            else if (buttonIndex == game.localData.Count + 3)
            {
                game.state = source.returnState;
                game.backSound.Play();
            }
        }
        public static void UpdateText(Game1 game, Window manageWindow, int buttonIndex)
        {
            //manageWindow.extraSprites.Clear();
            int offset = manageWindow.stringIndex - manageWindow.buttonIndex;
            if (offset < 0)
            {
                offset = 0;
            }
            while (offset + Math.Min(game.dataFiles[buttonIndex].Count - 2, 6) > game.dataFiles[buttonIndex].Count)
            {
                offset--;
            }
            for (int i = 0; i < Math.Min(game.dataFiles[buttonIndex].Count - 2, 6); i++)
            {
                game.manageWindow.extraSprites[i * 4].ToTextSprite().text = game.dataFiles[buttonIndex][i + offset].name;
                game.manageWindow.extraSprites[i * 4 + 1].ToTextSprite().text = game.dataFiles[buttonIndex][i + offset].size;
                game.manageWindow.extraSprites[i * 4 + 1].ToTextSprite().JustifyRight(new Vector2(1820, 175 + i * 130));
                game.manageWindow.extraSprites[i * 4 + 2].ToTextSprite().text = game.dataFiles[buttonIndex][i + offset].subTitle;
                game.manageWindow.extraSprites[i * 4 + 3].ToObjectSprite().textures[0] = game.dataFiles[buttonIndex][i + offset].icon;
            }
            // Adding tag with stringIndex info
            manageWindow.tags[0] = "" + manageWindow.stringIndex;
        }
    }
}
