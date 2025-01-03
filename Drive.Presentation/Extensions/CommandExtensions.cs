using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Factories;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Extensions;

public static class CommandExtensions
{
    //MAKE ONE FATHER INTERFACE ICOMMAND AND HAVE IT HAVE THE NAME AND THE DESCRIPTION

    public static void PrintCommand(ICommand command)
    {
        Console.WriteLine($"- {command.Name}\n\t{command.Description}");
    }

    public static void PrintCommand(IEditCommand command)
    {
        Console.WriteLine($"- {command.Name}\n\t{command.Description}");
    }

    public static void PrintCommand(ICommentCommand command)
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

    public static void PrintCommands(IList<IEditCommand> commands)
    {
        foreach (var command in commands)
        {
            PrintCommand(command);
        }
    }

    public static void PrintCommands(IList<ICommentCommand> commands)
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

    public static void PrintAllSharedDiskCommands(User user)
    {
        var allCommands = CommandFactory.CreateSharedDiskCommands(user);
        PrintCommands(allCommands);
    }

    public static void PrintAllEditCommands(User user, File fileToEdit, List<string>? newLinesOfText)
    {
        var allCommands = CommandFactory.CreateEditCommands(user, fileToEdit, newLinesOfText);
        PrintCommands(allCommands);
    }

    public static void PrintAllCommentCommands(User user, File fileToEdit)
    {
        var allCommands = CommandFactory.CreateCommentCommands(user, fileToEdit);
        PrintCommands(allCommands);
    }

    public static void Execute(this Command? command, ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments, User user)
    {
        //Maybe not needed to create all commands
        var allCommands = CommandFactory.CreateCommands(user);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(ref currentDirectory, currentFolders, currentFiles, commandArguments);
    }

    public static void Execute(this Domain.Enums.EditCommand? command, string? commandArguments, User user, File fileToEdit, List<string>? newLinesOfText)
    {
        //Maybe not needed to create all commands
        var allCommands = CommandFactory.CreateEditCommands(user, fileToEdit, newLinesOfText);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(commandArguments);
    }

    public static void Execute(this CommentCommand? command, string? commandArguments, User user, File fileToEdit)
    {
        //Maybe not needed to create all commands
        var allCommands = CommandFactory.CreateCommentCommands(user, fileToEdit);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(commandArguments);
    }

    public static void SharedExecute(this Command? command, ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments, User user)
    {
        //Maybe not needed to create all commands
        var allCommands = CommandFactory.CreateSharedDiskCommands(user);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(ref currentDirectory, currentFolders, currentFiles, commandArguments);
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

    public static Domain.Enums.EditCommand? GetEditCommandFromString(string? command)
    {
        if (command == null)
            return null;

        var commandSplitBySpace = command.Split(' ');

        if (!Enum.TryParse<Domain.Enums.EditCommand>(commandSplitBySpace[0], true, out var commandType))
        {
            return null;
        }

        return commandType;
    }

    public static CommentCommand? GetCommentCommandFromString(string? command)
    {
        if (command == null)
            return null;

        var commandSplitBySpace = command.Split(' ');

        if (!Enum.TryParse<CommentCommand>(commandSplitBySpace[0], true, out var commandType))
        {
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