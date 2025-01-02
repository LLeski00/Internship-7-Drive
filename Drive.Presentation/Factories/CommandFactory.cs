using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Commands;
using Drive.Presentation.Commands.Edit;
using Drive.Presentation.Commands.SharedDisk;
using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Factories;

public class CommandFactory
{
    public static IList<ICommand> CreateCommands(User user)
    {
        var commands = new List<ICommand>
        {
            new HelpCommand(user),
            new ChangeDirectoryCommand(),
            new CreateCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), user),
            new DeleteCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new RenameCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new EditCommand(user),
            new ShareCommand(RepositoryFactory.Create<UserRepository>()),
            new StopShareCommand(RepositoryFactory.Create<UserRepository>()),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new ExitCommand()
        };

        return commands;
    }

    public static IList<ICommand> CreateSharedDiskCommands(User user)
    {
        var commands = new List<ICommand>
        {
            new SharedDiskHelpCommand(user),
            new ChangeDirectoryCommand(),
            new SharedDiskDeleteCommand(RepositoryFactory.Create<SharedFileRepository>(), RepositoryFactory.Create<SharedFolderRepository>(), user),
            new RenameCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new EditCommand(user),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new ExitCommand()
        };

        return commands;
    }

    public static IList<IEditCommand> CreateEditCommands(User user, File fileToEdit, List<string>? newLinesOfText)
    {
        var commands = new List<IEditCommand>
        {
            new EditHelpCommand(user, fileToEdit, newLinesOfText),
            new CommentOpenCommand(user, fileToEdit),
            new EditSaveAndExitCommand(fileToEdit, newLinesOfText),
            new EditExitCommand(),
        };

        return commands;
    }

    public static IList<ICommentCommand> CreateCommentCommands(User user, File fileToEdit)
    {
        var commands = new List<ICommentCommand>
        {
            /*new CommentAddCommand(user),
            new CommentEditCommand(user),
            new CommentDeleteCommand(user),
            new CommentExitCommand()*/
        };

        return commands;
    }
}