# Installation & Setup

This workshop requires a few tools before you can start exploring the chapters.
Before continuing, follow the [Installation & Setup](../../INSTALLATION.md) guide at the repository root to download the workshop files. Then install the prerequisites below so you can run the exercises and open the provided solutions.

## Prerequisites

- Install the [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0).
- Install [**Visual Studio 2022**](https://visualstudio.microsoft.com/vs/) with the **Desktop development with C#** workload. Visual Studio offers the best debugging and syntax tree visualization experience.

### Syntax Visualizer and DGML editor

When adding components to Visual Studio, make sure the **.NET Compiler Platform SDK** (which includes the Syntax Visualizer) is selected. You can find it under **Compilers, build tools, and runtimes**. Also add the **DGML editor** from the *Code tools* section on the **Individual components** tab. These tools are required for Chapter 1.

- The remaining chapters work in Visual Studio 2022 or in [**Visual Studio Code**](https://code.visualstudio.com/). Other IDEs may work, but instructions are not provided.


## Opening the solutions

Each chapter has a ready-made solution in the `solutions` folder. Open the solution for the chapter you are working on:

1. Navigate to `solutions/<chapter-number>`.
2. Open the `.sln` or project folder in Visual Studio or VS Code.
3. Restore packages if prompted, then follow the steps in the corresponding chapter to explore the code.

## Tips

- In VS Code, installing the **C# Dev Kit** and **C#** extensions from Microsoft provides a smoother experience.
- Use Visual Studio's built-in Syntax Visualizer and DGML editors for Chapter 1.
- To preview PlantUML diagrams, install the [PlantUML extension](https://marketplace.visualstudio.com/items?itemName=jebbs.plantuml) with:
  ```text
  ext install jebbs.plantuml
  ```
  For local rendering you can run a PlantUML server via Docker:
  ```bash
  docker run -d -p 8080:8080 plantuml/plantuml-server:jetty
  ```

Once your environment is set up, you're ready to dive into the chapters and start analyzing .NET projects with Roslyn.
