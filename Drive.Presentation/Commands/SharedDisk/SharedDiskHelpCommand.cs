using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands
{
    public class SharedDiskHelpCommand : ICommand
    {
        public string Name { get; set; } = "help";
        public string Description { get; set; } = "Lists all commands. Usage: help";
        public User User { get; set; }

        public SharedDiskHelpCommand(User user)
        {
            User=user;
        }

        public void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            Console.WriteLine("All commands:");
            CommandExtensions.PrintAllSharedDiskCommands(User);
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (!string.IsNullOrEmpty(commandArguments))
            {
                return false;
            }

            return true;
        }
    }
}