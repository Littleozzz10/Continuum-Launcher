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
        /// <summary>
        /// Coverts a data size to a larger unit of storage measurement in needed, and adds a suffix to the end of the string
        /// </summary>
        public string ConvertDataSize(string size)
        {
            Logging.Write(LogType.Debug, Event.DataSizeConvert, "Data size conversion: " + size);
            double num = Convert.ToDouble(size);
            // Bytes
            if (num < 1000)
            {
                return "" + num + " B";
            }
            // Kilobytes
            else if (num < (1000 * 1000))
            {
                return "" + Math.Round((num / 1024), 3 - ("" + (int)(num / 1024)).Length) + " KB";
            }
            // Megabytes
            else if (num < (1000 * 1000 * 1000))
            {
                return "" + Math.Round((num / 1024 / 1024), 3 - ("" + (int)(num / 1024 / 1024)).Length) + " MB";
            }
            // Gigabytes
            else if (num < Math.Pow(1000, 4))
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024), Math.Max(0, 3 - ("" + (int)(num / 1024 / 1024 / 1024)).Length)) + " GB";
            }
            // Terabytes
            else if (num < Math.Pow(1000, 5))
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024 / 1024), Math.Max(0, 3 - ("" + (int)(num / 1024 / 1024 / 1024 / 1024)).Length)) + " TB";
            }
            return size;
        }

        /// <summary>
        /// Returns a Color based on the given string (Ex: "255 255 255 255")
        /// </summary>
        public Color ColorFromString(string str)
        {
            Logging.Write(LogType.Debug, Event.ColorFromString, "Color from string: " + str);
            Color toReturn = Color.FromNonPremultiplied(0, 0, 0, 0);
            string[] split = str.Split(" ");
            if (split.Length >= 4)
            {
                toReturn.R = Convert.ToByte(split[0]);
                toReturn.G = Convert.ToByte(split[1]);
                toReturn.B = Convert.ToByte(split[2]);
                toReturn.A = Convert.ToByte(split[3]);
            }
            return toReturn;
        }

        /// <summary>
        /// Returns a color with the alpha set to 0
        /// </summary>
        public Color GetTransparentColor(Color color)
        {
            return Color.FromNonPremultiplied(color.R, color.G, color.B, 0);
        }

        public string GetFilepathString(string path)
        {
            return GetFilepathString(path, false);
        }
        public string GetFilepathString(string path, bool removeBackslash)
        {
            Logging.Write(LogType.Debug, Event.FilepathStringCleanse, "Cleaning filepath string", new Dictionary<string, string>()
            {
                { "path", path },
                { "removeBackslash", removeBackslash.ToString() }
            });
            char[] invalidNameChars = Path.GetInvalidFileNameChars();
            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (removeBackslash)
            {
                List<char> charList = invalidNameChars.ToList();
                charList.Remove('\\');
                invalidNameChars = charList.ToArray();
            }
            string regexSearch = new string(new string(invalidNameChars) + new string(invalidPathChars));
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string newTitle = r.Replace(path, "");
            return newTitle;
        }

        /// <summary>
        /// Marks the currently selected games as having been played
        /// </summary>
        public void MarkGameAsPlayed()
        {
            gameData[index].timesLaunched++;
            gameData[index].lastPlayed = DateTime.Now.ToBinary();
            SaveGames();
        }

        /// <summary>
        /// Returns true if the current State indicates the Launcher is in the Select menu
        /// </summary>
        public bool CheckStateSelectMenu()
        {
            return (state == State.Select || state == State.Launch || state == State.Compat || state == State.GameMenu || state == State.DatabasePicker || state == State.DatabaseResult || state == State.GameInfo || state == State.ReleaseYear || state == State.ReleaseMonth || state == State.ReleaseDay || state == State.GameFilepaths || state == State.GameXeniaSettings || state == State.GameCategories || state == State.GameXEX);
        }
    }
}
