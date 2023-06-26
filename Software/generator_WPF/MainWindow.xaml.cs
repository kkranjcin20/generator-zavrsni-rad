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
        List<TableMetadata> metadataFromFile = new List<TableMetadata>();
        int indexMetadata = 0;
        int addedProperties = 0;
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

            cmbDataType.Items.Add("Integer");
            cmbDataType.Items.Add("Float");
            cmbDataType.Items.Add("Double");
            cmbDataType.Items.Add("String");
            cmbDataType.Items.Add("Character");
            cmbDataType.Items.Add("Bool");
            cmbDataType.Items.Add("DateTime");
            cmbDataType.SelectedItem = "Integer";
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
                metadataFromFile = JsonConvert.DeserializeObject<List<TableMetadata>>(jsonContent);

                string regexForInt = @"\b[intI][nN][tT]\b";
                string regexForPublic = @"\b[publicP][uU][bB][lL][iI][cC]\b";

                txtClassName.Text = metadataFromFile.FirstOrDefault().TableName;
                txtPropertyName.Text = metadataFromFile.FirstOrDefault().ColumnName;

                string dataType = metadataFromFile.FirstOrDefault().DataType;
                string accessModifier = metadataFromFile.FirstOrDefault().AccessModifier;

                CheckRegex(dataType, accessModifier, regexForInt, regexForPublic);
            }
        }

        private void CheckRegex(string dataType, string accessModifier, string regexInt, string regexPublic)
        {
            if (!string.IsNullOrEmpty(dataType))
            {
                if (Regex.IsMatch(dataType, regexInt, RegexOptions.IgnoreCase))
                {
                    cmbDataType.SelectedItem = "Integer";
                }
            }

            if (!string.IsNullOrEmpty(accessModifier))
            {
                if (Regex.IsMatch(accessModifier, regexPublic, RegexOptions.IgnoreCase))
                {
                    cmbAccessModifier.SelectedItem = "Public";
                }
            }
        }

        private void btnAddProperty_Click(object sender, RoutedEventArgs e)
        {
            if (metadataFromFile.Count > 0 && indexMetadata < metadataFromFile.Count)
            {
                TableMetadata metadata = new TableMetadata
                {
                    TableName = metadataFromFile[indexMetadata].TableName,
                    ColumnName = metadataFromFile[indexMetadata].ColumnName,
                    DataType = metadataFromFile[indexMetadata].DataType,
                    AccessModifier = metadataFromFile[indexMetadata].AccessModifier
                };
                tables.Add(metadata);
                addedProperties++;
                txtAddedProperties.Text = addedProperties.ToString();
                if (indexMetadata + 1 != metadataFromFile.Count)
                {
                    indexMetadata++;
                    txtClassName.Text = metadataFromFile[indexMetadata].TableName;
                    txtPropertyName.Text = metadataFromFile[indexMetadata].ColumnName;
                    cmbDataType.SelectedItem = metadataFromFile[indexMetadata].DataType;
                    cmbAccessModifier.SelectedItem = metadataFromFile[indexMetadata].AccessModifier;
                }
                else
                {
                    txtPropertyName.Text = "";
                    cmbDataType.SelectedItem = "Integer";
                    cmbAccessModifier.SelectedItem = "Public";
                    metadataFromFile.Clear();
                }
            }
            else if (txtClassName.Text.Length != 0 && txtPropertyName.Text.Length != 0)
            {
                TableMetadata metadata = new TableMetadata
                {
                    TableName = txtClassName.Text,
                    ColumnName = txtPropertyName.Text,
                    DataType = cmbDataType.SelectedItem.ToString(),
                    AccessModifier = cmbAccessModifier.SelectedItem.ToString()
                };
                tables.Add(metadata);
                addedProperties++;
                txtAddedProperties.Text = addedProperties.ToString();
                txtPropertyName.Text = "";
                txtClassName.IsEnabled = false;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Insert all attributes!");
            }
        }
        private void btnAddClass_Click(object sender, RoutedEventArgs e)
        {
            if(metadataFromFile.Count > 0)
            {
                metadataFromFile.Clear();
                indexMetadata = 0;
            }

            if (tables.Count > 0)
            {
                classes.Add(tables);
                tables.Clear();
                txtClassName.Text = "";
                txtPropertyName.Text = "";
                cmbAccessModifier.SelectedItem = "Public";
                txtAddedProperties.Text = "";
                txtClassName.IsEnabled = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Add at least one property to the class!");
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (classes.Count != 0)
            {
                foreach (var classToGenerate in classes)
                {
                    System.Windows.Forms.MessageBox.Show("classToGenerate.FirstOrDefault() = " + classToGenerate.FirstOrDefault().TableName.ToString());
                    generator.GenerateClass(classToGenerate);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No classes added to generate!");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}