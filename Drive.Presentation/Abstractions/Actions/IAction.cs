namespace Drive.Presentation.Abstractions.Actions;

public interface IAction
{
    int MenuIndex { get; set; }
    string Name { get; set; }
    void Open();
}