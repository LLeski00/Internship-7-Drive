namespace Drive.Presentation.Abstractions.Actions
{
    public interface IMenuAction : IAction
    {
        IList<IAction> Actions { get; set; }
    }
}