# DendroDocs Workshops - Part 1: Understanding Analyzers

Welcome to **Part 1** of the **DendroDocs Workshops**!

In this part we’ll explore the essentials of code analysis, beginning with a deep dive into how to analyze a project effectively.
Starting from the basics of parsing syntax trees, we’ll examine the structure of your code, breaking down its hierarchy to understand its various components.
This involves inspecting both the syntax and the semantic models, allowing us to go beyond the code’s surface to uncover its meaning—such as variable types,
method relationships, and other contextual details.

From there, we’ll focus on targeting specific areas within the syntax tree, teaching you how to isolate and act on relevant sections of your code,
whether for extracting methods, identifying classes, or gathering detailed properties.
Finally, we’ll cover how to document these insights by writing the analysis results into a JSON file that follows the DendroDocs schema.
This JSON output will serve as a structured representation of your project’s syntax and semantics, laying the groundwork for generating comprehensive documentation.

## Language-specific implementations

* [Analyzing .NET Projects with Roslyn](dotnet/README.md)
