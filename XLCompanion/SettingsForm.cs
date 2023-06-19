using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLCompanion
{
    using GameData = XeniaLauncher.Shared.GameData;
    public partial class SettingsForm : Form
    {
        GameData data;
        public SettingsForm(GameData data)
        {
            InitializeComponent();
            this.data = data;
            if (data.license == GameData.LicenseMask.None)
            {
                licenseBox.Text = "No License (0)";
            }
            else if (data.license == GameData.LicenseMask.First)
            {
                licenseBox.Text = "First License (1)";
            }
            else if (data.license == GameData.LicenseMask.All)
            {
                licenseBox.Text = "All Licenses (-1)";
            }
            canaryBox.Checked = data.preferCanary;
            horizontalBox.SelectedIndex = data.resX - 1;
            verticalBox.SelectedIndex = data.resY - 1;
            if (data.renderer == GameData.Renderer.Any)
            {
                rendererBox.SelectedIndex = 0;
            }
            else if (data.renderer == GameData.Renderer.Direct3D12)
            {
                rendererBox.SelectedIndex = 1;
            }
            else if (data.renderer == GameData.Renderer.Vulkan)
            {
                rendererBox.SelectedIndex = 2;
            }
            vSyncCheck.Checked = data.vsync;
            readbackCheck.Checked = data.cpuReadback;
            mountCacheCheck.Checked = data.mountCache;

            foreach (string xex in data.xexNames)
            {
                executableBox.Items.Add(xex);
            }

            switch (data.xeniaCompat)
            {
                case GameData.XeniaCompat.Unknown:
                    xeniaCompatBox.SelectedIndex = 7;
                    break;
                case GameData.XeniaCompat.Broken:
                    xeniaCompatBox.SelectedIndex = 0;
                    break;
                case GameData.XeniaCompat.Starts:
                    xeniaCompatBox.SelectedIndex = 1;
                    break;
                case GameData.XeniaCompat.Menu:
                    xeniaCompatBox.SelectedIndex = 2;
                    break;
                case GameData.XeniaCompat.Gameplay1:
                    xeniaCompatBox.SelectedIndex = 3;
                    break;
                case GameData.XeniaCompat.Gameplay2:
                    xeniaCompatBox.SelectedIndex = 4;
                    break;
                case GameData.XeniaCompat.Gameplay3:
                    xeniaCompatBox.SelectedIndex = 5;
                    break;
                case GameData.XeniaCompat.Playable:
                    xeniaCompatBox.SelectedIndex = 6;
                    break;
            }
            switch (data.canaryCompat)
            {
                case GameData.XeniaCompat.Unknown:
                    canaryCompatBox.SelectedIndex = 7;
                    break;
                case GameData.XeniaCompat.Broken:
                    canaryCompatBox.SelectedIndex = 0;
                    break;
                case GameData.XeniaCompat.Starts:
                    canaryCompatBox.SelectedIndex = 1;
                    break;
                case GameData.XeniaCompat.Menu:
                    canaryCompatBox.SelectedIndex = 2;
                    break;
                case GameData.XeniaCompat.Gameplay1:
                    canaryCompatBox.SelectedIndex = 3;
                    break;
                case GameData.XeniaCompat.Gameplay2:
                    canaryCompatBox.SelectedIndex = 4;
                    break;
                case GameData.XeniaCompat.Gameplay3:
                    canaryCompatBox.SelectedIndex = 5;
                    break;
                case GameData.XeniaCompat.Playable:
                    canaryCompatBox.SelectedIndex = 6;
                    break;
            }

            iconPathBox.Text = data.iconPath;
            if (data.iconPath != null && data.iconPath != "NULL")
            {
                try
                {
                    iconBox.Image = Image.FromFile(data.iconPath);
                }
                catch (Exception e)
                {

                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (licenseBox.SelectedIndex == 0)
            {
                data.license = GameData.LicenseMask.None;
            }
            else if (licenseBox.SelectedIndex == 1)
            {
                data.license = GameData.LicenseMask.First;
            }
            else if (licenseBox.SelectedIndex == 2)
            {
                data.license = GameData.LicenseMask.All;
            }
            data.preferCanary = canaryBox.Checked;
            data.resX = horizontalBox.SelectedIndex + 1;
            data.resY = verticalBox.SelectedIndex + 1;
            if (rendererBox.SelectedIndex == 0)
            {
                data.renderer = GameData.Renderer.Any;
            }
            else if (rendererBox.SelectedIndex == 1)
            {
                data.renderer = GameData.Renderer.Direct3D12;
            }
            else if (rendererBox.SelectedIndex == 2)
            {
                data.renderer = GameData.Renderer.Vulkan;
            }
            data.vsync = vSyncCheck.Checked;
            data.cpuReadback = readbackCheck.Checked;
            data.mountCache = mountCacheCheck.Checked;

            switch (xeniaCompatBox.SelectedIndex)
            {
                case 0:
                    data.xeniaCompat = GameData.XeniaCompat.Broken;
                    break;
                case 1:
                    data.xeniaCompat = GameData.XeniaCompat.Starts;
                    break;
                case 2:
                    data.xeniaCompat = GameData.XeniaCompat.Menu;
                    break;
                case 3:
                    data.xeniaCompat = GameData.XeniaCompat.Gameplay1;
                    break;
                case 4:
                    data.xeniaCompat = GameData.XeniaCompat.Gameplay2;
                    break;
                case 5:
                    data.xeniaCompat = GameData.XeniaCompat.Gameplay3;
                    break;
                case 6:
                    data.xeniaCompat = GameData.XeniaCompat.Playable;
                    break;
                case 7:
                    data.xeniaCompat = GameData.XeniaCompat.Unknown;
                    break;
            }
            switch (canaryCompatBox.SelectedIndex)
            {
                case 0:
                    data.canaryCompat = GameData.XeniaCompat.Broken;
                    break;
                case 1:
                    data.canaryCompat = GameData.XeniaCompat.Starts;
                    break;
                case 2:
                    data.canaryCompat = GameData.XeniaCompat.Menu;
                    break;
                case 3:
                    data.canaryCompat = GameData.XeniaCompat.Gameplay1;
                    break;
                case 4:
                    data.canaryCompat = GameData.XeniaCompat.Gameplay2;
                    break;
                case 5:
                    data.canaryCompat = GameData.XeniaCompat.Gameplay3;
                    break;
                case 6:
                    data.canaryCompat = GameData.XeniaCompat.Playable;
                    break;
                case 7:
                    data.canaryCompat = GameData.XeniaCompat.Unknown;
                    break;
            }

            Close();
        }

        private void addExecutableButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(executableNameBox.Text) || String.IsNullOrEmpty(executablePathBox.Text))
            {
                MessageBox.Show("The XEX must have a name and a path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (data.xexNames.Contains(executableNameBox.Text))
                {
                    MessageBox.Show("XEX cannot have the same name as an existing XEX", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    data.xexNames.Add(executableNameBox.Text);
                    data.xexPaths.Add(executablePathBox.Text);
                    executableBox.Items.Add(executableNameBox.Text);
                }
            }
        }

        private void deleteExecutableButton_Click(object sender, EventArgs e)
        {
            if (executableBox.Items.Contains(executableBox.Text))
            {
                data.xexPaths.RemoveAt(data.xexNames.IndexOf(executableBox.Text));
                data.xexNames.Remove(executableBox.Text);
                executableBox.Items.Remove(executableBox.Text);
                executableBox.Text = "";
            }
        }

        private void browseExecutableButton_Click(object sender, EventArgs e)
        {
            if (openExecutableDialog.ShowDialog() == DialogResult.OK)
            {
                executablePathBox.Text = openExecutableDialog.FileName;
            }
        }

        private void executableBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            executablePathBox.Text = data.xexPaths[data.xexNames.IndexOf(executableBox.Text)];
            executableNameBox.Text = executableBox.Text;
        }

        private void iconBrowseButton_Click(object sender, EventArgs e)
        {
            if (openIconDialog.ShowDialog() == DialogResult.OK)
            {
                iconPathBox.Text = openIconDialog.FileName;
                data.iconPath = iconPathBox.Text;
                if (data.iconPath != null && data.iconPath != "NULL")
                {
                    try
                    {
                        iconBox.Image = Image.FromFile(data.iconPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        private void iconPathBox_TextChanged(object sender, EventArgs e)
        {
            data.iconPath = iconPathBox.Text;
        }
    }
}
