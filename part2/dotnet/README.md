# Generating .NET Documentation

Welcome to the second part of the DendroDocs workshop for .NET.
Here you’ll use the analysis output from Part 1 to create living documentation.

Ideally, you would generate documentation for a project you already know well.
Because that’s not an option in this workshop, we’ll use the open source [Pitstop](https://github.com/EdwinVW/pitstop) project as a shared reference point.
This sample provides enough code to demonstrate how DendroDocs works without requiring in-depth domain knowledge.

This part focuses on the tooling provided by DendroDocs for .NET projects.
We’ll begin by producing the intermediate JSON file that drives all other documentation steps.

## Workshop Outline

1. [Generate an Intermediate File](01-generate-intermediate-file.md)

   Clone the sample project and run the analyzer to produce a JSON file for the rest of this part.


2. [Working with _TypeDescriptions_](02-work-with-types.md)

   Read the intermediate JSON file, get familiar with the type descriptions, and start querying relationships between classes, methods, invocations, and more.

3. [Generating Markdown Documentation](03-generate-markdown.md)

   Use your analyzed type information to create clear and maintainable Markdown documentation.
   In this chapter, you will build a generator that starts with static content and then adds dynamic sections, such as commands, events, and their properties, all derived directly from your source code.
   The result is documentation that automatically stays in sync with your application.

4. [Generating Class Diagrams](04-generating-class-diagrams.md)

   Extend your documentation generator to create class diagrams using PlantUML.
   You’ll learn how to visualize aggregates and render relationships directly from your codebase.

5. [Generating Sequence Diagrams](05-generating-sequence-diagrams.md)

   Add a new dimension to your documentation by automatically generating sequence diagrams that visualize the flow of messages between services.
   You’ll use prebuilt utilities to trace invocations and render PlantUML diagrams that help explain how commands are processed across your system.

Each chapter in Part 2 builds on the last, showing you how to turn static analysis results into clear, interactive documentation for your .NET projects.  
You now know how to process code metadata, generate readable Markdown, and visualize your system with class and sequence diagrams, all directly from your source code.

By the end of this workshop, you’ve built a documentation pipeline that stays up to date with every code change, making your architecture easier to understand and share.

## Complete solutions

You can find the complete solutions for each chapter in the [Solutions](solutions) folder.

