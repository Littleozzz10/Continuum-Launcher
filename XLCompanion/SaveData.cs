using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLCompanion
{
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
                    else if (currentChunk.dataType == DataType.Chunk)
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
}
