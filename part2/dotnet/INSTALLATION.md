# Installation & Setup

This workshop requires a few tools before you can start exploring the chapters.

Before continuing, follow the [Installation & Setup](../../INSTALLATION.md) guide at the repository root to download the workshop files.
Then install the prerequisites below so you can run the exercises and open the provided solutions.

## Prerequisites

* Install the [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0), or newer.
* An IDE, e.g.:
  * Install [**Visual Studio**](https://visualstudio.microsoft.com/vs/) with the **.NET desktop development** workload.
  * Install [**Visual Studio Code**](https://code.visualstudio.com/) with the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).
  If you install this IDE, also check the additional component mentioned in the next paragraph.

Other IDEs may work, but instructions are not provided.

### PlantUML

In Visual Studio Code, you can install the **PlantUML** extension to visualize UML diagrams from PlantUML code.

* `ext install jebbs.plantuml`
* <https://marketplace.visualstudio.com/items?itemName=jebbs.plantuml>

#### Settings

```json
"plantuml.render": "PlantUMLServer",
"plantuml.server": "https://www.plantuml.com/plantuml",
```

For example code it's fine to use the public renderer.
You can render locally by running a PlantUML server locally.
For example using Docker:

```sh
❯ docker run -d -p 8080:8080 plantuml/plantuml-server:jetty
```

and changing the `plantuml.server` setting to `http://localhost:8080/plantuml`.

## Opening the solutions

Each chapter has a ready-made solution in the `solutions` folder. Open the solution for the chapter you are working on:

1. Navigate to `solutions/<chapter-number>`.
2. Open the `.sln` or project folder in Visual Studio or VS Code.
3. Restore packages if prompted, then follow the steps in the corresponding chapter to explore the code.
