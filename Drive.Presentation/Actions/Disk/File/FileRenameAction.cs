using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileRenameAction : IAction
    {
        private readonly FileRepository _fileRepository;
        public string FileToRename { get; set; }
        public string UpdatedFile { get; set; }
        public ICollection<File> CurrentFiles { get; set; }

        public string Name { get; set; } = "Rename file";
        public int MenuIndex { get; set; }

        public FileRenameAction(FileRepository fileRepository, string fileToRename, string updatedFile, ICollection<File> currentFiles)
        {
            _fileRepository = fileRepository;
            FileToRename = fileToRename;
            UpdatedFile = updatedFile;
            CurrentFiles = currentFiles;
        }

        public void Open()
        {
            var fileSplitByDot = FileToRename.Split('.');
            var fileName = fileSplitByDot[0];
            var fileExtension = fileSplitByDot[1];

            var updatedFileSplitByDot = UpdatedFile.Split('.');
            var updatedFileName = updatedFileSplitByDot[0];
            var updatedFileExtension = updatedFileSplitByDot[1];

            var fileToUpdate = DiskExtensions.GetFileByName(CurrentFiles, fileName, fileExtension);

            if (fileToUpdate == null)
            {
                Writer.Error("File with that name doesn't exists in this folder!");
                return;
            }

            var updatedFile = DiskExtensions.GetFileByName(CurrentFiles, updatedFileName, updatedFileExtension);

            if (updatedFile != null)
            {
                Writer.Error("File with that name already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to rename this file?"))
                return;

            var fileResponse = _fileRepository.Rename(updatedFileName, updatedFileExtension, fileToUpdate.Id);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the file.");
                return;
            }

            fileToUpdate.Name = updatedFileName;
            fileToUpdate.Extension = updatedFileExtension;

            Console.WriteLine("File successfully renamed.");
        }
    }
}