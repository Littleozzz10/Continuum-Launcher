using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace XLCompanion
{
    public partial class ImportForm : Form
    {
        private Form1 form;
        public ImportForm(string filepath, Form1 form)
        {
            InitializeComponent();
            this.form = form;

            STFS file = new STFS(filepath);
            iconBox.Image = file.icon;
            nameBox.Text = file.data.displayName;
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            nameBox.Text = r.Replace(nameBox.Text, "");
            titleIdBox.Text = r.Replace(file.titleID, "");
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            string name = "";
            if (titleCheck.Checked)
            {
                name = nameBox.Text;
            }
            string titleId = "";
            if (titleIdCheck.Checked)
            {
                titleId = titleIdBox.Text;
            }
            form.SetImports(name, titleId);
            if (iconCheck.Checked)
            {
                saveIconDialog.FileName = titleIdBox.Text + ".png";
                if (saveIconDialog.ShowDialog() == DialogResult.OK)
                {
                    iconBox.Image.Save(saveIconDialog.FileName);
                    form.data[form.selectedIndex].iconPath = saveIconDialog.FileName;
                }
            }
            Close();
        }
    }
}
