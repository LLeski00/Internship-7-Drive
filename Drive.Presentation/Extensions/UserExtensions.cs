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

    public static bool ConfirmUserAction(string message)
    {
        Console.WriteLine($"{message} (to confirm press 'y')");
        var confirmChoice = Console.ReadLine();

        if (string.IsNullOrEmpty(confirmChoice) || confirmChoice.ToLower() != "y")
            return false;

        return true;
    }
}