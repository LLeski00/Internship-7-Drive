using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Directory
{
    public class ChangeDirectoryCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "cd";
        public string Description { get; set; } = "Changes current directory. Usage: cd 'path'";

        public ChangeDirectoryCommand()
        {
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var folderToEnter = currentFolders.FirstOrDefault(f => f.Name == commandArguments);

            if (folderToEnter == null)
            {
                Writer.Error("The folder does not exist.");
                return;
            }

            currentDirectory = folderToEnter;
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (string.IsNullOrEmpty(commandArguments) || commandArguments.Split(' ').Length != 1)
            {
                return false;
            }

            return true;
        }
    }
}