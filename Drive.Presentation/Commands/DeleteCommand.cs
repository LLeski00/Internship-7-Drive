using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Enums;

namespace Drive.Presentation.Commands
{
    public class DeleteCommand : ICommand
    {
        public string Name { get; set; } = "delete";
        public string Description { get; set; } = "Deletes a file or a folder in the current directory. Usage: delete file 'name.extension' or delete folder 'name'";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FolderFileRepository _folderFileRepository;

        public DeleteCommand(FileRepository fileRepository, FolderRepository folderRepository, FolderFileRepository folderFileRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            _folderFileRepository = folderFileRepository;
        }

        public void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
        {
            if (commandArguments == null || !IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var commandArgumentsSplit = commandArguments.Split(' ');
            var deleteType = commandArgumentsSplit[0];

            switch (deleteType)
            {
                case "file":
                    DeleteFile(commandArgumentsSplit[1], ref currentFiles);
                    break;
                case "folder":
                    DeleteFolder(commandArgumentsSplit[1], ref currentFolders);
                    break;
                default:
                    Writer.CommandError(Name, Description);
                    break;
            }
        }

        public bool IsCommandValid(string commandArguments)
        {
            if (string.IsNullOrEmpty(commandArguments))
                return false;

            var commandArgumentsSplit = commandArguments.Split(' ');

            if (commandArgumentsSplit.Length != 2 || (commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder"))
                return false;

            if (commandArgumentsSplit[0].ToLower() == "file" && !DiskExtensions.IsFileNameValid(commandArgumentsSplit[1]))
                return false;
            else if (commandArgumentsSplit[0].ToLower() == "folder" && !DiskExtensions.IsFolderNameValid(commandArgumentsSplit[1]))
                return false;

            return true;
        }

        public void DeleteFile(string file, ref ICollection<File> currentFiles)
        {
            var fileSplitByDot = file.Split('.');
            var fileName = fileSplitByDot[0];
            var fileExtension = fileSplitByDot[1];

            var fileToDelete = DiskExtensions.GetFileByName(currentFiles, fileName, fileExtension);

            if (fileToDelete == null)
            {
                Writer.Error("File with that name doesn't exist in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to delete this file?"))
                return;

            var fileResponse = _fileRepository.Delete(fileToDelete.Id);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with deleting the file.");
                return;
            }

            currentFiles.Remove(fileToDelete);
            Console.WriteLine("File successfully deleted.");
        }

        public void DeleteFolder(string folderName, ref ICollection<Folder> currentFolders)
        {
            var folderToDelete = DiskExtensions.GetFolderByName(currentFolders, folderName);

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

            currentFolders.Remove(folderToDelete);
            Console.WriteLine("Folder successfully deleted.");
        }
    }
}