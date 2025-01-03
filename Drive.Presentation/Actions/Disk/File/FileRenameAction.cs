using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Utils;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileRenameAction : IAction
    {
        private readonly FileRepository _fileRepository;
        public File FileToRename { get; set; }
        public (string Name, string Extension) NewFile { get; set; }

        public string Name { get; set; } = "Rename file";
        public int MenuIndex { get; set; }

        public FileRenameAction(FileRepository fileRepository, File fileToRename, (string Name, string Extension) newFile)
        {
            _fileRepository = fileRepository;
            FileToRename = fileToRename;
            NewFile = newFile;
        }

        public void Open()
        {
            if (!UserExtensions.ConfirmUserAction("Are you sure you want to rename this file?"))
                return;

            var fileResponse = _fileRepository.Rename(NewFile.Name, NewFile.Extension, FileToRename.Id);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the file.");
                return;
            }

            Console.WriteLine("File successfully renamed.");
        }
    }
}