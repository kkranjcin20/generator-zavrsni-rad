using generator_zavrsni_rad.Generator_BLL;
using System;
using System.Windows.Forms;

namespace generator_zavrsni_rad.Generator_PL
{
    public partial class FrmDatabase : Form
    {
        public FrmDatabase()
        {
            InitializeComponent();
        }

        private void FrmDatabase_Load(object sender, EventArgs e)
        {
            var generator = new Generator();
            var tables = generator.FetchTables();
            dgvTables.DataSource = tables;
            dgvTables.Columns[1].Visible = false;
            dgvTables.Columns[2].Visible = false;
            dgvTables.Columns[3].Visible = false;
        }

        private void btnFetchTableData_Click(object sender, EventArgs e)
        {
            if (dgvTables.SelectedRows.Count == 1)
            {
                var selectedTable = dgvTables.CurrentRow.DataBoundItem as TableMetadata;
                FrmGenerator frmGenerator = new FrmGenerator(selectedTable);
                frmGenerator.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a table from the list for which you want to see the details.");
            }
        }
    }
}
