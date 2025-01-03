using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Commands.Directory;
using Drive.Presentation.Commands.Edit;
using Drive.Presentation.Commands.SharedDisk;
using Drive.Presentation.Commands.Comment;
using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Abstractions.Commands;

namespace Drive.Presentation.Factories;

public class CommandFactory
{
    public static IList<IDirectoryCommand> CreateDirectoryCommands(User user)
    {
        var commands = new List<IDirectoryCommand>
        {
            new HelpCommand(user),
            new ChangeDirectoryCommand(),
            new CreateCommand(user),
            new DeleteCommand(),
            new RenameCommand(),
            new EditCommand(user),
            new ShareCommand(RepositoryFactory.Create<UserRepository>()),
            new StopShareCommand(RepositoryFactory.Create<UserRepository>()),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new NavigateCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), user),
            new ExitCommand()
        };

        return commands;
    }

    public static IList<IDirectoryCommand> CreateSharedDiskCommands(User user)
    {
        var commands = new List<IDirectoryCommand>
        {
            new SharedDiskHelpCommand(user),
            new ChangeDirectoryCommand(),
            new SharedDiskDeleteCommand(user),
            new RenameCommand(),
            new EditCommand(user),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new SharedDiskNavigateCommand(RepositoryFactory.Create<FolderRepository>(), RepositoryFactory.Create<SharedFolderRepository>(), RepositoryFactory.Create<SharedFileRepository>(), user),
            new ExitCommand()
        };

        return commands;
    }

    public static IList<IEditCommand> CreateEditCommands(User user, File fileToEdit, List<string>? newLinesOfText)
    {
        var commands = new List<IEditCommand>
        {
            new EditHelpCommand(user, fileToEdit, newLinesOfText),
            new EditOpenCommentsCommand(user, fileToEdit),
            new EditSaveAndExitCommand(fileToEdit, newLinesOfText),
            new EditExitCommand(),
        };

        return commands;
    }

    public static IList<ICommentCommand> CreateCommentCommands(User user, File file)
    {
        var commands = new List<ICommentCommand>
        {
            new CommentHelpCommand(user, file),
            new CommentAddCommand(user, file),
            new CommentEditCommand(RepositoryFactory.Create<FileCommentRepository>(), user, file),
            new CommentDeleteCommand(RepositoryFactory.Create<FileCommentRepository>(), user, file),
            new CommentExitCommand()
        };

        return commands;
    }
}