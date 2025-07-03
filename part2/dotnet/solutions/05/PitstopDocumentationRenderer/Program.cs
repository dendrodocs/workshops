using System.Text;
using System.Text.Json;
using DendroDocs;
using DendroDocs.Extensions;
using DendroDocs.Json;
using PlantUml.Builder;
using PlantUml.Builder.ClassDiagrams;
using PlantUml.Builder.SequenceDiagrams;

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

    var noHandler = true;
    foreach (var type in group)
    {
        if (types.CommandHandlerFor(type) is not null)
        {
            RenderCommandDiagram(stringBuilder, type); // Render the command diagram for the command type
            noHandler = false; // We found a command handler, so we set the flag to false
            break;
        }
    }

    if (noHandler) // If no command handler was found for the command type
    {
        stringBuilder.AppendLine("> [!WARNING]");
        stringBuilder.AppendLine("> No command handler found");
        stringBuilder.AppendLine();
    }
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

// Chapter for aggregates
stringBuilder.AppendLine("## Aggregates");
stringBuilder.AppendLine();

foreach (var aggregateRoot in types.Where(t => t.ImplementsTypeStartsWith("Pitstop.WorkshopManagementAPI.Domain.Core.AggregateRoot<")) // Because the class is generic, we check if it starts with the base type
    .OrderBy(a => a.Name))
{
    stringBuilder.AppendLine($"### {aggregateRoot.Name.ToSentenceCase()}");
    stringBuilder.AppendLine();

    stringBuilder.AppendLine("```plantuml");
    stringBuilder.AppendLine("@startuml");

    RenderClass(stringBuilder, aggregateRoot);

    stringBuilder.AppendLine("@enduml");
    stringBuilder.AppendLine("```");
    stringBuilder.AppendLine();
}

// Write the file to disk
File.WriteAllText("pitstop.generated.md", stringBuilder.ToString());

void RenderClass(StringBuilder stringBuilder, TypeDescription type)
{
    stringBuilder.ClassStart(type.Name, displayName: type.Name.ToSentenceCase());

    foreach (var property in type.Properties.Where(p => !p.IsPrivate())
        .OrderBy(p => p.Name))
    {
        stringBuilder.InlineClassMember($"{property.Name.ToSentenceCase()} : {property.Type.ForDiagram()}");
    }

    foreach (var method in type.Methods.Where(m => !m.IsPrivate() && !m.IsOverride())
        .OrderBy(m => m.Name))
    {
        var parameterList = string.Join(", ", method.Parameters.Select(p => p.Name.ToSentenceCase()));
        stringBuilder.InlineClassMember($"  {method.Name.ToSentenceCase()} ({parameterList})");
    }

    stringBuilder.ClassEnd();
    stringBuilder.AppendNewLine();

    foreach (var property in type.Properties)
    {
        var relatedType = types.FirstOrDefault(
            t => t.FullName == property.Type || // Type has been detected, it's part of the project
            (property.Type.IsEnumerable() && t.FullName == property.Type.GenericTypes()[0])); // Type is enumerable, and the generic type is part of the project
        if (relatedType is not null)
        {
            RenderClass(stringBuilder, relatedType); // Recursively render the related type

            if (!property.Type.IsEnumerable())
            {
                stringBuilder.Relationship(type.Name, "--", relatedType.Name); // Add a relationship line
            }
            else
            {
                stringBuilder.Relationship(type.Name, "--", relatedType.Name, label: "1..*"); // Add a relationship line and indicate a one-to-many relationship
            }
            stringBuilder.AppendNewLine();
            stringBuilder.AppendNewLine();
        }
    }
}

void RenderCommandDiagram(StringBuilder stringBuilder, TypeDescription type)
{
    var services = new List<string?>();
    var interactions = new InteractionTraverser(types).ExtractConsequences(type, services);

    // Fix missing caller
    var callingServices = types.GetSourceCommands(type);
    if (callingServices.Count == 1)
    {
      var service = InteractionTraverser.Service(callingServices[0]);

      services.Insert(0, service);
      interactions.Fragments.OfType<DendroDocs.Uml.Fragments.Arrow>().First().Source = service;
    }

    var innerBuilder = new StringBuilder();
    UmlFragmentRenderer.RenderTree(innerBuilder, interactions.Fragments, interactions);

    stringBuilder.AppendLine("```plantuml");
    stringBuilder.UmlDiagramStart();

    // Add styling
    stringBuilder.SkinParameter(SkinParameter.SequenceMessageAlignment, "reverseDirection");
    stringBuilder.SkinParameter(SkinParameter.SequenceGroupBodyBackgroundColor, "Transparent");
    stringBuilder.SkinParameter(SkinParameter.SequenceBoxBackgroundColor, "#Gainsboro");
    stringBuilder.SkinParameter(SkinParameter.SequenceArrowThickness, 2);
    stringBuilder.SkinParameter(SkinParameter.BoxPadding, 10);
    stringBuilder.SkinParameter(SkinParameter.ParticipantPadding, 10);
    stringBuilder.SkinParameter(SkinParameter.LifelineStrategy, "solid");

    // Hide footbox
    stringBuilder.HideFootBox();

    // Add participants
    foreach (var service in services)
    {
      stringBuilder.Participant(service, displayName: service?.ToSentenceCase());
    }

    stringBuilder.Append(innerBuilder);

    stringBuilder.UmlDiagramEnd();
    stringBuilder.AppendLine("```");
    stringBuilder.AppendLine();
}
