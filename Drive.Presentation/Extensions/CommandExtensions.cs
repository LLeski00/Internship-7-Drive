using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Factories;
using Drive.Presentation.Helpers;
using System.Collections.Generic;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Extensions;

public static class CommandExtensions
{
    public static void PrintCommand(ICommand command)
    {
        Console.WriteLine($"- {command.Name}\n\t{command.Description}");
    }

    public static void PrintCommands(IList<ICommand> commands)
    {
        foreach (var command in commands)
        {
            PrintCommand(command);
        }
    }

    public static void PrintAllCommands()
    {
        var allCommands = CommandFactory.CreateCommands();
        PrintCommands(allCommands);
    }

    public static void Execute(this Command? command, ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
    {
        var allCommands = CommandFactory.CreateCommands();
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(ref currentDirectory, ref currentFolders, ref currentFiles, commandArguments);
    }

    public static Command? GetCommandFromString(string? command)
    {
        if (command == null)
            return null;

        var commandSplitBySpace = command.Split(' ');

        if(!Enum.TryParse<Command>(commandSplitBySpace[0], true, out var commandType)){
            return null;
        }

        return commandType;
    }

    public static void Print(this IList<ICommand> commands)
    {
        foreach (var command in commands)
        {
            Console.WriteLine($"- {command.Name}\t{command.Description}");
        }
    }
}