using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions;

public class ExitMenuAction : IAction
{
    public int MenuIndex { get; set; }
    public string Name { get; set; } = "Exit";

    public ExitMenuAction()
    {
    }

    public void Open()
    {
    }
}