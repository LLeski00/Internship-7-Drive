using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Utils;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Directory
{
    public class HelpCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "help";
        public string Description { get; set; } = "Lists all commands. Usage: help";
        public User User { get; set; }

        public HelpCommand(User user)
        {
            User = user;
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            Console.WriteLine("All commands:");
            CommandUtils.PrintAllDirectoryCommands(User);
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