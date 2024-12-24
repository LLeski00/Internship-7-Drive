﻿using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Factories;
using Drive.Presentation.Helpers;
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

    public static void PrintAllCommands(User user)
    {
        var allCommands = CommandFactory.CreateCommands(user);
        PrintCommands(allCommands);
    }

    public static void Execute(this Command? command, ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments, User user)
    {
        var allCommands = CommandFactory.CreateCommands(user);
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

    public static void ProcessUserCommands(Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, User user)
    {
        do
        {
            DiskExtensions.PrintDirectory(currentFolders, currentFiles);
            Reader.ReadCommand(currentDirectory, out var userInput);
            var command = GetCommandFromString(userInput);

            if (command == null)
            {
                Writer.Error("Invalid command. Use 'help' for a list of commands.");
                continue;
            }

            if (userInput == Command.exit.ToString())
                break;

            var commandArguments = string.Join(' ', userInput.Split(' ').Skip(1));
            command.Execute(ref currentDirectory, ref currentFolders, ref currentFiles, commandArguments, user);
        } while (true);
    }
}