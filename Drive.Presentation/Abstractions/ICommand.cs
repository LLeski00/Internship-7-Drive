namespace Drive.Presentation.Abstractions;

public interface ICommand
{
    string Name { get; set; }
    string Description { get; set; }
    void Execute();
}