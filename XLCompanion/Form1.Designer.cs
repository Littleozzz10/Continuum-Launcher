namespace XLCompanion
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.folderGroupBox = new System.Windows.Forms.GroupBox();
            this.folderNameLabel = new System.Windows.Forms.Label();
            this.folderNameBox = new System.Windows.Forms.TextBox();
            this.deleteFolderButton = new System.Windows.Forms.Button();
            this.folderListBox = new System.Windows.Forms.CheckedListBox();
            this.createFolderButton = new System.Windows.Forms.Button();
            this.gamesGroupBox = new System.Windows.Forms.GroupBox();
            this.deleteGameButton = new System.Windows.Forms.Button();
            this.addGameButton = new System.Windows.Forms.Button();
            this.selectGameLabel = new System.Windows.Forms.Label();
            this.gameListBox = new System.Windows.Forms.ComboBox();
            this.configSettingsGroup = new System.Windows.Forms.GroupBox();
            this.saveConfigButton = new System.Windows.Forms.Button();
            this.openConfigButton = new System.Windows.Forms.Button();
            this.canaryFilepathBox = new System.Windows.Forms.TextBox();
            this.canaryLabel = new System.Windows.Forms.Label();
            this.xeniaLabel = new System.Windows.Forms.Label();
            this.xeniaFilepathBox = new System.Windows.Forms.TextBox();
            this.editGameGroup = new System.Windows.Forms.GroupBox();
            this.importButton = new System.Windows.Forms.Button();
            this.xeniaOptionsButton = new System.Windows.Forms.Button();
            this.gameFoldersLabel = new System.Windows.Forms.Label();
            this.gameFoldersBox = new System.Windows.Forms.CheckedListBox();
            this.coverPreviewLabel = new System.Windows.Forms.Label();
            this.artworkPathLabel = new System.Windows.Forms.Label();
            this.artPathBox = new System.Windows.Forms.TextBox();
            this.kinectBox = new System.Windows.Forms.ComboBox();
            this.kinectLabel = new System.Windows.Forms.Label();
            this.maxPlayersLabel = new System.Windows.Forms.Label();
            this.maxPlayersBox = new System.Windows.Forms.ComboBox();
            this.minPlayerLabel = new System.Windows.Forms.Label();
            this.minPlayersBox = new System.Windows.Forms.ComboBox();
            this.releaseLabel = new System.Windows.Forms.Label();
            this.releasePicker = new System.Windows.Forms.DateTimePicker();
            this.titleIdLabel = new System.Windows.Forms.Label();
            this.titleIdBox = new System.Windows.Forms.TextBox();
            this.publisherLabel = new System.Windows.Forms.Label();
            this.publisherBox = new System.Windows.Forms.TextBox();
            this.developerLabel = new System.Windows.Forms.Label();
            this.developerBox = new System.Windows.Forms.TextBox();
            this.displayNameLabel = new System.Windows.Forms.Label();
            this.displayNameBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.createConfigDialog = new System.Windows.Forms.SaveFileDialog();
            this.openConfigDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openGameDialog = new System.Windows.Forms.OpenFileDialog();
            this.openArtworkDialog = new System.Windows.Forms.OpenFileDialog();
            this.artworkBrowseButton = new System.Windows.Forms.Button();
            this.filepathBrowseButton = new System.Windows.Forms.Button();
            this.coverPreviewBox = new System.Windows.Forms.PictureBox();
            this.aboutButton = new System.Windows.Forms.Button();
            this.folderGroupBox.SuspendLayout();
            this.gamesGroupBox.SuspendLayout();
            this.configSettingsGroup.SuspendLayout();
            this.editGameGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coverPreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // folderGroupBox
            // 
            this.folderGroupBox.Controls.Add(this.folderNameLabel);
            this.folderGroupBox.Controls.Add(this.folderNameBox);
            this.folderGroupBox.Controls.Add(this.deleteFolderButton);
            this.folderGroupBox.Controls.Add(this.folderListBox);
            this.folderGroupBox.Controls.Add(this.createFolderButton);
            this.folderGroupBox.Location = new System.Drawing.Point(8, 103);
            this.folderGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.folderGroupBox.Name = "folderGroupBox";
            this.folderGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.folderGroupBox.Size = new System.Drawing.Size(308, 109);
            this.folderGroupBox.TabIndex = 0;
            this.folderGroupBox.TabStop = false;
            this.folderGroupBox.Text = "Manage Folders";
            // 
            // folderNameLabel
            // 
            this.folderNameLabel.AutoSize = true;
            this.folderNameLabel.Location = new System.Drawing.Point(4, 87);
            this.folderNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.folderNameLabel.Name = "folderNameLabel";
            this.folderNameLabel.Size = new System.Drawing.Size(95, 13);
            this.folderNameLabel.TabIndex = 8;
            this.folderNameLabel.Text = "New Folder Name:";
            // 
            // folderNameBox
            // 
            this.folderNameBox.Location = new System.Drawing.Point(98, 85);
            this.folderNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.folderNameBox.Name = "folderNameBox";
            this.folderNameBox.Size = new System.Drawing.Size(184, 20);
            this.folderNameBox.TabIndex = 8;
            this.toolTip.SetToolTip(this.folderNameBox, "Absolute filepath to the Xenia executable");
            // 
            // deleteFolderButton
            // 
            this.deleteFolderButton.Location = new System.Drawing.Point(215, 46);
            this.deleteFolderButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteFolderButton.Name = "deleteFolderButton";
            this.deleteFolderButton.Size = new System.Drawing.Size(89, 23);
            this.deleteFolderButton.TabIndex = 5;
            this.deleteFolderButton.Text = "Delete Folder";
            this.toolTip.SetToolTip(this.deleteFolderButton, "Deletes a folder");
            this.deleteFolderButton.UseVisualStyleBackColor = true;
            this.deleteFolderButton.Click += new System.EventHandler(this.deleteFolderButton_Click);
            // 
            // folderListBox
            // 
            this.folderListBox.CheckOnClick = true;
            this.folderListBox.FormattingEnabled = true;
            this.folderListBox.Location = new System.Drawing.Point(4, 16);
            this.folderListBox.Margin = new System.Windows.Forms.Padding(2);
            this.folderListBox.Name = "folderListBox";
            this.folderListBox.Size = new System.Drawing.Size(196, 64);
            this.folderListBox.Sorted = true;
            this.folderListBox.TabIndex = 0;
            // 
            // createFolderButton
            // 
            this.createFolderButton.Location = new System.Drawing.Point(215, 18);
            this.createFolderButton.Margin = new System.Windows.Forms.Padding(2);
            this.createFolderButton.Name = "createFolderButton";
            this.createFolderButton.Size = new System.Drawing.Size(89, 23);
            this.createFolderButton.TabIndex = 4;
            this.createFolderButton.Text = "Create Folder";
            this.toolTip.SetToolTip(this.createFolderButton, "Creates a new folder");
            this.createFolderButton.UseVisualStyleBackColor = true;
            this.createFolderButton.Click += new System.EventHandler(this.createFolderButton_Click);
            // 
            // gamesGroupBox
            // 
            this.gamesGroupBox.Controls.Add(this.deleteGameButton);
            this.gamesGroupBox.Controls.Add(this.addGameButton);
            this.gamesGroupBox.Controls.Add(this.selectGameLabel);
            this.gamesGroupBox.Controls.Add(this.gameListBox);
            this.gamesGroupBox.Location = new System.Drawing.Point(8, 216);
            this.gamesGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.gamesGroupBox.Name = "gamesGroupBox";
            this.gamesGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.gamesGroupBox.Size = new System.Drawing.Size(308, 87);
            this.gamesGroupBox.TabIndex = 1;
            this.gamesGroupBox.TabStop = false;
            this.gamesGroupBox.Text = "Imported Games";
            // 
            // deleteGameButton
            // 
            this.deleteGameButton.Location = new System.Drawing.Point(215, 49);
            this.deleteGameButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteGameButton.Name = "deleteGameButton";
            this.deleteGameButton.Size = new System.Drawing.Size(89, 23);
            this.deleteGameButton.TabIndex = 3;
            this.deleteGameButton.Text = "Delete Game";
            this.toolTip.SetToolTip(this.deleteGameButton, "Delete a game");
            this.deleteGameButton.UseVisualStyleBackColor = true;
            this.deleteGameButton.Click += new System.EventHandler(this.deleteGameButton_Click);
            // 
            // addGameButton
            // 
            this.addGameButton.Location = new System.Drawing.Point(215, 21);
            this.addGameButton.Margin = new System.Windows.Forms.Padding(2);
            this.addGameButton.Name = "addGameButton";
            this.addGameButton.Size = new System.Drawing.Size(89, 23);
            this.addGameButton.TabIndex = 2;
            this.addGameButton.Text = "Add New Game";
            this.toolTip.SetToolTip(this.addGameButton, "Add a new game");
            this.addGameButton.UseVisualStyleBackColor = true;
            this.addGameButton.Click += new System.EventHandler(this.addGameButton_Click);
            // 
            // selectGameLabel
            // 
            this.selectGameLabel.AutoSize = true;
            this.selectGameLabel.Location = new System.Drawing.Point(5, 27);
            this.selectGameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.selectGameLabel.Name = "selectGameLabel";
            this.selectGameLabel.Size = new System.Drawing.Size(78, 13);
            this.selectGameLabel.TabIndex = 1;
            this.selectGameLabel.Text = "Select a game:";
            // 
            // gameListBox
            // 
            this.gameListBox.FormattingEnabled = true;
            this.gameListBox.Location = new System.Drawing.Point(7, 42);
            this.gameListBox.Margin = new System.Windows.Forms.Padding(2);
            this.gameListBox.Name = "gameListBox";
            this.gameListBox.Size = new System.Drawing.Size(193, 21);
            this.gameListBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.gameListBox, "Selects a game to edit");
            this.gameListBox.SelectedIndexChanged += new System.EventHandler(this.gameListBox_SelectedIndexChanged);
            // 
            // configSettingsGroup
            // 
            this.configSettingsGroup.Controls.Add(this.aboutButton);
            this.configSettingsGroup.Controls.Add(this.saveConfigButton);
            this.configSettingsGroup.Controls.Add(this.openConfigButton);
            this.configSettingsGroup.Controls.Add(this.canaryFilepathBox);
            this.configSettingsGroup.Controls.Add(this.canaryLabel);
            this.configSettingsGroup.Controls.Add(this.xeniaLabel);
            this.configSettingsGroup.Controls.Add(this.xeniaFilepathBox);
            this.configSettingsGroup.Location = new System.Drawing.Point(12, 8);
            this.configSettingsGroup.Margin = new System.Windows.Forms.Padding(2);
            this.configSettingsGroup.Name = "configSettingsGroup";
            this.configSettingsGroup.Padding = new System.Windows.Forms.Padding(2);
            this.configSettingsGroup.Size = new System.Drawing.Size(304, 92);
            this.configSettingsGroup.TabIndex = 2;
            this.configSettingsGroup.TabStop = false;
            this.configSettingsGroup.Text = "Configuration Settings";
            // 
            // saveConfigButton
            // 
            this.saveConfigButton.Location = new System.Drawing.Point(180, 18);
            this.saveConfigButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveConfigButton.Name = "saveConfigButton";
            this.saveConfigButton.Size = new System.Drawing.Size(89, 23);
            this.saveConfigButton.TabIndex = 7;
            this.saveConfigButton.Text = "Save Config...";
            this.toolTip.SetToolTip(this.saveConfigButton, "Saves the Config file");
            this.saveConfigButton.UseVisualStyleBackColor = true;
            this.saveConfigButton.Click += new System.EventHandler(this.saveConfigButton_Click);
            // 
            // openConfigButton
            // 
            this.openConfigButton.Location = new System.Drawing.Point(29, 18);
            this.openConfigButton.Margin = new System.Windows.Forms.Padding(2);
            this.openConfigButton.Name = "openConfigButton";
            this.openConfigButton.Size = new System.Drawing.Size(89, 23);
            this.openConfigButton.TabIndex = 6;
            this.openConfigButton.Text = "Open Config...";
            this.toolTip.SetToolTip(this.openConfigButton, "Opens a Xenia Launcher Config");
            this.openConfigButton.UseVisualStyleBackColor = true;
            this.openConfigButton.Click += new System.EventHandler(this.openConfigButton_Click);
            // 
            // canaryFilepathBox
            // 
            this.canaryFilepathBox.Location = new System.Drawing.Point(94, 68);
            this.canaryFilepathBox.Margin = new System.Windows.Forms.Padding(2);
            this.canaryFilepathBox.Name = "canaryFilepathBox";
            this.canaryFilepathBox.Size = new System.Drawing.Size(184, 20);
            this.canaryFilepathBox.TabIndex = 3;
            this.toolTip.SetToolTip(this.canaryFilepathBox, "Absolute fliepath to the Xenia Canary executable");
            // 
            // canaryLabel
            // 
            this.canaryLabel.AutoSize = true;
            this.canaryLabel.Location = new System.Drawing.Point(7, 70);
            this.canaryLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.canaryLabel.Name = "canaryLabel";
            this.canaryLabel.Size = new System.Drawing.Size(83, 13);
            this.canaryLabel.TabIndex = 2;
            this.canaryLabel.Text = "Canary Filepath:";
            this.canaryLabel.Click += new System.EventHandler(this.canaryLabel_Click);
            // 
            // xeniaLabel
            // 
            this.xeniaLabel.AutoSize = true;
            this.xeniaLabel.Location = new System.Drawing.Point(7, 49);
            this.xeniaLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.xeniaLabel.Name = "xeniaLabel";
            this.xeniaLabel.Size = new System.Drawing.Size(77, 13);
            this.xeniaLabel.TabIndex = 1;
            this.xeniaLabel.Text = "Xenia Filepath:";
            // 
            // xeniaFilepathBox
            // 
            this.xeniaFilepathBox.Location = new System.Drawing.Point(94, 47);
            this.xeniaFilepathBox.Margin = new System.Windows.Forms.Padding(2);
            this.xeniaFilepathBox.Name = "xeniaFilepathBox";
            this.xeniaFilepathBox.Size = new System.Drawing.Size(184, 20);
            this.xeniaFilepathBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.xeniaFilepathBox, "Absolute filepath to the Xenia executable");
            // 
            // editGameGroup
            // 
            this.editGameGroup.Controls.Add(this.importButton);
            this.editGameGroup.Controls.Add(this.artworkBrowseButton);
            this.editGameGroup.Controls.Add(this.filepathBrowseButton);
            this.editGameGroup.Controls.Add(this.xeniaOptionsButton);
            this.editGameGroup.Controls.Add(this.gameFoldersLabel);
            this.editGameGroup.Controls.Add(this.gameFoldersBox);
            this.editGameGroup.Controls.Add(this.coverPreviewLabel);
            this.editGameGroup.Controls.Add(this.coverPreviewBox);
            this.editGameGroup.Controls.Add(this.artworkPathLabel);
            this.editGameGroup.Controls.Add(this.artPathBox);
            this.editGameGroup.Controls.Add(this.kinectBox);
            this.editGameGroup.Controls.Add(this.kinectLabel);
            this.editGameGroup.Controls.Add(this.maxPlayersLabel);
            this.editGameGroup.Controls.Add(this.maxPlayersBox);
            this.editGameGroup.Controls.Add(this.minPlayerLabel);
            this.editGameGroup.Controls.Add(this.minPlayersBox);
            this.editGameGroup.Controls.Add(this.releaseLabel);
            this.editGameGroup.Controls.Add(this.releasePicker);
            this.editGameGroup.Controls.Add(this.titleIdLabel);
            this.editGameGroup.Controls.Add(this.titleIdBox);
            this.editGameGroup.Controls.Add(this.publisherLabel);
            this.editGameGroup.Controls.Add(this.publisherBox);
            this.editGameGroup.Controls.Add(this.developerLabel);
            this.editGameGroup.Controls.Add(this.developerBox);
            this.editGameGroup.Controls.Add(this.displayNameLabel);
            this.editGameGroup.Controls.Add(this.displayNameBox);
            this.editGameGroup.Controls.Add(this.filepathLabel);
            this.editGameGroup.Controls.Add(this.filepathBox);
            this.editGameGroup.Location = new System.Drawing.Point(320, 8);
            this.editGameGroup.Margin = new System.Windows.Forms.Padding(2);
            this.editGameGroup.Name = "editGameGroup";
            this.editGameGroup.Padding = new System.Windows.Forms.Padding(2);
            this.editGameGroup.Size = new System.Drawing.Size(308, 295);
            this.editGameGroup.TabIndex = 3;
            this.editGameGroup.TabStop = false;
            this.editGameGroup.Text = "Edit Game Information";
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(207, 156);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(87, 23);
            this.importButton.TabIndex = 29;
            this.importButton.Text = "Import...";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // xeniaOptionsButton
            // 
            this.xeniaOptionsButton.Location = new System.Drawing.Point(149, 267);
            this.xeniaOptionsButton.Margin = new System.Windows.Forms.Padding(2);
            this.xeniaOptionsButton.Name = "xeniaOptionsButton";
            this.xeniaOptionsButton.Size = new System.Drawing.Size(73, 23);
            this.xeniaOptionsButton.TabIndex = 8;
            this.xeniaOptionsButton.Text = "More...";
            this.toolTip.SetToolTip(this.xeniaOptionsButton, "Show more options for game configuration");
            this.xeniaOptionsButton.UseVisualStyleBackColor = true;
            this.xeniaOptionsButton.Click += new System.EventHandler(this.xeniaOptionsButton_Click);
            // 
            // gameFoldersLabel
            // 
            this.gameFoldersLabel.AutoSize = true;
            this.gameFoldersLabel.Location = new System.Drawing.Point(9, 207);
            this.gameFoldersLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gameFoldersLabel.Name = "gameFoldersLabel";
            this.gameFoldersLabel.Size = new System.Drawing.Size(44, 13);
            this.gameFoldersLabel.TabIndex = 26;
            this.gameFoldersLabel.Text = "Folders:";
            // 
            // gameFoldersBox
            // 
            this.gameFoldersBox.CheckOnClick = true;
            this.gameFoldersBox.FormattingEnabled = true;
            this.gameFoldersBox.Location = new System.Drawing.Point(4, 228);
            this.gameFoldersBox.Margin = new System.Windows.Forms.Padding(2);
            this.gameFoldersBox.Name = "gameFoldersBox";
            this.gameFoldersBox.Size = new System.Drawing.Size(142, 64);
            this.gameFoldersBox.Sorted = true;
            this.gameFoldersBox.TabIndex = 6;
            this.gameFoldersBox.SelectedValueChanged += new System.EventHandler(this.gameFoldersBox_SelectedValueChanged);
            // 
            // coverPreviewLabel
            // 
            this.coverPreviewLabel.AutoSize = true;
            this.coverPreviewLabel.Location = new System.Drawing.Point(149, 235);
            this.coverPreviewLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.coverPreviewLabel.Name = "coverPreviewLabel";
            this.coverPreviewLabel.Size = new System.Drawing.Size(79, 13);
            this.coverPreviewLabel.TabIndex = 25;
            this.coverPreviewLabel.Text = "Cover Preview:";
            // 
            // artworkPathLabel
            // 
            this.artworkPathLabel.AutoSize = true;
            this.artworkPathLabel.Location = new System.Drawing.Point(9, 183);
            this.artworkPathLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.artworkPathLabel.Name = "artworkPathLabel";
            this.artworkPathLabel.Size = new System.Drawing.Size(71, 13);
            this.artworkPathLabel.TabIndex = 22;
            this.artworkPathLabel.Text = "Artwork Path:";
            // 
            // artPathBox
            // 
            this.artPathBox.Location = new System.Drawing.Point(82, 181);
            this.artPathBox.Margin = new System.Windows.Forms.Padding(2);
            this.artPathBox.Name = "artPathBox";
            this.artPathBox.Size = new System.Drawing.Size(197, 20);
            this.artPathBox.TabIndex = 23;
            this.toolTip.SetToolTip(this.artPathBox, "The absolute filepath to the image that the Launcher will use as the game\'s cover" +
        " art");
            this.artPathBox.TextChanged += new System.EventHandler(this.artPathBox_TextChanged);
            // 
            // kinectBox
            // 
            this.kinectBox.FormattingEnabled = true;
            this.kinectBox.Items.AddRange(new object[] {
            "None",
            "Supported",
            "Required"});
            this.kinectBox.Location = new System.Drawing.Point(91, 159);
            this.kinectBox.Margin = new System.Windows.Forms.Padding(2);
            this.kinectBox.Name = "kinectBox";
            this.kinectBox.Size = new System.Drawing.Size(102, 21);
            this.kinectBox.TabIndex = 20;
            this.toolTip.SetToolTip(this.kinectBox, "The level of Kinect support that the game has");
            this.kinectBox.SelectedIndexChanged += new System.EventHandler(this.kinectBox_SelectedIndexChanged);
            // 
            // kinectLabel
            // 
            this.kinectLabel.AutoSize = true;
            this.kinectLabel.Location = new System.Drawing.Point(9, 161);
            this.kinectLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.kinectLabel.Name = "kinectLabel";
            this.kinectLabel.Size = new System.Drawing.Size(80, 13);
            this.kinectLabel.TabIndex = 19;
            this.kinectLabel.Text = "Kinect Support:";
            // 
            // maxPlayersLabel
            // 
            this.maxPlayersLabel.AutoSize = true;
            this.maxPlayersLabel.Location = new System.Drawing.Point(187, 135);
            this.maxPlayersLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.maxPlayersLabel.Name = "maxPlayersLabel";
            this.maxPlayersLabel.Size = new System.Drawing.Size(67, 13);
            this.maxPlayersLabel.TabIndex = 18;
            this.maxPlayersLabel.Text = "Max Players:";
            // 
            // maxPlayersBox
            // 
            this.maxPlayersBox.FormattingEnabled = true;
            this.maxPlayersBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.maxPlayersBox.Location = new System.Drawing.Point(256, 133);
            this.maxPlayersBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxPlayersBox.Name = "maxPlayersBox";
            this.maxPlayersBox.Size = new System.Drawing.Size(39, 21);
            this.maxPlayersBox.TabIndex = 17;
            this.toolTip.SetToolTip(this.maxPlayersBox, "The maximum number of local players that can play at a time");
            this.maxPlayersBox.SelectedIndexChanged += new System.EventHandler(this.maxPlayersBox_SelectedIndexChanged);
            // 
            // minPlayerLabel
            // 
            this.minPlayerLabel.AutoSize = true;
            this.minPlayerLabel.Location = new System.Drawing.Point(9, 135);
            this.minPlayerLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.minPlayerLabel.Name = "minPlayerLabel";
            this.minPlayerLabel.Size = new System.Drawing.Size(64, 13);
            this.minPlayerLabel.TabIndex = 16;
            this.minPlayerLabel.Text = "Min Players:";
            // 
            // minPlayersBox
            // 
            this.minPlayersBox.FormattingEnabled = true;
            this.minPlayersBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.minPlayersBox.Location = new System.Drawing.Point(75, 133);
            this.minPlayersBox.Margin = new System.Windows.Forms.Padding(2);
            this.minPlayersBox.Name = "minPlayersBox";
            this.minPlayersBox.Size = new System.Drawing.Size(39, 21);
            this.minPlayersBox.TabIndex = 15;
            this.toolTip.SetToolTip(this.minPlayersBox, "The minimum number of local players required to play");
            this.minPlayersBox.SelectedIndexChanged += new System.EventHandler(this.minPlayersBox_SelectedIndexChanged);
            // 
            // releaseLabel
            // 
            this.releaseLabel.AutoSize = true;
            this.releaseLabel.Location = new System.Drawing.Point(157, 112);
            this.releaseLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.releaseLabel.Name = "releaseLabel";
            this.releaseLabel.Size = new System.Drawing.Size(55, 13);
            this.releaseLabel.TabIndex = 14;
            this.releaseLabel.Text = "Released:";
            // 
            // releasePicker
            // 
            this.releasePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.releasePicker.Location = new System.Drawing.Point(215, 109);
            this.releasePicker.Margin = new System.Windows.Forms.Padding(2);
            this.releasePicker.MinDate = new System.DateTime(2005, 11, 22, 0, 0, 0, 0);
            this.releasePicker.Name = "releasePicker";
            this.releasePicker.Size = new System.Drawing.Size(79, 20);
            this.releasePicker.TabIndex = 13;
            this.toolTip.SetToolTip(this.releasePicker, "The game\'s release date");
            this.releasePicker.Value = new System.DateTime(2005, 11, 22, 0, 0, 0, 0);
            this.releasePicker.ValueChanged += new System.EventHandler(this.releasePicker_ValueChanged);
            // 
            // titleIdLabel
            // 
            this.titleIdLabel.AutoSize = true;
            this.titleIdLabel.Location = new System.Drawing.Point(9, 112);
            this.titleIdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleIdLabel.Name = "titleIdLabel";
            this.titleIdLabel.Size = new System.Drawing.Size(44, 13);
            this.titleIdLabel.TabIndex = 11;
            this.titleIdLabel.Text = "Title ID:";
            // 
            // titleIdBox
            // 
            this.titleIdBox.Location = new System.Drawing.Point(55, 110);
            this.titleIdBox.Margin = new System.Windows.Forms.Padding(2);
            this.titleIdBox.Name = "titleIdBox";
            this.titleIdBox.Size = new System.Drawing.Size(91, 20);
            this.titleIdBox.TabIndex = 12;
            this.titleIdBox.TextChanged += new System.EventHandler(this.titleIdBox_TextChanged);
            // 
            // publisherLabel
            // 
            this.publisherLabel.AutoSize = true;
            this.publisherLabel.Location = new System.Drawing.Point(9, 88);
            this.publisherLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.publisherLabel.Name = "publisherLabel";
            this.publisherLabel.Size = new System.Drawing.Size(53, 13);
            this.publisherLabel.TabIndex = 9;
            this.publisherLabel.Text = "Publisher:";
            // 
            // publisherBox
            // 
            this.publisherBox.Location = new System.Drawing.Point(86, 86);
            this.publisherBox.Margin = new System.Windows.Forms.Padding(2);
            this.publisherBox.Name = "publisherBox";
            this.publisherBox.Size = new System.Drawing.Size(193, 20);
            this.publisherBox.TabIndex = 10;
            this.toolTip.SetToolTip(this.publisherBox, "The game\'s publisher");
            this.publisherBox.TextChanged += new System.EventHandler(this.publisherBox_TextChanged);
            // 
            // developerLabel
            // 
            this.developerLabel.AutoSize = true;
            this.developerLabel.Location = new System.Drawing.Point(9, 68);
            this.developerLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.developerLabel.Name = "developerLabel";
            this.developerLabel.Size = new System.Drawing.Size(59, 13);
            this.developerLabel.TabIndex = 7;
            this.developerLabel.Text = "Developer:";
            // 
            // developerBox
            // 
            this.developerBox.Location = new System.Drawing.Point(86, 66);
            this.developerBox.Margin = new System.Windows.Forms.Padding(2);
            this.developerBox.Name = "developerBox";
            this.developerBox.Size = new System.Drawing.Size(193, 20);
            this.developerBox.TabIndex = 8;
            this.toolTip.SetToolTip(this.developerBox, "The game\'s developer");
            this.developerBox.TextChanged += new System.EventHandler(this.developerBox_TextChanged);
            // 
            // displayNameLabel
            // 
            this.displayNameLabel.AutoSize = true;
            this.displayNameLabel.Location = new System.Drawing.Point(9, 47);
            this.displayNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.displayNameLabel.Name = "displayNameLabel";
            this.displayNameLabel.Size = new System.Drawing.Size(75, 13);
            this.displayNameLabel.TabIndex = 5;
            this.displayNameLabel.Text = "Display Name:";
            // 
            // displayNameBox
            // 
            this.displayNameBox.Location = new System.Drawing.Point(86, 45);
            this.displayNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.displayNameBox.Name = "displayNameBox";
            this.displayNameBox.Size = new System.Drawing.Size(193, 20);
            this.displayNameBox.TabIndex = 6;
            this.toolTip.SetToolTip(this.displayNameBox, "The title that will be displayed as the game\'s name in the Launcher");
            this.displayNameBox.TextChanged += new System.EventHandler(this.displayNameBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.AutoSize = true;
            this.filepathLabel.Location = new System.Drawing.Point(9, 26);
            this.filepathLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(47, 13);
            this.filepathLabel.TabIndex = 4;
            this.filepathLabel.Text = "Filepath:";
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(59, 24);
            this.filepathBox.Margin = new System.Windows.Forms.Padding(2);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(220, 20);
            this.filepathBox.TabIndex = 4;
            this.toolTip.SetToolTip(this.filepathBox, "The filepath to the game. If extracted, this must be an xex file");
            this.filepathBox.TextChanged += new System.EventHandler(this.filepathBox_TextChanged);
            // 
            // createConfigDialog
            // 
            this.createConfigDialog.FileName = "XLConfig.txt";
            this.createConfigDialog.Title = "Create a new config file";
            // 
            // openConfigDialog
            // 
            this.openConfigDialog.FileName = "XLConfig.txt";
            this.openConfigDialog.Title = "Open a config file";
            // 
            // openGameDialog
            // 
            this.openGameDialog.Title = "Select the game to be launched";
            // 
            // openArtworkDialog
            // 
            this.openArtworkDialog.Title = "Choose a cover imagae";
            // 
            // artworkBrowseButton
            // 
            this.artworkBrowseButton.Image = global::XLCompanion.Properties.Resources.FolderBrowserDialogControl;
            this.artworkBrowseButton.Location = new System.Drawing.Point(281, 181);
            this.artworkBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.artworkBrowseButton.Name = "artworkBrowseButton";
            this.artworkBrowseButton.Size = new System.Drawing.Size(23, 20);
            this.artworkBrowseButton.TabIndex = 28;
            this.toolTip.SetToolTip(this.artworkBrowseButton, "Browse for an image");
            this.artworkBrowseButton.UseVisualStyleBackColor = true;
            this.artworkBrowseButton.Click += new System.EventHandler(this.artworkBrowseButton_Click);
            // 
            // filepathBrowseButton
            // 
            this.filepathBrowseButton.Image = global::XLCompanion.Properties.Resources.FolderBrowserDialogControl;
            this.filepathBrowseButton.Location = new System.Drawing.Point(281, 24);
            this.filepathBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.filepathBrowseButton.Name = "filepathBrowseButton";
            this.filepathBrowseButton.Size = new System.Drawing.Size(23, 20);
            this.filepathBrowseButton.TabIndex = 27;
            this.toolTip.SetToolTip(this.filepathBrowseButton, "Browse for a game");
            this.filepathBrowseButton.UseVisualStyleBackColor = true;
            this.filepathBrowseButton.Click += new System.EventHandler(this.filepathBrowseButton_Click);
            // 
            // coverPreviewBox
            // 
            this.coverPreviewBox.Location = new System.Drawing.Point(242, 202);
            this.coverPreviewBox.Margin = new System.Windows.Forms.Padding(2);
            this.coverPreviewBox.Name = "coverPreviewBox";
            this.coverPreviewBox.Size = new System.Drawing.Size(62, 89);
            this.coverPreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.coverPreviewBox.TabIndex = 24;
            this.coverPreviewBox.TabStop = false;
            // 
            // aboutButton
            // 
            this.aboutButton.BackgroundImage = global::XLCompanion.Properties.Resources.continuum_comp;
            this.aboutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.aboutButton.Location = new System.Drawing.Point(132, 13);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(32, 32);
            this.aboutButton.TabIndex = 8;
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 322);
            this.Controls.Add(this.editGameGroup);
            this.Controls.Add(this.configSettingsGroup);
            this.Controls.Add(this.gamesGroupBox);
            this.Controls.Add(this.folderGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(659, 361);
            this.MinimumSize = new System.Drawing.Size(659, 361);
            this.Name = "Form1";
            this.Text = "Continuum Companion";
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.gamesGroupBox.ResumeLayout(false);
            this.gamesGroupBox.PerformLayout();
            this.configSettingsGroup.ResumeLayout(false);
            this.configSettingsGroup.PerformLayout();
            this.editGameGroup.ResumeLayout(false);
            this.editGameGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coverPreviewBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox folderGroupBox;
        private System.Windows.Forms.CheckedListBox folderListBox;
        private System.Windows.Forms.GroupBox gamesGroupBox;
        private System.Windows.Forms.ComboBox gameListBox;
        private System.Windows.Forms.GroupBox configSettingsGroup;
        private System.Windows.Forms.Label xeniaLabel;
        private System.Windows.Forms.TextBox xeniaFilepathBox;
        private System.Windows.Forms.Label canaryLabel;
        private System.Windows.Forms.Label selectGameLabel;
        private System.Windows.Forms.TextBox canaryFilepathBox;
        private System.Windows.Forms.Button deleteGameButton;
        private System.Windows.Forms.Button addGameButton;
        private System.Windows.Forms.GroupBox editGameGroup;
        private System.Windows.Forms.Label displayNameLabel;
        private System.Windows.Forms.TextBox displayNameBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label publisherLabel;
        private System.Windows.Forms.TextBox publisherBox;
        private System.Windows.Forms.Label developerLabel;
        private System.Windows.Forms.TextBox developerBox;
        private System.Windows.Forms.Label titleIdLabel;
        private System.Windows.Forms.TextBox titleIdBox;
        private System.Windows.Forms.Label releaseLabel;
        private System.Windows.Forms.DateTimePicker releasePicker;
        private System.Windows.Forms.Label maxPlayersLabel;
        private System.Windows.Forms.ComboBox maxPlayersBox;
        private System.Windows.Forms.Label minPlayerLabel;
        private System.Windows.Forms.ComboBox minPlayersBox;
        private System.Windows.Forms.ComboBox kinectBox;
        private System.Windows.Forms.Label kinectLabel;
        private System.Windows.Forms.Label coverPreviewLabel;
        private System.Windows.Forms.PictureBox coverPreviewBox;
        private System.Windows.Forms.Label artworkPathLabel;
        private System.Windows.Forms.TextBox artPathBox;
        private System.Windows.Forms.Button deleteFolderButton;
        private System.Windows.Forms.Button createFolderButton;
        private System.Windows.Forms.Label gameFoldersLabel;
        private System.Windows.Forms.CheckedListBox gameFoldersBox;
        private System.Windows.Forms.Button saveConfigButton;
        private System.Windows.Forms.Button openConfigButton;
        private System.Windows.Forms.SaveFileDialog createConfigDialog;
        private System.Windows.Forms.Button xeniaOptionsButton;
        private System.Windows.Forms.OpenFileDialog openConfigDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button artworkBrowseButton;
        private System.Windows.Forms.Button filepathBrowseButton;
        private System.Windows.Forms.OpenFileDialog openGameDialog;
        private System.Windows.Forms.OpenFileDialog openArtworkDialog;
        private System.Windows.Forms.Label folderNameLabel;
        private System.Windows.Forms.TextBox folderNameBox;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button aboutButton;
    }
}

