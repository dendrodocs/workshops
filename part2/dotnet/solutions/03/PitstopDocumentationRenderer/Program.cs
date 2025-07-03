using System.Text;
using System.Text.Json;
using DendroDocs;
using DendroDocs.Extensions;
using DendroDocs.Json;

List<TypeDescription> types;

// Path to the JSON file containing the analyzed types
var fileContents = File.ReadAllText("pitstop.analyzed.json");

types = JsonSerializer.Deserialize<List<TypeDescription>>(fileContents, JsonDefaults.DeserializerOptions())!;

// Populate base types and inherited members for all types
types.PopulateInheritedBaseTypes();
types.PopulateInheritedMembers();

// A builder to generate the documentation
var stringBuilder = new StringBuilder();

// Add a title and introduction to the documentation
stringBuilder.AppendLine("# Pitstop Generated Documentation");
stringBuilder.AppendLine();
stringBuilder.AppendLine("## Service Architecture");
stringBuilder.AppendLine();
stringBuilder.AppendLine("![Pitstop Service Architecture](https://github.com/EdwinVW/pitstop/wiki/img/solution-architecture.png)");
stringBuilder.AppendLine();

// Chapter for commands
stringBuilder.AppendLine("## Commands");
stringBuilder.AppendLine();

foreach (var group in types.Where(t => t.ImplementsType("Pitstop.Infrastructure.Messaging.Command"))
    .GroupBy(t => t.Name) // Group by command name
    .OrderBy(g => g.Key)) // Order by command name
{
    stringBuilder.AppendLine($"### {group.Key.ToSentenceCase()}"); // Command name as the title
    stringBuilder.AppendLine();

    stringBuilder.AppendLine("#### Services");
    stringBuilder.AppendLine();

    foreach (var command in group.OrderBy(c => c.Namespace)) // Order commands by namespace
    {
        stringBuilder.AppendLine($"- {command.Namespace.Split('.')[^2].ToSentenceCase()}"); // List namespaces where the command was defined
    }

    stringBuilder.AppendLine();

    stringBuilder.AppendLine("#### Properties");
    stringBuilder.AppendLine();

    stringBuilder.AppendLine("| Property | Type | Description |");
    stringBuilder.AppendLine("| --- | --- | --- |");

    foreach (var field in group.SelectMany(t => t.Fields) // Get all fields from the commands
        .GroupBy(f => (f.Type, f.Name)) // Group by type and name
        .Select(g => g.First()) // Select the first occurrence of each group
        .OrderBy(f => f.Name)) // Order by field name
    {
        stringBuilder.AppendLine($"| {field.Name} | {field.Type.ForDiagram()} | {field.DocumentationComments?.Summary} |");
    }

    stringBuilder.AppendLine();
}

// Chapter for events
stringBuilder.AppendLine("## Events");
stringBuilder.AppendLine();

foreach (var group in types.Where(t => t.ImplementsType("Pitstop.Infrastructure.Messaging.Event"))
    .GroupBy(t => t.Name)
    .OrderBy(t => t.Key))
{
    stringBuilder.AppendLine($"### {group.Key.ToSentenceCase()}");
    stringBuilder.AppendLine();

    stringBuilder.AppendLine("#### Services");
    stringBuilder.AppendLine();

    foreach (var @event in group.OrderBy(e => e.Namespace))
    {
        stringBuilder.AppendLine($"- {@event.Namespace.Split('.')[^2].ToSentenceCase()}");
    }

    stringBuilder.AppendLine();

    stringBuilder.AppendLine("#### Properties");
    stringBuilder.AppendLine();

    stringBuilder.AppendLine("| Property | Type | Description |");
    stringBuilder.AppendLine("| --- | --- | --- |");

    foreach (var field in group.SelectMany(t => t.Fields) // Get all fields from the events
        .GroupBy(f => (f.Type, f.Name)) // Group by type and name
        .Select(g => g.First()) // Select the first occurrence of each group
        .OrderBy(f => f.Name)) // Order by field name
    {
        stringBuilder.AppendLine($"| {field.Name} | {field.Type.ForDiagram()} | {field.DocumentationComments?.Summary} |");
    }

    stringBuilder.AppendLine();
}


// Write the file to disk
File.WriteAllText("pitstop.generated.md", stringBuilder.ToString());
