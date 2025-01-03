namespace Drive.Presentation.Abstractions.Commands;

public interface ICommentCommand : ICommand
{
    void Execute(string? commandArguments);
}