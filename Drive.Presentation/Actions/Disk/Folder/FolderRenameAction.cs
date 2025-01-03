using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;
using Drive.Presentation.Abstractions.Actions;
using Drive.Presentation.Utils;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderRenameAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        public Folder FolderToRename { get; set; }
        public string NewFolderName { get; set; }

        public string Name { get; set; } = "Rename folder";
        public int MenuIndex { get; set; }

        public FolderRenameAction(FolderRepository folderRepository, Folder folderToRename, string newFolderName)
        {
            _folderRepository = folderRepository;
            FolderToRename = folderToRename;
            NewFolderName = newFolderName;
        }

        public void Open()
        {
            if (!UserUtils.ConfirmUserAction("Are you sure you want to rename this folder?"))
                return;

            var response = _folderRepository.Rename(NewFolderName, FolderToRename.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the folder.");
                return;
            }

            Console.WriteLine("Folder successfully renamed.");
        }
    }
}
