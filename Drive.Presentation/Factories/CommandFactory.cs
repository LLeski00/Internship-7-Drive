using Drive.Presentation.Abstractions;
using Drive.Presentation.Commands;

namespace Drive.Presentation.Factories;

public class CommandFactory
{
    public static IList<ICommand> CreateCommands()
    {
        var commands = new List<ICommand>
        {
            new HelpCommand(),
            new ChangeDirectoryCommand(),
            new BackCommand(),
        };

        return commands;
    }
}