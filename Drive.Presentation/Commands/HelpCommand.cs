using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name { get; set; } = "help";
        public string Description { get; set; } = "Lists all commands";

        public HelpCommand()
        {
        }

        public void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
        {
            Console.WriteLine("All commands:");
            CommandExtensions.PrintAllCommands();
        }
    }
}