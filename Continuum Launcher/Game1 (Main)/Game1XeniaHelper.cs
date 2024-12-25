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
        /// Updates Xenia filepaths
        /// </summary>
        public void FindXenia()
        {
            if (File.Exists("Apps\\Xenia\\xenia.exe"))
            {
                xeniaPath = "Apps\\Xenia\\xenia.exe";
                Logging.Write(LogType.Critical, Event.XeniaPath, "Xenia path found", "xeniaPath", xeniaPath);
            }
            if (File.Exists("Apps\\Canary\\xenia_canary.exe"))
            {
                canaryPath = "Apps\\Canary\\xenia_canary.exe";
                Logging.Write(LogType.Critical, Event.XeniaPath, "Canary path found", "canaryPath", canaryPath);
            }
            if (File.Exists("Apps\\Dump\\xenia-vfs-dump.exe"))
            {
                extractPath = "Apps\\Dump\\xenia-vfs-dump.exe";
                Logging.Write(LogType.Critical, Event.XeniaPath, "Dump path found", "extractPath", extractPath);
            }
            else
            {
                extractPath = null;
            }
        }

        /// <summary>
        /// Returns the parameter string to attach to the command that launches Xenia
        /// </summary>
        /// <returns></returns>
        public string GetParamString()
        {
            string param = "";
            param = param + " --draw_resolution_scale_x=" + gameData[index].resX;
            param = param + " --draw_resolution_scale_y=" + gameData[index].resY;
            param = param + " --vsync=" + gameData[index].vsync.ToString().ToLower();
            param = param + " --d3d12_readback_resolve=" + gameData[index].cpuReadback.ToString().ToLower();
            param = param + " --mount_cache=" + gameData[index].mountCache.ToString().ToLower();
            if (gameData[index].renderer == GameData.Renderer.Any)
            {
                param = param + " --gpu=any";
            }
            else if (gameData[index].renderer == GameData.Renderer.Direct3D12)
            {
                param = param + " --gpu=d3d12";
            }
            else if (gameData[index].renderer == GameData.Renderer.Vulkan)
            {
                param = param + " --gpu=vulkan";
            }
            if (gameData[index].license == GameData.LicenseMask.None)
            {
                param = param + " --license_mask=0";
            }
            else if (gameData[index].license == GameData.LicenseMask.First)
            {
                param = param + " --license_mask=1";
            }
            else if (gameData[index].license == GameData.LicenseMask.All)
            {
                param = param + " --license_mask=-1";
            }
            param = param + " --user_language=" + (int)gameData[index].language;
            param = param + " --log_level=" + (int)logLevel;
            param = param + " --fullscreen=" + xeniaFullscreen.ToString().ToLower();
            param = param + " --headless=" + runHeadless.ToString().ToLower();
            if (gameData[index].extraParams != "")
            {
                param = param + " " + gameData[index].extraParams;
            }
            Logging.Write(LogType.Critical, Event.XeniaParam, "Xenia param string: " + param);
            return param;
        }

        public void LaunchXenia(string path)
        {
            FindXenia();

            string param = GetParamString();
            launchSound.Play();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "\"" + path + "\"" + param;
            string title = gameData[index].gameTitle;
            string newTitle = GetFilepathString(title);
            startInfo.WorkingDirectory = "XData\\Xenia\\" + newTitle;
            Directory.CreateDirectory("XData\\Xenia\\" + newTitle);
            if (consolidateFiles)
            {
                try
                {
                    if (!gameData[index].preferCanary)
                    {
                        File.Copy(xeniaPath, "XData\\Xenia\\" + newTitle + "\\xenia.exe");
                        Logging.Write(LogType.Important, Event.Launch, "Copied Xenia executable", "dir", startInfo.WorkingDirectory);
                    }
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Falied to copy Xenia executable", "dir", startInfo.WorkingDirectory);
                }
                try
                {
                    File.Create("XData\\Xenia\\" + newTitle + "\\portable.txt");
                    Logging.Write(LogType.Standard, Event.Launch, "Created portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Failed to create portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                startInfo.FileName = "XData\\Xenia\\" + newTitle + "\\xenia.exe";
            }
            else
            {
                startInfo.FileName = xeniaPath;
            }
            try
            {
                Process.Start(startInfo);
                OpenCompatWindow(false, compatWindowDelay);
                MarkGameAsPlayed();
                Logging.Write(LogType.Critical, Event.Launch, "Launched Xenia", new Dictionary<string, string>()
                {
                    { "gameTitle", title },
                    { "newTitle", newTitle },
                    { "titleID", gameData[index].titleId }
                });
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Failed to launch Xenia", "dir", startInfo.WorkingDirectory);
                message = new MessageWindow(this, "Launch Error", "Unable to launch Xenia", state);
                state = State.Message;
            }
        }
        public void LaunchXenia()
        {
            LaunchXenia(gameData[index].gamePath);
        }
        public void LaunchCanary(string path)
        {
            FindXenia();

            string param = GetParamString();
            launchSound.Play();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = "\"" + path + "\"" + param;
            string title = gameData[index].gameTitle;
            string newTitle = GetFilepathString(title);
            startInfo.WorkingDirectory = "XData\\Canary\\" + newTitle;
            Directory.CreateDirectory("XData\\Canary\\" + newTitle);
            if (consolidateFiles)
            {
                try
                {
                    if (!gameData[index].preferCanary)
                    {
                        File.Copy(canaryPath, "XData\\Canary\\" + newTitle + "\\xenia_canary.exe");
                        Logging.Write(LogType.Important, Event.Launch, "Copied Canary executable", "dir", startInfo.WorkingDirectory);
                    }
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Falied to copy Canary executable", "dir", startInfo.WorkingDirectory);
                }
                try
                {
                    File.Create("XData\\Canary\\" + newTitle + "\\portable.txt");
                    Logging.Write(LogType.Standard, Event.Launch, "Created portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                catch
                {
                    Logging.Write(LogType.Critical, Event.Error, "Failed to create portable.txt", "path", startInfo.WorkingDirectory + "\\portable.txt");
                }
                startInfo.FileName = "XData\\Canary\\" + newTitle + "\\xenia_canary.exe";
            }
            else
            {
                startInfo.FileName = canaryPath;
            }
            try
            {
                Process.Start(startInfo);
                OpenCompatWindow(true, compatWindowDelay);
                MarkGameAsPlayed();
                Logging.Write(LogType.Critical, Event.Launch, "Launched Canary", new Dictionary<string, string>()
                {
                    { "gameTitle", title },
                    { "newTitle", newTitle },
                    { "titleID", gameData[index].titleId }
                });
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Failed to launch Canary", "dir", startInfo.WorkingDirectory);
                message = new MessageWindow(this, "Launch Error", "Unable to launch Canary", state);
                state = State.Message;
            }
        }
        public void LaunchCanary()
        {
            LaunchCanary(gameData[index].gamePath);
        }
    }
}
