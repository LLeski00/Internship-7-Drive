using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderDeleteAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        public string FolderName { get; set; }
        public ICollection<Folder> CurrentFolders { get; set; }

        public string Name { get; set; } = "Delete folder";
        public int MenuIndex { get; set; }

        public FolderDeleteAction(FolderRepository folderRepository, string folderName, ICollection<Folder> currentFolders)
        {
            _folderRepository = folderRepository;
            FolderName = folderName;
            CurrentFolders = currentFolders;
        }

        public void Open()
        {
            var folderToDelete = DiskExtensions.GetFolderByName(CurrentFolders, FolderName);

            if (folderToDelete == null)
            {
                Writer.Error("Folder with that name doesn't exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to delete this folder?"))
                return;

            var response = _folderRepository.Delete(folderToDelete.Id);

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
