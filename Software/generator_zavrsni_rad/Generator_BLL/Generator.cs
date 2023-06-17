using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace generator_zavrsni_rad.Generator_BLL
{
    public class Generator
    {
        TableMetadata tableMetadata = new TableMetadata();

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
            string filePath = Path.Combine("C:\\Users\\Korisnik\\source\\repos\\generator_zavrsni_rad\\generator_zavrsni_rad", tableMetadata.TableName + ".cs");

            if (!File.Exists(filePath))
            {
                //FrmGenerating frmGenerating = new FrmGenerating();
                //frmGenerating.ShowDialog();

                UsingDirectiveSyntax generatedUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));

                // Define a namespace for the generated code
                NamespaceDeclarationSyntax generatedNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("generator_zavrsni_rad"));

                // Define a class for the generated code
                ClassDeclarationSyntax generatedClass = SyntaxFactory.ClassDeclaration(tableMetadata.TableName);

                // Add properties to the generated class based on the table metadata
                foreach (var column in tableMetadata.Columns)
                {
                    // Create a property for the column
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

                    // Add the property to the generated class
                    generatedClass = generatedClass.AddMembers(property);
                }

                // Add the generated class to the generated namespace
                generatedNamespace = generatedNamespace.AddMembers(generatedClass);

                // Create a compilation unit and add the generated namespace
                var compilationUnit = SyntaxFactory.CompilationUnit().AddUsings(generatedUsingDirective).AddMembers(generatedNamespace);

                // Generate the C# code
                string generatedCode = compilationUnit.NormalizeWhitespace().ToFullString();

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(generatedCode);
                    }
                }

                string projectPath = Path.Combine("C:\\Users\\Korisnik\\source\\repos\\generator_zavrsni_rad\\generator_zavrsni_rad", "generator_zavrsni_rad.csproj");
                Project project = new Project(projectPath);
                project.AddItem("Compile", filePath);
                project.Save();

                // Open the file with Notepad
                System.Diagnostics.Process.Start("notepad.exe", filePath);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
