using generator.Generator_BLL;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {	        
        List<TableMetadata> classes = new List<TableMetadata>();
        List<ColumnMetadata> columns = new List<ColumnMetadata>();
        TableMetadata currentClass;
        DatabaseMetadataFetcher metadataFetcher = new DatabaseMetadataFetcher();
        Generator generator = new Generator();
        int addedProperties = 0;
        bool firstTime = true;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupComboboxes();
        }

        private void SetupComboboxes()
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
            if(txtConnectionString.Text.Length != 0 && txtNamespace.Text.Length != 0)
            {
                if(!Regex.IsMatch(txtNamespace.Text, @"^\d"))
                {
                    FetchedTables fetchedTables = new FetchedTables(txtConnectionString.Text, txtNamespace.Text);
                    fetchedTables.Closed += (s, args) =>
                    {
                        txtConnectionString.Text = "";
                        txtNamespace.Text = "";
                    };
                    fetchedTables.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Namespace can not start with a digit!", "Invalid Names", (MessageBoxButton)System.Windows.Forms.MessageBoxButtons.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Insert the Connection String and Namespace for your Class!");
            }
        }

        private void btnAddProperty_Click(object sender, RoutedEventArgs e)
        {
            if (txtClassName.Text.Length != 0 && txtPropertyName.Text.Length != 0 && txtNamespace.Text.Length != 0)
            {
                bool isInvalidNamespaceName = Regex.IsMatch(txtNamespace.Text, @"^\d");
                bool isInvalidClassName = Regex.IsMatch(txtClassName.Text, @"^\d");
                bool isInvalidPropertyName = Regex.IsMatch(txtPropertyName.Text, @"^\d");
                if (!isInvalidNamespaceName && !isInvalidClassName && !isInvalidPropertyName)
                {
                    if (txtClassName.Text != txtPropertyName.Text)
                    {
                        if (firstTime)
                        {
                            currentClass = new TableMetadata();
                            currentClass.Name = txtClassName.Text;
                            currentClass.Namespace = txtNamespace.Text;
                        }

                        string dataType = GetDataType();
                        ColumnMetadata column = new ColumnMetadata
                        {
                            Name = txtPropertyName.Text,
                            DataType = dataType,
                            AccessModifier = cmbAccessModifier.SelectedItem.ToString()
                        };
                        columns.Add(column);
                    
                        addedProperties++;
                        txtAddedProperties.Text = addedProperties.ToString();
                        txtPropertyName.Text = "";
                        txtClassName.IsEnabled = false;
                        txtNamespace.IsEnabled = false;
                        firstTime = false;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Class name and Property name can not be the same!", "Invalid Names", (MessageBoxButton)System.Windows.Forms.MessageBoxButtons.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Namespace, Class and Property names can not start with a digit!", "Invalid Names", (MessageBoxButton)System.Windows.Forms.MessageBoxButtons.OK, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Error);
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
                currentClass.Columns = new List<ColumnMetadata>();
                currentClass.Columns.AddRange(new List<ColumnMetadata>(columns));
                classes.Add(currentClass);
                columns.Clear();

                firstTime = true;
                txtNamespace.Text = "";
                txtClassName.Text = "";
                txtPropertyName.Text = "";
                cmbAccessModifier.SelectedItem = "Public";
                txtAddedProperties.Text = "";
                txtClassName.IsEnabled = true;
                txtNamespace.IsEnabled = true;
                addedProperties = 0;
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