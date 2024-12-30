using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using Drive.Data.Entities.Models;

namespace Drive.Presentation.Actions.Disk
{
    public class FileDeleteSharedAction : IAction
    {
        private readonly SharedFileRepository _sharedFileRepository;
        public string FileToDelete { get; set; }
        public ICollection<Data.Entities.Models.File> CurrentFiles { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Delete shared file";
        public int MenuIndex { get; set; }

        public FileDeleteSharedAction(SharedFileRepository sharedFileRepository, string fileToDelete, ICollection<Data.Entities.Models.File> currentFiles, User user)
        {
            _sharedFileRepository = sharedFileRepository;
            FileToDelete = fileToDelete;
            CurrentFiles = currentFiles;
            User = user;
        }

        public void Open()
        {
            var fileSplitByDot = FileToDelete.Split('.');
            var fileName = fileSplitByDot[0];
            var fileExtension = fileSplitByDot[1];

            var fileToDelete = DiskExtensions.GetFileByName(CurrentFiles, fileName, fileExtension);

            if (fileToDelete == null)
            {
                Writer.Error("File with that name doesn't exist in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to delete this file?"))
                return;

            var fileResponse = _sharedFileRepository.Delete(fileToDelete.Id, User.Id);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with deleting the file.");
                return;
            }

            CurrentFiles.Remove(fileToDelete);
            Console.WriteLine("File successfully deleted.");
        }
    }
}