using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;
using Drive.Presentation.Abstractions.Actions;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Utils;

namespace Drive.Presentation.Actions.Disk
{
    public class FileDeleteAction : IAction
    {
        private readonly FileRepository _fileRepository;
        public File FileToDelete { get; set; }

        public string Name { get; set; } = "Delete file";
        public int MenuIndex { get; set; }

        public FileDeleteAction(FileRepository fileRepository, File fileToDelete)
        {
            _fileRepository = fileRepository;
            FileToDelete = fileToDelete;
        }

        public void Open()
        {
            if (!UserUtils.ConfirmUserAction("Are you sure you want to delete this file?"))
                return;

            var fileResponse = _fileRepository.Delete(FileToDelete.Id);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with deleting the file.");
                return;
            }

            Console.WriteLine("File successfully deleted.");
        }
    }
}