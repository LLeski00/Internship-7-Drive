using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;
using Drive.Presentation.Abstractions.Actions;
using Drive.Presentation.Utils;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderDeleteAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        public Folder FolderToDelete { get; set; }

        public string Name { get; set; } = "Delete folder";
        public int MenuIndex { get; set; }

        public FolderDeleteAction(FolderRepository folderRepository, Folder folderToDelete)
        {
            _folderRepository = folderRepository;
            FolderToDelete = folderToDelete;
        }

        public void Open()
        {
            if (!UserUtils.ConfirmUserAction("Are you sure you want to delete this folder?"))
                return;

            var response = _folderRepository.Delete(FolderToDelete.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with deleting the folder.");
                return;
            }

            Console.WriteLine("Folder successfully deleted.");
        }
    }
}
