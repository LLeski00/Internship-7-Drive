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
    public class CreateCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "create";
        public string Description { get; set; } = "Creates a file or a folder in the current directory. Usage: create file 'name.extension' or create folder 'name'";
        public User User { get; set; }

        public CreateCommand(User user)
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
            var createType = commandArgumentsSplit[0];

            switch (createType)
            {
                case "file":
                    CreateFile(currentFiles, commandArgumentsSplit[1], currentDirectory);
                    break;
                case "folder":
                    CreateFolder(currentFolders, commandArgumentsSplit[1], currentDirectory);
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

        public void CreateFile(ICollection<File> currentFiles, string fileString, Folder currentDirectory)
        {
            if (!Reader.TryReadNameAndExtensionFromFile(fileString, out (string Name, string Extension) file)){
                Writer.Error("Invalid file name!");
                return;
            }
            else if (DiskUtils.GetFileByName(currentFiles, file.Name, file.Extension) != null)
            {
                Console.WriteLine("The file with that name already exists in this directory!");
                return;
            }

            var fileToAdd = new File(file.Name, file.Extension, User.Id, currentDirectory.Id);

            var fileAddAction = new FileAddAction(RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<SharedFolderRepository>(), fileToAdd);
            fileAddAction.Open();
        }

        public void CreateFolder(ICollection<Folder> currentFolders, string folderName, Folder currentDirectory)
        {
            if (DiskUtils.GetFolderByName(currentFolders, folderName) != null)
            {
                Console.WriteLine("The folder with that name already exists in this directory!");
                return;
            }

            var folderToAdd = new Folder(folderName, User.Id, currentDirectory.Id);

            var folderAddAction = new FolderAddAction(RepositoryFactory.Create<FolderRepository>(), folderToAdd);
            folderAddAction.Open();
        }
    }
}