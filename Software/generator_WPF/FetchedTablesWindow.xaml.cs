using generator.Generator_BLL;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace generator
{
    /// <summary>
    /// Interaction logic for FetchedTables.xaml
    /// </summary>
    public partial class FetchedTables : Window
    {
        SSMSMetadataFetcher metadataFetcher = new SSMSMetadataFetcher();
        Generator generator = new Generator();
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
            var tables = metadataFetcher.FetchTables(_connectionString);
            dgTables.ItemsSource = tables;
            dgTables.Columns[1].Visibility = Visibility.Hidden;
            dgTables.Columns[2].Visibility = Visibility.Hidden;
        }

        private void btnChooseTable_Click(object sender, RoutedEventArgs e)
        {
            if (dgTables.SelectedItem != null)
            {
                List<string> classNames = new List<string>();
                List<string> generatedClassCodes = new List<string>();
                foreach (var selectedItem in dgTables.SelectedItems)
                {
                    var selectedTable = selectedItem as TableMetadata;
                    selectedTable.Namespace = _classNamespace;
                    generatedClassCodes.Add(generator.GenerateClass(selectedTable));
                    classNames.Add(selectedTable.Name);
                }
                SaveClassWindow saveClassWindow = new SaveClassWindow(classNames, generatedClassCodes);
                saveClassWindow.ShowDialog();
            }
        }
    }
}