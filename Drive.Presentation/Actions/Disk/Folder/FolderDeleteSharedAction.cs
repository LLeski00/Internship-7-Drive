using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using Drive.Data.Entities.Models;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderDeleteSharedAction : IAction
    {
        private readonly SharedFolderRepository _sharedFolderRepository;
        public string FolderToDelete { get; set; }
        public ICollection<Folder> CurrentFolders { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Delete shared folder";
        public int MenuIndex { get; set; }

        public FolderDeleteSharedAction(SharedFolderRepository sharedFolderRepository, string folderToDelete, ICollection<Folder> currentFolders, User user)
        {
            _sharedFolderRepository = sharedFolderRepository;
            FolderToDelete = folderToDelete;
            CurrentFolders = currentFolders;
            User = user;
        }

        public void Open()
        {
            var folderToDelete = DiskExtensions.GetFolderByName(CurrentFolders, FolderToDelete);

            if (folderToDelete == null)
            {
                Writer.Error("Folder with that name doesn't exist in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to delete this folder?"))
                return;

            var response = _sharedFolderRepository.Delete(folderToDelete.Id, User.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with deleting the folder.");
                return;
            }

            CurrentFolders.Remove(folderToDelete);
            Console.WriteLine("Folder successfully deleted.");
        }
    }
}