using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Actions.Disk
{
    public class SharedDiskAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        private readonly SharedFileRepository _sharedFileRepository;
        private readonly SharedFolderRepository _sharedFolderRepository;
        public User User { get; set; }

        public string Name { get; set; } = "Shared disk";
        public int MenuIndex { get; set; }

        public SharedDiskAction(FolderRepository folderRepository, SharedFileRepository sharedFileRepository, SharedFolderRepository sharedFolderRepository, User user)
        {
            _folderRepository = folderRepository;
            _sharedFileRepository = sharedFileRepository;
            _sharedFolderRepository = sharedFolderRepository;
            User = user;
        }

        public void Open()
        {
            var root = _folderRepository.GetUsersRoot(User);

            if (root == null)
            {
                Writer.Error("Error while fetching users root folder.");
                return;
            }

            var currentFolders = _sharedFolderRepository.GetFoldersByUser(User);
            var currentFiles = _sharedFileRepository.GetFilesByUser(User);

            Console.Clear();
            CommandExtensions.ProcessUserCommands(root, currentFolders, currentFiles, User);
        }
    }
}