using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Presentation.Actions;
using Drive.Presentation.Actions.Account;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;

namespace Drive.Presentation.Factories;

public class LoginMenuFactory
{
    public static IList<IAction> CreateActions()
    {
        var actions = new List<IAction>
        {
            new LoginAction(RepositoryFactory.Create<UserRepository>()),
            new SignupAction(RepositoryFactory.Create<UserRepository>(), RepositoryFactory.Create<FolderRepository>()),
            new ExitMenuAction(),
        };

        actions.SetActionIndexes();

        return actions;
    }
}