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
    public class GraphicsWindowEffects : IWindowEffects
    {
        public int resolutionIndex;
        public static Vector2[] resolutions = new Vector2[13]
        {
            new Vector2(576, 324),
            new Vector2(768, 432),
            new Vector2(960, 540),
            new Vector2(1152, 648),
            new Vector2(1344, 756),
            new Vector2(1536, 864),
            new Vector2(1728, 972),
            new Vector2(1920, 1080),
            new Vector2(2304, 1296),
            new Vector2(2688, 1512),
            new Vector2(3072, 1728),
            new Vector2(3456, 1944),
            new Vector2(3840, 2160)
        };
        public void SetupEffects(Game1 game, Window window)
        {
            CalculateResolutionIndex(game);
            AdjustResolution(game, window);
            AdjustFullscreen(game, window, false);
            AdjustVSync(game, window, false);
            AdjustShowRings(game, window, false);
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    if (resolutionIndex > 0)
                    {
                        resolutionIndex--;
                    }
                    else
                    {
                        resolutionIndex = resolutions.Length - 1;
                    }
                    AdjustResolution(game, source);
                    break;
                case 1:
                    if (resolutionIndex < resolutions.Length - 1)
                    {
                        resolutionIndex++;
                    }
                    else
                    {
                        resolutionIndex = 0;
                    }
                    AdjustResolution(game, source);
                    break;
                case 2:
                    AdjustFullscreen(game, source, true);
                    break;
                case 3:
                    AdjustFullscreen(game, source, true);
                    break;
                case 4:
                    AdjustVSync(game, source, true);
                    break;
                case 5:
                    AdjustVSync(game, source, true);
                    break;
                case 6:
                    AdjustShowRings(game, source, true);
                    break;
                case 7:
                    AdjustShowRings(game, source, true);
                    break;
                case 8:
                    game.state = Game1.State.Options;
                    game.backSound.Play();
                    break;
            }
        }
        private void AdjustResolution(Game1 game, Window source)
        {
            if (resolutionIndex != -1)
            {
                source.extraSprites[1].ToTextSprite().text = "" + resolutions[resolutionIndex].X + "x" + resolutions[resolutionIndex].Y;
                game.SetResolution(resolutions[resolutionIndex]);
            }
            else
            {
                source.extraSprites[1].ToTextSprite().text = "" + game.GraphicsDevice.Viewport.Width + "x" + game.GraphicsDevice.Viewport.Height;
            }
            source.extraSprites[1].ToTextSprite().Centerize(new Vector2(1155, 400));
            game.SaveConfig();
        }
        private void AdjustFullscreen(Game1 game, Window source, bool toggle)
        {
            bool fullscreen = false;
            if (toggle)
            {
                fullscreen = game.ToggleFullscreen();
            }
            else
            {
                fullscreen = game.GetFullscreen();
            }
            resolutionIndex = -1;
            AdjustResolution(game, source);
            CalculateResolutionIndex(game);
            if (fullscreen)
            {
                source.extraSprites[3].ToTextSprite().text = "On";
            }
            else
            {
                source.extraSprites[3].ToTextSprite().text = "Off";
            }
            source.extraSprites[3].ToTextSprite().Centerize(new Vector2(1155, 500));
            game.SaveConfig();
        }
        private void AdjustVSync(Game1 game, Window source, bool toggle)
        {
            bool vsync = false;
            if (toggle)
            {
                vsync = game.ToggleVSync();
            }
            else
            {
                vsync = game.GetVSync();
            }
            if (vsync)
            {
                source.extraSprites[5].ToTextSprite().text = "On";
            }
            else
            {
                source.extraSprites[5].ToTextSprite().text = "Off";
            }
            source.extraSprites[5].ToTextSprite().Centerize(new Vector2(1155, 600));
            game.SaveConfig();
        }
        private void AdjustShowRings(Game1 game, Window source, bool toggle)
        {
            bool show = false;
            if (toggle)
            {
                game.showRings = !game.showRings;
                show = game.showRings;
            }
            else
            {
                show = game.showRings;
            }
            if (show)
            {
                source.extraSprites[7].ToTextSprite().text = "On";
            }
            else
            {
                source.extraSprites[7].ToTextSprite().text = "Off";
            }
            source.extraSprites[7].ToTextSprite().Centerize(new Vector2(1155, 700));
            game.SaveConfig();
        }
        private void CalculateResolutionIndex(Game1 game)
        {
            resolutionIndex = -1;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (game.GraphicsDevice.Viewport.Width == resolutions[i].X)
                {
                    resolutionIndex = i;
                    break;
                }
            }
        }
    }
}
