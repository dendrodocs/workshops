# Installation & Setup

This workshop requires a few tools before you can start exploring the chapters.

Before continuing, follow the [Installation & Setup](../../INSTALLATION.md) guide at the repository root to download the workshop files.
Then install the prerequisites below so you can run the exercises and open the provided solutions.

## Prerequisites

* Install the [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0), or newer.
* An IDE, e.g.:
  * Install [**Visual Studio**](https://visualstudio.microsoft.com/vs/) with the **.NET desktop development** workload.
  If you install this IDE, also check the additional components mentioned in the next paragraph.
  Visual Studio offers the best debugging and syntax tree visualization experience.
  * Install [**Visual Studio Code**](https://code.visualstudio.com/) with the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).

Other IDEs may work, but instructions are not provided.

### Syntax Visualizer and DGML editor

When adding components to Visual Studio, select the **Individual components** tab,
and make sure the **.NET Compiler Platform SDK** (which includes the Syntax Visualizer) is added.
You can find it under _Compilers, build tools, and runtimes_ section.
Also add the **DGML editor** from the _Code tools_ section.

The Syntax Visualizer and DGML editor are only used in [Chapter 1](01-syntax-trees.md) to visualize the syntax tree and semantic model of a C# program.
The remaining chapters work in both Visual Studio and Visual Studio Code.

## Opening the solutions

Each chapter has a ready-made solution in the `solutions` folder. Open the solution for the chapter you are working on:

1. Navigate to `solutions/<chapter-number>`.
2. Open the `.sln` or project folder in Visual Studio or VS Code.
3. Restore packages if prompted, then follow the steps in the corresponding chapter to explore the code.

## Tips

* In VS Code, installing the **C# Dev Kit** and **C#** extensions from Microsoft provides a smoother experience.
* Use Visual Studio's built-in Syntax Visualizer and DGML editors for Chapter 1.

Once your environment is set up, you're ready to dive into the chapters and start analyzing .NET projects with Roslyn.
