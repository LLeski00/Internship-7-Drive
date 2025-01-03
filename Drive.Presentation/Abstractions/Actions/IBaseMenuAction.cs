using Drive.Presentation.Extensions;

namespace Drive.Presentation.Abstractions.Actions;

public class IBaseMenuAction : IMenuAction
{
    public int MenuIndex { get; set; }
    public string Name { get; set; } = "NoName action";
    public IList<IAction> Actions { get; set; }

    public IBaseMenuAction(IList<IAction> actions)
    {
        actions.SetActionIndexes();
        Actions = actions;
    }

    public virtual void Open()
    {
        Actions.PrintActionsAndOpen();
    }
}