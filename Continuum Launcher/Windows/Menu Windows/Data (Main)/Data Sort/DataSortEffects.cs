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
using System.Text.RegularExpressions;
using System.Data;

namespace XeniaLauncher
{
    public class DataSortEffects : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    game.dataSort = Game1.DataSort.NameAZ; break;
                case 1:
                    game.dataSort = Game1.DataSort.NameZA; break;
                case 2:
                    game.dataSort = Game1.DataSort.SizeHighLow; break;
                case 3:
                    game.dataSort = Game1.DataSort.SizeLowHigh; break;
                case 4:
                    game.dataSort = Game1.DataSort.FileCountHighLow; break;
                case 5:
                    game.dataSort = Game1.DataSort.FileCountLowHigh; break;
            }
            DataWindowEffects.RefreshData(game, game.dataWindow);
        }
        public void SetupEffects(Game1 game, Window window)
        {
            
        }
    }

    public class DataFilterEffects : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    game.dataFilter = Shared.DataFilter.All; break;
                case 1:
                    game.dataFilter = Shared.DataFilter.Games; break;
                case 2:
                    game.dataFilter = Shared.DataFilter.DLC; break;
                case 3:
                    game.dataFilter = Shared.DataFilter.TempContent; break;
                case 4:
                    game.dataFilter = Shared.DataFilter.Videos; break;
            }
            DataWindowEffects.RefreshData(game, game.dataWindow);
        }
        public void SetupEffects(Game1 game, Window window)
        {

        }
    }
}
