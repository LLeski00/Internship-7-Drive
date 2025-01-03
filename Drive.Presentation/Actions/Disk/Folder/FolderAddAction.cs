using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FolderAddAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        public Folder FolderToAdd { get; set; }

        public string Name { get; set; } = "Add folder";
        public int MenuIndex { get; set; }

        public FolderAddAction(FolderRepository folderRepository, Folder folderToAdd)
        {
            _folderRepository = folderRepository;
            FolderToAdd = folderToAdd;
        }

        public void Open()
        {
            if (!UserExtensions.ConfirmUserAction("Are you sure you want to add this folder?"))
                return;

            var response = _folderRepository.Add(FolderToAdd);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the folder.");
                return;
            }

            Console.WriteLine("Folder successfully added.");
        }
    }
}
