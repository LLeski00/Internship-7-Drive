using Drive.Data.Entities.Models;
using Drive.Presentation.Factories;

namespace Drive.Presentation.Extensions;

public static class UserExtensions
{
    public static void PrintUserActions(User user)
    {
        var mainMenuActions = MainMenuFactory.CreateActions(user);
        mainMenuActions.PrintActionsAndOpen();
    }
}