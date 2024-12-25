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
using GameData = XeniaLauncher.Shared.GameData;

namespace XeniaLauncher
{
    public class CompatIndexChange : IButtonIndexChangeEffects
    {
        public Game1 game;
        public Window window;
        public GameData.XeniaCompat compat;
        public CompatIndexChange(Game1 game, Window window)
        {
            this.game = game;
            this.window = window;
            compat = game.gameData[game.index].xeniaCompat;
        }
        public CompatIndexChange(Game1 game, Window window, GameData.XeniaCompat compat) : this(game, window)
        {
            this.compat = compat;
        }

        public void IndexChanged(int buttonIndex)
        {
            window.extraSprites[3].ToObjectSprite().visible = true;
            window.extraSprites[5].visible = false;
            switch (buttonIndex)
            {
                case 0:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["nothing"];
                    compat = GameData.XeniaCompat.Broken;
                    break;
                case 1:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["intro"];
                    compat = GameData.XeniaCompat.Starts;
                    break;
                case 2:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["menu"];
                    compat = GameData.XeniaCompat.Menu;
                    break;
                case 3:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["reach"];
                    compat = GameData.XeniaCompat.Gameplay1;
                    break;
                case 4:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["plays"];
                    compat = GameData.XeniaCompat.Gameplay2;
                    break;
                case 5:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["most"];
                    compat = GameData.XeniaCompat.Gameplay3;
                    break;
                case 6:
                    window.extraSprites[3].ToObjectSprite().textures[0] = game.compatBars["perfect"];
                    compat = GameData.XeniaCompat.Playable;
                    break;
                case 7:
                    window.extraSprites[3].ToObjectSprite().visible = false;
                    window.extraSprites[5].visible = true;
                    compat = GameData.XeniaCompat.Unknown;
                    break;
            }
        }
    }
}
