using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Abstractions.Commands;
using Drive.Domain.Factories;

namespace Drive.Presentation.Commands.Directory
{
    public class RenameCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "rename";
        public string Description { get; set; } = "Changes the name of the file or folder in the current directory. Usage: rename file 'name.extension' 'newName.newExtension' or rename folder 'name' 'name'";

        public RenameCommand()
        {
        }

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
                    RenameFile(currentFiles, commandArgumentsSplit[1], commandArgumentsSplit[2], currentDirectory);
                    break;
                case "folder":
                    RenameFolder(currentFolders, commandArgumentsSplit[1], commandArgumentsSplit[2], currentDirectory);
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

            if (commandArgumentsSplit.Length != 3 || commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder")
                return false;

            if (commandArgumentsSplit[0].ToLower() == "file" && (!DiskUtils.IsFileNameValid(commandArgumentsSplit[1]) || !DiskUtils.IsFileNameValid(commandArgumentsSplit[2])))
                return false;
            else if (commandArgumentsSplit[0].ToLower() == "folder" && (!DiskUtils.IsFolderNameValid(commandArgumentsSplit[1]) || !DiskUtils.IsFolderNameValid(commandArgumentsSplit[2])))
                return false;

            return true;
        }

        public void RenameFile(ICollection<File> currentFiles, string name, string newName, Folder currentDirectory)
        {
            if (!Reader.TryReadNameAndExtensionFromFile(name, out (string Name, string Extension) file) ||
                !Reader.TryReadNameAndExtensionFromFile(newName, out (string Name, string Extension) newFile))
            {
                Writer.Error("Invalid file name!");
                return;
            }

            var fileToRename = DiskUtils.GetFileByName(currentFiles, file.Name, file.Extension);

            if (fileToRename == null)
            {
                Console.WriteLine("The file with that name doesn't exist in this directory!");
                return;
            }

            if (DiskUtils.GetFileByName(currentFiles, newFile.Name, newFile.Extension) != null)
            {
                Console.WriteLine("The file with that name already exists in this directory!");
                return;
            }

            var fileRenameAction = new FileRenameAction(RepositoryFactory.Create<FileRepository>(), fileToRename, newFile);
            fileRenameAction.Open();
        }

        public void RenameFolder(ICollection<Folder> currentFolders, string name, string newName, Folder currentDirectory)
        {
            var folderToRename = DiskUtils.GetFolderByName(currentFolders, name);

            if (folderToRename == null)
            {
                Console.WriteLine("The folder with that name doesn't exist in this directory!");
                return;
            }

            if (DiskUtils.GetFolderByName(currentFolders, newName) != null)
            {
                Console.WriteLine("The folder with that name already exists in this directory!");
                return;
            }

            var folderRenameAction = new FolderRenameAction(RepositoryFactory.Create<FolderRepository>(), folderToRename, newName);
            folderRenameAction.Open();
        }
    }
}