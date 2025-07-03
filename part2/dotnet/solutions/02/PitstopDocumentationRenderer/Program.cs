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

// Retrieve and display information about a specific type
var type = types.First("Pitstop.TimeService.Events.DayHasPassed");

Console.WriteLine("Base types:");
foreach (var baseType in type.BaseTypes)
{
    Console.WriteLine($"- {baseType}");
}

Console.WriteLine();
Console.WriteLine("Fields:");
foreach (var field in type.Fields)
{
    Console.WriteLine($"- {field.Name}");
}

// Get a list of all commands
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Commands:");
var commands = types.Where(t => t.ImplementsType("Pitstop.Infrastructure.Messaging.Command"));
foreach (var command in commands)
{
    Console.WriteLine($"- {command.FullName}");
}

// Get a list of all events
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Events:");
var events = types.Where(t => t.ImplementsType("Pitstop.Infrastructure.Messaging.Event"));
foreach (var @event in events)
{
    Console.WriteLine($"- {@event.FullName}");
}

// Get a list of command handlers with the commands they handle
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Command Handlers:");
var commandHandlers = types
    .Where(t => t.IsClass() && t.Methods.Any(m => m.Parameters.Any(p => p.Attributes.Any(a => a.Type.Equals("Microsoft.AspNetCore.Mvc.FromBodyAttribute")))))
    // For fun, group the commands per command handler 
    .ToDictionary(
        t => t.FullName,
        t => t.Methods.Where(m => m.Parameters.Any(p => p.Attributes.Any(a => a.Type.Equals("Microsoft.AspNetCore.Mvc.FromBodyAttribute"))))
            .Select(m => types.First(m.Parameters.Last().Type).Name)
            .ToList()
    );

foreach (var (commandHandler, handledCommands) in commandHandlers)
{
    Console.WriteLine($"- {commandHandler}");

    foreach (var command in handledCommands)
    {
        Console.WriteLine($"  - {command}");
    }

    Console.WriteLine();
}

// Get a list of events and the services that handle them
Console.WriteLine();
Console.WriteLine("Event Receiving Services:");

var eventHandlerClasses = types
    .Where(t => t.IsClass() && t.ImplementsType("Pitstop.Infrastructure.Messaging.IMessageHandlerCallback"))
    .Where(t => t.Methods.Any(m => m.Name == "HandleAsync" && m.Parameters.Any(p => types.First(p.Type).ImplementsType("Pitstop.Infrastructure.Messaging.Event"))))
    .Select(t => (EventHandlerClass: t, Events: t.Methods.Where(m => m.Name == "HandleAsync" && m.Parameters.Any(p => types.First(p.Type).ImplementsType("Pitstop.Infrastructure.Messaging.Event"))).Select(m => types.First(m.Parameters[0].Type).Name).ToList()));

// Pivot events and eventHandlers
var query = from ehc in eventHandlerClasses
            from e in ehc.Events
            group ehc.EventHandlerClass.Namespace by e into g
            select g;

foreach (var eventHandlersGroup in query)
{
    Console.WriteLine($"- {eventHandlersGroup.Key}");

    foreach (var eventHandler in eventHandlersGroup)
    {
        Console.WriteLine($"  - {eventHandler}");
    }

    Console.WriteLine();
}

// Get a list of published events when the RegisterCustomer command is handled
Console.WriteLine();
Console.WriteLine("Follow Statements:");
var customerController = types.First("Pitstop.Application.CustomerManagementAPI.Controllers.CustomersController");
var handlingMethod = customerController.Methods.First(m => m.Name.Equals("RegisterAsync") && m.Parameters[0].Type == "Pitstop.CustomerManagementAPI.Commands.RegisterCustomer");
var messagePublishStatements = handlingMethod.Statements.SelectMany(
    s => FlattenStatements(s).OfType<InvocationDescription>().Where(s => s.Name == "PublishMessageAsync")
);

foreach (var statement in messagePublishStatements)
{
    Console.WriteLine($"- When a RegisterCustomer command is handled, a {types.First(statement.Arguments[1].Type).Name} event will be published.");
}


static IEnumerable<Statement> FlattenStatements(Statement sourceStatement, List<Statement>? statements = default)
{
    statements ??= [];

    switch (sourceStatement)
    {
        case InvocationDescription invocation:
            statements.Add(invocation);
            break;

        case Switch @switch:
            foreach (var statement in @switch.Sections.SelectMany(s => s.Statements))
            {
                FlattenStatements(statement, statements);
            }
            break;

        case If @if:
            foreach (var statement in @if.Sections.SelectMany(s => s.Statements))
            {
                FlattenStatements(statement, statements);
            }
            break;

        case Statement statementBlock:
            foreach (var statement in statementBlock.Statements)
            {
                FlattenStatements(statement, statements);
            }
            break;
    }

    return statements;
}
