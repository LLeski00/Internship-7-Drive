using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Actions;
using Drive.Presentation.Actions.Account.Settings;

namespace Drive.Presentation.Factories;

public class AccountSettingsFactory
{
    public static AccountSettingsAction Create(User user)
    {
        var actions = new List<IAction>
        {
            new AccountEditEmailAction(RepositoryFactory.Create<UserRepository>(), user),
            new AccountEditPasswordAction(RepositoryFactory.Create<UserRepository>(), user),
            new ExitMenuAction()
        };

        var menuAction = new AccountSettingsAction(actions);
        return menuAction;
    }
} 