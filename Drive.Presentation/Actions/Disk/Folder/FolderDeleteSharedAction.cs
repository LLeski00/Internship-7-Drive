using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;
using Drive.Data.Entities.Models;
using Drive.Domain.Factories;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderDeleteSharedAction : IAction
    {
        private readonly SharedFolderRepository _sharedFolderRepository;
        private readonly SharedFileRepository _sharedFileRepository;
        public Folder FolderToDelete { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Delete shared folder";
        public int MenuIndex { get; set; }

        public FolderDeleteSharedAction(SharedFolderRepository sharedFolderRepository, SharedFileRepository sharedFileRepository, Folder folderToDelete, User user)
        {
            _sharedFolderRepository = sharedFolderRepository;
            _sharedFileRepository = sharedFileRepository;
            FolderToDelete = folderToDelete;
            User = user;
        }

        public void Open()
        {
            if (DeleteFolderAndChildren(FolderToDelete) == ResponseResultType.Success)
                Console.WriteLine("The folder was successfully removed from shared disk.");
        }

        public ResponseResultType DeleteFolderAndChildren(Folder folderToDelete)
        {
            var subfolders = _sharedFolderRepository.GetFoldersByUser(User, folderToDelete.Id);

            foreach (var subfolder in subfolders)
            {
                var response = DeleteFolderAndChildren(subfolder);
                if (response != ResponseResultType.Success)
                    return response;
            }

            var childrenFiles = _sharedFileRepository.GetFilesByUser(User, folderToDelete.Id);

            foreach (var child in childrenFiles)
            {
                var deleteSharedFileAction = new FileDeleteSharedAction(RepositoryFactory.Create<SharedFileRepository>(), child, User);
                deleteSharedFileAction.Open();
            }

            var folderResponse = _sharedFolderRepository.Delete(folderToDelete.Id, User.Id);

            if (folderResponse != ResponseResultType.Success)
                Writer.Error($"ERROR: Something went wrong with deleting the folder {folderToDelete.Name}.");

            return folderResponse;
        }
    }
}