using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Enums;
using Drive.Presentation.Actions.Disk;

namespace Drive.Presentation.Commands
{
    public class CreateCommand : ICommand
    {
        public string Name { get; set; } = "create";
        public string Description { get; set; } = "Creates a file or a folder in the current directory. Usage: create file 'name.extension' or create folder 'name'";
        public User User { get; set; }
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;

        public CreateCommand(FileRepository fileRepository, FolderRepository folderRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            User = user;
        }

        //Refactor like ShareCommand
        //Maybe change the FileAddActions and FolderAddActions to just take the file/folder not the name

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
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
                    var fileAddAction = new FileAddAction(_fileRepository, commandArgumentsSplit[1], currentDirectory, currentFiles, User);
                    fileAddAction.Open();
                    break;
                case "folder":
                    var folderAddAction = new FolderAddAction(_folderRepository, commandArgumentsSplit[1], currentDirectory, currentFolders, User);
                    folderAddAction.Open();
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
    }
}