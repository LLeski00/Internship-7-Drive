using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Commands;

namespace Drive.Presentation.Factories;

public class CommandFactory
{
    public static IList<ICommand> CreateCommands()
    {
        var commands = new List<ICommand>
        {
            new HelpCommand(),
            new ChangeDirectoryCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
        };

        return commands;
    }
}