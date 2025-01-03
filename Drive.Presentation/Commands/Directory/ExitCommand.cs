using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Directory
{
    public class ExitCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "exit";
        public string Description { get; set; } = "Exits from the my disk menu. Usage: exit";

        public ExitCommand()
        {
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }
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