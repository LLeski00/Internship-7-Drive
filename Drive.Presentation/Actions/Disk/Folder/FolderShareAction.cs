using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Domain.Factories;
using Drive.Presentation.Extensions;
using Drive.Data.Entities.Models;
using System.IO;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderShareAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;
        private readonly SharedFolderRepository _sharedFolderRepository;
        public Folder FolderToShare { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Share folder with another user";
        public int MenuIndex { get; set; }

        public FolderShareAction(FolderRepository folderRepository, FileRepository fileRepository, SharedFolderRepository sharedFolderRepository, Folder folderToShare, User user)
        {
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            _sharedFolderRepository = sharedFolderRepository;
            FolderToShare = folderToShare;
            User = user;
        }

        public void Open()
        {
            ShareFolderAndChildren(FolderToShare);
        }

        public void ShareFolderAndChildren(Folder folder)
        {
            var subFolders = _folderRepository.GetByParent(folder.Id);

            foreach (var subFolder in subFolders)
            {
                ShareFolderAndChildren(subFolder);
            }

            var childFiles = _fileRepository.GetByParent(folder.Id);

            foreach (var childFile in childFiles)
            {
                var fileShareAction = new FileShareAction(RepositoryFactory.Create<SharedFileRepository>(), childFile, User);
                fileShareAction.Open();
            }

            if (folder.OwnerId ==  User.Id)
            {
                Writer.Error("You cannot share the folder with yourself!");
                return;
            }

            if (_sharedFolderRepository.GetAllFoldersByUser(User).Any(f => f.Id == folder.Id))
            {
                Writer.Error("The folder is already shared with this user!");
                return;
            }

            var sharedFolder = new SharedFolder(User.Id, folder.Id);
            var response = _sharedFolderRepository.Add(sharedFolder);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the folder.");
                return;
            }

            Console.WriteLine($"{FolderToShare.Name} successfully shared.");
        }
    }
}