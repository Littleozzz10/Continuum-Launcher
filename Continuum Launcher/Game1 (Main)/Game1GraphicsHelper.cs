using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Convert = System.Convert;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
using SaveData = XeniaLauncher.Shared.SaveData;
using SaveDataObject = XeniaLauncher.Shared.SaveData.SaveDataObject;
using SaveDataChunk = XeniaLauncher.Shared.SaveData.SaveDataChunk;
using SequenceFade = XeniaLauncher.OzzzFramework.SequenceFade;
using GameData = XeniaLauncher.Shared.GameData;
using DescriptionBox = XeniaLauncher.OzzzFramework.DescriptionBox;
using DBSpawnPos = XeniaLauncher.OzzzFramework.DescriptionBox.SpawnPositions;
using LogType = Continuum_Launcher.Logging.LogType;
using Event = Continuum_Launcher.Logging.LogEvent;
using SharpDX.MediaFoundation;
using SharpFont;
using XLCompanion;
using STFS;
using Newtonsoft.Json;
using Continuum_Launcher;
using Assimp;
using static XeniaLauncher.OzzzFramework.KeyboardInput;

namespace XeniaLauncher
{
    public partial class Game1
    {
        public void SetResolution(Vector2 resolution)
        {
            _graphics.PreferredBackBufferWidth = (int)resolution.X;
            _graphics.PreferredBackBufferHeight = (int)resolution.Y;
            _graphics.ApplyChanges();
            Ozzz.scale = resolution / new Vector2(1920, 1080);
            Logging.Write(LogType.Important, Event.ResolutionSet, "Set resolution", new Dictionary<string, string>()
            {
                { "resolution", "(" + resolution.X + ", " + resolution.Y + ")" },
                { "scale", "(" + Ozzz.scale.X + ", " + Ozzz.scale.Y + ")" }
            });
        }
        public Vector2 GetResolution()
        {
            return new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }
        public bool ToggleFullscreen()
        {
            if (_graphics.IsFullScreen)
            {
                _graphics.IsFullScreen = false;
            }
            else
            {
                _graphics.IsFullScreen = true;
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }
            _graphics.ApplyChanges();
            Ozzz.scale = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) / new Vector2(1920, 1080);
            Logging.Write(LogType.Important, Event.ResolutionSet, "Fullscreen toggle", new Dictionary<string, string>()
            {
                { "isFullScreen", _graphics.IsFullScreen.ToString() },
                { "resolution", "(" + _graphics.PreferredBackBufferWidth + ", " + _graphics.PreferredBackBufferHeight + ")" },
                { "scale", "(" + Ozzz.scale.X + ", " + Ozzz.scale.Y + ")" }
            });
            return _graphics.IsFullScreen;
        }
        public bool GetFullscreen()
        {
            return _graphics.IsFullScreen;
        }
        public bool ToggleVSync()
        {
            _graphics.SynchronizeWithVerticalRetrace = !_graphics.SynchronizeWithVerticalRetrace;
            Logging.Write(LogType.Important, Event.VSyncToggle, "VSync toggle");
            return _graphics.SynchronizeWithVerticalRetrace;
        }
        public bool GetVSync()
        {
            return _graphics.SynchronizeWithVerticalRetrace;
        }
    }
}
