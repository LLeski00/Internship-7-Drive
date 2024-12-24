using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Actions.Disk
{
    public class MyDiskAction : IAction
    {
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        public User User { get; set; }

        public string Name { get; set; } = "My disk";
        public int MenuIndex { get; set; }

        public MyDiskAction(FileRepository fileRepository, FolderRepository folderRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
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

            var currentFolders = _folderRepository.GetByUser(User, root.Id);
            var currentFiles = _fileRepository.GetByUser(User, root.Id);

            Console.Clear();
            CommandExtensions.ProcessUserCommands(root, currentFolders, currentFiles, User);
        }
    }
}