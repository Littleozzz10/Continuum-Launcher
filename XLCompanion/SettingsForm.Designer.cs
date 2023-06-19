namespace XLCompanion
{
    partial class SettingsForm
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
            this.generalGroup = new System.Windows.Forms.GroupBox();
            this.mountCacheCheck = new System.Windows.Forms.CheckBox();
            this.licenseBox = new System.Windows.Forms.ComboBox();
            this.canaryBox = new System.Windows.Forms.CheckBox();
            this.graphicsGroup = new System.Windows.Forms.GroupBox();
            this.readbackCheck = new System.Windows.Forms.CheckBox();
            this.vSyncCheck = new System.Windows.Forms.CheckBox();
            this.renderText = new System.Windows.Forms.Label();
            this.rendererBox = new System.Windows.Forms.ComboBox();
            this.verticalScaleLabel = new System.Windows.Forms.Label();
            this.verticalBox = new System.Windows.Forms.ComboBox();
            this.horizontalScaleLabel = new System.Windows.Forms.Label();
            this.horizontalBox = new System.Windows.Forms.ComboBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.executablePathBox = new System.Windows.Forms.TextBox();
            this.executablePathLabel = new System.Windows.Forms.Label();
            this.executableNameLabel = new System.Windows.Forms.Label();
            this.executableNameBox = new System.Windows.Forms.TextBox();
            this.browseExecutableButton = new System.Windows.Forms.Button();
            this.executableBox = new System.Windows.Forms.ComboBox();
            this.executableLabel = new System.Windows.Forms.Label();
            this.addExecutableButton = new System.Windows.Forms.Button();
            this.deleteExecutableButton = new System.Windows.Forms.Button();
            this.openExecutableDialog = new System.Windows.Forms.OpenFileDialog();
            this.xexGroup = new System.Windows.Forms.GroupBox();
            this.compatGroup = new System.Windows.Forms.GroupBox();
            this.canaryCompatBox = new System.Windows.Forms.ComboBox();
            this.xeniaCompatBox = new System.Windows.Forms.ComboBox();
            this.canaryCompatLabel = new System.Windows.Forms.Label();
            this.xeniaCompatLabel = new System.Windows.Forms.Label();
            this.iconGroup = new System.Windows.Forms.GroupBox();
            this.iconBrowseButton = new System.Windows.Forms.Button();
            this.iconLabel = new System.Windows.Forms.Label();
            this.iconPathBox = new System.Windows.Forms.TextBox();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.openIconDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.generalGroup.SuspendLayout();
            this.graphicsGroup.SuspendLayout();
            this.xexGroup.SuspendLayout();
            this.compatGroup.SuspendLayout();
            this.iconGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // generalGroup
            // 
            this.generalGroup.Controls.Add(this.mountCacheCheck);
            this.generalGroup.Controls.Add(this.licenseBox);
            this.generalGroup.Controls.Add(this.canaryBox);
            this.generalGroup.Location = new System.Drawing.Point(12, 12);
            this.generalGroup.Name = "generalGroup";
            this.generalGroup.Size = new System.Drawing.Size(340, 85);
            this.generalGroup.TabIndex = 0;
            this.generalGroup.TabStop = false;
            this.generalGroup.Text = "General Settings";
            // 
            // mountCacheCheck
            // 
            this.mountCacheCheck.AutoSize = true;
            this.mountCacheCheck.Location = new System.Drawing.Point(200, 52);
            this.mountCacheCheck.Name = "mountCacheCheck";
            this.mountCacheCheck.Size = new System.Drawing.Size(130, 24);
            this.mountCacheCheck.TabIndex = 8;
            this.mountCacheCheck.Text = "Mount Cache";
            this.mountCacheCheck.UseVisualStyleBackColor = true;
            // 
            // licenseBox
            // 
            this.licenseBox.FormattingEnabled = true;
            this.licenseBox.Items.AddRange(new object[] {
            "No License (0)",
            "First License (1)",
            "All Licenses (-1)"});
            this.licenseBox.Location = new System.Drawing.Point(4, 29);
            this.licenseBox.Name = "licenseBox";
            this.licenseBox.Size = new System.Drawing.Size(164, 28);
            this.licenseBox.TabIndex = 1;
            // 
            // canaryBox
            // 
            this.canaryBox.AutoSize = true;
            this.canaryBox.Location = new System.Drawing.Point(200, 20);
            this.canaryBox.Name = "canaryBox";
            this.canaryBox.Size = new System.Drawing.Size(132, 24);
            this.canaryBox.TabIndex = 0;
            this.canaryBox.Text = "Prefer Canary";
            this.toolTip.SetToolTip(this.canaryBox, "When checked, will launch Canary on Quickstart");
            this.canaryBox.UseVisualStyleBackColor = true;
            // 
            // graphicsGroup
            // 
            this.graphicsGroup.Controls.Add(this.readbackCheck);
            this.graphicsGroup.Controls.Add(this.vSyncCheck);
            this.graphicsGroup.Controls.Add(this.renderText);
            this.graphicsGroup.Controls.Add(this.rendererBox);
            this.graphicsGroup.Controls.Add(this.verticalScaleLabel);
            this.graphicsGroup.Controls.Add(this.verticalBox);
            this.graphicsGroup.Controls.Add(this.horizontalScaleLabel);
            this.graphicsGroup.Controls.Add(this.horizontalBox);
            this.graphicsGroup.Location = new System.Drawing.Point(12, 103);
            this.graphicsGroup.Name = "graphicsGroup";
            this.graphicsGroup.Size = new System.Drawing.Size(340, 183);
            this.graphicsGroup.TabIndex = 1;
            this.graphicsGroup.TabStop = false;
            this.graphicsGroup.Text = "Graphics Settings";
            // 
            // readbackCheck
            // 
            this.readbackCheck.AutoSize = true;
            this.readbackCheck.Location = new System.Drawing.Point(189, 148);
            this.readbackCheck.Name = "readbackCheck";
            this.readbackCheck.Size = new System.Drawing.Size(145, 24);
            this.readbackCheck.TabIndex = 7;
            this.readbackCheck.Text = "CPU Readback";
            this.readbackCheck.UseVisualStyleBackColor = true;
            // 
            // vSyncCheck
            // 
            this.vSyncCheck.AutoSize = true;
            this.vSyncCheck.Location = new System.Drawing.Point(196, 118);
            this.vSyncCheck.Name = "vSyncCheck";
            this.vSyncCheck.Size = new System.Drawing.Size(127, 24);
            this.vSyncCheck.TabIndex = 6;
            this.vSyncCheck.Text = "Vertical Sync";
            this.vSyncCheck.UseVisualStyleBackColor = true;
            // 
            // renderText
            // 
            this.renderText.AutoSize = true;
            this.renderText.Location = new System.Drawing.Point(6, 95);
            this.renderText.Name = "renderText";
            this.renderText.Size = new System.Drawing.Size(80, 20);
            this.renderText.TabIndex = 5;
            this.renderText.Text = "Renderer:";
            // 
            // rendererBox
            // 
            this.rendererBox.FormattingEnabled = true;
            this.rendererBox.Items.AddRange(new object[] {
            "Any (Default)",
            "Direct3D 12",
            "Vulkan"});
            this.rendererBox.Location = new System.Drawing.Point(10, 118);
            this.rendererBox.Name = "rendererBox";
            this.rendererBox.Size = new System.Drawing.Size(126, 28);
            this.rendererBox.TabIndex = 4;
            // 
            // verticalScaleLabel
            // 
            this.verticalScaleLabel.AutoSize = true;
            this.verticalScaleLabel.Location = new System.Drawing.Point(200, 35);
            this.verticalScaleLabel.Name = "verticalScaleLabel";
            this.verticalScaleLabel.Size = new System.Drawing.Size(110, 20);
            this.verticalScaleLabel.TabIndex = 3;
            this.verticalScaleLabel.Text = "Vertical Scale:";
            // 
            // verticalBox
            // 
            this.verticalBox.FormattingEnabled = true;
            this.verticalBox.Items.AddRange(new object[] {
            "720 (1x)",
            "1440 (2x)",
            "2160 (3x)"});
            this.verticalBox.Location = new System.Drawing.Point(196, 58);
            this.verticalBox.Name = "verticalBox";
            this.verticalBox.Size = new System.Drawing.Size(112, 28);
            this.verticalBox.TabIndex = 2;
            // 
            // horizontalScaleLabel
            // 
            this.horizontalScaleLabel.AutoSize = true;
            this.horizontalScaleLabel.Location = new System.Drawing.Point(6, 35);
            this.horizontalScaleLabel.Name = "horizontalScaleLabel";
            this.horizontalScaleLabel.Size = new System.Drawing.Size(129, 20);
            this.horizontalScaleLabel.TabIndex = 1;
            this.horizontalScaleLabel.Text = "Horizontal Scale:";
            // 
            // horizontalBox
            // 
            this.horizontalBox.FormattingEnabled = true;
            this.horizontalBox.Items.AddRange(new object[] {
            "1280 (1x)",
            "2560 (2x)",
            "3840 (3x)"});
            this.horizontalBox.Location = new System.Drawing.Point(10, 58);
            this.horizontalBox.Name = "horizontalBox";
            this.horizontalBox.Size = new System.Drawing.Size(112, 28);
            this.horizontalBox.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(12, 423);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(159, 37);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.toolTip.SetToolTip(this.closeButton, "Close the Xenia Settings menu");
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // executablePathBox
            // 
            this.executablePathBox.Location = new System.Drawing.Point(102, 109);
            this.executablePathBox.Name = "executablePathBox";
            this.executablePathBox.Size = new System.Drawing.Size(247, 26);
            this.executablePathBox.TabIndex = 4;
            this.toolTip.SetToolTip(this.executablePathBox, "Absolute filepath to the XEX");
            // 
            // executablePathLabel
            // 
            this.executablePathLabel.AutoSize = true;
            this.executablePathLabel.Location = new System.Drawing.Point(12, 114);
            this.executablePathLabel.Name = "executablePathLabel";
            this.executablePathLabel.Size = new System.Drawing.Size(83, 20);
            this.executablePathLabel.TabIndex = 5;
            this.executablePathLabel.Text = "XEX Path:";
            // 
            // executableNameLabel
            // 
            this.executableNameLabel.AutoSize = true;
            this.executableNameLabel.Location = new System.Drawing.Point(12, 155);
            this.executableNameLabel.Name = "executableNameLabel";
            this.executableNameLabel.Size = new System.Drawing.Size(92, 20);
            this.executableNameLabel.TabIndex = 6;
            this.executableNameLabel.Text = "XEX Name:";
            // 
            // executableNameBox
            // 
            this.executableNameBox.Location = new System.Drawing.Point(102, 152);
            this.executableNameBox.Name = "executableNameBox";
            this.executableNameBox.Size = new System.Drawing.Size(247, 26);
            this.executableNameBox.TabIndex = 7;
            this.toolTip.SetToolTip(this.executableNameBox, "Name for the executable");
            // 
            // browseExecutableButton
            // 
            this.browseExecutableButton.Location = new System.Drawing.Point(51, 194);
            this.browseExecutableButton.Name = "browseExecutableButton";
            this.browseExecutableButton.Size = new System.Drawing.Size(254, 37);
            this.browseExecutableButton.TabIndex = 8;
            this.browseExecutableButton.Text = "Browse For XEX...";
            this.toolTip.SetToolTip(this.browseExecutableButton, "Browse for an XEX to add as an executable");
            this.browseExecutableButton.UseVisualStyleBackColor = true;
            this.browseExecutableButton.Click += new System.EventHandler(this.browseExecutableButton_Click);
            // 
            // executableBox
            // 
            this.executableBox.FormattingEnabled = true;
            this.executableBox.Location = new System.Drawing.Point(72, 68);
            this.executableBox.Name = "executableBox";
            this.executableBox.Size = new System.Drawing.Size(277, 28);
            this.executableBox.TabIndex = 2;
            this.executableBox.SelectedIndexChanged += new System.EventHandler(this.executableBox_SelectedIndexChanged);
            // 
            // executableLabel
            // 
            this.executableLabel.AutoSize = true;
            this.executableLabel.Location = new System.Drawing.Point(12, 71);
            this.executableLabel.Name = "executableLabel";
            this.executableLabel.Size = new System.Drawing.Size(54, 20);
            this.executableLabel.TabIndex = 9;
            this.executableLabel.Text = "XEXs:";
            // 
            // addExecutableButton
            // 
            this.addExecutableButton.Location = new System.Drawing.Point(16, 25);
            this.addExecutableButton.Name = "addExecutableButton";
            this.addExecutableButton.Size = new System.Drawing.Size(146, 37);
            this.addExecutableButton.TabIndex = 10;
            this.addExecutableButton.Text = "Add XEX";
            this.toolTip.SetToolTip(this.addExecutableButton, "Add an executable (XEX) to this game");
            this.addExecutableButton.UseVisualStyleBackColor = true;
            this.addExecutableButton.Click += new System.EventHandler(this.addExecutableButton_Click);
            // 
            // deleteExecutableButton
            // 
            this.deleteExecutableButton.Location = new System.Drawing.Point(202, 25);
            this.deleteExecutableButton.Name = "deleteExecutableButton";
            this.deleteExecutableButton.Size = new System.Drawing.Size(146, 37);
            this.deleteExecutableButton.TabIndex = 11;
            this.deleteExecutableButton.Text = "Delete XEX";
            this.deleteExecutableButton.UseVisualStyleBackColor = true;
            this.deleteExecutableButton.Click += new System.EventHandler(this.deleteExecutableButton_Click);
            // 
            // openExecutableDialog
            // 
            this.openExecutableDialog.Title = "Choose an XEX";
            // 
            // xexGroup
            // 
            this.xexGroup.Controls.Add(this.addExecutableButton);
            this.xexGroup.Controls.Add(this.deleteExecutableButton);
            this.xexGroup.Controls.Add(this.executablePathBox);
            this.xexGroup.Controls.Add(this.executableLabel);
            this.xexGroup.Controls.Add(this.executablePathLabel);
            this.xexGroup.Controls.Add(this.executableBox);
            this.xexGroup.Controls.Add(this.executableNameLabel);
            this.xexGroup.Controls.Add(this.browseExecutableButton);
            this.xexGroup.Controls.Add(this.executableNameBox);
            this.xexGroup.Location = new System.Drawing.Point(362, 12);
            this.xexGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xexGroup.Name = "xexGroup";
            this.xexGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xexGroup.Size = new System.Drawing.Size(362, 258);
            this.xexGroup.TabIndex = 12;
            this.xexGroup.TabStop = false;
            this.xexGroup.Text = "Additional Executables";
            // 
            // compatGroup
            // 
            this.compatGroup.Controls.Add(this.canaryCompatBox);
            this.compatGroup.Controls.Add(this.xeniaCompatBox);
            this.compatGroup.Controls.Add(this.canaryCompatLabel);
            this.compatGroup.Controls.Add(this.xeniaCompatLabel);
            this.compatGroup.Location = new System.Drawing.Point(362, 275);
            this.compatGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compatGroup.Name = "compatGroup";
            this.compatGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.compatGroup.Size = new System.Drawing.Size(362, 192);
            this.compatGroup.TabIndex = 13;
            this.compatGroup.TabStop = false;
            this.compatGroup.Text = "Compatibility";
            // 
            // canaryCompatBox
            // 
            this.canaryCompatBox.FormattingEnabled = true;
            this.canaryCompatBox.Items.AddRange(new object[] {
            "Broken, Doesn\'t Start",
            "Starts, Crashes",
            "Reaches Menus",
            "Reaches Gameplay",
            "Somewhat Playable",
            "Mostly Playable",
            "Near-Perfect Gameplay",
            "Unknown, Untested"});
            this.canaryCompatBox.Location = new System.Drawing.Point(16, 140);
            this.canaryCompatBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.canaryCompatBox.Name = "canaryCompatBox";
            this.canaryCompatBox.Size = new System.Drawing.Size(330, 28);
            this.canaryCompatBox.TabIndex = 3;
            // 
            // xeniaCompatBox
            // 
            this.xeniaCompatBox.FormattingEnabled = true;
            this.xeniaCompatBox.Items.AddRange(new object[] {
            "Broken, Doesn\'t Start",
            "Starts, Crashes",
            "Reaches Menus",
            "Reaches Gameplay",
            "Somewhat Playable",
            "Mostly Playable",
            "Near-Perfect Gameplay",
            "Unknown, Untested"});
            this.xeniaCompatBox.Location = new System.Drawing.Point(16, 69);
            this.xeniaCompatBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xeniaCompatBox.Name = "xeniaCompatBox";
            this.xeniaCompatBox.Size = new System.Drawing.Size(330, 28);
            this.xeniaCompatBox.TabIndex = 2;
            // 
            // canaryCompatLabel
            // 
            this.canaryCompatLabel.AutoSize = true;
            this.canaryCompatLabel.Location = new System.Drawing.Point(14, 115);
            this.canaryCompatLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.canaryCompatLabel.Name = "canaryCompatLabel";
            this.canaryCompatLabel.Size = new System.Drawing.Size(156, 20);
            this.canaryCompatLabel.TabIndex = 1;
            this.canaryCompatLabel.Text = "Canary Compatibility:";
            // 
            // xeniaCompatLabel
            // 
            this.xeniaCompatLabel.AutoSize = true;
            this.xeniaCompatLabel.Location = new System.Drawing.Point(14, 45);
            this.xeniaCompatLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.xeniaCompatLabel.Name = "xeniaCompatLabel";
            this.xeniaCompatLabel.Size = new System.Drawing.Size(147, 20);
            this.xeniaCompatLabel.TabIndex = 0;
            this.xeniaCompatLabel.Text = "Xenia Compatibility:";
            // 
            // iconGroup
            // 
            this.iconGroup.Controls.Add(this.iconBrowseButton);
            this.iconGroup.Controls.Add(this.iconLabel);
            this.iconGroup.Controls.Add(this.iconPathBox);
            this.iconGroup.Controls.Add(this.iconBox);
            this.iconGroup.Location = new System.Drawing.Point(12, 294);
            this.iconGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.iconGroup.Name = "iconGroup";
            this.iconGroup.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.iconGroup.Size = new System.Drawing.Size(340, 117);
            this.iconGroup.TabIndex = 14;
            this.iconGroup.TabStop = false;
            this.iconGroup.Text = "Icon Settings";
            // 
            // iconBrowseButton
            // 
            this.iconBrowseButton.Image = global::XLCompanion.Properties.Resources.FolderBrowserDialogControl;
            this.iconBrowseButton.Location = new System.Drawing.Point(200, 45);
            this.iconBrowseButton.Name = "iconBrowseButton";
            this.iconBrowseButton.Size = new System.Drawing.Size(34, 31);
            this.iconBrowseButton.TabIndex = 29;
            this.toolTip.SetToolTip(this.iconBrowseButton, "Browse for an icon");
            this.iconBrowseButton.UseVisualStyleBackColor = true;
            this.iconBrowseButton.Click += new System.EventHandler(this.iconBrowseButton_Click);
            // 
            // iconLabel
            // 
            this.iconLabel.AutoSize = true;
            this.iconLabel.Location = new System.Drawing.Point(9, 51);
            this.iconLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.iconLabel.Name = "iconLabel";
            this.iconLabel.Size = new System.Drawing.Size(81, 20);
            this.iconLabel.TabIndex = 4;
            this.iconLabel.Text = "Icon Path:";
            // 
            // iconPathBox
            // 
            this.iconPathBox.Location = new System.Drawing.Point(10, 77);
            this.iconPathBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.iconPathBox.Name = "iconPathBox";
            this.iconPathBox.Size = new System.Drawing.Size(223, 26);
            this.iconPathBox.TabIndex = 3;
            this.toolTip.SetToolTip(this.iconPathBox, "Absolute filepath to an icon");
            this.iconPathBox.TextChanged += new System.EventHandler(this.iconPathBox_TextChanged);
            // 
            // iconBox
            // 
            this.iconBox.Location = new System.Drawing.Point(244, 12);
            this.iconBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(96, 98);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconBox.TabIndex = 2;
            this.iconBox.TabStop = false;
            // 
            // openIconDialog
            // 
            this.openIconDialog.Title = "Choose an icon";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 451);
            this.Controls.Add(this.iconGroup);
            this.Controls.Add(this.compatGroup);
            this.Controls.Add(this.xexGroup);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.graphicsGroup);
            this.Controls.Add(this.generalGroup);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(742, 507);
            this.MinimumSize = new System.Drawing.Size(742, 507);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Xenia Settings";
            this.generalGroup.ResumeLayout(false);
            this.generalGroup.PerformLayout();
            this.graphicsGroup.ResumeLayout(false);
            this.graphicsGroup.PerformLayout();
            this.xexGroup.ResumeLayout(false);
            this.xexGroup.PerformLayout();
            this.compatGroup.ResumeLayout(false);
            this.compatGroup.PerformLayout();
            this.iconGroup.ResumeLayout(false);
            this.iconGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox generalGroup;
        private System.Windows.Forms.ComboBox licenseBox;
        private System.Windows.Forms.CheckBox canaryBox;
        private System.Windows.Forms.GroupBox graphicsGroup;
        private System.Windows.Forms.ComboBox horizontalBox;
        private System.Windows.Forms.Label horizontalScaleLabel;
        private System.Windows.Forms.Label verticalScaleLabel;
        private System.Windows.Forms.ComboBox verticalBox;
        private System.Windows.Forms.Label renderText;
        private System.Windows.Forms.ComboBox rendererBox;
        private System.Windows.Forms.CheckBox readbackCheck;
        private System.Windows.Forms.CheckBox vSyncCheck;
        private System.Windows.Forms.CheckBox mountCacheCheck;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox executablePathBox;
        private System.Windows.Forms.Label executablePathLabel;
        private System.Windows.Forms.Label executableNameLabel;
        private System.Windows.Forms.TextBox executableNameBox;
        private System.Windows.Forms.Button browseExecutableButton;
        private System.Windows.Forms.ComboBox executableBox;
        private System.Windows.Forms.Label executableLabel;
        private System.Windows.Forms.Button addExecutableButton;
        private System.Windows.Forms.Button deleteExecutableButton;
        private System.Windows.Forms.OpenFileDialog openExecutableDialog;
        private System.Windows.Forms.GroupBox xexGroup;
        private System.Windows.Forms.GroupBox compatGroup;
        private System.Windows.Forms.ComboBox canaryCompatBox;
        private System.Windows.Forms.ComboBox xeniaCompatBox;
        private System.Windows.Forms.Label canaryCompatLabel;
        private System.Windows.Forms.Label xeniaCompatLabel;
        private System.Windows.Forms.GroupBox iconGroup;
        private System.Windows.Forms.Label iconLabel;
        private System.Windows.Forms.TextBox iconPathBox;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.Button iconBrowseButton;
        private System.Windows.Forms.OpenFileDialog openIconDialog;
        private System.Windows.Forms.ToolTip toolTip;
    }
}