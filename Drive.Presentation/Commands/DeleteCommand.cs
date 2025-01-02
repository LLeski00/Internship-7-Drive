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
    public class DeleteCommand : ICommand
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

        //Maybe change the delete actions to take the files/folders rather than strings

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
                    var fileDeleteAction = new FileDeleteAction(_fileRepository, commandArgumentsSplit[1], currentFiles);
                    fileDeleteAction.Open();
                    break;
                case "folder":
                    var folderDeleteAction = new FolderDeleteAction(_folderRepository, commandArgumentsSplit[1], currentFolders);
                    folderDeleteAction.Open();
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