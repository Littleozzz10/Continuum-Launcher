using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XeniaLauncher;

namespace Continuum_Launcher
{
    public static class Logging
    {
        public enum LogType
        {
            Off, Startup, Critical, Important, Standard, Debug
        }
        public static LogType logType = LogType.Debug;

        /// <summary>
        /// Events that can be logged.
        /// </summary>
        public enum LogEvent
        {
            StringMessage, // A generic message to display in the log file.
            Error, // A generic error
            ErrorFatal, // A fatal error exception
            TimeReport, // Event to report the current time
            VerReport, // EVent to report Continuum's version
            LogInit, // Event to report log file initialization
            Startup, // Events related to Continuum's startup process
            InitEvent, // Events from Continuum's Initialization
            Init, // Event when Continuum has finished Initialization
            XeniaPath, // Events related to Xenia executable filepaths
            GameLoad, // Event triggered when loading in a game
            GameLoadComplete, // Triggered when a game load has been successful
            FolderCreate, // Event triggered when creating a Category
            SettingsSave, // Event to report when the settings file is saved
            SettingsRead, // Events for reading from the settings file
            DataSizeConvert, // Triggered when the ConvertDataSize method is called
            ColorFromString, // Triggered when the ColorFromString method is called
            ThemeReset, // Triggered when resetting the active Theme
            BorderReset, // Triggered when resetting Borders
            GameIconReset, // Triggered when resetting Game Icons
            FolderReset, // Triggered when resetting Continuum to prepare for a new Category
            MainTransition, // Triggered when the BeginMainTransition method is called
            DriveSpaceTextSet, // Triggered when the SetDriveSpaceText method is called
            CompatTextureSet, // Triggered when the SetCompatTextures method is called
            FilepathStringCleanse, // Triggered when the GetFilepathString method is called
            XeniaParam, // Event that returns a new Xenia parameter string that is about to be used
            Launch, // Events related to launching a game
            HowInTheEverlastingHeckDidYouEvenGetThisErrorMessage, // It's rather self explanatory
            GameSave, // Event for saving games to the config file
            GameSaveComplete, // Triggered when a game save has been successful
            NewCoverDetect, // Event for detecting a cover art with the new auto system (cover0.jpg, etc)
            CoverLoaded, // Triggered when cover art is loaded
            IconLoaded, // Triggered when an icon is loaded
            ArtLoadComplete, // Triggered when external artwork has finished loading
            GameRename, // Triggered when the RenameGame method is called
            XeniaContentFolderRename, // Triggered when renaming a Xenia content folder for a game
            AlphaAsFix, // Triggered when updating the alphaAs value when renaming a game
            ResolutionSet, // Triggered when the resolution gets set
            FullscreenToggle, // Triggered when toggling fullscreen
            VSyncToggle, // Triggered when toggling VSync
            CoverAdjust, // Triggered when calling the AdjustCover method
            MobyDataLoad, // Triggered when loading a database file
            MobyDataReleaseFix, // Triggered when fixing a release date in the database
            MobyDataLoadComplete, // Triggered when database has been loaded
            FindingSTFS, // Triggered when the FindSTFSGames method is called
            AddingSTFS, // Event for adding an STFS file to a game
            AddingMainSTFS, // Event for when an STFS file for a game will be the main file
            ContentLoadEvent, // Events from Continuum's LoadContent
            ContentLoad, // Triggered when LoadContent has completed
            TriviaLoad, // Event for loading new trivia
            ResearchPrompt, // Triggered when showing the user research prompt
            XGameChange, // Triggered when switching games in the Dashboard
            GameSelected, // Triggered when a game is selected in the Dashboard
            KinectWarning, // Triggered when a game is selected that has kinect set to Required
            DashSort, // Triggered when the Dashboard is sorted
            DashFolderSwitch, // Triggered when the active Folder on the Dashboard is switched
            FileDelete, // Event for deleting files
            CanaryImport, // Events relating to importing content to Xenia Canary
            Extract, // Events related to extracting content
            FolderAdd, // Triggered when adding a new folder
            FolderRename, // Triggered when a folder is renamed
            FolderDelete, // Triggered when a folder is deleted
            AddXEX, // Event for adding a new XEX
            RenameXEX, // Event for renaming an XEX
            RepathXEX, // Event for changing an XEX's path
            DeleteXEX, // Event for deleting an XEX
            DeleteGame, // Event for deleting a game
            DirCreate, // Generic event for creating a directory
            IconSave, // Triggered when saving an icon
            NewGameStartSTFS, // Triggered when starting the add game process
            NewGameDuplicate, // Triggered when a new game has the same name as an existing game
            NewGameCoverFound, // Triggered when a game cover is found while adding a new game
            NewGameProcessed, // Triggered when a new game has finished processing
            ManageDataRefresh, // Triggered when the Manage Data window has refreshed
            ControllerCountChange, // Event to be triggered whenever a change in the number of connected controllers occurs
            TriviaChange, // Event to be triggered whenever the active trivia is changed
            WindowOpen, // Event to be triggered whenever a new Window is opened
            WindowStringAdded, // Triggered when a string (text) is added to a Window
            WindowClose, // Triggered when a Window is closed
            WindowButtonActivated, // Triggered when a button in a Window is clicked
            WindowButtonIndexChanged, // Event for when a new button is selected in a Window
            MessageWindow, // Event for when a MessageWindow is displayed
            ManageDataPreCheckFileFound, // Event for when a file is found in the pre-check process for the Manage Data window
            ManageDataGameAdded, // Event for when a game is added to the ManageData window
            ManageDataFileAdded, // Event for when a file for a game is found
            MissingGame, // Triggered when a game is reported missing by the ManageData window
            GameFileFound, // Triggered when a game's file is found by the ManageData window
            Exit // Event when Continuum is exited
        }

