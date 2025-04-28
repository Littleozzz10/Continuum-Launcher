using Continuum_Launcher;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class LanguageStrings
    {
        /// <summary>
        /// Stores the strings used with this language. Accessed like "globals, launcher".
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> strings;

        public LanguageStrings()
        {
            strings = new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>
        /// Import strings from a data file. Returns true if the file was successfully imported
        /// </summary>
        /// <param name="filename">The file to import from.</param>
        public bool ImportFile(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);
                string? header = reader.ReadLine();
                if (!String.IsNullOrEmpty(header))
                {
                    strings.Add(header, new Dictionary<string, string>());
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!String.IsNullOrEmpty(line))
                        {
                            // Skipping over if the line is a comment
                            if (line[0] != '\'')
                            {
                                line = line.TrimStart().Replace("\\n", "\n");
                                int equalsIndex = line.IndexOf("=");
                                if (equalsIndex != -1)
                                {
                                    strings[header].Add(line.Substring(0, equalsIndex), line.Substring(equalsIndex + 1, line.Length - equalsIndex - 1));
                                }
                                else
                                {
                                    Logging.Write(Logging.LogType.Critical, Logging.LogEvent.LanguageFileInvalid, "Invalid line");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Logging.Write(Logging.LogType.Critical, Logging.LogEvent.LanguageFileInvalid, "Empty file");
                    return false;
                }
            }
            catch (FileNotFoundException e)
            {
                Logging.Write(Logging.LogType.Critical, Logging.LogEvent.LanguageFileNotFound, filename);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrives a text string from the language module
        /// </summary>
        /// <param name="location">The file/location to search for the string in</param>
        /// <param name="key">The dictionary key to access the string</param>
        public string GetText(string location, string key)
        {
            location = location.TrimStart();
            key = key.TrimStart();

            if (strings.ContainsKey(location))
            {
                if (strings[location].ContainsKey(key))
                {
                    string text = strings[location][key];
                    while (text.Contains("{"))
                    {
                        int leftIndex = text.IndexOf("{");
                        int rightIndex = text.IndexOf("}");
                        string newText = text.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
                        // Accessing another string
                        if (newText.Contains(','))
                        {
                            string[] split = newText.Split(',');
                            text = text.Replace(text.Substring(leftIndex, rightIndex - leftIndex + 1), GetText(split[0], split[1]));
                        }
                        // Accessing a special value
                        else
                        {
                            text = text.Replace(text.Substring(leftIndex, rightIndex - leftIndex + 1), GetSpecialText(newText));
                        }
                    }
                    return text;
                }
                else
                {
                    Logging.Write(Logging.LogType.Debug, Logging.LogEvent.LanguageInvalidStringKey, "Valid location, invalid key", new Dictionary<string, string>()
                    {
                        { "Location", location },
                        { "Key", key }
                    });
                }
            }
            else
            {
                Logging.Write(Logging.LogType.Debug, Logging.LogEvent.LanguageInvalidStringKey, "Location: " + location);
            }
            return "";
        }

        /// <summary>
        /// Retrives a special text (Current time, current game index, etc)
        /// </summary>
        /// <param name="key">The key to access</param>
        /// <returns></returns>
        private string GetSpecialText(string key)
        {
            switch (key)
            {
                case "test":
                    return "Working!";
                case "vol":
                    return "" + SoundEffect.MasterVolume * 10;
                case "resx":
                    return "" + OzzzFramework.GetResolution().X;
                case "resy":
                    return "" + OzzzFramework.GetResolution().Y;
            }
            return "";
        }
    }
}
