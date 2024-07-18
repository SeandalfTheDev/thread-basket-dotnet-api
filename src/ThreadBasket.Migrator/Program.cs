// See https://aka.ms/new-console-template for more information

using System.Reflection;
using DbUp;
using Microsoft.Extensions.Hosting;

var bld = Host.CreateApplicationBuilder();

using IHost host = bld.Build();

var connectionString = bld.Configuration["ConnectionStrings:Postgres"];

EnsureDatabase.For
    .PostgresqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .JournalToPostgresqlTable("public", "journal")
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;

await host.RunAsync();