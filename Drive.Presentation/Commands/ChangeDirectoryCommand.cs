using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands
{
    public class ChangeDirectoryCommand : ICommand
    {
        public string Name { get; set; } = "cd";
        public string Description { get; set; } = "Changes current directory. Usage: cd 'path'";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;

        public ChangeDirectoryCommand(FileRepository fileRepository, FolderRepository folderRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
        }

        public void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
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
            currentFolders = _folderRepository.GetByParent(currentDirectory.Id);
            currentFiles = _fileRepository.GetByParent(currentDirectory.Id);
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