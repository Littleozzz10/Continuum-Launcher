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
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;

namespace XeniaLauncher
{
    public class ManageFileEffects : IWindowEffects
    {
        public enum OptionTypes
        {
            General, Image, STFS, SVOD, InstallSTFS, XeniaData, VideoSTFS, Extract
        }
        public static string OptionTypeToString(OptionTypes type)
        {
            switch (type)
            {
                case OptionTypes.General:
                    return "General Files";
                case OptionTypes.Image:
                    return "Image";
                case OptionTypes.STFS:
                    return "STFS";
                case OptionTypes.SVOD:
                    return "STFS + SVOD";
                case OptionTypes.InstallSTFS:
                    return "STFS Installable";
                case OptionTypes.XeniaData:
                    return "Xenia Data";
                case OptionTypes.VideoSTFS:
                    return "STFS Video";
                case OptionTypes.Extract:
                    return "Deletable Extract";
            }
            return "Unknown";
        }
        public static OptionTypes SubtitleToOptionType(string subTitle)
        {
            switch (subTitle)
            {
                case "Installed Disc Game":
                    return OptionTypes.SVOD;
                case "Installed Game on Demand":
                    return OptionTypes.SVOD;
                case "Xbox Live Arcade Title":
                    return OptionTypes.STFS;
                case "Xbox 360 Theme":
                    return OptionTypes.STFS;
                case "Gamer Picture":
                    return OptionTypes.STFS;
                case "Title Update":
                    return OptionTypes.InstallSTFS;
                case "Downloadable Content":
                    return OptionTypes.InstallSTFS;
                case "Localized Xenia Data":
                    return OptionTypes.XeniaData;
                case "Xenia Installed Content":
                    return OptionTypes.XeniaData;
                case "Xenia Game Save":
                    return OptionTypes.XeniaData;
                case "Video":
                    return OptionTypes.VideoSTFS;
                case "Extracted Content":
                    return OptionTypes.Extract;
            }
            return OptionTypes.General;
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (source.strings[buttonIndex] == Shared.FileManageStrings["explorer"])
            {
                game.selectSound.Play();
                try
                {
                    Process.Start("explorer", new FileInfo(game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex].filepath).DirectoryName);
                }
                catch
                {
                    game.message = new MessageWindow(game, "Error", "Failed to launch Explorer. Filepath may be broken.", Game1.State.ManageFile);
                    game.state = Game1.State.Message;
                }
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["install"])
            {
                if (!String.IsNullOrEmpty(game.extractPath))
                {
                    game.toImport = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
                    game.message = new MessageWindow(game, "Import DLC", "Do you want to import " + game.toImport.name + "?", Game1.State.ManageFile, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.message = new MessageWindow(game, "Import DLC", "Error: extraction tool not found or bad extract path", Game1.State.ManageFile);
                    game.state = Game1.State.Message;
                }
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["extract"])
            {
                if (!String.IsNullOrEmpty(game.extractPath))
                {
                    game.toExtract = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
                    game.message = new MessageWindow(game, "Extract STFS Container", "Do you want to extract " + game.toExtract.name + "? This will overwrite any existing extract.", Game1.State.ManageFile, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.message = new MessageWindow(game, "Extract STFS Container", "Error: extraction tool not found or bad extract path", Game1.State.ManageFile);
                    game.state = Game1.State.Message;
                }
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["delete"])
            {
                game.toDelete = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
                game.message = new MessageWindow(game, "Delete File", "Are you sure you want to delete " + game.toDelete.name + "?", Game1.State.ManageFile, MessageWindow.MessagePrompts.YesNo);
                game.state = Game1.State.Message;
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["video"])
            {
                string[] split = game.localData[game.selectedDataIndex].gamePath.Split("\\");
                string currentDir = "";
                for (int i = 0; i < split.Length - 2; i++)
                {
                    currentDir += split[i] + "\\";
                }
                currentDir += "_EXTRACT";
                currentDir += "\\000C0000\\" + game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex].name;
                currentDir = game.GetFilepathString(currentDir, true).Insert(1, ":");
                if (Directory.Exists(currentDir))
                {
                    if (File.Exists(currentDir + "\\default.wmv"))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("explorer", currentDir + "\\default.wmv");
                        startInfo.WorkingDirectory = currentDir;
                        Process.Start(startInfo);
                    }
                }
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            DataEntry data = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
            window.extraSprites.Add(new TextSprite(game.font, "File: " + data.name, 0.375f, new Vector2(1240, 200), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Type: " + data.subTitle, 0.375f, new Vector2(1240, 240), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Size: " + data.size, 0.375f, new Vector2(1240, 280), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Options: " + OptionTypeToString(SubtitleToOptionType(data.subTitle)), 0.375f, new Vector2(1240, 360), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new ObjectSprite(data.icon, new Rectangle(300, 412, 256, 256), Color.FromNonPremultiplied(255, 255, 255, 0)));
        }
    }
}
