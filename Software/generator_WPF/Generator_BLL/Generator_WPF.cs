using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Windows.Forms;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.Windows;

namespace generator_WPF.Generator_BLL
{
    public class Generator_WPF
    {
        public void GenerateClass(List<TableMetadata> tablesList)
        {
            System.Windows.MessageBox.Show("GenerateClass method");
            if(tablesList.Count == 0)
            {
                System.Windows.MessageBox.Show("count == 0");
            }
            foreach (TableMetadata table in tablesList)
            {
                string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
                System.Windows.MessageBox.Show("projectDir = " + projectDir);

                //string filePath = chosenPath + "\\" + table.TableName + ".cs";
                string filePath = projectDir + "\\" + table.TableName + ".cs";
                System.Windows.MessageBox.Show("filePath = " + filePath);

                string solutionName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                if (solutionName.EndsWith("exe"))
                {
                    solutionName = Path.ChangeExtension(solutionName, null);
                }

                string projectFilePath = Path.Combine(projectDir, $"{solutionName}.csproj");

                UsingDirectiveSyntax generatedUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
                NamespaceDeclarationSyntax generatedNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName($"{solutionName}"));
                ClassDeclarationSyntax generatedClass = SyntaxFactory.ClassDeclaration(table.TableName);

                PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(table.DataType), table.TableName)
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