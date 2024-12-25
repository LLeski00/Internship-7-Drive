using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Enums;

namespace Drive.Presentation.Commands
{
    public class CreateCommand : ICommand
    {
        public string Name { get; set; } = "create";
        public string Description { get; set; } = "Creates a file or a folder in the current directory. Usage: create file 'name.extension' or create folder 'name'";
        public User User { get; set; }
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FolderFileRepository _folderFileRepository;

        //NEEDS REFACTORING

        public CreateCommand(FileRepository fileRepository, FolderRepository folderRepository, FolderFileRepository folderFileRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            _folderFileRepository = folderFileRepository;
            User = user;
        }

        public void Execute(ref Folder currentDirectory, ref ICollection<Folder> currentFolders, ref ICollection<File> currentFiles, string? commandArguments)
        {
            if (commandArguments == null || !IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var commandArgumentsSplit = commandArguments.Split(' ');
            var createType = commandArgumentsSplit[0];

            switch (createType)
            {
                case "file":
                    CreateFile(commandArgumentsSplit[1], currentDirectory, ref currentFiles);
                    break;
                case "folder":
                    CreateFolder(commandArgumentsSplit[1], currentDirectory, ref currentFolders);
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

        public void CreateFile(string file, Folder parentFolder, ref ICollection<File> currentFiles)
        {
            var fileSplitByDot = file.Split('.');
            var fileName = fileSplitByDot[0];
            var fileExtension = fileSplitByDot[1];

            if (DiskExtensions.GetFileByName(currentFiles, fileName, fileExtension) != null)
            {
                Writer.Error("File with that name and extension already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to add this file?"))
                return;

            var newFile = new File(fileName, fileExtension, User.Id);
            var fileResponse = _fileRepository.Add(newFile);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the file.");
                return;
            }

            var newFolderFile = new FolderFile(parentFolder.Id, newFile.Id);
            var folderFileResponse = _folderFileRepository.Add(newFolderFile);

            if (folderFileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the file.");
                _fileRepository.Delete(newFile.Id);
            }

            currentFiles.Add(newFile);
            Console.WriteLine("File successfully added.");
        }

        public void CreateFolder(string folderName, Folder parentFolder, ref ICollection<Folder> currentFolders)
        {
            if (DiskExtensions.GetFolderByName(currentFolders, folderName) != null)
            {
                Writer.Error("Folder with that name already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to add this folder?"))
                return;

            var newFolder = new Folder(folderName, User.Id, parentFolder.Id);
            var response = _folderRepository.Add(newFolder);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the folder.");
                return;
            }

            currentFolders.Add(newFolder);
            Console.WriteLine("Folder successfully added.");
        }
    }
}