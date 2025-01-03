using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Utils;

public static class ActionUtils
{
    public static void PrintActions(IList<IAction> actions)
    {
        foreach (var action in actions)
        {
            Console.WriteLine($"{action.MenuIndex}. {action.Name}");
        }
    }
}