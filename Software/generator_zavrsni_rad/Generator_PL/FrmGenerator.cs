using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.Build.Evaluation;
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
