//BSD

using System.CommandLine;


var bundleCommand = new Command("bundle", "bundle files to one file");
bundleCommand.SetHandler(() =>
{
    Console.WriteLine("bundle command");
});

var RootComand = new RootCommand("root comand bundle files");
RootComand.Add(bundleCommand);
await RootCommand.InvokeAsync(args);

