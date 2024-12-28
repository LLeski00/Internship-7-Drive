using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Presentation.Actions.Account;
using Drive.Presentation.Actions.Disk;
using Drive.Domain.Repositories;
using Drive.Domain.Factories;
using Drive.Data.Entities.Models;
using Drive.Presentation.Factories;

namespace Drive.Presentation.Factories;

public class MainMenuFactory
{
    public static IList<IAction> CreateActions(User user)
    {
        var actions = new List<IAction>
        {
            new MyDiskAction(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<FolderRepository>(), user),
            //new SharedAction(RepositoryFactory.Create<UserRepository>()),
            AccountSettingsFactory.Create(user),
            new LogoutAction(),
        };

        actions.SetActionIndexes();

        return actions;
    }
}