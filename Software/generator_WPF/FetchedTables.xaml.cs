using generator_WPF.Generator_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace generator_WPF
{
    /// <summary>
    /// Interaction logic for FetchedTables.xaml
    /// </summary>
    public partial class FetchedTables : Window
    {
        Generator_WPF generator = new Generator_WPF();
        private string _connectionString;
        public FetchedTables(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var tables = generator.FetchTables(_connectionString);
            dgTables.ItemsSource = tables;
        }

        private void btnChooseTable_Click(object sender, RoutedEventArgs e)
        {
            if(dgTables.SelectedItem != null)
            {
                var selectedTable = dgTables.SelectedItem as TableMetadata;
                var table = generator.FetchTableMetadata(selectedTable);
                generator.GenerateClass(table);
            }
        }
    }
}
