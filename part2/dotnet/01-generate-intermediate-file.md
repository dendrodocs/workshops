# Generate an Intermediate File

In this chapter, we’ll generate an intermediate JSON file describing a real-world .NET codebase using DendroDocs.  
You will get familiar with the **Pitstop – Garage Management System** sample project and the **DendroDocs Tool**.

## Prerequisites

You’ll need the **.NET SDK** and **git** installed.

## Step 1: Clone the *Pitstop sample project*

To start generating documentation, you need a project with enough code to generate something meaningful.

Clone the [workshops-dotnet-sample-pitstop](https://github.com/dendrodocs/workshops-dotnet-sample-pitstop) repository.

```shell
git clone https://github.com/dendrodocs/workshops-dotnet-sample-pitstop
```

### Pitstop - Garage Management System

![Pitstop garage](https://github.com/dendrodocs/workshops-dotnet-sample-pitstop/raw/main/pitstop-garage.png)

The solution consists of several services that communicate using a message broker and web APIs.

![Pitstop solution architecture](https://github.com/EdwinVW/pitstop/wiki/img/solution-architecture.png)

## Step 2: Install the *DendroDocs Tool*

Manually exploring every project in a solution is time-consuming.
Instead, use the **DendroDocs Tool** to automatically generate a reusable JSON file whenever your source code changes.

```shell
dotnet tool install --global DendroDocs.Tool
```

## Step 3: Analyze the *Pitstop Solution*

```shell
cd workshops-dotnet-sample-pitstop/src
dendrodocs-analyze --solution "pitstop.sln" --output "pitstop.analyzed.json"
```

This process can take about 30–60 seconds depending on your machine.

```text
DendroDocs Analysis output generated in 15246ms at pitstop.analyzed.json
- 152 types found
- JSON is valid according to the schema.
```

The generated JSON file contains an array of classes, methods, variables, and control structures you can use as input for documentation generators.

For example, here’s the [`Pitstop.Infrastructure.Messaging.Message` class](https://github.com/dendrodocs/workshops-dotnet-sample-pitstop/blob/main/src/Infrastructure.Messaging/Message.cs)
and its JSON representation:

```json
{
  "FullName": "Pitstop.Infrastructure.Messaging.Message",
  "Modifiers": 2,
  "Fields": [
    {
      "Type": "System.Guid",
      "Name": "MessageId",
      "Modifiers": 130
    },
    {
      "Type": "string",
      "Name": "MessageType",
      "Modifiers": 130
    }
  ],
  "Constructors": [
    {
      "Name": "Message",
      "Modifiers": 2
    },
    {
      "Parameters": [
        {
          "Type": "System.Guid",
          "Name": "messageId"
        }
      ],
      "Statements": [
        {
          "$type": "DendroDocs.AssignmentDescription, DendroDocs.Shared",
          "Left": "MessageId",
          "Operator": "=",
          "Right": "messageId"
        },
        {
          "$type": "DendroDocs.AssignmentDescription, DendroDocs.Shared",
          "Left": "MessageType",
          "Operator": "=",
          "Right": "this.GetType().Name"
        },
        {
          "$type": "DendroDocs.InvocationDescription, DendroDocs.Shared",
          "ContainingType": "object",
          "Name": "GetType"
        }
      ],
      "Name": "Message",
      "Modifiers": 2
    },
    {
      "Parameters": [
        {
          "Type": "string",
          "Name": "messageType"
        }
      ],
      "Statements": [
        {
          "$type": "DendroDocs.AssignmentDescription, DendroDocs.Shared",
          "Left": "MessageType",
          "Operator": "=",
          "Right": "messageType"
        }
      ],
      "Name": "Message",
      "Modifiers": 2
    },
    {
      "Parameters": [
        {
          "Type": "System.Guid",
          "Name": "messageId"
        },
        {
          "Type": "string",
          "Name": "messageType"
        }
      ],
      "Statements": [
        {
          "$type": "DendroDocs.AssignmentDescription, DendroDocs.Shared",
          "Left": "MessageId",
          "Operator": "=",
          "Right": "messageId"
        },
        {
          "$type": "DendroDocs.AssignmentDescription, DendroDocs.Shared",
          "Left": "MessageType",
          "Operator": "=",
          "Right": "messageType"
        }
      ],
      "Name": "Message",
      "Modifiers": 2
    }
  ]
}
```

> [!TIP]
> To inspect the JSON structure, add the `--pretty` switch to the command.

> [!TIP]
> If you run into issues like missing classes or unexpected output, use the `--verbose` switch to see compile errors and warnings.
