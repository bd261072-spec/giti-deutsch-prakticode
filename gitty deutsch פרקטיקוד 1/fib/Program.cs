////BSD

//using System.CommandLine;

//var bundleCommand = new Command("bundle", "bundle files to one file");
//bundleCommand.SetHandler(() =>
//{
//    Console.WriteLine("bundle command");
//});

//var rootCommand = new RootCommand("root comand bundle files");
//rootCommand.Add(bundleCommand);
//rootCommand.InvokeAsync(args);

using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

var bundleCommand = new Command("bundle", "Bundle files to one file")
{
    new Argument<string>("inputFiles", "Comma-separated list of input files"),
    new Argument<string>("outputFile", "Output file name")
};

bundleCommand.SetHandler((InvocationContext context) =>
{
    var inputFiles = context.ParseResult.GetValueForArgument(bundleCommand.Arguments[0]) as string;
    var outputFile = context.ParseResult.GetValueForArgument(bundleCommand.Arguments[1]) as string;

    var files = inputFiles.Split(',');
    using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
    {
        foreach (var file in files)
        {
            using (var inputStream = new FileStream(file.Trim(), FileMode.Open, FileAccess.Read))
            {
                inputStream.CopyTo(outputStream);
            }
        }
    }
    Console.WriteLine($"Bundled files into {outputFile}");
});

var rootCommand = new RootCommand("Root command to bundle files");
rootCommand.Add(bundleCommand);
await rootCommand.InvokeAsync(args);
