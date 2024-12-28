using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderAddAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        public string FolderName { get; set; }
        public Folder ParentFolder { get; set; }
        public ICollection<Folder> CurrentFolders { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Add folder";
        public int MenuIndex { get; set; }

        public FolderAddAction(FolderRepository folderRepository, string folderName, Folder parentFolder, ICollection<Folder> currentFolders, User user)
        {
            _folderRepository = folderRepository;
            FolderName = folderName;
            ParentFolder = parentFolder;
            CurrentFolders = currentFolders;
            User = user;
        }

        public void Open()
        {
            if (DiskExtensions.GetFolderByName(CurrentFolders, FolderName) != null)
            {
                Writer.Error("Folder with that name already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to add this folder?"))
                return;

            var newFolder = new Folder(FolderName, User.Id, ParentFolder.Id);
            var response = _folderRepository.Add(newFolder);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the folder.");
                return;
            }

            CurrentFolders.Add(newFolder);
            Console.WriteLine("Folder successfully added.");
        }
    }
}
