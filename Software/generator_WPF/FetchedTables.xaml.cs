using generator_WPF.Generator_BLL;
using System.Windows;

namespace generator_WPF
{
    /// <summary>
    /// Interaction logic for FetchedTables.xaml
    /// </summary>
    public partial class FetchedTables : Window
    {
        Generator_WPF generator = new Generator_WPF();
        private string _connectionString;
        private string _classNamespace;
        public FetchedTables(string connectionString, string classNamespace)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _classNamespace = classNamespace;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var tables = generator.FetchTables(_connectionString);
            dgTables.ItemsSource = tables;
        }

        private void btnChooseTable_Click(object sender, RoutedEventArgs e)
        {
            if (dgTables.SelectedItem != null)
            {
                foreach(var selectedItem in dgTables.SelectedItems)
                {
                    var selectedTable = selectedItem as TableMetadata;
                    var table = generator.FetchTableMetadata(selectedTable);
                    table.Namespace = _classNamespace;
                    generator.GenerateClass(table);
                }
            }
        }
    }
}