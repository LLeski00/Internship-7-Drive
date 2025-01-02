﻿using Drive.Domain.Factories;
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
            new ChangeDirectoryCommand(),
            new CreateCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), user),
            new DeleteCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new RenameCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new EditCommand(),
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
            new EditCommand(),
            new BackCommand(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new ExitCommand()
        };

        return commands;
    }
}