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
        public void GenerateClass(TableMetadata classToGenerate)
        {
            string filePath = Path.GetTempFileName() + ".cs";
            string formattedClassName = Regex.Replace(classToGenerate.Name, @"\s", "_");
            string formattedNamespace = Regex.Replace(classToGenerate.Namespace, @"\s", "_");

            UsingDirectiveSyntax generatedUsingDirective = GenerateUsingDirective();
            ClassDeclarationSyntax generatedClass = GenerateClass(formattedClassName);
            PropertyDeclarationSyntax property;

            foreach (ColumnMetadata column in classToGenerate.Columns)
            {
                SyntaxTokenList modifiers = GetAccessModifier(column);

                string formattedDataType = Regex.Replace(column.DataType, @"\s", "_");
                string formattedColumnName = Regex.Replace(column.Name, @"\s", "_");
                property = GenerateProperty(modifiers, formattedDataType, formattedColumnName);
                generatedClass = generatedClass.AddMembers(property);
            }

            NamespaceDeclarationSyntax generatedNamespace = GenerateNamespace(formattedNamespace, generatedClass);
            var compilationUnit = SyntaxFactory.CompilationUnit().AddUsings(generatedUsingDirective).AddMembers(generatedNamespace);
            string generatedCode = compilationUnit.NormalizeWhitespace().ToFullString();

            FileCreation file = new FileCreation();
            file.CreateFile(filePath, generatedCode);
        }

        private UsingDirectiveSyntax GenerateUsingDirective()
        {
            return SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
        }

        private NamespaceDeclarationSyntax GenerateNamespace(string formattedNamespace, ClassDeclarationSyntax generatedClass)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(formattedNamespace))
                                .AddMembers(generatedClass);
        }

        private ClassDeclarationSyntax GenerateClass(string formattedClassName)
        {
            return SyntaxFactory.ClassDeclaration(formattedClassName);
        }

        private PropertyDeclarationSyntax GenerateProperty(SyntaxTokenList modifiers, string formattedDataType, string formattedColumnName)
        {
            return SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(formattedDataType), formattedColumnName)
                                .WithModifiers(modifiers)
                                .WithAccessorList(SyntaxFactory.AccessorList(
                                    SyntaxFactory.List(new[]
                                    {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                    })));
        }

        private SyntaxTokenList GetAccessModifier(ColumnMetadata column)
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

            return modifiers;
        }
    }
}