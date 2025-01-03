using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Abstractions.Commands;
using Drive.Domain.Factories;

namespace Drive.Presentation.Commands.SharedDisk
{
    public class SharedDiskDeleteCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "delete";
        public string Description { get; set; } = "Deletes a file or a folder in the current directory. Usage: delete file 'name.extension' or delete folder 'name'";
        public User User { get; set; }

        public SharedDiskDeleteCommand(User user)
        {
            User = user;
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
                    DeleteSharedFile(currentFiles, commandArgumentsSplit[1], User);
                    break;
                case "folder":
                    DeleteSharedFolder(currentFolders, commandArgumentsSplit[1], User);
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

            if (commandArgumentsSplit.Length != 2 ||
               (commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder") ||
               (commandArgumentsSplit[0].ToLower() == "file" && !DiskUtils.IsFileNameValid(commandArgumentsSplit[1])) ||
               (commandArgumentsSplit[0].ToLower() == "folder" && !DiskUtils.IsFolderNameValid(commandArgumentsSplit[1])))
                return false;

            return true;
        }

        public void DeleteSharedFile(ICollection<File> currentFiles, string name, User user)
        {
            if (!Reader.TryReadNameAndExtensionFromFile(name, out (string Name, string Extension) file))
            {
                Writer.Error("The file name is not valid!");
                return;
            }

            var fileToDelete = DiskUtils.GetFileByName(currentFiles, file.Name, file.Extension);

            if (fileToDelete == null)
            {
                Console.WriteLine("The file was not found.");
                return;
            }

            if (!UserUtils.ConfirmUserAction("Are you sure you want to delete this file from your shared disk?"))
                return;

            var fileDeleteAction = new FileDeleteSharedAction(RepositoryFactory.Create<SharedFileRepository>(), fileToDelete, User);
            fileDeleteAction.Open();
        }

        public void DeleteSharedFolder(ICollection<Folder> currentFolders, string name, User user)
        {
            var folderToDelete = DiskUtils.GetFolderByName(currentFolders, name);

            if (folderToDelete == null)
            {
                Console.WriteLine("The folder was not found.");
                return;
            }

            if (!UserUtils.ConfirmUserAction("Are you sure you want to delete this folder from your shared disk?"))
                return;

            var folderDeleteAction = new FolderDeleteSharedAction(RepositoryFactory.Create<SharedFolderRepository>(), RepositoryFactory.Create<SharedFileRepository>(), folderToDelete, User);
            folderDeleteAction.Open();
        }
    }
}