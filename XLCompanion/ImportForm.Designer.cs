namespace XLCompanion
{
    partial class ImportForm
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
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.iconCheck = new System.Windows.Forms.CheckBox();
            this.titleCheck = new System.Windows.Forms.CheckBox();
            this.titleIdCheck = new System.Windows.Forms.CheckBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.titleIdBox = new System.Windows.Forms.TextBox();
            this.importButton = new System.Windows.Forms.Button();
            this.saveIconDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // iconBox
            // 
            this.iconBox.Location = new System.Drawing.Point(86, 3);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(64, 64);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconBox.TabIndex = 1;
            this.iconBox.TabStop = false;
            // 
            // iconCheck
            // 
            this.iconCheck.AutoSize = true;
            this.iconCheck.Checked = true;
            this.iconCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.iconCheck.Location = new System.Drawing.Point(12, 30);
            this.iconCheck.Name = "iconCheck";
            this.iconCheck.Size = new System.Drawing.Size(50, 17);
            this.iconCheck.TabIndex = 2;
            this.iconCheck.Text = "Icon:";
            this.iconCheck.UseVisualStyleBackColor = true;
            // 
            // titleCheck
            // 
            this.titleCheck.AutoSize = true;
            this.titleCheck.Checked = true;
            this.titleCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.titleCheck.Location = new System.Drawing.Point(12, 83);
            this.titleCheck.Name = "titleCheck";
            this.titleCheck.Size = new System.Drawing.Size(57, 17);
            this.titleCheck.TabIndex = 3;
            this.titleCheck.Text = "Name:";
            this.titleCheck.UseVisualStyleBackColor = true;
            // 
            // titleIdCheck
            // 
            this.titleIdCheck.AutoSize = true;
            this.titleIdCheck.Checked = true;
            this.titleIdCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.titleIdCheck.Location = new System.Drawing.Point(12, 109);
            this.titleIdCheck.Name = "titleIdCheck";
            this.titleIdCheck.Size = new System.Drawing.Size(63, 17);
            this.titleIdCheck.TabIndex = 4;
            this.titleIdCheck.Text = "Title ID:";
            this.titleIdCheck.UseVisualStyleBackColor = true;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(86, 81);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(203, 20);
            this.nameBox.TabIndex = 5;
            // 
            // titleIdBox
            // 
            this.titleIdBox.Location = new System.Drawing.Point(86, 106);
            this.titleIdBox.Name = "titleIdBox";
            this.titleIdBox.Size = new System.Drawing.Size(83, 20);
            this.titleIdBox.TabIndex = 6;
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(202, 107);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(122, 23);
            this.importButton.TabIndex = 7;
            this.importButton.Text = "Import Selected";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // saveIconDialog
            // 
            this.saveIconDialog.Title = "Save Icon...";
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 138);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.titleIdBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.titleIdCheck);
            this.Controls.Add(this.titleCheck);
            this.Controls.Add(this.iconCheck);
            this.Controls.Add(this.iconBox);
            this.MaximumSize = new System.Drawing.Size(352, 177);
            this.MinimumSize = new System.Drawing.Size(352, 177);
            this.Name = "ImportForm";
            this.Text = "Import From STFS/SVOD";
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.CheckBox iconCheck;
        private System.Windows.Forms.CheckBox titleCheck;
        private System.Windows.Forms.CheckBox titleIdCheck;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox titleIdBox;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.SaveFileDialog saveIconDialog;
    }
}