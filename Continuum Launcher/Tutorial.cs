using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XeniaLauncher;
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
using SaveData = XeniaLauncher.Shared.SaveData;
using SaveDataObject = XeniaLauncher.Shared.SaveData.SaveDataObject;
using SaveDataChunk = XeniaLauncher.Shared.SaveData.SaveDataChunk;
using SequenceFade = XeniaLauncher.OzzzFramework.SequenceFade;
using GameData = XeniaLauncher.Shared.GameData;
using DescriptionBox = XeniaLauncher.OzzzFramework.DescriptionBox;
using DBSpawnPos = XeniaLauncher.OzzzFramework.DescriptionBox.SpawnPositions;
using LogType = Continuum_Launcher.Logging.LogType;
using Event = Continuum_Launcher.Logging.LogEvent;
using System.Security.Policy;
using System.IO;
using System.Transactions;

namespace Continuum_Launcher
{
    public class Tutorial
    {
        public Game1 game;
        public TutorialBox tutorialBox;
        public StreamReader tutorialFile;
        public List<string> instructions;
        public string infoPath, nextFile, dir;
        public int startupPromptsLeft; // 2: Starting prompt, 1: Exit notification prompt, 0: Tutorial proper
        public int quitFrames;
        public bool end;
        // From tutinfo file
        public string initialFile, title, description;
        public float size;
        public bool startupPrompt, endPrompt, requiresGames, rollingText, retriggered;
        // Reference dictionaries
        Dictionary<string, Vector2> positions;
        Dictionary<string, DBSpawnPos> spawns;
        public Tutorial(Game1 game, string infoPath)
        {
            this.game = game;
            this.infoPath = infoPath;
            SetupDictionaries();
            dir = Directory.GetParent(infoPath).FullName;
        }
        private void SetupDictionaries()
        {
            // Positions
            positions = new Dictionary<string, Vector2>();
            positions.Add("centerTopLeft", new Vector2(50, 50));
            positions.Add("centerTop", new Vector2(960, 50));
            positions.Add("centerTopRight", new Vector2(1870, 50));

            // Spawns
            spawns = new Dictionary<string, DBSpawnPos>();
            spawns.Add("centerTopLeft", DBSpawnPos.CenterPerfectLeft);
            spawns.Add("centerTop", DBSpawnPos.CenterPerfect);
            spawns.Add("centerTopRight", DBSpawnPos.CenterPerfectRight);
        }
        private void LoadInstructions(string filepath)
        {
            instructions = new List<string>();
            tutorialFile = new StreamReader(filepath);
            while (!tutorialFile.EndOfStream)
            {
                string line = tutorialFile.ReadLine();
                if (line != null && line.Replace(" ", "") != "" && line != "-")
                {
                    instructions.Add(line);
                    Logging.Write(LogType.Debug, Event.TutorialInstruction, "Tutorial instruction", "instruction", instructions.Last());
                }
            }
            tutorialFile.Close();
            Logging.Write(LogType.Important, Event.TutorialInstructionsRead, "Tutorial instructions read", "infoPath", infoPath);
        }
        private void CreateBox(string pos, string text, float size, int cooldown)
        {
            text = text.Replace("\\n", "\n");
            tutorialBox = new TutorialBox(game, positions[pos], text, size, cooldown, rollingText);
            tutorialBox.spawnPos = spawns[pos];
        }
        private void CreateBox(string pos, string text, float size, Game1.State targetState)
        {
            text = text.Replace("\\n", "\n");
            tutorialBox = new TutorialBox(game, positions[pos], text, size, targetState, rollingText);
            tutorialBox.spawnPos = spawns[pos];
        }

