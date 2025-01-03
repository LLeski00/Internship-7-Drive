using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Enums;
using Drive.Presentation.Factories;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Utils;

public static class CommandUtils
{
    public static void PrintCommand(ICommand command)
    {
        Console.WriteLine($"- {command.Name}\n\t{command.Description}");
    }

    public static void PrintCommands(IEnumerable<ICommand> commands)
    {
        foreach (var command in commands)
        {
            PrintCommand(command);
        }
    }

    public static void PrintAllDirectoryCommands(User user)
    {
        var allCommands = CommandFactory.CreateDirectoryCommands(user);
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

    public static DirectoryCommand? GetDirectoryCommandFromString(string? command)
    {
        if (command == null)
            return null;

        var commandSplitBySpace = command.Split(' ');

        if (!Enum.TryParse<DirectoryCommand>(commandSplitBySpace[0], true, out var commandType))
        {
            return null;
        }

        return commandType;
    }

    public static SharedDiskCommand? GetSharedDiskCommandFromString(string? command)
    {
        if (command == null)
            return null;

        var commandSplitBySpace = command.Split(' ');

        if (!Enum.TryParse<SharedDiskCommand>(commandSplitBySpace[0], true, out var commandType))
        {
            return null;
        }

        return commandType;
    }

    public static EditCommand? GetEditCommandFromString(string? command)
    {
        if (command == null)
            return null;

        var commandSplitBySpace = command.Split(' ');

        if (!Enum.TryParse<EditCommand>(commandSplitBySpace[0], true, out var commandType))
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
}