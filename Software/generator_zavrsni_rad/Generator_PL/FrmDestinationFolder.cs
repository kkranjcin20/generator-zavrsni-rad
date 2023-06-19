using generator_zavrsni_rad.Generator_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;

namespace generator_zavrsni_rad.Generator_PL
{
    public partial class FrmDestinationFolder : Form
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private string _projectDir;

        public FrmDestinationFolder()
        {
            InitializeComponent();
        }

        private void FrmDestinationFolder_Load(object sender, EventArgs e)
        {
            _projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            txtPath.Text = _projectDir;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = _projectDir;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog.SelectedPath;

                string solutionName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                if (solutionName.EndsWith("exe"))
                {
                    solutionName = Path.ChangeExtension(solutionName, null);
                }

                string projectPath = Path.Combine(_projectDir, $"{solutionName}.csproj");

                try
                {
                    Project project = new Project(_projectDir);
                    project.AddItem("Compile", projectPath);
                    project.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating file: " + ex.Message, "File Creation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Choose path to the file!");
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            FrmGenerator frmGenerator = new FrmGenerator
            {
                chosenPath = txtPath.Text.ToString()
            };
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
