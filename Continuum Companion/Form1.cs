using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace XLCompanion
{
    using SaveData = XeniaLauncher.Shared.SaveData;
    using GameData = XeniaLauncher.Shared.GameData;
    using SaveDataChunk = XeniaLauncher.Shared.SaveData.SaveDataChunk;
    using SaveDataObject = XeniaLauncher.Shared.SaveData.SaveDataObject;
    using DataType = XeniaLauncher.Shared.SaveData.DataType;
    public partial class Form1 : Form
    {
        public List<XeniaLauncher.Shared.GameData> data;
        public List<string> folders;
        public string configPath;
        public int createConfigClicks, selectedIndex;
        public static string DEFAULT_CONFIG_FILEPATH = "XLConfig.txt";
        public Form1()
        {
            InitializeComponent();
            xeniaFilepathBox.Enabled = false;
            canaryFilepathBox.Enabled = false;
            xeniaLabel.Enabled = false;
            canaryLabel.Enabled = false;
            saveConfigButton.Enabled = false;
            folderGroupBox.Enabled = false;
            gamesGroupBox.Enabled = false;
            editGameGroup.Enabled = false;
            deleteFolderButton.Enabled = false;
            data = new List<XeniaLauncher.Shared.GameData>();
            folders = new List<string>();

            // Checking if an XLConfig.txt file is where it defaults to
            if (!(File.Exists(DEFAULT_CONFIG_FILEPATH)))
            {
                CreateConfigFile(DEFAULT_CONFIG_FILEPATH);
            }
            ReadConfigFile(DEFAULT_CONFIG_FILEPATH);

        }

        public void CreateNewConfig()
        {
            if (createConfigDialog.ShowDialog() == DialogResult.OK)
            {
                CreateConfigFile(createConfigDialog.FileName);
            }
        }
        private void CreateConfigFile(string filepath)
        {
            SaveData save = new SaveData(filepath);
            save.AddSaveObject(new SaveDataObject("xenia", xeniaFilepathBox.Text, DataType.String));
            save.AddSaveObject(new SaveDataObject("canary", canaryFilepathBox.Text, DataType.String));
            save.AddSaveChunk(new SaveDataChunk("games"));
            save.savedData.data.Last().GetChunk().AddChunk(new GameData().Save());
            save.SaveToFile();
        }

        private void ReadConfigFile(string filepath)
        {
            SaveData read = new SaveData(filepath);
            configPath = filepath;
            if (read.ReadFile())
            {
                data.Clear();
                gameListBox.Items.Clear();
                xeniaFilepathBox.Text = read.savedData.FindData("xenia").data;
                canaryFilepathBox.Text = read.savedData.FindData("canary").data;
                SaveDataChunk games = read.savedData.FindData("games").GetChunk();
                foreach (SaveDataChunk game in games.saveDataObjects)
                {
                    GameData gameData = new GameData();
                    gameData.Read(game);
                    data.Add(gameData);
                    foreach (string folder in gameData.folders)
                    {
                        if (!folderListBox.Items.Contains(folder))
                        {
                            folderListBox.Items.Add(folder);
                        }
                    }
                    if (folderListBox.Items.Count > 0)
                    {
                        deleteFolderButton.Enabled = true;
                    }
                }
                data = data.OrderBy(o => o.gameTitle).ToList();
                foreach (GameData game in data)
                {
                    gameListBox.Items.Add(game.gameTitle);
                }
                xeniaFilepathBox.Enabled = true;
                canaryFilepathBox.Enabled = true;
                xeniaLabel.Enabled = true;
                canaryLabel.Enabled = true;
                saveConfigButton.Enabled = true;
                folderGroupBox.Enabled = true;
                gamesGroupBox.Enabled = true;
                editGameGroup.Enabled = false;
                gameListBox.Text = "";
            }
        }

        private void canaryLabel_Click(object sender, EventArgs e)
        {
            createConfigClicks++;
            if (createConfigClicks >= 5)
            {
                createConfigClicks = 0;
                CreateNewConfig();
            }
        }

        private void xeniaOptionsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(data[selectedIndex]);
            settingsForm.Show();
        }

        private void openConfigButton_Click(object sender, EventArgs e)
        {
            canaryLabel.Enabled = true;
            if (openConfigDialog.ShowDialog() == DialogResult.OK)
            {
                ReadConfigFile(openConfigDialog.FileName);
            }
            else
            {
                MessageBox.Show("Invalid file. Make sure the file is a valid Continuum Launcher Config file (Not a Xenia config file)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void addGameButton_Click(object sender, EventArgs e)
        {
            data.Add(new GameData());
            gameListBox.Items.Add(data.Last().gameTitle);
        }

        private void filepathBox_TextChanged(object sender, EventArgs e)
        {
            data[selectedIndex].gamePath = filepathBox.Text;
        }

        private void saveConfigButton_Click(object sender, EventArgs e)
        {
            SaveData save = new SaveData(configPath);
            save.AddSaveObject(new SaveDataObject("xenia", xeniaFilepathBox.Text, DataType.String));
            save.AddSaveObject(new SaveDataObject("canary", canaryFilepathBox.Text, DataType.String));
            SaveDataChunk chunk = new SaveDataChunk("games");
            foreach (GameData game in data)
            {
                chunk.AddChunk(game.Save());
            }
            save.AddSaveChunk(chunk);
            save.SaveToFile();
            MessageBox.Show("The config file has been successfully saved", "Config Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void displayNameBox_TextChanged(object sender, EventArgs e)
        {
            data[selectedIndex].gameTitle = displayNameBox.Text;
            gameListBox.Items[selectedIndex] = displayNameBox.Text;
        }

        private void developerBox_TextChanged(object sender, EventArgs e)
        {
            data[selectedIndex].developer = developerBox.Text;
        }

        private void publisherBox_TextChanged(object sender, EventArgs e)
        {
            data[selectedIndex].publisher = publisherBox.Text;
        }

        private void titleIdBox_TextChanged(object sender, EventArgs e)
        {
            data[selectedIndex].titleId = titleIdBox.Text;
        }

        private void releasePicker_ValueChanged(object sender, EventArgs e)
        {
            data[selectedIndex].day = releasePicker.Value.Day;
            data[selectedIndex].month = releasePicker.Value.Month;
            data[selectedIndex].year = releasePicker.Value.Year;
        }

        private void minPlayersBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            data[selectedIndex].minPlayers = Convert.ToInt32(minPlayersBox.Text);
        }

        private void maxPlayersBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            data[selectedIndex].maxPlayers = Convert.ToInt32(maxPlayersBox.Text);
        }

        private void artPathBox_TextChanged(object sender, EventArgs e)
        {
            data[selectedIndex].artPath = artPathBox.Text;
        }

        private void deleteGameButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this game from the config? This cannot be undone", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                data.RemoveAt(selectedIndex);
                gameListBox.Items.RemoveAt(selectedIndex);
                gameListBox.Text = "";
                editGameGroup.Enabled = false;
            }
        }

        private void filepathBrowseButton_Click(object sender, EventArgs e)
        {
            if (openGameDialog.ShowDialog() == DialogResult.OK)
            {
                filepathBox.Text = openGameDialog.FileName;
            }
        }

        private void artworkBrowseButton_Click(object sender, EventArgs e)
        {
            if (openArtworkDialog.ShowDialog() == DialogResult.OK)
            {
                artPathBox.Text = openArtworkDialog.FileName;
            }
        }

        private void createFolderButton_Click(object sender, EventArgs e)
        {
            if (!folderListBox.Items.Contains(folderNameBox.Text))
            {
                folders.Add(folderNameBox.Text);
                folderListBox.Items.Add(folderNameBox.Text);
                deleteFolderButton.Enabled = true;
                gameFoldersBox.Items.Add(folderNameBox.Text);
            }
            else
            {
                MessageBox.Show("Duplicate folder names are not allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void deleteFolderButton_Click(object sender, EventArgs e)
        {
            if (folderListBox.CheckedItems.Count > 0 && MessageBox.Show("Are you sure you want to delete this folder? This cannot be undone", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                string folder = folderListBox.CheckedItems[0].ToString();
                folderListBox.Items.Remove(folder);
                folders.Remove(folder);
                gameFoldersBox.Items.Remove(folder);
                if (folderListBox.CheckedItems.Count == 0)
                {
                    deleteFolderButton.Enabled = false;
                }
            }
        }

        private void gameFoldersBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (data[selectedIndex].folders.Contains(gameFoldersBox.Items[gameFoldersBox.SelectedIndex]))
            {
                data[selectedIndex].folders.Remove(gameFoldersBox.Items[gameFoldersBox.SelectedIndex].ToString());
            }
            else
            {
                data[selectedIndex].folders.Add(gameFoldersBox.Items[gameFoldersBox.SelectedIndex].ToString());
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If the launched file is an unextracted GoD or XBLA game in an STFS or SVOD format, some data can be extracted and imported into Continuum. (Note: Any games ripped from an Xbox 360 should work without changes, as long as it isn't extracted. XEX files will not work for importing data). Proceed?", "Import Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ImportForm importForm = new ImportForm(filepathBox.Text, this);
                importForm.Show();
            }
        }

        private void gameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndex = gameListBox.SelectedIndex;
            editGameGroup.Enabled = true;

            filepathBox.Text = data[selectedIndex].gamePath;
            displayNameBox.Text = data[selectedIndex].gameTitle;
            developerBox.Text = data[selectedIndex].developer;
            publisherBox.Text = data[selectedIndex].publisher;
            titleIdBox.Text = data[selectedIndex].titleId;
            releasePicker.Text = "" + data[selectedIndex].month + "/" + data[selectedIndex].day + "/" + data[selectedIndex].year;
            minPlayersBox.Text = "" + data[selectedIndex].minPlayers;
            maxPlayersBox.Text = "" + data[selectedIndex].maxPlayers;
            artPathBox.Text = data[selectedIndex].artPath;
            try
            {
                coverPreviewBox.Image = Image.FromFile(artPathBox.Text);
            }
            catch (Exception ex)
            {
                coverPreviewBox.Image = null;
                Console.WriteLine(ex);
            }

            gameFoldersBox.Items.Clear();
            int i = 0;
            foreach (string item in folderListBox.Items)
            {
                gameFoldersBox.Items.Add(item);
                if (data[selectedIndex].folders.Contains(item))
                {
                    gameFoldersBox.SetItemChecked(i, true);
                }
                i++;
            }
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        public void SetImports(string name, string titleId)
        {
            if (name != "")
            {
                displayNameBox.Text = name;
            }
            if (titleId != "")
            {
                titleIdBox.Text = titleId;
            }
        }
    }
}
