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
    public class SharedDiskDeleteCommand : ICommand
    {
        public string Name { get; set; } = "delete";
        public string Description { get; set; } = "Deletes a file or a folder in the current directory. Usage: delete file 'name.extension' or delete folder 'name'";
        private readonly SharedFileRepository _sharedFileRepository;
        private readonly SharedFolderRepository _sharedFolderRepository;
        public User User { get; set; }

        public SharedDiskDeleteCommand(SharedFileRepository sharedFileRepository, SharedFolderRepository sharedFolderRepository, User user)
        {
            _sharedFileRepository = sharedFileRepository;
            _sharedFolderRepository = sharedFolderRepository;
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
            var deleteType = commandArgumentsSplit[0];

            OpenActionByType(deleteType, commandArgumentsSplit[1], currentFiles, currentFolders);
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

        public void OpenActionByType(string deleteType, string name, ICollection<File> currentFiles, ICollection<Folder> currentFolders)
        {
            switch (deleteType)
            {
                case "file":
                    if (!UserExtensions.ConfirmUserAction("Are you sure you want to delete this file?"))
                        return;

                    var nameSplit = name.Split('.');
                    var fileToDelete = DiskExtensions.GetFileByName(currentFiles, nameSplit[0], nameSplit[1]);

                    if (fileToDelete == null)
                    {
                        Console.WriteLine("The file was not found.");
                        return;
                    }

                    var fileDeleteAction = new FileDeleteSharedAction(_sharedFileRepository, fileToDelete, User);
                    fileDeleteAction.Open();
                    break;
                case "folder":
                    if (!UserExtensions.ConfirmUserAction("Are you sure you want to delete this file?"))
                        return;

                    var folderToDelete = DiskExtensions.GetFolderByName(currentFolders, name);

                    if (folderToDelete == null)
                    {
                        Console.WriteLine("The folder was not found.");
                        return;
                    }

                    var folderDeleteAction = new FolderDeleteSharedAction(_sharedFolderRepository, _sharedFileRepository, folderToDelete, User);
                    folderDeleteAction.Open();
                    break;
                default:
                    Writer.CommandError(Name, Description);
                    break;
            }
        }
    }
}