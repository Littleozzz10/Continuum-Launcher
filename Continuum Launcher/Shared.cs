﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    using SaveDataChunk = Shared.SaveData.SaveDataChunk;
    using SaveDataObject = Shared.SaveData.SaveDataObject;
    using DataType = Shared.SaveData.DataType;
    public static class Shared
    {
        public enum DataFilter
        {
            All, Games, DLC, TempContent, Videos
        }

        public static readonly string VERSION = "1.2.01";
        public static readonly string COMPILED = "August 30, 2024";
        public static readonly int VERNUM = 2018;
        public static readonly Dictionary<string, string> contentTypes = new Dictionary<string, string>() {
            { "00000001", "Xbox 360 Saved Game"  },
            { "00000002", "Downloadable Content" },
            { "00001000", "Xbox 360 Title" },
            { "00002000", "IPTV Pause Buffer" },
            { "00004000", "Installed Disc Game" },
            { "00005000", "Xbox Original Game" },
            { "00007000", "Installed Game on Demand" },
            { "00009000", "Avatar Item" },
            { "00010000", "Profile" },
            { "00020000", "Gamer Picture" },
            { "00030000", "Xbox 360 Theme" },
            { "00040000", "Cache File" },
            { "00050000", "Storage Download" },
            { "00060000", "Xbox Saved Game" },
            { "00070000", "Download Progress" },
            { "00080000", "Game Demo" },
            { "00090000", "Video" },
            { "000A0000", "Game Title" },
            { "000B0000", "Title Update" },
            { "000C0000", "Game Trailer" },
            { "000D0000", "Xbox Live Arcade Title" },
            { "000E0000", "XNA Content" },
            { "000F0000", "License Store" },
            { "00100000", "Movie" },
            { "00200000", "TV" },
            { "00300000", "Music Video" },
            { "00400000", "Game Video" },
            { "00500000", "Podcast Video" },
            { "00600000", "Viral Video" },
            { "02000000", "Indie Game" },
            { "_EXTRACT", "Extracted Content" }
        };
        public static readonly Dictionary<DataFilter, List<string>> filteredContentTypes = new Dictionary<DataFilter, List<string>>() {
            { DataFilter.All, new List<string>() {
                "00000001",
                "00000002",
                "00001000",
                "00002000",
                "00004000",
                "00005000",
                "00007000",
                "00009000",
                "00010000",
                "00020000",
                "00030000",
                "00040000",
                "00050000",
                "00060000",
                "00070000",
                "00080000",
                "00090000",
                "000A0000",
                "000B0000",
                "000C0000",
                "000D0000",
                "000E0000",
                "000F0000",
                "00100000",
                "00200000",
                "00300000",
                "00400000",
                "00500000",
                "00600000",
                "02000000",
                "_EXTRACT"
            } },
            { DataFilter.Games, new List<string>() {
                "00001000",
                "00004000",
                "00005000",
                "00007000",
                "00080000",
                "000D0000",
                "000E0000",
                "02000000"
            } },
            { DataFilter.DLC, new List<string>() {
                "00000002",
                "000B0000"
            } },
            { DataFilter.TempContent, new List<string>() {
                "_EXTRACT"
            } },
            { DataFilter.Videos, new List<string>() {
                "00090000",
                "000C0000",
                "00100000",
                "00200000",
                "00300000",
                "00400000",
                "00500000",
                "00600000"
            } }
        };
        public static readonly Dictionary<string, string> FileManageStrings = new Dictionary<string, string>() {
            { "explorer", "Show in Explorer" },
            { "metadata", "View Metadata" },
            { "extract", "Extract STFS Container" },
            { "install", "Install Content" },
            { "delete", "Delete Content" },
            { "video", "Play Video" },
            { "backup", "Create Backup" }
        };
        public class SaveData
        {
            /// <summary>
            /// The different data types that can be saved in a SaveData file
            /// </summary>
            public enum DataType
            {
                String, Number, Decimal, Boolean, Chunk
            }
            /// <summary>
            /// Class for storing a read-in piece of data from a SaveData object.
            /// </summary>
            public class SaveDataObject
            {
                /// <summary>
                /// The saved data. Even if the data type if something other than a string, it is saved
                /// as such until it needs to be converted.
                /// </summary>
                public string data;
                /// <summary>
                /// The name of the data.
                /// </summary>
                public string name;
                /// <summary>
                /// The type of the data read in.
                /// </summary>
                public DataType dataType;
                /// <summary>
                /// The containing SaveDataChunk of this piece of save data. Nulled if this object is the eldest.
                /// </summary>
                public SaveDataChunk parentChunk;
                public SaveDataObject(string name, string data, DataType dataType)
                {
                    this.name = name;
                    this.data = data;
                    this.dataType = dataType;
                    parentChunk = null;
                }
                public SaveDataObject(string name, string data, DataType dataType, SaveDataChunk parentChunk) : this(name, data, dataType)
                {
                    this.parentChunk = parentChunk;
                }
                /// <summary>
                /// Returns the SaveDataObject as a SaveDataChunk, if possible.
                /// </summary>
                public SaveDataChunk GetChunk()
                {
                    if (dataType == DataType.Chunk)
                    {
                        return (SaveDataChunk)this;
                    }
                    return null;
                }
                public static int GetDataAsInt(SaveDataObject data)
                {
                    if (data.dataType == DataType.Number || data.dataType == DataType.Decimal)
                    {
                        return Convert.ToInt32(data.data);
                    }
                    throw new InvalidCastException();
                }
                public static double GetDataAsDouble(SaveDataObject data)
                {
                    if (data.dataType == DataType.Decimal)
                    {
                        return Convert.ToDouble(data.data);
                    }
                    throw new InvalidCastException();
                }
                public static bool GetDataAsBool(SaveDataObject data)
                {
                    if (data.dataType == DataType.Boolean)
                    {
                        return Convert.ToBoolean(data.data);
                    }
                    throw new InvalidCastException();
                }
            }
            /// <summary>
            /// Class for storing a chunk of SaveData. 
            /// </summary>
            public class SaveDataChunk : SaveDataObject
            {
                /// <summary>
                /// List for storing SaveDataObjects.
                /// </summary>
                public List<SaveDataObject> saveDataObjects;
                public SaveDataChunk(string name) : base(name, null, DataType.Chunk)
                {
                    saveDataObjects = new List<SaveDataObject>();
                }
                public SaveDataChunk(string name, SaveDataChunk parentChunk) : this(name)
                {
                    this.parentChunk = parentChunk;
                }
                /// <summary>
                /// Adds a SaveDataObject to the chunk's data.
                /// </summary>
                /// <param name="name">The name of the data.</param>
                /// <param name="data">The saved data.</param>
                /// <param name="dataType">The type of the data.</param>
                public void AddData(string name, string data, DataType dataType)
                {
                    saveDataObjects.Add(new SaveDataObject(name, data, dataType));
                }
                /// <summary>
                /// Adds a SaveDataChunk to the chunk's data.
                /// </summary>
                /// <param name="name">The name of the chunk.</param>
                public void AddChunk(string name)
                {
                    saveDataObjects.Add(new SaveDataChunk(name));
                }
                /// <summary>
                /// Adds a SaveDataChunk to the chunk's data.
                /// </summary>
                /// <param name="name">The name of the chunk.</param>
                public void AddChunk(SaveDataChunk chunk)
                {
                    saveDataObjects.Add(chunk);
                }
                public void AddToLastChunk(SaveDataObject obj)
                {
                    saveDataObjects.Last().GetChunk().AddData(obj.name, obj.data, obj.dataType);
                }
                public void AddChunkToLastChunk(SaveDataChunk chunk)
                {
                    saveDataObjects.Last().GetChunk().saveDataObjects.Add(chunk);
                }
                /// <summary>
                /// Returns the last SaveDataChunk added to the data.
                /// </summary>
                public SaveDataChunk GetLastChunk()
                {
                    return saveDataObjects.Last().GetChunk();
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                /// <param name="destructive">Whether or not to destroy the original data in the save file. Recommended if duplicate names may be present.</param>
                public SaveDataObject FindData(string name, bool destructive)
                {
                    SaveDataObject toReturn = null;
                    foreach (SaveDataObject obj in saveDataObjects)
                    {
                        if (obj.name == name)
                        {
                            toReturn = obj;
                            break;
                        }
                    }
                    if (destructive && toReturn != null)
                    {
                        saveDataObjects.Remove(toReturn);
                    }
                    return toReturn;
                }
            }
            /// <summary>
            /// Class for storing and managing a SaveData object's data.
            /// </summary>
            public class SaveDataCollection
            {
                /// <summary>
                /// The List used to store all of the data. SaveDataChunks are polymorphic with SaveDataObjects.
                /// </summary>
                public List<SaveDataObject> data;
                /// <summary>
                /// The SaveData object that created this collection.
                /// </summary>
                public SaveData origin;
                public SaveDataCollection(SaveData origin)
                {
                    data = new List<SaveDataObject>();
                    this.origin = origin;
                }
                /// <summary>
                /// Clears the SaveDataCollection of all data.
                /// </summary>
                public void Clear()
                {
                    data.Clear();
                }
                /// <summary>
                /// Adds a SaveDataObject.
                /// </summary>
                /// <param name="name">The name of the data.</param>
                /// <param name="data">The saved data.</param>
                /// <param name="dataType">The type of the data.</param>
                public void AddData(string name, string data, DataType dataType)
                {
                    this.data.Add(new SaveDataObject(name, data, dataType));
                }
                /// <summary>
                /// Adds a SaveDataChunk to the chunk's data.
                /// </summary>
                /// <param name="chunkName">The name of the chunk.</param>
                public void AddData(string chunkName)
                {
                    data.Add(new SaveDataChunk(chunkName));
                }
                /// <summary>
                /// Returns the last SaveDataChunk added to the data.
                /// </summary>
                public SaveDataChunk GetLastChunk()
                {
                    return data.Last().GetChunk();
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                public SaveDataObject FindData(string name)
                {
                    return FindData(name, false);
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                /// <param name="destructive">Whether or not to destroy the original data in the save file. Recommended if duplicate names may be present.</param>
                public SaveDataObject FindData(string name, bool destructive)
                {
                    SaveDataObject toReturn = null;
                    foreach (SaveDataObject obj in data)
                    {
                        if (obj.name == name)
                        {
                            toReturn = obj;
                            break;
                        }
                    }
                    if (destructive && toReturn != null)
                    {
                        data.Remove(toReturn);
                    }
                    return toReturn;
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given, searching through all chunks.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                /// <param name="destructive">Whether or not to destroy the original data in the save file. Recommended if duplicate names may be present.</param>
                public SaveDataObject FindChunkedData(string name, bool destructive)
                {
                    SaveDataObject toReturn = null;
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].name == name)
                        {
                            toReturn = data[i];
                            if (destructive)
                            {
                                data.Remove(toReturn);
                            }
                            break;
                        }
                        // Beginning recursion to search through chunks
                        else if (data[i].dataType == DataType.Chunk)
                        {
                            toReturn = FindChunkedData(name, destructive, data[i].GetChunk());
                            if (toReturn != null)
                            {
                                break;
                            }
                        }
                    }
                    return toReturn;
                }
                private SaveDataObject FindChunkedData(string name, bool destructive, SaveDataChunk currentChunk)
                {
                    SaveDataObject toReturn = null;
                    for (int i = 0; i < currentChunk.saveDataObjects.Count; i++)
                    {
                        if (currentChunk.saveDataObjects[i].name == name)
                        {
                            toReturn = currentChunk.saveDataObjects[i];
                            if (destructive)
                            {
                                currentChunk.saveDataObjects.Remove(toReturn);
                            }
                            return toReturn;
                        }
                        else if (currentChunk.saveDataObjects[i].dataType == DataType.Chunk)
                        {
                            toReturn = FindChunkedData(name, destructive, data[i].GetChunk());
                        }
                    }
                    return toReturn;
                }
            }

            /// <summary>
            /// The StreamReader object that reads in data.
            /// </summary>
            public StreamReader reader;
            /// <summary>
            /// The StreamWrite object that writes data to the file.
            /// </summary>
            public StreamWriter writer;
            /// <summary>
            /// The saved data from reading the file.
            /// </summary>
            public SaveDataCollection savedData;
            /// <summary>
            /// The filepath where the save file is located.
            /// </summary>
            public string filepath;
            /// <summary>
            /// The version of the SaveData reader/writer.
            /// </summary>
            public string version;
            public SaveData(string filepath)
            {
                this.filepath = filepath;
                version = "beta1";
                savedData = new SaveDataCollection(this);
                // Creating the file if no file exists at 'filepath'
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Close();
                }
            }
            /// <summary>
            /// Reads in the entire save file. Deletes anything previously stored in saved data.
            /// </summary>
            /// <returns>Whether or not the file was successfully read in its entirety</returns>
            public bool ReadFile()
            {
                reader = new StreamReader(filepath);
                try
                {
                    savedData.Clear();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    reader.ReadLine();
                    // Checking for version compatibility
                    string verCheck = reader.ReadLine();
                    verCheck = verCheck.Replace("ver ", "");
                    if (verCheck == version)
                    {
                        SaveDataChunk chunk = null;
                        reader.ReadLine();
                        // Reading the save data
                        while (!(reader.EndOfStream))
                        {
                            // Reference: nameLine header: 'name='
                            string nameLine = reader.ReadLine().TrimStart();
                            nameLine = nameLine.Substring(5);
                            // Reference: typeLine header: 'type='
                            string typeLine = reader.ReadLine().TrimStart();
                            typeLine = typeLine.Substring(5);
                            // Reference: dataLine header: 'data='
                            string dataLine = reader.ReadLine().TrimStart();
                            dataLine = dataLine.Substring(5);
                            reader.ReadLine();
                            // Checking if to exit the chunk
                            if (dataLine == "endchunk" && typeLine == "chk" && chunk != null)
                            {
                                chunk = chunk.parentChunk;
                            }
                            else
                            {
                                if (chunk == null)
                                {
                                    switch (typeLine)
                                    {
                                        case "str":
                                            savedData.AddData(nameLine, dataLine, DataType.String);
                                            break;
                                        case "num":
                                            savedData.AddData(nameLine, dataLine, DataType.Number);
                                            break;
                                        case "dec":
                                            savedData.AddData(nameLine, dataLine, DataType.Decimal);
                                            break;
                                        case "tof":
                                            savedData.AddData(nameLine, dataLine, DataType.Boolean);
                                            break;
                                        case "chk":
                                            savedData.AddData(nameLine);
                                            chunk = savedData.GetLastChunk();
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (typeLine)
                                    {
                                        case "str":
                                            chunk.AddData(nameLine, dataLine, DataType.String);
                                            break;
                                        case "num":
                                            chunk.AddData(nameLine, dataLine, DataType.Number);
                                            break;
                                        case "dec":
                                            chunk.AddData(nameLine, dataLine, DataType.Decimal);
                                            break;
                                        case "tof":
                                            chunk.AddData(nameLine, dataLine, DataType.Boolean);
                                            break;
                                        case "chk":
                                            chunk.AddChunk(nameLine);
                                            chunk.GetLastChunk().parentChunk = chunk;
                                            chunk = chunk.GetLastChunk();
                                            break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    reader.Close();
                }
                // Returning false since reading must have failed
                return false;
            }
            /// <summary>
            /// Adds a SaveDataObject to the save data to be written to file.
            /// </summary>
            /// <param name="data">The data to be added.</param>
            /// <returns>The provided SaveDataObject</returns>
            public SaveDataObject AddSaveObject(SaveDataObject data)
            {
                savedData.data.Add(data);
                return data;
            }
            /// <summary>
            /// Adds a SaveDataChunk to the save data to be written to file.
            /// </summary>
            /// <param name="chunk">The chunk to be added.</param>
            /// <returns>The provided SaveDataChunk</returns>
            public SaveDataChunk AddSaveChunk(SaveDataChunk chunk)
            {
                savedData.data.Add(chunk);
                return chunk;
            }
            /// <summary>
            /// Forces the StreamReader and StreamWriter closed.
            /// </summary>
            public void ForceCloseStreams()
            {
                reader.Close();
                writer.Close();
            }
            private void Write(string name, DataType dataType, string data, string indent)
            {
                string type = "";
                switch (dataType)
                {
                    case DataType.String:
                        type = "str";
                        break;
                    case DataType.Number:
                        type = "num";
                        break;
                    case DataType.Decimal:
                        type = "dec";
                        break;
                    case DataType.Boolean:
                        type = "tof";
                        break;
                    case DataType.Chunk:
                        type = "chk";
                        break;
                }
                writer.WriteLine(indent + "name=" + name);
                writer.WriteLine(indent + "type=" + type);
                writer.WriteLine(indent + "data=" + data);
                writer.WriteLine();
            }
            /// <summary>
            /// Saves all saved data to file. Truncates the file's contents.
            /// <returns>Whether or not the file was saved in its entirety</returns>
            /// </summary>
            public bool SaveToFile()
            {
                writer = new StreamWriter(filepath, false);
                writer.WriteLine("OzzzFramework Save Data File");
                try
                {
                    writer.WriteLine("ver " + version);
                    writer.WriteLine("----------------------------");
                    // Writing the saved data
                    foreach (SaveDataObject obj in savedData.data)
                    {
                        if (obj.dataType == DataType.Chunk)
                        {
                            Write(obj.name, DataType.Chunk, "", "");
                            SaveChunkToFile(obj.GetChunk(), "");
                        }
                        else
                        {
                            Write(obj.name, obj.dataType, obj.data, "");
                        }
                    }
                    writer.Close();
                    return true;
                }
                catch
                {
                    writer.Close();
                }
                return false;
            }
            private void SaveChunkToFile(SaveDataChunk chunk, string indent)
            {
                indent = indent + "  ";
                foreach (SaveDataObject obj in chunk.saveDataObjects)
                {
                    if (obj.dataType == DataType.Chunk)
                    {
                        Write(obj.name, DataType.Chunk, "", indent);
                        SaveChunkToFile(obj.GetChunk(), indent);
                    }
                    else
                    {
                        Write(obj.name, obj.dataType, obj.data, indent);
                    }
                }
                Write("", DataType.Chunk, "endchunk", indent);
            }
        }
        public class GameData
        {
            public List<string> folders, xexPaths, xexNames;
            public string gameTitle, developer, publisher, titleId, gamePath, artPath, iconPath, alphaAs, extraParams;
            public double fileSize;
            public long lastPlayed;
            public int year, month, day, minPlayers, maxPlayers, timesLaunched, resX, resY, fileCount, vernum;
            public bool preferCanary, hasCoverArt, cpuReadback, vsync, mountCache;

            private static int nextID;
            private int internalID;

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
                Broken, Starts, Menu, Gameplay1, Gameplay2, Gameplay3, Playable, Unknown
            }
            public XeniaCompat xeniaCompat, canaryCompat;
            public enum Language
            {
                English = 1,
                Japanese = 2,
                German = 3,
                French = 4,
                Spanish = 5,
                Italian = 6,
                Korean = 7,
                TraditionalChinese = 8,
                Portuguese = 9,
                Polish = 11,
                Russian = 12,
                Swedish = 13,
                Turkish = 14,
                Norwegian = 15,
                Dutch = 16,
                SimplifiedChinese = 17
            }
            public Language language;

            public GameData()
            {
                folders = new List<string>();
                xexPaths = new List<string>();
                xexNames = new List<string>();
                gameTitle = "No Title";
                developer = "No Developer";
                publisher = "No Publisher";
                titleId = "0x00000000";
                gamePath = "NULL";
                artPath = "NULL";
                iconPath = "NULL";
                alphaAs = "No Title";
                extraParams = "";
                fileSize = 0;
                lastPlayed = 0;
                year = 2005;
                month = 11;
                day = 22;
                timesLaunched = 0;
                minPlayers = 0;
                maxPlayers = 0;
                resX = 1;
                resY = 1;
                fileCount = 0;
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
                language = Language.English;

                internalID = nextID;
                nextID++;
            }
            public SaveDataChunk Save()
            {
                SaveDataChunk chunk = new SaveDataChunk("GameData");
                chunk.AddChunk("folders");
                SaveDataChunk folderChunk = chunk.GetLastChunk();
                folderChunk.parentChunk = chunk;
                for (int i = 0; i < folders.Count; i++)
                {
                    if (folders[i] != "All Games")
                    {
                        folderChunk.AddData("folder" + i, folders[i], DataType.String);
                    }
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
                chunk.AddData("alphaAs", alphaAs, DataType.String);
                chunk.AddData("extraParams", extraParams, DataType.String);
                chunk.AddData("fileSize", "" + fileSize, DataType.Number);
                chunk.AddData("lastPlayed", "" + lastPlayed, DataType.Number);
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
                chunk.AddData("language", "" + (int)language, DataType.Number);
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
                    else if (obj.name == "alphaAs")
                    {
                        alphaAs = obj.data;
                    }
                    else if (obj.name == "extraParams")
                    {
                        extraParams = obj.data;
                    }
                    else if (obj.name == "fileSize")
                    {
                        fileSize = Convert.ToDouble(obj.data);
                    }
                    else if (obj.name == "lastPlayed")
                    {
                        lastPlayed = Convert.ToInt64(obj.data);
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
                    else if (obj.name == "language")
                    {
                        language = (Language)Convert.ToInt32(obj.data);
                    }
                    // Filling in Alpha As values for older config files (Pre-2010, aka before 1.2.0 Alpha 10)
                    if (alphaAs == "No Title" && gameTitle != "No Title")
                    {
                        alphaAs = gameTitle;
                    }
                }
            }
            public bool Equals(GameData data)
            {
                return data.internalID == internalID;
            }
        }
    }
}
