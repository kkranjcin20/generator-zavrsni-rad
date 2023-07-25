using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;

namespace generator.Generator_BLL
{
    public class Generator
    {
        private string _connectionString;
        public List<TableMetadata> FetchTables(string connectionString)
        {
            _connectionString = connectionString;
            var tables = new List<TableMetadata>();

            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var table = new TableMetadata();
                    table.Name = reader["TABLE_NAME"].ToString();
                    tables.Add(table);
                }
                reader.Close();
                connection.Close();
            }
            return tables;
        }

        public TableMetadata FetchTableMetadata(TableMetadata table)
        {
            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["TABLE_NAME"].ToString() == table.Name)
                    {
                        table.Name = reader["TABLE_NAME"].ToString();
                    }
                }
                reader.Close();

                command.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table.Name}'";
                reader = command.ExecuteReader();

                table.Columns = new List<ColumnMetadata>();
                while (reader.Read())
                {
                    ColumnMetadata column = new ColumnMetadata();
                    column.Name = reader["COLUMN_NAME"].ToString();
                    if (reader["DATA_TYPE"].ToString() == "varchar")
                    {
                        column.DataType = "string";
                    }
                    else if (reader["DATA_TYPE"].ToString() == "date")
                    {
                        column.DataType = "DateTime";
                    }
                    else
                    {
                        column.DataType = reader["DATA_TYPE"].ToString();
                    }
                    table.Columns.Add(column);
                }
                reader.Close();

                connection.Close();
            }

            return table;
        }

        public void GenerateClass(TableMetadata classToGenerate)
        {
            string filePath = Path.GetTempFileName() + ".cs";
            string formattedClassName = Regex.Replace(classToGenerate.Name, @"\s", "_");
            string formattedNamespace = Regex.Replace(classToGenerate.Namespace, @"\s", "_");

            UsingDirectiveSyntax generatedUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
            NamespaceDeclarationSyntax generatedNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(formattedNamespace));
            ClassDeclarationSyntax generatedClass = SyntaxFactory.ClassDeclaration(formattedClassName);
            PropertyDeclarationSyntax property;
            
            foreach (ColumnMetadata column in classToGenerate.Columns)
            {
                SyntaxTokenList modifiers;
                if (column.AccessModifier == "Private")
                {
                    modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
                }
                else if (column.AccessModifier == "Protected")
                {
                    modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword));
                }
                else
                {
                    modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                }

                string formattedDataType = Regex.Replace(column.DataType, @"\s", "_");
                string formattedColumnName = Regex.Replace(column.Name, @"\s", "_");

                property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(formattedDataType), formattedColumnName)
                    .WithModifiers(modifiers)
                    .WithAccessorList(SyntaxFactory.AccessorList(
                        SyntaxFactory.List(new[]
                        {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        })));
                generatedClass = generatedClass.AddMembers(property);
            }
                
            generatedNamespace = generatedNamespace.AddMembers(generatedClass);

            var compilationUnit = SyntaxFactory.CompilationUnit().AddUsings(generatedUsingDirective).AddMembers(generatedNamespace);

            string generatedCode = compilationUnit.NormalizeWhitespace().ToFullString();

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(generatedCode);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error creating file: " + ex.Message, "File Creation", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }

            System.Diagnostics.Process.Start("notepad.exe", filePath);
        }
    }
}