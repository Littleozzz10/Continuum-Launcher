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
using System.Globalization;
using SharpDX.MediaFoundation;

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
                    if (i < 6)
                    {
                        game.manageWindow.AddButton(new Rectangle(20, 150 + i * 130, 1880, 120));
                        game.manageWindow.extraSprites.Add(new TextSprite(game.font, game.dataFiles[buttonIndex][i].name, 0.6f, new Vector2(140, 160 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0)));
                        TextSprite sizeText = new TextSprite(game.font, game.dataFiles[buttonIndex][i].size, 0.6f, new Vector2(1140, 160 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0));
                        sizeText.JustifyRight(new Vector2(1820, 175 + i * 130));
                        game.manageWindow.extraSprites.Add(sizeText);
                        TextSprite subTitleSprite = new TextSprite(game.font, game.dataFiles[buttonIndex][i].subTitle, 0.4f, new Vector2(140, 220 + i * 130), Color.FromNonPremultiplied(0, 0, 0, 0));
                        subTitleSprite.tags.Add("gray");
                        game.manageWindow.extraSprites.Add(subTitleSprite);
                        game.manageWindow.extraSprites.Add(new ObjectSprite(game.dataFiles[buttonIndex][i].icon, new Rectangle(32, 162 + i * 130, 96, 96), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    }
                    game.manageWindow.AddText("" + i);
                }
                game.manageWindow.tags.Add("0");
                game.state = Game1.State.Manage;
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
            for (int i = 0; i < Math.Min(game.dataFiles[buttonIndex].Count, 6); i++)
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
