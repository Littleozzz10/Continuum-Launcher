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
    public class GameInfoWindow : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            
        }
        public void SetupEffects(Game1 game, Window window)
        {
            for (int i = 0; i < 8; i++)
            {
                window.extraSprites.Add(new TextSprite(game.bold, ""));
            }
            foreach (TextSprite sprite in window.extraSprites)
            {
                sprite.scale = 0.6f;
                sprite.color = Color.FromNonPremultiplied(255, 255, 255, 0);
            }
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
            source.extraSprites[0].Centerize(new Vector2(615, 380));
        }
        private void AdjustCanary(Game1 game, Window source)
        {
            string canary = "No";
            if (game.gameData[game.index].preferCanary)
            {
                canary = "Yes";
            }
            source.extraSprites[1].ToTextSprite().text = "Prefer Canary: " + canary;
            source.extraSprites[1].Centerize(new Vector2(615, 480));
        }
        private void AdjustMountCache(Game1 game, Window source)
        {
            string cache = "Off";
            if (game.gameData[game.index].mountCache)
            {
                cache = "On";
            }
            source.extraSprites[2].ToTextSprite().text = "Mount Cache: " + cache;
            source.extraSprites[2].Centerize(new Vector2(615, 580));
        }
        private void AdjustReadback(Game1 game, Window source)
        {
            string readback = "Off";
            if (game.gameData[game.index].cpuReadback)
            {
                readback = "On";
            }
            source.extraSprites[3].ToTextSprite().text = "CPU Readback: " + readback;
            source.extraSprites[3].Centerize(new Vector2(615, 680));
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
            source.extraSprites[4].Centerize(new Vector2(1305, 380));
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
            source.extraSprites[5].Centerize(new Vector2(1305, 480));
        }
        private void AdjustVSync(Game1 game, Window source)
        {
            string vsync = "Off";
            if (game.gameData[game.index].vsync)
            {
                vsync = "On";
            }
            source.extraSprites[6].ToTextSprite().text = "V-Sync: " + vsync;
            source.extraSprites[6].Centerize(new Vector2(1305, 580));
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
            source.extraSprites[7].Centerize(new Vector2(1305, 680));
        }
    }
}
