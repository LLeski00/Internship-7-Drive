using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Enums;

namespace Drive.Presentation.Commands
{
    public class RenameCommand : ICommand
    {
        public string Name { get; set; } = "rename";
        public string Description { get; set; } = "Changes the name of the file or folder in the current directory. Usage: rename file 'name.extension' 'newName.newExtension' or rename folder 'name' 'name'";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FolderFileRepository _folderFileRepository;

        public RenameCommand(FileRepository fileRepository, FolderRepository folderRepository, FolderFileRepository folderFileRepository)
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
            var renameType = commandArgumentsSplit[0];

            switch (renameType)
            {
                case "file":
                    RenameFile(commandArgumentsSplit[1], commandArgumentsSplit[2], ref currentFiles);
                    break;
                case "folder":
                    RenameFolder(commandArgumentsSplit[1], commandArgumentsSplit[2], ref currentFolders);
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

            if (commandArgumentsSplit.Length != 3 || (commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder"))
                return false;

            if (commandArgumentsSplit[0].ToLower() == "file" && (!DiskExtensions.IsFileNameValid(commandArgumentsSplit[1]) || !DiskExtensions.IsFileNameValid(commandArgumentsSplit[2])))
                return false;
            else if (commandArgumentsSplit[0].ToLower() == "folder" && (!DiskExtensions.IsFolderNameValid(commandArgumentsSplit[1]) || !DiskExtensions.IsFolderNameValid(commandArgumentsSplit[2])))
                return false;

            return true;
        }

        public void RenameFile(string file, string newFile, ref ICollection<File> currentFiles)
        {
            var fileSplitByDot = file.Split('.');
            var fileName = fileSplitByDot[0];
            var fileExtension = fileSplitByDot[1];

            var updatedFileSplitByDot = newFile.Split('.');
            var updatedFileName = updatedFileSplitByDot[0];
            var updatedFileExtension = updatedFileSplitByDot[1];

            var fileToUpdate = DiskExtensions.GetFileByName(currentFiles, fileName, fileExtension);

            if (fileToUpdate == null)
            {
                Writer.Error("File with that name doesn't exists in this folder!");
                return;
            }

            var updatedFile = DiskExtensions.GetFileByName(currentFiles, updatedFileName, updatedFileExtension);

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

        public void RenameFolder(string folderName, string newFolderName, ref ICollection<Folder> currentFolders)
        {
            var folderToUpdate = DiskExtensions.GetFolderByName(currentFolders, folderName);

            if (folderToUpdate == null)
            {
                Writer.Error("Folder with that name doesn't exists in this folder!");
                return;
            }

            var updatedFolder = DiskExtensions.GetFolderByName(currentFolders, newFolderName);

            if (updatedFolder != null)
            {
                Writer.Error("Folder with that name already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to rename this folder?"))
                return;

            var response = _folderRepository.Rename(newFolderName, folderToUpdate.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the folder.");
                return;
            }

            folderToUpdate.Name = newFolderName;

            Console.WriteLine("Folder successfully renamed.");
        }
    }
}