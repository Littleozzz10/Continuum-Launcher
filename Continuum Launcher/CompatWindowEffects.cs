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
using SaveData = XeniaLauncher.Shared.SaveData;
using SaveDataObject = XeniaLauncher.Shared.SaveData.SaveDataObject;
using SaveDataChunk = XeniaLauncher.Shared.SaveData.SaveDataChunk;
using DataType = XeniaLauncher.Shared.SaveData.DataType;
using GameData = XeniaLauncher.Shared.GameData;

namespace XeniaLauncher
{
    public class CompatWindowEffects : IWindowEffects
    {
        public void SetupEffects(Game1 game, Window window)
        {

        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (source.titleSprite.text.IndexOf("Canary") == -1)
            {
                game.gameData[game.index].xeniaCompat = (GameData.XeniaCompat)buttonIndex;
            }
            else
            {
                game.gameData[game.index].canaryCompat = (GameData.XeniaCompat)buttonIndex;
            }
            game.SetCompatTextures();
            game.state = source.returnState;
            game.backSound.Play();

            // Saving compatibility to config file
            for (int i = 0; i < game.masterData.Count; i++)
            {
                if (game.masterData[i].Equals(game.gameData[game.index]))
                {
                    game.masterData[i] = game.gameData[game.index];
                    break;
                }
            }
            SaveData save = new SaveData(game.configPath);
            save.AddSaveObject(new SaveDataObject("xenia", game.xeniaPath, DataType.String));
            save.AddSaveObject(new SaveDataObject("canary", game.canaryPath, DataType.String));
            SaveDataChunk chunk = new SaveDataChunk("games");
            foreach (GameData data in game.masterData)
            {
                chunk.AddChunk(data.Save());
            }
            save.AddSaveChunk(chunk);
            save.SaveToFile();
        }
    }
}
