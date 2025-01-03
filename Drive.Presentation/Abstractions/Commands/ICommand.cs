namespace Drive.Presentation.Abstractions.Commands;

public interface ICommand
{
    string Name { get; set; }
    string Description { get; set; }
    bool IsCommandValid(string? commandArguments);
}