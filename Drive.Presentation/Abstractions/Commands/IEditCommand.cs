namespace Drive.Presentation.Abstractions.Commands;

public interface IEditCommand : ICommand
{
    void Execute(string? commandArguments);
}