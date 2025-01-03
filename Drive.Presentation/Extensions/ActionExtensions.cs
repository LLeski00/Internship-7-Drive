﻿using Drive.Presentation.Abstractions.Actions;
using Drive.Presentation.Actions;
using Drive.Presentation.Actions.Account;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;

namespace Drive.Presentation.Extensions;

public static class ActionExtensions
{
    public static void PrintActionsAndOpen(this IList<IAction> actions)
    {
        const string INVALID_INPUT_MSG = "Please type in number!";
        const string INVALID_ACTION_MSG = "Please select valid action!";

        var isExitSelected = false;
        do
        {
            Console.Clear();
            ActionUtils.PrintActions(actions);

            var isValidInput = int.TryParse(Console.ReadLine(), out var actionIndex);
            if (!isValidInput)
            {
                Writer.Error(INVALID_INPUT_MSG);
                continue;
            }

            var action = actions.FirstOrDefault(a => a.MenuIndex == actionIndex);
            if (action is null)
            {
                Writer.Error(INVALID_ACTION_MSG);
                continue;
            }

            action.Open();

            isExitSelected = action is ExitMenuAction || action is LogoutAction;
        } while (!isExitSelected);
    }

    public static void SetActionIndexes(this IList<IAction> actions)
    {
        var index = 0;
        foreach (var action in actions)
        {
            action.MenuIndex = ++index;
        }
    }
}