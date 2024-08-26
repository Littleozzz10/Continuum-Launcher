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
using SharpDX.XAudio2;

namespace XeniaLauncher
{
    public class XeniaGameSettings : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                game.gameData[game.index].license--;
                if ((int)game.gameData[game.index].license < 0 )
                {
                    game.gameData[game.index].license = GameData.LicenseMask.All;
                }
                AdjustLicense(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 1)
            {
                game.gameData[game.index].license++;
                if ((int)game.gameData[game.index].license > 2)
                {
                    game.gameData[game.index].license = GameData.LicenseMask.None;
                }
                AdjustLicense(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 2 || buttonIndex == 3)
            {
                game.gameData[game.index].preferCanary = !game.gameData[game.index].preferCanary;
                AdjustCanary(game, source, false);
                game.SaveGames();
            }
            else if (buttonIndex == 4 || buttonIndex == 5)
            {
                game.gameData[game.index].mountCache = !game.gameData[game.index].mountCache;
                AdjustMountCache(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 6 || buttonIndex == 7)
            {
                game.gameData[game.index].cpuReadback = !game.gameData[game.index].cpuReadback;
                AdjustReadback(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 8)
            {
                game.gameData[game.index].language--;
                if ((int)game.gameData[game.index].language == 10)
                {
                    game.gameData[game.index].language--;
                }
                AdjustLanguage(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 9)
            {
                game.gameData[game.index].language++;
                if ((int)game.gameData[game.index].language == 10)
                {
                    game.gameData[game.index].language++;
                }
                AdjustLanguage(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 10)
            {
                game.gameData[game.index].resX--;
                if (game.gameData[game.index].resX < 1)
                {
                    game.gameData[game.index].resX = 3;
                }
                AdjustHRes(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 11)
            {
                game.gameData[game.index].resX++;
                if (game.gameData[game.index].resX > 3)
                {
                    game.gameData[game.index].resX = 1;
                }
                AdjustHRes(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 12)
            {
                game.gameData[game.index].resY--;
                if (game.gameData[game.index].resY < 1)
                {
                    game.gameData[game.index].resY = 3;
                }
                AdjustVRes(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 13)
            {
                game.gameData[game.index].resY++;
                if (game.gameData[game.index].resY > 3)
                {
                    game.gameData[game.index].resY = 1;
                }
                AdjustVRes(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 14 || buttonIndex == 15)
            {
                game.gameData[game.index].vsync = !game.gameData[game.index].vsync;
                AdjustVSync(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 16)
            {
                game.gameData[game.index].renderer--;
                if ((int)game.gameData[game.index].renderer < 0)
                {
                    game.gameData[game.index].renderer = GameData.Renderer.Vulkan;
                }
                AdjustRenderer(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 17)
            {
                game.gameData[game.index].renderer++;
                if ((int)game.gameData[game.index].renderer > 2)
                {
                    game.gameData[game.index].renderer = GameData.Renderer.Any;
                }
                AdjustRenderer(game, source);
                game.SaveGames();
            }
            else if (buttonIndex == 18)
            {
                game.text = new TextInputWindow(game, "Enter Additional Parameters", game.gameData[game.index].extraParams, Game1.State.GameXeniaSettings);
            }
            else if (buttonIndex == 19)
            {
                game.state = Game1.State.GameMenu;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            for (int i = 0; i < 9; i++)
            {
                window.extraSprites.Add(new TextSprite(game.font, ""));
            }
            foreach (TextSprite sprite in window.extraSprites)
            {
                sprite.scale = 0.6f;
                sprite.color = Color.FromNonPremultiplied(250, 255, 255, 0);
            }
            AdjustLicense(game, window);
            AdjustCanary(game, window, true);
            AdjustMountCache(game, window);
            AdjustReadback(game, window);
            AdjustHRes(game, window);
            AdjustVRes(game, window);
            AdjustVSync(game, window);
            AdjustRenderer(game, window);
            AdjustLanguage(game, window);
        }
        private void AdjustLicense(Game1 game, Window source)
        {
            int license = 0;
            if (game.gameData[game.index].license == GameData.LicenseMask.First)
            {
                license = 1;
            }
            else if (game.gameData[game.index].license == GameData.LicenseMask.All)
            {
                license = -1;
            }
            source.extraSprites[0].ToTextSprite().text = "License Mask: " + license;
            source.extraSprites[0].Centerize(new Vector2(620, 370));
        }
        private void AdjustCanary(Game1 game, Window source, bool first)
        {
            string canary = "No";
            if (game.gameData[game.index].preferCanary)
            {
                canary = "Yes";
                if (!first)
                {
                    game.message = new MessageWindow(game, "Note", "This setting disables part of Continuum's launch process. When enabled, Xenia executables must be manually copied to the XData folder", Game1.State.GameXeniaSettings);
                    game.state = Game1.State.Message;
                }
            }
            source.extraSprites[1].ToTextSprite().text = "Allow Custom Builds: " + canary;
            source.extraSprites[1].ToTextSprite().scale = 0.5f;
            source.extraSprites[1].Centerize(new Vector2(620, 470));
        }
        private void AdjustMountCache(Game1 game, Window source)
        {
            string cache = "Off";
            if (game.gameData[game.index].mountCache)
            {
                cache = "On";
            }
            source.extraSprites[2].ToTextSprite().text = "Mount Cache: " + cache;
            source.extraSprites[2].Centerize(new Vector2(620, 570));
        }
        private void AdjustReadback(Game1 game, Window source)
        {
            string readback = "Off";
            if (game.gameData[game.index].cpuReadback)
            {
                readback = "On";
            }
            source.extraSprites[3].ToTextSprite().text = "CPU Readback: " + readback;
            source.extraSprites[3].Centerize(new Vector2(620, 670));
        }
        private void AdjustHRes(Game1 game, Window source)
        {
            string res = "1280 (1x)";
            if (game.gameData[game.index].resX == 2)
            {
                res = "2560 (2x)";
            }
            else if (game.gameData[game.index].resX == 3)
            {
                res = "3840 (3x)";
            }
            source.extraSprites[4].ToTextSprite().text = "Hor. Scale: " + res;
            source.extraSprites[4].Centerize(new Vector2(1310, 370));
        }
        private void AdjustVRes(Game1 game, Window source)
        {
            string res = "720 (1x)";
            if (game.gameData[game.index].resY == 2)
            {
                res = "1440 (2x)";
            }
            else if (game.gameData[game.index].resY == 3)
            {
                res = "2160 (3x)";
            }
            source.extraSprites[5].ToTextSprite().text = "Ver. Scale: " + res;
            source.extraSprites[5].Centerize(new Vector2(1310, 470));
        }
        private void AdjustVSync(Game1 game, Window source)
        {
            string vsync = "Off";
            if (game.gameData[game.index].vsync)
            {
                vsync = "On";
            }
            source.extraSprites[6].ToTextSprite().text = "V-Sync: " + vsync;
            source.extraSprites[6].Centerize(new Vector2(1310, 570));
        }
        private void AdjustRenderer(Game1 game, Window source)
        {
            string res = "Any";
            if (game.gameData[game.index].renderer == GameData.Renderer.Direct3D12)
            {
                res = "Direct3D12";
            }
            else if (game.gameData[game.index].renderer == GameData.Renderer.Vulkan)
            {
                res = "Vulkan";
            }
            source.extraSprites[7].ToTextSprite().text = "Renderer: " + res;
            source.extraSprites[7].Centerize(new Vector2(1310, 670));
        }
        private void AdjustLanguage(Game1 game, Window source)
        {
            if ((game.gameData[game.index].language).ToString() == "17")
            {
                game.gameData[game.index].language = GameData.Language.English;
            }
            else if ((game.gameData[game.index].language).ToString() == "0")
            {
                game.gameData[game.index].language = GameData.Language.SimplifiedChinese;
            }

            string lang = "English";
            lang = (game.gameData[game.index].language).ToString();
            if (game.gameData[game.index].language == GameData.Language.TraditionalChinese)
            {
                lang = "Trad. Chinese";
            }
            else if (game.gameData[game.index].language == GameData.Language.SimplifiedChinese)
            {
                lang = "Simp. Chinese";
            }

            source.extraSprites[8].ToTextSprite().text = "Language: " + lang;
            source.extraSprites[8].Centerize(new Vector2(620, 770));
        }
    }
    public class XeniaGameInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 1 || buttonIndex == 10 || buttonIndex == 11)
            {
                buttonIndex = 19;
            }
            else if (buttonIndex == 19)
            {
                buttonIndex--;
            }
            else
            {
                buttonIndex -= 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 8 || buttonIndex == 9 || buttonIndex == 18)
            {
                buttonIndex = 19;
            }
            else if (buttonIndex == 17)
            {
                buttonIndex = 18;
            }
            else if (buttonIndex == 19)
            {
                buttonIndex = 10;
            }
            else
            {
                buttonIndex += 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 8)
            {
                buttonIndex = 18;
            }
            else if (buttonIndex < 10 && buttonIndex % 2 == 0)
            {
                buttonIndex += 11;
            }
            else if (buttonIndex == 19)
            {
                buttonIndex = 8;
            }
            else if (buttonIndex < 10 || (buttonIndex >= 10 && buttonIndex % 2 == 1))
            {
                buttonIndex--;
            }
            else
            {
                buttonIndex -= 9;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex < 10 && buttonIndex % 2 == 1)
            {
                buttonIndex += 9;
            }
            else if (buttonIndex == 18)
            {
                buttonIndex = 8;
            }
            else if (buttonIndex < 10 || (buttonIndex >= 10 && buttonIndex % 2 == 0))
            {
                buttonIndex++;
            }
            else if (buttonIndex == 19)
            {
                buttonIndex = 18;
            }
            else
            {
                buttonIndex -= 11;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
    }
}
