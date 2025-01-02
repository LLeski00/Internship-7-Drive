using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;

namespace Drive.Presentation.Commands
{
    public class RenameCommand : ICommand
    {
        public string Name { get; set; } = "rename";
        public string Description { get; set; } = "Changes the name of the file or folder in the current directory. Usage: rename file 'name.extension' 'newName.newExtension' or rename folder 'name' 'name'";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;

        public RenameCommand(FileRepository fileRepository, FolderRepository folderRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
        }

        //refactor like shareCommand
        //change Rename actions to take actual files/folders

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
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
                    var fileRenameAction = new FileRenameAction(_fileRepository, commandArgumentsSplit[1], commandArgumentsSplit[2], currentFiles);
                    fileRenameAction.Open();
                    break;
                case "folder":
                    var folderRenameAction = new FolderRenameAction(_folderRepository, commandArgumentsSplit[1], commandArgumentsSplit[2], currentFolders);
                    folderRenameAction.Open();
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

            if (commandArgumentsSplit.Length != 3 || (commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder"))
                return false;

            if (commandArgumentsSplit[0].ToLower() == "file" && (!DiskExtensions.IsFileNameValid(commandArgumentsSplit[1]) || !DiskExtensions.IsFileNameValid(commandArgumentsSplit[2])))
                return false;
            else if (commandArgumentsSplit[0].ToLower() == "folder" && (!DiskExtensions.IsFolderNameValid(commandArgumentsSplit[1]) || !DiskExtensions.IsFolderNameValid(commandArgumentsSplit[2])))
                return false;

            return true;
        }
    }
}