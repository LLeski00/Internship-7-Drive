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

    public static void PrintUsersDisk(User user)
    {
        //Get users files
        //Get users folders
        //Display all folders who have user's root folder as their parent
        //Get FolderFiles for the user
        //Display all files who are linked with a users root folder
    }
}