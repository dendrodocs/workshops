using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

SyntaxTree tree = CSharpSyntaxTree.ParseText(
    """
    using System;

    namespace ConsoleApp1
    {
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello World!");
            }
        }
    }
    """);

CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();

UsingDirectiveSyntax @using = root.Usings.First();
NamespaceDeclarationSyntax @namespace = root.Members.OfType<NamespaceDeclarationSyntax>().First();
ClassDeclarationSyntax @class = @namespace.Members.OfType<ClassDeclarationSyntax>().First();
MethodDeclarationSyntax method = @class.Members.OfType<MethodDeclarationSyntax>().First();
ExpressionStatementSyntax expressionStatement = method.Body!.Statements.OfType<ExpressionStatementSyntax>().First();

#region Optional
InvocationExpressionSyntax invocationExpression = (InvocationExpressionSyntax)expressionStatement.Expression;
MemberAccessExpressionSyntax memberAccessExpression = (MemberAccessExpressionSyntax)invocationExpression.Expression;
#endregion

SourceText sourceText = tree.GetText();
Console.WriteLine(sourceText.GetSubText(@using.Span));
Console.WriteLine(sourceText.GetSubText(expressionStatement.Span));

#region Optional
Console.WriteLine(sourceText.GetSubText(memberAccessExpression!.Expression.Span));
Console.WriteLine(sourceText.GetSubText(memberAccessExpression.Name.Span));
#endregion

#region Awesomesauce
ArgumentSyntax argument = invocationExpression.ArgumentList.Arguments.First();
Console.WriteLine(sourceText.GetSubText(argument.Span));
#endregion
