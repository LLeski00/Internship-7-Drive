using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderStopShareAction : IAction
    {
        private readonly SharedFolderRepository _sharedFolderRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;
        public Folder SharedFolder { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Stop sharing the folder with another user";
        public int MenuIndex { get; set; }

        public FolderStopShareAction(SharedFolderRepository sharedFolderRepository, FolderRepository folderRepository, FileRepository fileRepository, Folder sharedFolder, User user)
        {
            _sharedFolderRepository = sharedFolderRepository;
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
            SharedFolder = sharedFolder;
            User = user;
        }

        public void Open()
        {
            if (!_sharedFolderRepository.GetAllFoldersByUser(User).Any(f => f.Id == SharedFolder.Id))
            {
                Writer.Error("The folder is not shared with this user!");
                return;
            }

            StopSharingFolderAndChildren(SharedFolder);
        }

        public void StopSharingFolderAndChildren(Folder folder)
        {
            var subFolders = _folderRepository.GetByParent(folder.Id);

            foreach (var subFolder in subFolders)
            {
                StopSharingFolderAndChildren(subFolder);
            }

            var childFiles = _fileRepository.GetByParent(folder.Id);

            foreach (var childFile in childFiles)
            {
                var fileShareAction = new FileStopShareAction(RepositoryFactory.Create<SharedFileRepository>(), childFile, User);
                fileShareAction.Open();
            }

            var response = _sharedFolderRepository.Delete(folder.Id, User.Id);

            switch (response)
            {
                case ResponseResultType.Success:
                    Console.WriteLine($"{SharedFolder.Name} successfully stopped sharing to {User.Email}.");
                    break;
                case ResponseResultType.NotFound:
                    Console.WriteLine("The folder is not shared with the user!");
                    break;
                default:
                    Writer.Error("ERROR: Something went wrong with stopping the share of the folder.");
                    break;
            }
        }
    }
}