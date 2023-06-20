using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Windows.Forms;
using generator_zavrsni_rad.Generator_BLL;
using generator_zavrsni_rad.Generator_PL;
using static System.Net.WebRequestMethods;

namespace generator_zavrsni_rad
{
    public partial class FrmGenerator : Form
    {
        Generator generator = new Generator();
        private TableMetadata _table;
        
        public FrmGenerator(TableMetadata table)
        {
            InitializeComponent();
            _table = table;
        }
        public FrmGenerator()
        {
        }
        private void FrmGenerator_Load(object sender, EventArgs e)
        {
            _table = generator.FetchTableMetadata(_table.TableName);
            dgvTableColumns.DataSource = _table.Columns.ToList();
        }

        private void btnFetchData_Click(object sender, EventArgs e)
        {
            _table = generator.FetchTableMetadata(_table.TableName);
            dgvTableColumns.DataSource = _table.Columns.ToList();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var frmDestinationFolder = new FrmDestinationFolder();
            frmDestinationFolder.FormClosed += (s, args) =>
            {
                if (generator.GenerateClass())
                {
                    NotifyUser();
                }
            };
            frmDestinationFolder.ShowDialog();
        }

        private void NotifyUser()
        {
            MessageBox.Show("Class was successfully generated! \nYou will have to reload the project to see them in the Solution Explorer.", "Generated class", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
