using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

string objectLocation = typeof(object).Assembly.Location;
string runtimeLocation = Path.Combine(Path.GetDirectoryName(objectLocation)!, "System.Runtime.dll");
string consoleLocation = typeof(Console).Assembly.Location;

CSharpCompilation compilation = CSharpCompilation
    .Create("Workshop")
    .AddReferences(MetadataReference.CreateFromFile(runtimeLocation))
    .AddReferences(MetadataReference.CreateFromFile(objectLocation))
    .AddReferences(MetadataReference.CreateFromFile(consoleLocation))
    .AddSyntaxTrees(tree);

ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();

foreach (Diagnostic error in diagnostics.Where(d => d.Severity > DiagnosticSeverity.Hidden))
{
    Console.WriteLine(error);
}

CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();

InvocationExpressionSyntax? invocationExpression = root
    .Members.OfType<NamespaceDeclarationSyntax>().First()
    .Members.OfType<ClassDeclarationSyntax>().First()
    .Members.OfType<MethodDeclarationSyntax>().First()
    .Body?.Statements.OfType<ExpressionStatementSyntax>().First()
    .Expression as InvocationExpressionSyntax;

SemanticModel semanticModel = compilation.GetSemanticModel(tree);

ISymbol methodSymbol = semanticModel.GetSymbolInfo(invocationExpression!).Symbol!;

Console.WriteLine(methodSymbol);
Console.WriteLine(methodSymbol.ContainingType);
Console.WriteLine(methodSymbol.ContainingAssembly);

#region Awesomesauce

Console.WriteLine();

ImmutableArray<SymbolDisplayPart> displayParts = methodSymbol.ToDisplayParts();
foreach (SymbolDisplayPart part in displayParts)
{
    Console.WriteLine($"{part.Kind,-13} = {part}");
}

#endregion
