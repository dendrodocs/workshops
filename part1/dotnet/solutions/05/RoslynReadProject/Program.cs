using System.Collections.Immutable;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

AnalyzerManager analyzerManager = new();
IProjectAnalyzer projectAnalyzer = analyzerManager.GetProject(@"D:\workshop\ConsoleApp1\ConsoleApp1.csproj");

IAnalyzerResult analyzerResults = projectAnalyzer.Build().First();

Console.WriteLine($"{analyzerResults.References.Length} references");
Console.WriteLine($"{analyzerResults.SourceFiles.Length} source files");

Console.WriteLine();
foreach (string file in analyzerResults.SourceFiles)
{
    Console.WriteLine(file);
}

AdhocWorkspace workspace = projectAnalyzer.GetWorkspace();

Project project = workspace.CurrentSolution.Projects.First();
Compilation compilation = project.GetCompilationAsync().Result!;

ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();

foreach (Diagnostic error in diagnostics.Where(l => l.Severity > DiagnosticSeverity.Hidden))
{
    Console.WriteLine(error);
}

foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
{
    CompilationUnitSyntax root = (CompilationUnitSyntax)syntaxTree.GetRoot();
    SourceText text = root.GetText();

    Console.WriteLine();
    Console.WriteLine(Path.GetFileName(syntaxTree.FilePath));
    Console.WriteLine(text.GetSubText(root.Span));

    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
}
