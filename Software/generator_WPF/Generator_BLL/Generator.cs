using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Text.RegularExpressions;

namespace generator.Generator_BLL
{
    public class Generator
    {
        public string GenerateClass(TableMetadata classToGenerate)
        {
            string formattedClassName = Regex.Replace(classToGenerate.Name, @"\s", "_");
            string formattedNamespace = Regex.Replace(classToGenerate.Namespace, @"\s", "_");

            UsingDirectiveSyntax generatedUsingDirective = GenerateUsingDirectiveCode();
            ClassDeclarationSyntax generatedClass = GenerateClassCode(formattedClassName);
            PropertyDeclarationSyntax property;

            foreach (ColumnMetadata column in classToGenerate.Columns)
            {
                SyntaxTokenList modifiers = GetAccessModifier(column);

                string formattedDataType = Regex.Replace(column.DataType, @"\s", "_");
                string formattedColumnName = Regex.Replace(column.Name, @"\s", "_");
                property = GeneratePropertyCode(modifiers, formattedDataType, formattedColumnName);
                generatedClass = generatedClass.AddMembers(property);
            }

            NamespaceDeclarationSyntax generatedNamespace = GenerateNamespaceCode(formattedNamespace, generatedClass);
            var compilationUnit = SyntaxFactory.CompilationUnit().AddUsings(generatedUsingDirective).AddMembers(generatedNamespace);
            string generatedCode = compilationUnit.NormalizeWhitespace().ToFullString();

            return generatedCode;
        }

        private UsingDirectiveSyntax GenerateUsingDirectiveCode()
        {
            return SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
        }

        private NamespaceDeclarationSyntax GenerateNamespaceCode(string formattedNamespace, ClassDeclarationSyntax generatedClass)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(formattedNamespace))
                                .AddMembers(generatedClass);
        }

        private ClassDeclarationSyntax GenerateClassCode(string formattedClassName)
        {
            return SyntaxFactory.ClassDeclaration(formattedClassName);
        }

        private PropertyDeclarationSyntax GeneratePropertyCode(SyntaxTokenList modifiers, string formattedDataType, string formattedColumnName)
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