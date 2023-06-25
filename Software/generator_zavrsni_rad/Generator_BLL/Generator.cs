using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.CodeAnalysis;
using System.Windows.Forms;

namespace generator_zavrsni_rad.Generator_BLL
{
    public class Generator
    {
        TableMetadata tableMetadata = new TableMetadata();
        public static string chosenPath;

        public List<TableMetadata> FetchTables()
        {
            var tables = new List<TableMetadata>();

            string connectionString = "Data Source=DESKTOP-0I6GRQT;Initial Catalog=zavrsni_bp;Integrated Security=True";
            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var table = new TableMetadata();
                    table.TableName = reader["TABLE_NAME"].ToString();
                    tables.Add(table);
                }
                reader.Close();
                connection.Close();
            }
            return tables;
        }

        public TableMetadata FetchTableMetadata(string tableName)
        {
            string connectionString = "Data Source=DESKTOP-0I6GRQT;Initial Catalog=zavrsni_bp;Integrated Security=True";
            string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if(reader["TABLE_NAME"].ToString() == tableName)
                    {
                        tableMetadata.TableName = reader["TABLE_NAME"].ToString();
                        tableMetadata.TableSchema = reader["TABLE_SCHEMA"].ToString();
                    }
                }
                reader.Close();

                command.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableMetadata.TableName}'";
                reader = command.ExecuteReader();
                tableMetadata.Columns = new List<ColumnMetadata>();
                while (reader.Read())
                {
                    ColumnMetadata column = new ColumnMetadata();
                    column.ColumnName = reader["COLUMN_NAME"].ToString();
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
                    column.IsNullable = (reader["IS_NULLABLE"].ToString() == "YES");
                    //column.IsPrimaryKey = (reader["COLUMN_KEY"].ToString() == "PRI");
                    //column.IsForeignKey = (reader["COLUMN_KEY"].ToString() == "MUL");
                    column.IsUnique = (reader["COLUMN_NAME"].ToString() == "UNIQUE");
                    tableMetadata.Columns.Add(column);
                }
                reader.Close();

                connection.Close();
            }

            return tableMetadata;
        }

        public bool GenerateClass()
        {
            string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));

            string filePath = chosenPath + "\\" + tableMetadata.TableName + ".cs";

            string solutionName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            if (solutionName.EndsWith("exe"))
            {
                solutionName = Path.ChangeExtension(solutionName, null);
            }

            string projectFilePath = Path.Combine(projectDir, $"{solutionName}.csproj");

            if (!File.Exists(filePath))
            {
                UsingDirectiveSyntax generatedUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
                NamespaceDeclarationSyntax generatedNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"{solutionName}"));
                ClassDeclarationSyntax generatedClass = SyntaxFactory.ClassDeclaration(tableMetadata.TableName);

                foreach (var column in tableMetadata.Columns)
                {
                    PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(column.DataType), column.ColumnName)
                        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
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
                    MessageBox.Show("Error creating file: " + ex.Message, "File Creation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string relativePath;
                if (projectDir.Length+1 > filePath.Length)
                {
                    relativePath = filePath.Substring(projectDir.Length + 1);
                }
                else
                {
                    relativePath= filePath;
                }

                Project project = new Project(projectFilePath);
                project.AddItem("Compile", relativePath);
                project.Save();

                return true;
            }
            else
            {
                MessageBox.Show("Class is already generated!");
                return false;
            }
        }
    }
}
