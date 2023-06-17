using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using generator_zavrsni_rad.Generator_BLL;

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
            if (!generator.GenerateClass())
            {
                MessageBox.Show("Class is already generated!");
            }
        }
    }
}
