using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace generator_WPF.Generator_BLL
{
    public class Generator_WPF
    {

        public void GenerateClass(List<TableMetadata> classesList, string folderPath, string classNamespace)
        {
            UsingDirectiveSyntax generatedUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
            NamespaceDeclarationSyntax generatedNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(classNamespace));
            ClassDeclarationSyntax generatedClass = SyntaxFactory.ClassDeclaration(classesList.FirstOrDefault().TableName);
            PropertyDeclarationSyntax property;
            
            foreach (TableMetadata classProperty in classesList)
            {
                property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(classProperty.DataType), classProperty.ColumnName)
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
                using (var fileStream = new FileStream(folderPath, FileMode.Create))
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

            System.Diagnostics.Process.Start("notepad.exe", folderPath);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "C# Class (*.cs)|*.cs";
            saveFileDialog.Title = classesList.FirstOrDefault().TableName;
        }
    }
}