        public static Game1 game;
        public static StreamWriter log;
        public static bool init;

        public static string GenerateTimeString()
        {
            return DateTime.Now.ToString().Replace("/", "-").Replace(":", "-");
        }
        /// <summary>
        /// Initializes the Logging system.
        /// </summary>
        /// <returns>Returns whether or not the Logging system was successfully initialized.</returns>
        public static bool Initialize(Game1 game)
        {
            if (!init)
            {
                try
                {
                    // Creating Logs directory if it doesn't already exist
                    if (!Directory.Exists("Logs"))
                    {
                        Directory.CreateDirectory("Logs");
                    }

                    log = new StreamWriter("Logs\\continuum-log-" + GenerateTimeString() + ".txt");
                    log.AutoFlush = true;
                    log.WriteLine("Continuum Launcher Log File");
                    ReportTime();
                    ReportVersion();
                    Write(LogType.Startup, LogEvent.LogInit, "The log file has been initialized");
                    init = true;
                }
                catch
                {
                    init = false;
                }
                return init;
            }
            return true;
        }

        /// <summary>
        /// Writes to the log file.
        /// </summary>
        /// /// <param name="type">The log file type</param>
        /// <param name="eventName">The name of the event being reported</param>
        public static void Write(LogType type, LogEvent eventName)
        {
            Write(type, eventName, "No additional message was provided for this event", new Dictionary<string, string>());
        }
        /// <summary>
        /// Writes to the log file.
        /// </summary>
        /// /// <param name="type">The log file type</param>
        /// <param name="eventName">The name of the event being reported</param>
        /// <param name="message">The message to accompany the event</param>
        public static void Write(LogType type, LogEvent eventName, string message)
        {
            Write(type, eventName, message, new Dictionary<string, string>());
        }
        /// <summary>
        /// Writes to the log file.
        /// </summary>
        /// /// <param name="type">The log file type</param>
        /// <param name="eventName">The name of the event being reported</param>
        /// <param name="message">The message to accompany the event</param>
        /// <param name="variableName">Name of an additional variable to report</param>
        /// <param name="variable">An additional variable to report</param>
        public static void Write(LogType type, LogEvent eventName, string message, string variableName, string variable)
        {
            Write(type, eventName, message, new Dictionary<string, string>(){ { variableName, variable } });
        }
        /// <summary>
        /// Writes to the log file.
        /// </summary>
        /// <param name="type">The log file type</param>
        /// <param name="eventName">The name of the event being reported</param>
        /// <param name="message">The message to accompany the event</param>
        /// <param name="variables">Any additional variables to report</param>
        public static void Write(LogType type, LogEvent eventName, string message, Dictionary<string, string> variables)
        {
            if (log != null && type <= logType)
            {
                try
                {
                    log.WriteLine("Event. " + type.ToString() + ". " + eventName.ToString() + ". " + message + ".");

                    // Adding additional variables
                    variables.Add("timeIndex", "" + DateTime.Now.ToBinary().ToString());

                    foreach (string var in variables.Keys)
                    {
                        log.WriteLine("    " + var + ": " + variables[var]);
                    }
                }
                catch (ObjectDisposedException e)
                {

                }
            }
        }

        /// <summary>
        /// Reports the current time.
        /// </summary>
        public static void ReportTime()
        {
            Write(LogType.Startup, LogEvent.TimeReport, "Current Time: " + GenerateTimeString());
        }
        /// <summary>
        /// Reports the current version.
        /// </summary>
        public static void ReportVersion()
        {
            Write(LogType.Startup, LogEvent.VerReport, "Current Version: " + Shared.VERSION, new Dictionary<string, string>() {
                { "vernum", "" + Shared.VERNUM },
                { "compiled", Shared.COMPILED }
            });
        }

        /// <summary>
        /// Closes the Logging system and any associated files.
        /// </summary>
        public static void Close()
        {
            Write(LogType.Startup, LogEvent.Exit, "User has triggered Exit", new Dictionary<string, string>());
            log.Close();
        }
    }
}
