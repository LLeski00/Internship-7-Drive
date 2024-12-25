using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Commands;
using Drive.Data.Entities.Models;

namespace Drive.Presentation.Factories;

public class CommandFactory
{
    public static IList<ICommand> CreateCommands(User user)
    {
        var commands = new List<ICommand>
        {
            new HelpCommand(user),
            new ChangeDirectoryCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new CreateCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), RepositoryFactory.Create<FolderFileRepository>(), user),
            new DeleteCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), RepositoryFactory.Create<FolderFileRepository>()),
            new RenameCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), RepositoryFactory.Create<FolderFileRepository>()),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new ExitCommand()
        };

        return commands;
    }
}