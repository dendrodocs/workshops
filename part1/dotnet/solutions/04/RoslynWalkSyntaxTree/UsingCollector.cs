using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class UsingCollector : CSharpSyntaxWalker
{
    public readonly List<UsingDirectiveSyntax> Usings = [];

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        if (node.Name!.ToString() != "System" &&
            !node.Name.ToString().StartsWith("System.", StringComparison.Ordinal))
        {
            Usings.Add(node);
        }
    }
}