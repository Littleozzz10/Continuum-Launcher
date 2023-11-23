using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XLCompanion
{
    using SaveDataChunk = SaveData.SaveDataChunk;
    using SaveDataObject = SaveData.SaveDataObject;
    using DataType = SaveData.DataType;
    public class GameData
    {
        public List<string> folders, xexPaths, xexNames;
        public string gameTitle, developer, publisher, titleId, gamePath, artPath, iconPath;
        public double fileSize;
        public int year, month, day, minPlayers, maxPlayers, timesLaunched, resX, resY;
        public bool preferCanary, hasCoverArt, cpuReadback, vsync, mountCache;
        public enum KinectCompat
        {
            None, Supported, Required
        }
        public KinectCompat kinect;
        public enum LicenseMask
        {
            None, First, All
        }
        public LicenseMask license;
        public enum Renderer
        {
            Any, Direct3D12, Vulkan
        }
        public Renderer renderer;
        public enum XeniaCompat
        {
            Unknown, Broken, Starts, Menu, Gameplay1, Gameplay2, Gameplay3, Playable
        }
        public XeniaCompat xeniaCompat, canaryCompat;
        
        public GameData()
        {
            folders = new List<string>();
            xexPaths = new List<string>();
            xexNames = new List<string>();
            gameTitle = "No Title";
            developer = "No Developer";
            publisher = "No Publisher";
            titleId = "00000000";
            gamePath = "NULL";
            artPath = "NULL";
            iconPath = "NULL";
            fileSize = 0;
            year = 2005;
            month = 11;
            day = 22;
            timesLaunched = 0;
            minPlayers = 0;
            maxPlayers = 0;
            resX = 1;
            resY = 1;
            preferCanary = false;
            hasCoverArt = false;
            cpuReadback = false;
            vsync = true;
            mountCache = false;
            kinect = KinectCompat.None;
            license = LicenseMask.None;
            renderer = Renderer.Any;
            xeniaCompat = XeniaCompat.Unknown;
            canaryCompat = XeniaCompat.Unknown;
        }
        public SaveDataChunk Save()
        {
            SaveDataChunk chunk = new SaveDataChunk("GameData");
            chunk.AddChunk("folders");
            SaveDataChunk folderChunk = chunk.GetLastChunk();
            folderChunk.parentChunk = chunk;
            for (int i = 0; i < folders.Count; i++)
            {
                folderChunk.AddData("folder" + i, folders[i], DataType.String);
            }
            chunk.AddChunk("xex");
            SaveDataChunk exeChunk = chunk.GetLastChunk();
            exeChunk.parentChunk = chunk;
            for (int i = 0; i < xexPaths.Count; i++)
            {
                exeChunk.AddData(xexNames[i], xexPaths[i], DataType.String);
            }
            chunk.AddData("gameTitle", gameTitle, DataType.String);
            chunk.AddData("developer", developer, DataType.String);
            chunk.AddData("publisher", publisher, DataType.String);
            chunk.AddData("titleId", titleId, DataType.String);
            chunk.AddData("gamePath", gamePath, DataType.String);
            chunk.AddData("artPath", artPath, DataType.String);
            chunk.AddData("iconPath", iconPath, DataType.String);
            chunk.AddData("fileSize", "" + fileSize, DataType.Number);
            chunk.AddData("year", "" + year, DataType.Number);
            chunk.AddData("month", "" + month, DataType.Number);
            chunk.AddData("day", "" + day, DataType.Number);
            chunk.AddData("timesLaunched", "" + timesLaunched, DataType.Number);
            chunk.AddData("minPlayers", "" + minPlayers, DataType.Number);
            chunk.AddData("maxPlayers", "" + maxPlayers, DataType.Number);
            chunk.AddData("resX", "" + resX, DataType.Number);
            chunk.AddData("resY", "" + resY, DataType.Number);
            chunk.AddData("preferCanary", "" + preferCanary, DataType.Boolean);
            chunk.AddData("hasCoverArt", "" + hasCoverArt, DataType.Boolean);
            chunk.AddData("cpuReadback", "" + cpuReadback, DataType.Boolean);
            chunk.AddData("vsync", "" + vsync, DataType.Boolean);
            chunk.AddData("mountCache", "" + mountCache, DataType.Boolean);
            chunk.AddData("kinect", kinect.ToString(), DataType.String);
            chunk.AddData("license", license.ToString(), DataType.String);
            chunk.AddData("renderer", renderer.ToString(), DataType.String);
            chunk.AddData("xeniaCompat", xeniaCompat.ToString(), DataType.String);
            chunk.AddData("canaryCompat", canaryCompat.ToString(), DataType.String);
            return chunk;
        }
        public void Read(SaveDataChunk chunk)
        {
            foreach (SaveDataObject obj in chunk.saveDataObjects)
            {
                if (obj.dataType == DataType.Chunk)
                {
                    if (obj.name == "folders")
                    {
                        SaveDataChunk folderChunk = obj.GetChunk();
                        foreach (SaveDataObject folder in folderChunk.saveDataObjects)
                        {
                            folders.Add(folder.data);
                        }
                    }
                    else if (obj.name == "xex")
                    {
                        SaveDataChunk xexChunk = obj.GetChunk();
                        foreach (SaveDataObject xex in xexChunk.saveDataObjects)
                        {
                            xexNames.Add(xex.name);
                            xexPaths.Add(xex.data);
                        }
                    }
                }
                else if (obj.name == "gameTitle")
                {
                    gameTitle = obj.data;
                }
                else if (obj.name == "developer")
                {
                    developer = obj.data;
                }
                else if (obj.name == "publisher")
                {
                    publisher = obj.data;
                }
                else if (obj.name == "titleId")
                {
                    titleId = obj.data;
                }
                else if (obj.name == "gamePath")
                {
                    gamePath = obj.data;
                }
                else if (obj.name == "artPath")
                {
                    artPath = obj.data;
                }
                else if (obj.name == "iconPath")
                {
                    iconPath = obj.data;
                }
                else if (obj.name == "fileSize")
                {
                    fileSize = Convert.ToDouble(obj.data);
                }
                else if (obj.name == "year")
                {
                    year = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "month")
                {
                    month = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "day")
                {
                    day = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "timesLaunched")
                {
                    timesLaunched = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "minPlayers")
                {
                    minPlayers = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "maxPlayers")
                {
                    maxPlayers = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "resX")
                {
                    resX = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "resY")
                {
                    resY = Convert.ToInt32(obj.data);
                }
                else if (obj.name == "preferCanary")
                {
                    preferCanary = Convert.ToBoolean(obj.data);
                }
                else if (obj.name == "hasCoverArt")
                {
                    hasCoverArt = Convert.ToBoolean(obj.data);
                }
                else if (obj.name == "vsync")
                {
                    vsync = Convert.ToBoolean(obj.data);
                }
                else if (obj.name == "cpuReadback")
                {
                    cpuReadback = Convert.ToBoolean(obj.data);
                }
                else if (obj.name == "mountCache")
                {
                    mountCache = Convert.ToBoolean(obj.data);
                }
                else if (obj.name == "kinect")
                {
                    if (obj.data == "Supported")
                    {
                        kinect = KinectCompat.Supported;
                    }
                    else if (obj.data == "Required")
                    {
                        kinect = KinectCompat.Required;
                    }
                    else
                    {
                        kinect = KinectCompat.None;
                    }
                }
                else if (obj.name == "license")
                {
                    if (obj.data == "None")
                    {
                        license = LicenseMask.None;
                    }
                    else if (obj.data == "First")
                    {
                        license = LicenseMask.First;
                    }
                    else
                    {
                        license = LicenseMask.All;
                    }
                }
                else if (obj.name == "renderer")
                {
                    if (obj.data == "Any")
                    {
                        renderer = Renderer.Any;
                    }
                    else if (obj.data == "Direct3D12")
                    {
                        renderer = Renderer.Direct3D12;
                    }
                    else
                    {
                        renderer = Renderer.Vulkan;
                    }
                }
                else if (obj.name == "xeniaCompat")
                {
                    if (obj.data == "Unknown")
                    {
                        xeniaCompat = XeniaCompat.Unknown;
                    }
                    else if (obj.data == "Broken")
                    {
                        xeniaCompat = XeniaCompat.Broken;
                    }
                    else if (obj.data == "Starts")
                    {
                        xeniaCompat = XeniaCompat.Starts;
                    }
                    else if (obj.data == "Menu")
                    {
                        xeniaCompat = XeniaCompat.Menu;
                    }
                    else if (obj.data == "Gameplay1")
                    {
                        xeniaCompat = XeniaCompat.Gameplay1;
                    }
                    else if (obj.data == "Gameplay2")
                    {
                        xeniaCompat = XeniaCompat.Gameplay2;
                    }
                    else if (obj.data == "Gameplay3")
                    {
                        xeniaCompat = XeniaCompat.Gameplay3;
                    }
                    else
                    {
                        xeniaCompat = XeniaCompat.Playable;
                    }
                }
                else if (obj.name == "canaryCompat")
                {
                    if (obj.data == "Unknown")
                    {
                        canaryCompat = XeniaCompat.Unknown;
                    }
                    else if (obj.data == "Broken")
                    {
                        canaryCompat = XeniaCompat.Broken;
                    }
                    else if (obj.data == "Starts")
                    {
                        canaryCompat = XeniaCompat.Starts;
                    }
                    else if (obj.data == "Menu")
                    {
                        canaryCompat = XeniaCompat.Menu;
                    }
                    else if (obj.data == "Gameplay1")
                    {
                        canaryCompat = XeniaCompat.Gameplay1;
                    }
                    else if (obj.data == "Gameplay2")
                    {
                        canaryCompat = XeniaCompat.Gameplay2;
                    }
                    else if (obj.data == "Gameplay3")
                    {
                        canaryCompat = XeniaCompat.Gameplay3;
                    }
                    else
                    {
                        canaryCompat = XeniaCompat.Playable;
                    }
                }
            }
        }
    }
}
