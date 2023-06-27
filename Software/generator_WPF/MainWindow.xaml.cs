using EnvDTE;
using generator_WPF.Generator_BLL;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell;
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
    public partial class MainWindow : System.Windows.Window
    {	        
        List<TableMetadata> properties = new List<TableMetadata>();
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

        private void btnFetchMetadata_Click(object sender, RoutedEventArgs e)
        {
            FetchedTables fetchedTables = new FetchedTables();
            fetchedTables.ShowDialog();
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
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
            if (txtClassName.Text.Length != 0 && txtPropertyName.Text.Length != 0 && txtNamespace.Text.Length != 0)
            {
                if (txtClassName.Text != txtPropertyName.Text)
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
                        properties.Add(metadata);
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
                    else
                    {
                        string dataType = GetDataType();

                        TableMetadata metadata = new TableMetadata
                        {
                            TableName = txtClassName.Text,
                            ColumnName = txtPropertyName.Text,
                            DataType = dataType,
                            AccessModifier = cmbAccessModifier.SelectedItem.ToString()
                        };
                        properties.Add(metadata);
                        addedProperties++;
                        txtAddedProperties.Text = addedProperties.ToString();
                        txtPropertyName.Text = "";
                        txtClassName.IsEnabled = false;
                        txtNamespace.IsEnabled = false;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Class name and Property name can not be the same!", "Class and Member names", (MessageBoxButton)System.Windows.Forms.MessageBoxButtons.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Insert all attributes!");
            }
        }

        private string GetDataType()
        {
            string dataType = "";
            string selectedItem = cmbDataType.SelectedItem.ToString();

            switch (selectedItem)
            {
                case "Integer":
                    dataType = "int";
                    break;
                case "Float":
                    dataType = "float";
                    break;
                case "Double":
                    dataType = "double";
                    break;
                case "String":
                    dataType = "string";
                    break;
                case "Character":
                    dataType = "char";
                    break;
                case "Bool":
                    dataType = "bool";
                    break;
                case "DateTime":
                    dataType = "DateTime";
                    break;
            }
            return dataType;
        }

        private void btnAddClass_Click(object sender, RoutedEventArgs e)
        {
            if(addedProperties != 0)
            {
                if(metadataFromFile.Count > 0)
                {
                    metadataFromFile.Clear();
                    indexMetadata = 0;
                }

                if (properties.Count > 0)
                {
                    classes.Add(new List<TableMetadata>(properties));
                    properties.Clear();
                    txtNamespace.Text = "";
                    txtClassName.Text = "";
                    txtPropertyName.Text = "";
                    cmbAccessModifier.SelectedItem = "Public";
                    txtAddedProperties.Text = "";
                    txtClassName.IsEnabled = true;
                    addedProperties = 0;
                }
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
                    generator.GenerateClass(classToGenerate, txtNamespace.Text);
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