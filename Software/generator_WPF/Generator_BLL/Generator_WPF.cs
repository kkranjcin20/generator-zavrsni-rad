using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Windows.Forms;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;

namespace generator_WPF.Generator_BLL
{
    public class Generator_WPF
    {
        TableMetadata tableMetadata = new TableMetadata();
        public static string chosenPath;

        public void GenerateClass(List<TableMetadata> tablesList)
        {
            foreach (TableMetadata table in tablesList)
            {
                string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));

                string filePath = chosenPath + "\\" + table.ColumnName + ".cs";

                string solutionName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                if (solutionName.EndsWith("exe"))
                {
                    solutionName = Path.ChangeExtension(solutionName, null);
                }

                string projectFilePath = Path.Combine(projectDir, $"{solutionName}.csproj");
                UsingDirectiveSyntax generatedUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
                NamespaceDeclarationSyntax generatedNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"{solutionName}"));
                ClassDeclarationSyntax generatedClass = SyntaxFactory.ClassDeclaration(this.tableMetadata.ColumnName);
                /*
                foreach (var table in tableMetadata)
                {
                    PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(table.DataType), table.Name)
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
                */
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
                    System.Windows.MessageBox.Show("Error creating file: " + ex.Message, "File Creation", (System.Windows.MessageBoxButton)MessageBoxButtons.OK, (System.Windows.MessageBoxImage)MessageBoxIcon.Error);
                    //return false;
                }

                string relativePath;
                if (projectDir.Length + 1 > filePath.Length)
                {
                    relativePath = filePath.Substring(projectDir.Length + 1);
                }
                else
                {
                    relativePath = filePath;
                }

                Project project = new Project(projectFilePath);
                project.AddItem("Compile", relativePath);
                project.Save();
            }
        }
    }
}