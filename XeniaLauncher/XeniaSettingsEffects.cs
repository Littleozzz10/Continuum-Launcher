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
    public class XeniaSettingsEffects : IWindowEffects
    {
        public int log;
        public void SetupEffects(Game1 game, Window window)
        {
            log = (int)game.logLevel;
            AdjustLog(game, window);
            AdjustFullscreen(game, window, false);
            AdjustConsolidate(game, window, false);
            AdjustDebug(game, window, false);
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    if (log == 0)
                    {
                        log = 3;
                    }
                    else
                    {
                        log--;
                    }
                    AdjustLog(game, source);
                    break;
                case 1:
                    if (log == 3)
                    {
                        log = 0;
                    }
                    else
                    {
                        log++;
                    }
                    AdjustLog(game, source);
                    break;
                case 2:
                    AdjustFullscreen(game, source, true);
                    break;
                case 3:
                    AdjustFullscreen(game, source, true);
                    break;
                case 4:
                    AdjustConsolidate(game, source, true);
                    break;
                case 5:
                    AdjustConsolidate(game, source, true);
                    break;
                case 6:
                    AdjustDebug(game, source, true);
                    break;
                case 7:
                    AdjustDebug(game, source, true);
                    break;
                case 8:
                    game.state = Game1.State.Options;
                    game.backSound.Play();
                    break;
            }
        }
        private void AdjustLog(Game1 game, Window source)
        {
            switch (log)
            {
                case 0:
                    source.extraSprites[1].ToTextSprite().text = "0 (Errors)";
                    break;
                case 1:
                    source.extraSprites[1].ToTextSprite().text = "1 (Warnings)";
                    break;
                case 2:
                    source.extraSprites[1].ToTextSprite().text = "2 (Info)";
                    break;
                case 3:
                    source.extraSprites[1].ToTextSprite().text = "3 (Debug)";
                    break;
            }
            source.extraSprites[1].ToTextSprite().Centerize(new Vector2(1155, 400));
            game.SaveConfig();
        }
        private void AdjustFullscreen(Game1 game, Window source, bool toggle)
        {
            if (game.xeniaFullscreen)
            {
                source.extraSprites[3].ToTextSprite().text = "Yes";
                if (toggle)
                {
                    source.extraSprites[3].ToTextSprite().text = "No";
                    game.xeniaFullscreen = false;
                }
            }
            else if (toggle)
            {
                source.extraSprites[3].ToTextSprite().text = "No";
                if (toggle)
                {
                    source.extraSprites[3].ToTextSprite().text = "Yes";
                    game.xeniaFullscreen = true;
                }
            }
            source.extraSprites[3].ToTextSprite().Centerize(new Vector2(1155, 500));
            game.SaveConfig();
        }
        private void AdjustConsolidate(Game1 game, Window source, bool toggle)
        {
            if (game.consolidateFiles)
            {
                source.extraSprites[5].ToTextSprite().text = "Yes";
                if (toggle)
                {
                    game.consolidateFiles = false;
                    source.extraSprites[5].ToTextSprite().text = "No";
                }
            }
            else
            {
                source.extraSprites[5].ToTextSprite().text = "No";
                if (toggle)
                {
                    game.consolidateFiles = true;
                    source.extraSprites[5].ToTextSprite().text = "Yes";
                }
            }
            source.extraSprites[5].ToTextSprite().Centerize(new Vector2(1155, 600));
            game.SaveConfig();
        }
        private void AdjustDebug(Game1 game, Window source, bool toggle)
        {
            if (game.runHeadless)
            {
                source.extraSprites[7].ToTextSprite().text = "Yes";
                if (toggle)
                {
                    game.runHeadless = false;
                    source.extraSprites[7].ToTextSprite().text = "No";
                }
            }
            else
            {
                source.extraSprites[7].ToTextSprite().text = "No";
                if (toggle)
                {
                    game.runHeadless = true;
                    source.extraSprites[7].ToTextSprite().text = "Yes";
                }
            }
            source.extraSprites[7].ToTextSprite().Centerize(new Vector2(1155, 700));
            game.SaveConfig();
        }
    }
}
