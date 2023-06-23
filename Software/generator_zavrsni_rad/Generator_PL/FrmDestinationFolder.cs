using generator_zavrsni_rad.Generator_BLL;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace generator_zavrsni_rad.Generator_PL
{
    public partial class FrmDestinationFolder : Form
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private string _userProfile;
        private string _reposDirectory;

        public FrmDestinationFolder()
        {
            InitializeComponent();
        }

        private void FrmDestinationFolder_Load(object sender, EventArgs e)
        {
            Close();
            _userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); ;
            _reposDirectory = Path.Combine(_userProfile, "source", "repos");
            txtPath.Text = _reposDirectory;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = _reposDirectory;

            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowFolderBrowserDialog()));
            }
            else
            {
                ShowFolderBrowserDialog();
            }

        }

        private void ShowFolderBrowserDialog()
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog.SelectedPath.ToString();
            }
            else
            {
                MessageBox.Show("Choose path to the file!");
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            Generator.chosenPath = txtPath.Text.ToString();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
