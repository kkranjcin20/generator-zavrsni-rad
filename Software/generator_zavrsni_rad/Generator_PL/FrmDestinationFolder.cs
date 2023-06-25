using generator_zavrsni_rad.Generator_BLL;
using System;
using System.IO;
using System.Windows.Forms;

namespace generator_zavrsni_rad.Generator_PL
{
    public partial class FrmDestinationFolder : Form
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private string _projectDir;
        public static bool isCancel;

        public FrmDestinationFolder()
        {
            InitializeComponent();
        }

        private void FrmDestinationFolder_Load(object sender, EventArgs e)
        {
            _projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            txtPath.Text = _projectDir;
            isCancel = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Select a folder in which you want to save the generated classes.";
            folderBrowserDialog.SelectedPath = _projectDir;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog.SelectedPath.ToString();
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            Generator.chosenPath = txtPath.Text.ToString();
            isCancel = false;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            Close();
        }
    }
}
