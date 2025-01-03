using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Abstractions.Commands;

namespace Drive.Presentation.Commands.Directory
{
    public class DeleteCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "delete";
        public string Description { get; set; } = "Deletes a file or a folder in the current directory. Usage: delete file 'name.extension' or delete folder 'name'";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;

        public DeleteCommand(FileRepository fileRepository, FolderRepository folderRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
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
                    DeleteFile(currentFiles, commandArgumentsSplit[1], currentDirectory);
                    break;
                case "folder":
                    DeleteFolder(currentFolders, commandArgumentsSplit[1], currentDirectory);
                    break;
                default:
                    Writer.CommandError(Name, Description);
                    break;
            }
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (string.IsNullOrEmpty(commandArguments))
                return false;

            var commandArgumentsSplit = commandArguments.Split(' ');

            if (commandArgumentsSplit.Length != 2 || commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder")
                return false;

            if (commandArgumentsSplit[0].ToLower() == "file" && !DiskUtils.IsFileNameValid(commandArgumentsSplit[1]))
                return false;
            else if (commandArgumentsSplit[0].ToLower() == "folder" && !DiskUtils.IsFolderNameValid(commandArgumentsSplit[1]))
                return false;

            return true;
        }

        public void DeleteFile(ICollection<File> currentFiles, string fileString, Folder currentDirectory)
        {
            if (!Reader.TryReadNameAndExtensionFromFile(fileString, out (string Name, string Extension) file))
            {
                Writer.Error("Invalid file name!");
                return;
            }

            var fileToDelete = DiskUtils.GetFileByName(currentFiles, file.Name, file.Extension);

            if (fileToDelete == null)
            {
                Console.WriteLine("The file with that name doesn't exist in this directory!");
                return;
            }

            var fileDeleteAction = new FileDeleteAction(_fileRepository, fileToDelete);
            fileDeleteAction.Open();
        }

        public void DeleteFolder(ICollection<Folder> currentFolders, string folderName, Folder currentDirectory)
        {
            var folderToDelete = DiskUtils.GetFolderByName(currentFolders, folderName);

            if (folderToDelete == null)
            {
                Console.WriteLine("The folder with that name doesn't exist in this directory!");
                return;
            }

            var folderDeleteAction = new FolderDeleteAction(_folderRepository, folderToDelete);
            folderDeleteAction.Open();
        }
    }
}