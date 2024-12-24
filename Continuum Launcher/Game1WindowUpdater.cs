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
using SharpDX.XAudio2;

namespace XeniaLauncher
{
    public partial class Game1
    {
        public void UpdateExtraLaunchWindows()
        {
            if (xexWindow != null && state == State.Launch)
            {
                xexWindow.Update();
            }
            else if (launchWindow != null && state == State.Select)
            {
                launchWindow.Update();
            }
        }

        public void UpdateMenu()
        {
            if (state == State.Menu)
            {
                menuWindow.Update();
            }
        }

        public void UpdateGameMenu()
        {
            if (state == State.GameMenu)
            {
                gameManageWindow.Update();
                // Checking for database searches
                if (databaseGameInfo != null && databaseGameInfo.Count > 0)
                {
                    if (databaseGameInfo.Count == 1) // Skipping game selection
                    {
                        databaseResultIndex = 0;
                        OpenDatabaseResult(State.GameMenu);
                    }
                    else
                    {
                        OpenDatabasePicker();
                    }
                }
            }
        }

        public void UpdateGameXeniaSettings()
        {
            if (state == State.GameXeniaSettings)
            {
                gameXeniaSettingsWindow.Update();
                if (textWindowInput != null)
                {
                    gameData[index].extraParams = textWindowInput;
                    textWindowInput = null;
                }
            }
        }

        public void UpdateDatabasePicker()
        {
            if (state == State.DatabasePicker)
            {
                databasePickerWindow.Update();
            }
        }

        public void UpdateDatabaseResult()
        {
            if (state == State.DatabaseResult)
            {
                databaseResultWindow.Update();
                if (textWindowInput != null)
                {
                    tempGameTitle = textWindowInput;
                    textWindowInput = null;
                }
            }
        }

        public void UpdateGameInfo()
        {
            if (state == State.GameInfo)
            {
                gameInfoWindow.Update();
                if (textWindowInput != null)
                {
                    if (gameInfoWindow.buttonIndex == 0 && gameData[index].gameTitle != textWindowInput)
                    {
                        tempGameTitle = textWindowInput;
                        RenameGame();
                        gameInfoWindow.titleSprite.text = "Info for " + tempGameTitle;
                        gameManageWindow.titleSprite.text = "Manage " + tempGameTitle;
                    }
                    else if (gameInfoWindow.buttonIndex == 1)
                    {
                        gameData[index].developer = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameInfoWindow.buttonIndex == 2)
                    {
                        gameData[index].publisher = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameInfoWindow.buttonIndex == 3)
                    {
                        gameData[index].titleId = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameInfoWindow.buttonIndex == 4)
                    {
                        gameData[index].alphaAs = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    textWindowInput = null;
                }
            }
        }

        public void UpdateReleaseWindow()
        {
            if (state == State.ReleaseYear || state == State.ReleaseMonth || state == State.ReleaseDay)
            {
                releaseWindow.Update();
            }
        }

        public void UpdateGameFilepaths()
        {
            if (state == State.GameFilepaths)
            {
                gameFilepathsWindow.Update();
                if (textWindowInput != null)
                {
                    if (gameFilepathsWindow.buttonIndex == 0)
                    {
                        gameData[index].gamePath = textWindowInput;
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameFilepathsWindow.buttonIndex == 1)
                    {
                        try
                        {
                            arts[gameData[index].gameTitle] = Texture2D.FromFile(_graphics.GraphicsDevice, textWindowInput);
                            gameData[index].artPath = textWindowInput;
                        }
                        catch
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid image path", "filepath", textWindowInput);
                            message = new MessageWindow(this, "Error", "Invalid image path", State.GameFilepaths);
                            state = State.Message;
                        }
                        SaveGames();
                        textWindowInput = null;
                    }
                    else if (gameFilepathsWindow.buttonIndex == 2)
                    {
                        try
                        {
                            icons[gameData[index].gameTitle] = Texture2D.FromFile(_graphics.GraphicsDevice, textWindowInput);
                            gameData[index].iconPath = textWindowInput;
                        }
                        catch
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid icon path", "filepath", textWindowInput);
                            message = new MessageWindow(this, "Error", "Invalid icon path", State.GameFilepaths);
                            state = State.Message;
                        }
                        SaveGames();
                        textWindowInput = null;
                    }
                }
            }
        }