        /// <summary>
        /// Starts the Tutorial saved in the infoPath variable. Returns false if the Tutorial fails to start.
        /// </summary>
        /// <returns></returns>
        public bool StartTutorial()
        {
            Logging.Write(LogType.Important, Event.TutorialStart, "Starting tutorial", "infoPath", infoPath);
            try
            {
                // Loading variables from info file
                LoadInstructions(infoPath);
                foreach (string instruction in instructions)
                {
                    string[] split = instruction.Split('=');
                    switch (split[0])
                    {
                        case "startupPrompt":
                            startupPrompt = Convert.ToBoolean(split[1]);
                            break;
                        case "endPrompt":
                            endPrompt = Convert.ToBoolean(split[1]);
                            break;
                        case "requiresGames":
                            requiresGames = Convert.ToBoolean(split[1]);
                            break;
                        case "rollingText":
                            rollingText = Convert.ToBoolean(split[1]);
                            break;
                        case "size":
                            size = (float)Convert.ToDouble(split[1]);
                            break;
                        case "initialFile":
                            initialFile = split[1];
                            break;
                        case "title":
                            title = split[1];
                            break;
                        case "description":
                            description = split[1];
                            break;
                    }
                }

                // Checking if the tutorial requires games to be imported
                bool hasGame = false;
                if (requiresGames)
                {
                    foreach (GameData game in game.masterData)
                    {
                        if (File.Exists(game.gamePath))
                        {
                            hasGame = true;
                            break;
                        }
                    }
                    if (!hasGame)
                    {
                        game.message = new MessageWindow(game, "Nothing to see here. Move along.", "This Tutorial requires at least one valid game to be imported.", Game1.State.TutorialSelect);
                        game.state = Game1.State.Message;
                        return false;
                    }
                }
                else
                {
                    hasGame = true;
                }

                // Starting tutorial
                if (hasGame)
                {
                    if (startupPrompt)
                    {
                        startupPromptsLeft = 2;
                        TriggerNextStep();
                    }
                    nextFile = initialFile;
                    instructions.Clear();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Critical, Event.Error, "Tutorial failed to start", "exception", e.ToString());
            }
            return false;
        }
        private void RetriggerNewFile(string newFile)
        {
            nextFile = newFile;
            instructions.Clear();
            TriggerNextStep();
            retriggered = true;
        }
        private bool CheckFileManageType(string fileType)
        {
            return game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex].subTitle == fileType;
        }

