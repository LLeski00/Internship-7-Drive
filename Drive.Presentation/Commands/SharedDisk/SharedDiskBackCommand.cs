using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands
{
    public class SharedDiskBackCommand : ICommand
    {
        public string Name { get; set; } = "back";
        public string Description { get; set; } = "Goes back to parent folder. Usage: back";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        private readonly SharedFileRepository _sharedFileRepository;
        private readonly SharedFolderRepository _sharedFolderRepository;
        public User User {  get; set; }

        public SharedDiskBackCommand(FileRepository fileRepository, FolderRepository folderRepository, SharedFileRepository sharedFileRepository, SharedFolderRepository sharedFolderRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            _sharedFileRepository = sharedFileRepository;
            _sharedFolderRepository = sharedFolderRepository;
            User=user;
        }

        public void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
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

            if (newCurrentDirectory == null)
            {
                Writer.Error("Could not find parent folder.");
                return;
            }

            if (!IsFolderShared(newCurrentDirectory))
            {
                newCurrentDirectory = _folderRepository.GetUsersRoot(User);

                if (newCurrentDirectory != null) 
                    currentDirectory = newCurrentDirectory;

                currentFolders = _sharedFolderRepository.GetFoldersFromRootByUser(User);
                currentFiles = _sharedFileRepository.GetFilesFromRootByUser(User);
                return;
            }

            currentDirectory = newCurrentDirectory;
            currentFolders = _sharedFolderRepository.GetFoldersByUser(User, currentDirectory.Id);
            currentFiles = _sharedFileRepository.GetFilesByUser(User, currentDirectory.Id);
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (!string.IsNullOrEmpty(commandArguments))
            {
                return false;
            }

            return true;
        }

        public bool IsFolderShared(Folder currentDirectory)
        {
            if (_sharedFolderRepository.GetUsersByFolder(currentDirectory).Contains(User))
                return true;

            return false;
        }
    }
}