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
    public class DataWindowEffects : IWindowEffects
    {
        public void SetupEffects(Game1 game, Window source)
        {

        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex < game.masterData.Count)
            {
                game.selectedDataIndex = buttonIndex;
                game.manageWindow = new Window(game, game.dataWindow.rect, game.masterData[buttonIndex].gameTitle, new ManageDataEffects(), new StdInputEvent(Math.Min(game.dataFiles[buttonIndex].Count, 6)), new GenericStart(), Game1.State.Data);
                for (int i = 0; i < game.dataFiles[buttonIndex].Count; i++)
                {
                    game.manageWindow.AddButton(new Rectangle(20, 150 + i * 130, 1880, 120));
                    game.manageWindow.AddText("");
                    game.manageWindow.extraSprites.Add(new TextSprite(game.font, game.dataFiles[buttonIndex][i].name, 0.6f, new Vector2(140, 160 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    TextSprite sizeText = new TextSprite(game.font, game.dataFiles[buttonIndex][i].size, 0.6f, new Vector2(1140, 160 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0));
                    sizeText.JustifyRight(new Vector2(1820, 175 + i * 130));
                    game.manageWindow.extraSprites.Add(sizeText);
                    TextSprite subTitleSprite = new TextSprite(game.font, game.dataFiles[buttonIndex][i].subTitle, 0.4f, new Vector2(140, 220 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0));
                    subTitleSprite.tags.Add("gray");
                    game.manageWindow.extraSprites.Add(subTitleSprite);
                    game.manageWindow.extraSprites.Add(new ObjectSprite(game.dataFiles[buttonIndex][i].icon, new Rectangle(32, 162 + i * 130, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                }
                game.state = Game1.State.Manage;
            }
        }
    }
}
