using generator_WPF.Generator_BLL;
using Microsoft.CodeAnalysis;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace generator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {	        
        List<TableMetadata> tables = new List<TableMetadata>();
        List<List<TableMetadata>> classes = new List<List<TableMetadata>>();
        Generator_WPF generator = new Generator_WPF();

		public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbAccessModifier.Items.Add("Public");
            cmbAccessModifier.Items.Add("Private");
            cmbAccessModifier.Items.Add("Protected");
            cmbAccessModifier.SelectedItem = "Public";

            cmbAccessModifierFromFile.Items.Add("Public");
            cmbAccessModifierFromFile.Items.Add("Private");
            cmbAccessModifierFromFile.Items.Add("Protected");

            cmbDataType.Items.Add("Integer");
            cmbDataType.Items.Add("Float");
            cmbDataType.Items.Add("Double");
            cmbDataType.Items.Add("String");
            cmbDataType.Items.Add("Character");
            cmbDataType.Items.Add("Bool");
            cmbDataType.Items.Add("DateTime");

            cmbDataTypeFromFile.Items.Add("Integer");
            cmbDataTypeFromFile.Items.Add("Float");
            cmbDataTypeFromFile.Items.Add("Double");
            cmbDataTypeFromFile.Items.Add("String");
            cmbDataTypeFromFile.Items.Add("Character");
            cmbDataTypeFromFile.Items.Add("Bool");
            cmbDataTypeFromFile.Items.Add("DateTime");
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "JSON files (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string jsonContent = File.ReadAllText(selectedFilePath);
                var metadata = JsonConvert.DeserializeObject<List<TableMetadata>>(jsonContent);

                string regexForInt = @"\b[intI][nN][tT]\b";
                string regexForPublic = @"\b[publicP][uU][bB][lL][iI][cC]\b";

                txtClassNameFromFile.Text = metadata.FirstOrDefault().TableName;
                txtPropertyNameFromFile.Text = metadata.FirstOrDefault().ColumnName;

                string dataType = metadata.FirstOrDefault().DataType;
                string accessModifier = metadata.FirstOrDefault().AccessModifier;

                if (!string.IsNullOrEmpty(dataType))
                {
                    if (Regex.IsMatch(dataType, regexForInt, RegexOptions.IgnoreCase))
                    {
                        cmbDataTypeFromFile.SelectedItem = "Integer";
                    }
                }

                if (!string.IsNullOrEmpty(accessModifier))
                {
                    if(Regex.IsMatch(accessModifier, regexForPublic, RegexOptions.IgnoreCase))
                    {
                        cmbAccessModifierFromFile.SelectedItem = "Public";
                    }
                }

                tables.AddRange(metadata);

                /*
                //openFileDialog.ValidateNames = true;
                foreach (string file in openFileDialog.FileNames)
                {
                    if (openFileDialog.CheckFileExists && openFileDialog.CheckPathExists)
                    {
                        //var fileStream = new FileStream(selectedFilePath, FileMode.Open);
                        string selectedFilePath = openFileDialog.FileName;

                        try
                        {
                            string jsonContent = File.ReadAllText(selectedFilePath);
                            var metadata1 = JsonConvert.DeserializeObject<TablesMetadata>(jsonContent);
                            var metadata2 = JsonConvert.DeserializeObject<TableMetadata>(jsonContent);

                            System.Windows.Forms.MessageBox.Show("Test = " + metadata2.Name.ToString());

                            txtTableNameFromFile.Text = metadata2.Name.ToString();
                            txtColumnNameFromFile.Text = metadata2.Columns.FirstOrDefault().Name.ToString();

                            //foreach (var table in metadata.Tables)
                            //{
                            //    generator.GenerateClass();
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Greška prilikom čitanja datoteke: " + ex.Message);
                        }
                    }
                }
                */
            }
        }

        private void btnAddProperty_Click(object sender, RoutedEventArgs e)
        {
            if (txtClassName.Text.Length != 0 &&
                txtPropertyName.Text.Length != 0 &&
                cmbDataType.SelectedItem.ToString() == "")
            {
                TableMetadata metadata = new TableMetadata
                {
                    TableName = txtClassName.Text,
                    ColumnName = txtPropertyName.Text,
                    DataType = cmbDataType.SelectedItem.ToString(),
                    AccessModifier = cmbAccessModifier.SelectedItem.ToString()
                };
                tables.Add(metadata);

                txtPropertyName.Text = "";

                txtAddedProperties.Text = tables.Count.ToString();
                txtClassName.IsEnabled = false;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Insert all attributes!");
            }
        }
        private void btnAddClass_Click(object sender, RoutedEventArgs e)
        {
            classes.Add(tables);
            tables.Clear();
            txtClassName.Text = "";
            txtPropertyName.Text = "";
            cmbAccessModifier.SelectedItem = "Public";
            txtAddedProperties.Text = "";
            txtClassName.IsEnabled = true;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            foreach(var classToGenerate in classes)
            {
                generator.GenerateClass(classToGenerate);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}