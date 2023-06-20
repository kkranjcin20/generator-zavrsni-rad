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
