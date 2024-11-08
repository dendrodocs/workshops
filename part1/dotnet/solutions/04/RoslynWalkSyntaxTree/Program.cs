﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

SyntaxTree tree = CSharpSyntaxTree.ParseText(
    """
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    namespace TopLevel
    {
        using Microsoft;
        using System.ComponentModel;

        namespace Child1
        {
            using Microsoft.Win32;
            using System.Runtime.InteropServices;

            class Foo { }
        }

        namespace Child2
        {
            using System.CodeDom;
            using Microsoft.CSharp;

            class Bar { }
        }
    }
    """);

CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();

UsingCollector collector = new();
collector.Visit(root);

foreach (UsingDirectiveSyntax usingDirective in collector.Usings)
{
    Console.WriteLine(usingDirective.Name);
}
