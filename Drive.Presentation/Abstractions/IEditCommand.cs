namespace Drive.Presentation.Abstractions;

public interface IEditCommand
{
    string Name { get; set; }
    string Description { get; set; }
    void Execute(string? commandArguments);
}