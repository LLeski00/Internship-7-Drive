using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands
{
    public class BackCommand : ICommand
    {
        public string Name { get; set; } = "back";
        public string Description { get; set; } = "Goes back to parent folder. Usage: back";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;

        public BackCommand(FileRepository fileRepository, FolderRepository folderRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            if (currentDirectory.ParentFolderId == null) 
            {
                Writer.Error("You are in the root folder.");
                return;
            }

            var newCurrentDirectory = _folderRepository.GetById(currentDirectory.ParentFolderId);

            if (newCurrentDirectory == null) {
                Writer.Error("Could not find parent folder.");
                return;
            }

            currentDirectory = newCurrentDirectory;
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