# Analyzing .NET Projects with Roslyn

Welcome to the first part of the DendroDocs workshop for .NET, where you’ll get hands-on with Roslyn, officially known as the *[.NET Compiler Platform SDK](https://learn.microsoft.com/dotnet/csharp/roslyn-sdk/?wt.mc_id=AZ-MVP-5004268)*.

Before you begin, see the [Installation & Setup](INSTALLATION.md) guide to configure your environment.

Roslyn can help improve code quality, generate reports, and most importantly, automate documentation,
making it invaluable for developers looking to deepen their code analysis capabilities.

This part is designed to help you understand how to leverage Roslyn to analyze the structure of .NET codebases, parse syntax trees, and build custom analysis tools.
While this part is optional for those familiar with Roslyn, it provides valuable insights into the platform that can make later sections easier to navigate.

For newcomers, each chapter in this section will progressively deepen your understanding of Roslyn,
allowing you to explore its potential for code analysis and documentation generation.
Each chapter also includes optional, “Awesomesauce” challenges that can help you test your knowledge, along with solutions for reference.

## Workshop Outline

1. [Getting Familiar with Syntax Trees](01-syntax-trees.md)

   Visualize and explore the syntax tree of a sample application to understand its structure.

2. [Parsing Source Code](02-parse-trees.md)

   Learn to parse a syntax tree and inspect its structure to see how code is broken down.

3. [Compiling Source Code](03-compile-code.md)

   Compile syntax trees and explore semantic models to gain deeper insights into the code’s meaning and relationships.

4. [Walking Through a Syntax Tree](04-walk-trees.md)

   Discover how to navigate specific parts of a syntax tree using the visitor pattern to target and act on relevant nodes.

5. [Loading Projects and Solutions](05-load-a-project.md)

   Learn how to load an entire project or solution, enabling you to work with more extensive codebases.

Each chapter builds upon the previous one, allowing you to gradually master Roslyn for .NET code analysis and documentation.
You’ll come away with practical skills for working with syntax trees, semantic models, and generating documentation that stays aligned with your code.
Enjoy exploring Roslyn, and let’s get started!

## Complete solutions

You can find the complete solutions for each chapter in the [Solutions](solutions) folder.