        public void UpdateGameCategories()
        {
            if (state == State.GameCategories)
            {
                gameCategoriesWindow.Update();
                if (textWindowInput != null)
                {
                    // Create Category
                    if (gameCategoriesWindow.buttonIndex == 3)
                    {
                        if (!folders.Contains(textWindowInput))
                        {
                            folders.Add(textWindowInput);
                            gameData[index].folders.Add(textWindowInput);
                            SaveGames();
                            Logging.Write(LogType.Important, Event.FolderAdd, "Added a new folder", "name", textWindowInput);
                        }
                        else
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Duplicate folder name", "name", textWindowInput);
                            message = new MessageWindow(this, "Error", "Cannot have duplicate Category names", State.GameCategories);
                            state = State.Message;
                        }
                        textWindowInput = null;
                    }
                    // Rename Category
                    else if (gameCategoriesWindow.buttonIndex == 4)
                    {
                        if (!folders.Contains(textWindowInput))
                        {
                            string oldName = folders[tempCategoryIndex];
                            folders.Add(textWindowInput);
                            foreach (GameData game in masterData)
                            {
                                if (game.folders.Contains(oldName))
                                {
                                    game.folders.Remove(oldName);
                                    game.folders.Add(textWindowInput);
                                }
                            }
                            SaveGames();
                            FolderReset();
                            Initialize();
                            Logging.Write(LogType.Important, Event.FolderRename, "Renamed folder", new Dictionary<string, string>()
                            {
                                { "oldName", oldName },
                                { "newName", textWindowInput }
                            });
                        }
                        else
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Duplicate folder name", "name", textWindowInput);
                            message = new MessageWindow(this, "Error", "Cannot have duplicate Category names", State.GameCategories);
                            state = State.Message;
                        }
                        textWindowInput = null;
                    }
                }
                if (messageYes)
                {
                    // Deleting Category
                    if (gameCategoriesWindow.buttonIndex == 5)
                    {
                        string folderName = folders[tempCategoryIndex];
                        foreach (GameData game in masterData)
                        {
                            game.folders.Remove(folders[tempCategoryIndex]);
                        }
                        folders.Remove(folders[tempCategoryIndex]);
                        SaveGames();
                        FolderReset();
                        Initialize();
                        Logging.Write(LogType.Important, Event.FolderDelete, "Deleted folder", "name", folderName);
                    }
                    messageYes = false;
                }
            }
        }

        public void UpdateGameXEX()
        {
            if (state == State.GameXEX)
            {
                gameXEXWindow.Update();

                if (textWindowInput != null)
                {
                    if (gameXEXWindow.buttonIndex == 2)
                    {
                        if (String.IsNullOrEmpty(newXEX))
                        {
                            newXEX = textWindowInput;
                            text = new TextInputWindow(this, "Edit XEX Filepath", "", State.GameXEX);
                        }
                        else
                        {
                            gameData[index].xexNames.Add(newXEX);
                            gameData[index].xexPaths.Add(textWindowInput);
                            Logging.Write(LogType.Important, Event.AddXEX, "Added new XEX", new Dictionary<string, string>()
                            {
                                { "xexName", newXEX },
                                { "xexPath", textWindowInput }
                            });
                            newXEX = null;
                            SaveGames();
                        }
                        textWindowInput = null;
                    }
                    else if (gameXEXWindow.buttonIndex == 3)
                    {
                        gameData[index].xexNames[tempCategoryIndex] = textWindowInput;
                        gameXEXWindow.extraSprites[0].ToTextSprite().text = textWindowInput;
                        SaveGames();
                        Logging.Write(LogType.Important, Event.RenameXEX, "Renamed XEX", "name", textWindowInput);
                        textWindowInput = null;
                    }
                    else if (gameXEXWindow.buttonIndex == 4)
                    {
                        gameData[index].xexPaths[tempCategoryIndex] = textWindowInput;
                        SaveGames();
                        Logging.Write(LogType.Important, Event.RepathXEX, "Changed XEX path", "path", textWindowInput);
                        textWindowInput = null;
                    }
                }
                if (messageYes)
                {
                    if (gameXEXWindow.buttonIndex == 5)
                    {
                        if (tempCategoryIndex == -1)
                        {
                            Logging.Write(LogType.Critical, Event.DeleteGame, "Deleting game", "gameTitle", gameData[index].gameTitle);
                            int masterIndex = -1;
                            int count = 0;
                            foreach (GameData game in masterData)
                            {
                                if (game.Equals(gameData[index]))
                                {
                                    masterIndex = count;
                                    break;
                                }
                                count++;
                            }
                            gameData.RemoveAt(index);
                            masterData.RemoveAt(masterIndex);
                            SaveGames();
                            newGameProcess = true;
                            Initialize();
                            LoadArts();
                            index = 0;
                            BeginMainTransition();
                            FolderReset();
                            state = State.Main;
                        }
                        else
                        {
                            Logging.Write(LogType.Critical, Event.DeleteXEX, "Deleted XEX", "name", gameData[index].xexNames[tempCategoryIndex]);
                            gameData[index].xexNames.RemoveAt(tempCategoryIndex);
                            gameData[index].xexPaths.RemoveAt(tempCategoryIndex);
                            SaveGames();
                        }
                        messageYes = false;
                    }
                }
            }
        }

        public void UpdateOptionsWindow()
        {
            if (state == State.Options)
            {
                optionsWindow.Update();
            }
        }

        public void UpdateGraphicsWindow()
        {
            if (state == State.Graphics)
            {
                graphicsWindow.Update();
            }
        }

        public void UpdateSettingsWindow()
        {
            if (state == State.Settings)
            {
                settingsWindow.Update();
            }
        }

        public void UpdateNewGame()
        {
            if (state == State.NewGame)
            {
                if (stfsFiles == null)
                {
                    stfsFiles = new Dictionary<string, string>();
                }
                newGameWindow.Update();

                if (newGameProcess)
                {
                    state = State.Main;
                    Initialize();
                    FolderReset();
                    index = 0;
                }
                if (textWindowInput != null)
                {
                    if (newGameWindow.buttonIndex == 0)
                    {
                        // New advanced import
                        try
                        {
                            // Finding the directory
                            tempFilepathSTFS = textWindowInput;
                            newGamePath = tempFilepathSTFS;
                            DirectoryInfo dirInfo = new DirectoryInfo(tempFilepathSTFS);
                            textWindowInput = null;
                            if (dirInfo.Exists)
                            {
                                string mainGame = ""; // The name of the STFS file that will be the main executable

                                // Searching for STFS files
                                string[] contentTypes = ["00004000", "00007000", "000D0000", "00080000"];
                                for (int i = 0; i < contentTypes.Length; i++)
                                {
                                    string newMain = FindSTFSGames(dirInfo.FullName, contentTypes[i], stfsFiles);
                                    // Getting main game
                                    if (mainGame == "")
                                    {
                                        mainGame = newMain;
                                    }
                                }

                                // If a game was found
                                if (mainGame != "")
                                {
                                    STFS24 newSTFS = new STFS24(stfsFiles[mainGame]);
                                    tempTitleSTFS = mainGame;
                                    tempIdSTFS = STFS24.GetByteArrayAsHex(newSTFS.ReturnMetadata().GetTitleID());
                                    tempFilepathSTFS = stfsFiles[mainGame];
                                    if (newSTFS.ReturnMetadata().GetTransferFlags() == XTransferFlags.KinectEnabled)
                                    {
                                        kinectGameAdded = 3;
                                    }
                                    newSTFS.CloseStream();
                                    // Icon code (My new STFS24 library doesn't support icon extraction yet)
                                    STFS stfs = new STFS(stfsFiles[mainGame]);
                                    tempIconSTFS = stfs.icon;
                                    stfsFiles.Remove(mainGame);
                                    message = new MessageWindow(this, "STFS Results", "Title: " + tempTitleSTFS + ", Title ID: " + tempIdSTFS, State.NewGame, MessageWindow.MessagePrompts.OKCancel);
                                    state = State.Message;
                                }
                            }
                            else // Directory doesn't exist
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception e)
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid game directory", "dir", textWindowInput);
                            message = new MessageWindow(this, "Error", "Invalid Directory\n" + e.ToString(), State.NewGame);
                            textWindowInput = null;
                            state = State.Message;
                        }
                    }
                    else if (newGameWindow.buttonIndex == 1)
                    {
                        if (String.IsNullOrEmpty(newXEX) && !String.IsNullOrEmpty(textWindowInput))
                        {
                            newXEX = textWindowInput;
                            text = new TextInputWindow(this, "Filepath for " + newXEX, "", State.NewGame);
                        }
                        else if (File.Exists(textWindowInput))
                        {
                            masterData.Add(new GameData());
                            masterData.Last().gameTitle = newXEX;
                            masterData.Last().gamePath = textWindowInput;
                            gameData.Add(masterData.Last());
                            SaveGames();
                            textWindowInput = null;
                            newGameProcess = true;
                            newXEX = "";
                            index = gameData.Count - 1;
                            Logging.Write(LogType.Critical, Event.NewGameProcessed, "New game added via manual import", new Dictionary<string, string>()
                            {
                                { "newXEX", newXEX },
                                { "path", textWindowInput }
                            });
                            EditGame();
                        }
                        else if (!File.Exists(textWindowInput) && !String.IsNullOrEmpty(textWindowInput))
                        {
                            message = new MessageWindow(this, "Not to worry, we're still flying half a ship", "Provided filepath does not exist", State.NewGame);
                            state = State.Message;
                            textWindowInput = null;
                            newXEX = null;
                        }
                        else
                        {
                            message = new MessageWindow(this, "Who are you?", "A new game requires a valid name", State.NewGame);
                            state = State.Message;
                            textWindowInput = null;
                            newXEX = null;
                        }
                    }
                    else if (newGameWindow.buttonIndex == 0 || newGameWindow.buttonIndex == 2)
                    {
                        // Importing from STFS
                        try
                        {
                            STFS stfs = new STFS(textWindowInput);
                            tempFilepathSTFS = textWindowInput;
                            textWindowInput = null;
                            tempTitleSTFS = stfs.data.displayName;
                            tempIdSTFS = stfs.titleID;
                            tempIconSTFS = stfs.icon;
                            message = new MessageWindow(this, "STFS Results", "Title: " + tempTitleSTFS + ", Title ID: " + tempIdSTFS, State.NewGame, MessageWindow.MessagePrompts.OKCancel);
                            state = State.Message;
                        }
                        catch (Exception e)
                        {
                            Logging.Write(LogType.Critical, Event.Error, "Invalid STFS/SVOD file", "exception", e.ToString());
                            message = new MessageWindow(this, "Error", "Invalid STFS/SVOD file\n" + e.ToString(), State.NewGame);
                            state = State.Message;
                        }
                    }
                }
                if (messageYes)
                {
                    kinectGameAdded--;
                    if (kinectGameAdded == 2)
                    {
                        messageYes = false;
                        message = new MessageWindow(this, "This one is strong in the... ah, that's trademarked", "Xenia is not compatible with the Kinect peripheral. Import this game anyways?", state, MessageWindow.MessagePrompts.YesNo);
                        state = State.Message;
                    }
                    else
                    {
                        if (newGameWindow.buttonIndex == 0)
                        {
                            Logging.Write(LogType.Important, Event.NewGameStartSTFS, "Starting game add process", "tempIdSTFS", tempIdSTFS);
                            // Saving icon
                            if (!Directory.Exists("IconData"))
                            {
                                Directory.CreateDirectory("IconData");
                                Logging.Write(LogType.Critical, Event.DirCreate, "Created IconData directory");
                            }
                            tempIconSTFS.Save("IconData\\" + tempIdSTFS + ".png");
                            Logging.Write(LogType.Critical, Event.IconSave, "Saved icon", "path", "IconData\\" + tempIdSTFS + ".png");
                            // Creating new game
                            masterData.Add(new GameData());
                            // Checking if a game with this name is already imported
                            int duplicateNum = 1;
                            for (int i = 0; i < masterData.Count; i++)
                            {
                                string titleCheck = tempTitleSTFS;
                                if (duplicateNum >= 2)
                                {
                                    titleCheck = titleCheck + " (" + duplicateNum + ")";
                                }
                                if (titleCheck == masterData[i].gameTitle)
                                {
                                    duplicateNum++;
                                    i = 0;
                                }
                            }
                            // Changing name to avoid duplicates, if needed
                            if (duplicateNum >= 2)
                            {
                                tempTitleSTFS = tempTitleSTFS + " (" + duplicateNum + ")";
                                Logging.Write(LogType.Standard, Event.NewGameDuplicate, "New game has duplicate name, fixing name", "newName", tempTitleSTFS);
                            }
                            // If the game is a Kinect game, marking it as such
                            if (kinectGameAdded >= 1)
                            {
                                masterData.Last().kinect = GameData.KinectCompat.Required;
                                kinectGameAdded = 0;
                            }
                            // Grabbing a cover for the game
                            if (Directory.Exists(newGamePath + "\\_covers"))
                            {
                                if (File.Exists(newGamePath + "\\_covers\\cover0.jpg"))
                                {
                                    masterData.Last().artPath = newGamePath + "\\_covers\\cover0.jpg";
                                    arts[masterData.Last().gameTitle] = Texture2D.FromFile(_graphics.GraphicsDevice, masterData.Last().artPath);
                                    masterData.Last().hasCoverArt = true;
                                    Logging.Write(LogType.Debug, Event.NewGameCoverFound, "cover0.jpg found");
                                }
                            }
                            // Adding game to masterData
                            masterData.Last().gameTitle = tempTitleSTFS;
                            masterData.Last().alphaAs = tempTitleSTFS;
                            masterData.Last().gamePath = tempFilepathSTFS;
                            masterData.Last().titleId = tempIdSTFS;
                            masterData.Last().iconPath = "IconData\\" + tempIdSTFS + ".png";
                            masterData.Last().xexNames = stfsFiles.Keys.ToList();
                            Logging.Write(LogType.Critical, Event.NewGameProcessed, "New game added (\"A fine addition to my collection\")", new Dictionary<string, string>()
                        {
                            { "gameTitle", tempTitleSTFS },
                            { "gamePath", tempFilepathSTFS },
                            { "titleID", tempIdSTFS },
                            { "iconPath", masterData.Last().iconPath },
                            { "xexCount", "" + masterData.Last().xexNames.Count }
                        });
                            for (int i = 0; i < stfsFiles.Keys.Count; i++)
                            {
                                masterData.Last().xexPaths.Add(stfsFiles[masterData.Last().xexNames[i]]);
                            }
                            stfsFiles = null;
                            gameData.Add(masterData.Last());
                            SaveGames();
                            newGameProcess = true;
                            messageYes = false;
                            index = gameData.Count - 1;
                            EditGame();
                        }
                    }
                }
            }
        }

        public void UpdateDataWindow()
        {
            if (state == State.Data)
            {
                if (refreshData)
                {
                    DataWindowEffects.RefreshData(this, dataWindow);
                    refreshData = false;
                    Logging.Write(LogType.Debug, Event.ManageDataRefresh, "Data refresh");
                }
                dataWindow.Update();
            }
        }

        public void UpdateDataSort()
        {
            if (state == State.DataSort)
            {
                dataSortWindow.Update();
            }
        }

        public void UpdateDataFilter()
        {
            if (state == State.DataFilter)
            {
                dataFilterWindow.Update();
            }
        }

        public void UpdateCompatWindow()
        {
            if (state == State.Compat && IsActive && compatWaitFrames <= 0)
            {
                compatWindow.Update();
                if (!lastActiveCheck)
                {
                    if (GetFullscreen())
                    {
                        if (_graphics.IsFullScreen)
                        {
                            _graphics.IsFullScreen = false;
                            _graphics.ApplyChanges();
                            fullscreenDelay = 30;
                        }
                    }
                }
            }
        }

        public void UpdateManageWindow()
        {
            if (state == State.Manage)
            {
                manageWindow.Update();
                if (Convert.ToInt32(manageWindow.tags[0]) != manageWindow.stringIndex)
                {
                    DataWindowEffects.UpdateText(this, manageWindow, dataWindow.stringIndex);
                }
            }
        }

        public void UpdateManageFileWindow()
        {
            if (state == State.ManageFile)
            {
                fileManageWindow.Update();
            }
        }

        public void UpdateMetadataWindow()
        {
            if (state == State.Metadata)
            {
                metadataWindow.Update();
                if (KeyboardInput.keys["S"].IsFirstDown())
                {
                    hideSecretMetadata = !hideSecretMetadata;
                    fileManageWindow.buttonEffects.ActivateButton(this, fileManageWindow, fileManageWindow.buttons[fileManageWindow.stringIndex], fileManageWindow.stringIndex);
                }
            }
        }

        public void UpdateTutorialSelect()
        {
            if (state == State.TutorialSelect)
            {
                tutorialWindow.Update();
            }
        }

        public void UpdateCredits()
        {
            if (state == State.Credits)
            {
                creditsWindow.Update();
                if (KeyboardInput.keys["V"].IsFirstDown())
                {
#if DEBUG
                    string verType = "DEBUG";
#else
                    string verType = "RELEASE";
#endif
                    if (enableExp)
                    {
                        verType += " + EXP";
                    }
                    message = new MessageWindow(this, "Version Info", "VERNUM " + Shared.VERNUM + ". " + verType, State.Credits);
                    state = State.Message;
                }
            }
        }

        public void UpdateWelcome()
        {
            if (state == State.Welcome)
            {
                welcomeWindow.Update();
            }
        }

        public void TutorialExitCheck()
        {
            if (tutorialExitPrompt && messageYes)
            {
                ExitTutorial();
            }
            else if (compatWaitFrames > 0)
            {
                compatWaitFrames--;
            }
        }

        public void UpdateMessageWindow()
        {
            if (state == State.Message)
            {
                message.Update();
            }
        }

        public void UpdateTextInputWindow()
        {
            if (state == State.Text)
            {
                text.Update();
            }
        }

        public void UnloadWindows()
        {
            if (state == State.Main)
            {
                launchWindow = null;
                menuWindow = null;
                gameXEXWindow = null;
                newGameWindow = null;
                tutorialWindow = null;
                welcomeWindow = null;
            }
            else if (state == State.Select)
            {
                xexWindow = null;
                compatWindow = null;
                gameManageWindow = null;
            }
            else if (state == State.GameMenu)
            {
                gameXeniaSettingsWindow = null;
                databaseResultWindow = null;
                databasePickerWindow = null;
                databaseGameInfo = null;
                gameInfoWindow = null;
                releaseWindow = null;
                gameFilepathsWindow = null;
                gameCategoriesWindow = null;
                gameXEXWindow = null;
            }
            else if (state == State.DatabaseResult)
            {
                releaseWindow = null;
            }
            else if (state == State.DatabasePicker)
            {
                databaseResultWindow = null;
            }
            else if (state == State.GameInfo)
            {
                if (releaseWindow != null)
                {
                    gameData[index].year = tempYear;
                    gameData[index].month = tempMonth;
                    gameData[index].day = tempDay;
                }
                releaseWindow = null;
                databasePickerWindow = null;
            }
            else if (state == State.Menu)
            {
                optionsWindow = null;
                dataWindow = null;
                tutorialWindow = null;
                creditsWindow = null;
                newGameWindow = null;
            }
            else if (state == State.NewGame)
            {
                gameManageWindow = null;
            }
            else if (state == State.Options)
            {
                graphicsWindow = null;
                settingsWindow = null;
            }
            else if (state == State.Launch)
            {
                compatWindow = null;
            }
            else if (state == State.Data)
            {
                manageWindow = null;
                fileManageWindow = null;
                dataSortWindow = null;
                dataFilterWindow = null;
            }
            else if (state == State.Manage)
            {
                fileManageWindow = null;
            }
            else if (state == State.ManageFile)
            {
                metadataWindow = null;
            }
            else if (state == State.Welcome)
            {
                tutorialWindow = null;
            }
            if (state != State.Message)
            {
                message = null;
            }
            if (state != State.Text)
            {
                text = null;
            }
        }
    }
}
