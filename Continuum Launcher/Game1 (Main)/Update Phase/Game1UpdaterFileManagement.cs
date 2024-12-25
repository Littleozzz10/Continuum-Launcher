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
        public void CheckFileDeletionImportExtraction()
        {
            // File deletion check
            if (messageYes && toDelete != null)
            {
                string fileTitle = GetFilepathString(localData[selectedDataIndex].gameTitle);
                try
                {
                    if (toDelete.name.Contains("Xenia {Temporary Copy}"))
                    {
                        if (File.Exists("XData\\Xenia\\" + fileTitle + "\\xenia.exe"))
                        {
                            File.Delete("XData\\Xenia\\" + fileTitle + "\\xenia.exe");
                        }
                        if (File.Exists("XData\\Xenia\\" + fileTitle + "\\xenia.log"))
                        {
                            File.Delete("XData\\Xenia\\" + fileTitle + "\\xenia.log");
                        }
                        if (File.Exists("XData\\Xenia\\" + fileTitle + "\\xenia.config.toml"))
                        {
                            File.Delete("XData\\Xenia\\" + fileTitle + "\\xenia.config.toml");
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Xenia temp copy", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Xenia Canary {Temporary Copy}"))
                    {
                        if (File.Exists("XData\\Canary\\" + fileTitle + "\\xenia_canary.exe"))
                        {
                            File.Delete("XData\\Canary\\" + fileTitle + "\\xenia_canary.exe");
                        }
                        if (File.Exists("XData\\Canary\\" + fileTitle + "\\xenia.log"))
                        {
                            File.Delete("XData\\Canary\\" + fileTitle + "\\xenia.log");
                        }
                        if (File.Exists("XData\\Canary\\" + fileTitle + "\\xenia-canary.config.toml"))
                        {
                            File.Delete("XData\\Canary\\" + fileTitle + "\\xenia-canary.config.toml");
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Canary temp copy", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Save Data (Xenia)"))
                    {
                        if (Directory.Exists("XData\\Xenia\\" + fileTitle + "\\content"))
                        {
                            foreach (string filepath in Directory.GetFiles("XData\\Xenia\\" + fileTitle + "\\content", "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete("XData\\Xenia\\" + fileTitle + "\\content", true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Xenia save data", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Save Data (Canary)"))
                    {
                        string dir = "XData\\Canary\\" + fileTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\profile";
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Canary save data", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Game Data (Canary)"))
                    {
                        string dir = "XData\\Canary\\" + fileTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\00000001";
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Canary save data", "fileTitle", fileTitle);
                    }
                    else if (toDelete.name.Contains("Installed DLC"))
                    {
                        string dir = "XData\\Canary\\" + localData[selectedDataIndex].gameTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\00000002";
                        dir = GetFilepathString(dir, true);
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted installed DLC", "dir", dir);
                    }
                    else if (toDelete.name.Contains("Installed Title Update"))
                    {
                        string dir = "XData\\Canary\\" + localData[selectedDataIndex].gameTitle + "\\content\\" + localData[selectedDataIndex].titleId + "\\000B0000";
                        dir = GetFilepathString(dir, true);
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted installed Title Update", "dir", dir);
                    }
                    else if (toDelete.name.Contains("Extract"))
                    {
                        string[] split = localData[selectedDataIndex].gamePath.Split("\\");
                        string currentDir = "";
                        for (int i = 0; i < split.Length - 2; i++)
                        {
                            currentDir += split[i] + "\\";
                        }
                        currentDir += "_EXTRACT";
                        //currentDir = GetFilepathString(currentDir, true).Insert(1, ":");
                        //currentDir += "\\" + contentId;
                        //currentDir += "\\" + dataFiles[selectedDataIndex][manageWindow.stringIndex].name;
                        //currentDir = GetFilepathString(currentDir, true).Insert(1, ":");

                        string dir = currentDir;
                        if (Directory.Exists(dir))
                        {
                            foreach (string filepath in Directory.GetFiles(dir, "", SearchOption.AllDirectories))
                            {
                                File.Delete(filepath);
                            }
                            Directory.Delete(dir, true);
                        }
                        Logging.Write(LogType.Important, Event.FileDelete, "Deleted Extract", "dir", dir);
                    }
                    refreshData = true;
                    message = new MessageWindow(this, "File Deleted", "The file was successfully deleted", State.Data);
                    state = State.Message;
                }
                catch
                {
                    message = new MessageWindow(this, "Error", "Unable to delete file. It may currently be in use", State.Data);
                    state = State.Message;
                    Logging.Write(LogType.Critical, Event.Error, "Error when deleting file", "fileTitle", fileTitle);
                }
                messageYes = false;
                toDelete = null;
            }
            // File import check (DLC and Title Updates)
            else if (messageYes && toImport != null)
            {
                if (File.Exists(toImport.filepath))
                {
                    // Making first folder (Content ID, such as 00000002 for DLC)
                    string contentId = "";
                    if (toImport.subTitle == "Downloadable Content")
                    {
                        contentId = "00000002";
                    }
                    else if (toImport.subTitle == "Title Update")
                    {
                        contentId = "000B0000";
                    }
                    // Ensuring a content folder is present
                    string currentDir = "XData\\Canary\\";
                    currentDir += localData[selectedDataIndex].gameTitle + "\\";
                    currentDir = GetFilepathString(currentDir, true);
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += "content" + "\\";
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += localData[selectedDataIndex].titleId + "\\";
                    currentDir = GetFilepathString(currentDir, true);
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += contentId + "\\";
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += toImport.filepath.Split("\\").Last();
                    currentDir = GetFilepathString(currentDir, true);
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    // Deleting any existing data
                    else
                    {
                        foreach (string filepath in Directory.GetFiles(currentDir))
                        {
                            File.Delete(filepath);
                        }
                    }
                    // Extracting content to folder
                    string args = "\"" + toImport.filepath + "\" \"" + Directory.GetCurrentDirectory() + "\\" + currentDir + "\"";
                    ProcessStartInfo startInfo = new ProcessStartInfo(extractPath, args);
                    startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    Process.Start(startInfo);
                    Logging.Write(LogType.Critical, Event.CanaryImport, "Launched dump tool", "args", args);
                }
                else
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error when importing content", "filepath", toImport.filepath);
                    message = new MessageWindow(this, "Error", "Import filepath does not exist", State.Manage);
                    state = State.Message;
                }
                messageYes = false;
                toImport = null;
            }
            // File extraction check
            else if (messageYes && toExtract != null)
            {
                if (File.Exists(toExtract.filepath))
                {
                    // Making first folder (Content ID, such as 00000002 for DLC)
                    string contentId = "";
                    if (toExtract.subTitle == "Downloadable Content")
                    {
                        contentId = "00000002";
                    }
                    else if (toExtract.subTitle == "Title Update")
                    {
                        contentId = "000B0000";
                    }
                    else if (toExtract.subTitle == "Installed Xbox 360 Game")
                    {
                        contentId = "00004000";
                    }
                    else if (toExtract.subTitle == "Installed Game on Demand")
                    {
                        contentId = "00007000";
                    }
                    else if (toExtract.subTitle == "Video")
                    {
                        contentId = "00090000";
                    }
                    else if (toExtract.subTitle == "Game Trailer")
                    {
                        contentId = "000C0000";
                    }
                    else if (toExtract.subTitle == "Xbox Live Arcade Title")
                    {
                        contentId = "000D0000";
                    }
                    else if (toExtract.subTitle == "Gamer Picture")
                    {
                        contentId = "00020000";
                    }
                    else if (toExtract.subTitle == "Xbox 360 Theme")
                    {
                        contentId = "00030000";
                    }
                    // Ensuring a content folder is present
                    string[] split = localData[selectedDataIndex].gamePath.Split("\\");
                    string currentDir = "";
                    for (int i = 0; i < split.Length - 2; i++)
                    {
                        currentDir += split[i] + "\\";
                    }
                    currentDir += "_EXTRACT";
                    currentDir = GetFilepathString(currentDir, true).Insert(1, ":");
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += "\\" + contentId;
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    currentDir += "\\" + dataFiles[selectedDataIndex][manageWindow.stringIndex].name;
                    currentDir = GetFilepathString(currentDir, true).Insert(1, ":");
                    if (!Directory.Exists(currentDir))
                    {
                        Directory.CreateDirectory(currentDir);
                    }
                    // Deleting any existing data
                    else
                    {
                        foreach (string filepath in Directory.GetFiles(currentDir))
                        {
                            File.Delete(filepath);
                        }
                    }
                    // Extracting content to folder
                    string args = "\"" + toExtract.filepath + "\" \"" + currentDir + "\"";
                    ProcessStartInfo startInfo = new ProcessStartInfo(extractPath, args);
                    startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                    Process.Start(startInfo);
                    Logging.Write(LogType.Critical, Event.Extract, "Launched dump tool", "args", args);
                }
                else
                {
                    Logging.Write(LogType.Critical, Event.Error, "Error when extracting content", "filepath", toExtract.filepath);
                    message = new MessageWindow(this, "Error", "Extract filepath does not exist", State.Manage);
                    state = State.Message;
                }
                messageYes = false;
                toExtract = null;
            }
        }
    }
}
