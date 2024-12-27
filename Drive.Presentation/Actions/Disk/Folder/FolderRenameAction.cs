using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderRenameAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        public string FolderToRename { get; set; }
        public string UpdatedFolder { get; set; }
        public ICollection<Folder> CurrentFolders { get; set; }

        public string Name { get; set; } = "Delete folder";
        public int MenuIndex { get; set; }

        public FolderRenameAction(FolderRepository folderRepository, string folderToRename, string updatedFolder, ICollection<Folder> currentFolders)
        {
            _folderRepository = folderRepository;
            FolderToRename = folderToRename;
            UpdatedFolder = updatedFolder;
            CurrentFolders = currentFolders;
        }

        public void Open()
        {
            var folderToUpdate = DiskExtensions.GetFolderByName(CurrentFolders, FolderToRename);

            if (folderToUpdate == null)
            {
                Writer.Error("Folder with that name doesn't exists in this folder!");
                return;
            }

            var updatedFolder = DiskExtensions.GetFolderByName(CurrentFolders, UpdatedFolder);

            if (updatedFolder != null)
            {
                Writer.Error("Folder with that name already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to rename this folder?"))
                return;

            var response = _folderRepository.Rename(UpdatedFolder, folderToUpdate.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the folder.");
                return;
            }

            folderToUpdate.Name = UpdatedFolder;
            Console.WriteLine("Folder successfully renamed.");
        }
    }
}
