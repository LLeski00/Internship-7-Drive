using Drive.Data.Entities.Models;
using Drive.Presentation.Enums;
using Drive.Presentation.Factories;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Extensions;

public static class CommandExtensions
{
    public static void Execute(this DirectoryCommand? command, ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments, User user)
    {
        var allCommands = CommandFactory.CreateDirectoryCommands(user);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(ref currentDirectory, currentFolders, currentFiles, commandArguments);
    }

    public static void Execute(this EditCommand? command, string? commandArguments, User user, File fileToEdit, List<string>? newLinesOfText)
    {
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
        var allCommands = CommandFactory.CreateCommentCommands(user, fileToEdit);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(commandArguments);
    }

    public static void Execute(this SharedDiskCommand? command, ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments, User user)
    {
        var allCommands = CommandFactory.CreateSharedDiskCommands(user);
        var commandToBeExecuted = allCommands.FirstOrDefault(c => c.Name == command.ToString());

        if (commandToBeExecuted == null)
        {
            Writer.Error("The command was not found.");
            return;
        }

        commandToBeExecuted.Execute(ref currentDirectory, currentFolders, currentFiles, commandArguments);
    }
}