using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name { get; set; } = "help";
        public string Description { get; set; } = "Lists all commands";

        public HelpCommand()
        {
        }

        public void Execute()
        {
            Console.WriteLine("All commands:");
            CommandExtensions.PrintAllCommands();
        }
    }
}