        /// <summary>
        /// Triggeres the next step of the Tutorial.
        /// </summary>
        public void TriggerNextStep()
        {
            if (startupPromptsLeft == 2)
            {
                CreateBox("centerTopRight", "Welcome to the " + title + " tutorial.\n" + description + ".", size, 120);
                game.tutorialLock = true;
                Logging.Write(LogType.Standard, Event.TutorialStart, "Startup Prompt #1 created");
                startupPromptsLeft--;
            }
            else if (startupPromptsLeft == 1)
            {
                CreateBox("centerTopRight", "You can exit the tutorial at any time\nby holding down [ESC] or (B).", size, 120);
                game.tutorialLock = true;
                Logging.Write(LogType.Standard, Event.TutorialStart, "Startup Prompt #2 created");
                startupPromptsLeft--;
            }
            else
            {
                game.tutorialLock = false;
                if (end)
                {
                    game.ExitTutorial();
                }
                else
                {
                    try
                    {
                        if (instructions.Count == 0)
                        {
                            LoadInstructions(dir + "\\" + nextFile + ".dat");
                            nextFile = "";
                        }
                        string header = instructions[0].Split("=")[0];
                        string type = instructions[0].Split("=")[1];
                        Dictionary<string, string> tempInst = new Dictionary<string, string>();
                        if (header == "type")
                        {
                            instructions.RemoveAt(0);
                            header = "";
                            while (header != "type")
                            {
                                string[] split = instructions[0].Split("=");
                                tempInst.Add(split[0], split[1]);
                                instructions.RemoveAt(0);
                                if (instructions.Count != 0)
                                {
                                    header = instructions[0].Split("=")[0];
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        if (tempInst.Count > 0)
                        {
                            switch (type)
                            {
                                case "box":
                                    // Adding size value, since it isn't required
                                    if (!tempInst.ContainsKey("size"))
                                    {
                                        tempInst.Add("size", "" + size);
                                    }

                                    // Adding custom position values
                                    if (!positions.ContainsKey(tempInst["position"]))
                                    {
                                        string[] posSplit = tempInst["position"].Split(",");
                                        positions.Add(tempInst["position"], new Vector2(Convert.ToInt32(posSplit[0]), Convert.ToInt32(posSplit[1])));
                                        spawns.Add(tempInst["position"], DBSpawnPos.CenterPerfectLeft);
                                    }

                                    if (tempInst["end"] == "cooldown")
                                    {
                                        CreateBox(tempInst["position"], tempInst["text"], (float)Convert.ToDouble(tempInst["size"]), (int)Convert.ToInt32(tempInst["frames"]));
                                        game.tutorialLock = true;
                                    }
                                    else if (tempInst["end"] == "state")
                                    {
                                        Game1.State targetState = Game1.State.None;
                                        Enum.TryParse<Game1.State>(tempInst["state"], out targetState);
                                        CreateBox(tempInst["position"], tempInst["text"], (float)Convert.ToDouble(tempInst["size"]), targetState);
                                    }
                                    break;
                                case "split":
                                    switch (tempInst["condition"])
                                    {
                                        case "none":
                                            RetriggerNewFile(tempInst["newFile"]);
                                            break;
                                        case "noGames":
                                            if (game.masterData.Count == 0)
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "stfsImportCheck":
                                            if (game.text != null)
                                            {
                                                if (game.text.titleSprite.text.Contains("Folder"))
                                                {
                                                    RetriggerNewFile(tempInst["newFile"]);
                                                }
                                            }
                                            break;
                                        case "manualImportCheck":
                                            if (game.text != null)
                                            {
                                                if (game.text.titleSprite.text.Contains("Title"))
                                                {
                                                    RetriggerNewFile(tempInst["newFile"]);
                                                }
                                            }
                                            break;
                                        // Manage Data stuff
                                        case "id1":
                                            if (CheckFileManageType("Xbox 360 Saved Game"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id2":
                                            if (CheckFileManageType("Downloadable Content"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id4000":
                                            if (CheckFileManageType("Installed Disc Game"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id7000":
                                            if (CheckFileManageType("Installed Game on Demand"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id9000":
                                            if (CheckFileManageType("Avatar Item"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id20000":
                                            if (CheckFileManageType("Gamer Picture"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id30000":
                                            if (CheckFileManageType("Xbox 360 Theme"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id80000":
                                            if (CheckFileManageType("Game Demo"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "id90000":
                                            if (CheckFileManageType("Video"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idB0000":
                                            if (CheckFileManageType("Title Update"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idC0000":
                                            if (CheckFileManageType("Game Trailer"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idD0000":
                                            if (CheckFileManageType("Xbox Live Arcade Title"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idXenia1":
                                            if (CheckFileManageType("Xenia Installed Content"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idXenia2":
                                            if (CheckFileManageType("Localized Xenia Data"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idXenia3":
                                            if (CheckFileManageType("Xenia Game Save"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idCont1":
                                            if (CheckFileManageType("Resources"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;
                                        case "idCont2":
                                            if (CheckFileManageType("Extracted Content"))
                                            {
                                                RetriggerNewFile(tempInst["newFile"]);
                                            }
                                            break;

                                    }
                                    if (!retriggered)
                                    {
                                        TriggerNextStep();
                                    }
                                    retriggered = false;
                                    break;
                                case "end":
                                    if (endPrompt)
                                    {
                                        switch (tempInst["promptType"])
                                        {
                                            case "0":
                                                CreateBox("centerTopRight", "The " + title + " tutorial has come to an end.\nCheck out the Tutorials window in the Menu for more.", size, 120);
                                                end = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        game.ExitTutorial();
                                    }
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Write(LogType.Critical, Event.Error, "Tutorial crashed", "exception", e.ToString());
                        game.ExitTutorial();
                        game.message = new MessageWindow(game, "The negotiations were short...", "Tutorial has failed to load the next instruction file. The Tutorial has ended.", game.state);
                        game.state = Game1.State.Message;
                    }
                }
            }
        }

        public void Update()
        {
            // Tutorial quit sequence
            if (KeyboardInput.keys["Escape"].IsDown() || GamepadInput.digitals[PlayerIndex.One].IsButtonDown(GamePad.GetState(PlayerIndex.One), Buttons.B))
            {
                quitFrames++;
            }
            else
            {
                quitFrames = 0;
            }
            if (quitFrames >= 120)
            {
                game.message = new MessageWindow(game, "Exit this tutorial?", "Would you like to exit the tutorial?", game.state, MessageWindow.MessagePrompts.YesNo);
                game.state = Game1.State.Message;
                game.tutorialExitPrompt = true;
                quitFrames = 0;
            }

            if (tutorialBox != null)
            {
                tutorialBox.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (tutorialBox != null)
            {
                tutorialBox.Draw(sb);
            }
        }
    }
}
