namespace Drive.Presentation.Abstractions;

public interface ICommentCommand
{
    string Name { get; set; }
    string Description { get; set; }
    void Execute(string? commandArguments);
}