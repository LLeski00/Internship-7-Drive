using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;
using Drive.Domain.Factories;
using Drive.Presentation.Abstractions.Commands;

namespace Drive.Presentation.Commands.Directory
{
    public class StopShareCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "stopShare";
        public string Description { get; set; } = "Stops sharing the file or folder with the user. Usage: stopShare file 'name.extension' 'usersEmail' or stopShare folder 'name' 'usersEmail'";
        private readonly UserRepository _userRepository;

        public StopShareCommand(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (commandArguments == null || !IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var commandArgumentsSplit = commandArguments.Split(' ');
            var shareType = commandArgumentsSplit[0];
            var user = _userRepository.GetByEmail(commandArgumentsSplit[2]);

            if (user == null)
            {
                Writer.Error("The user was not found.");
                return;
            }

            switch (shareType)
            {
                case "file":
                    OpenFileStopShareAction(currentFiles, commandArgumentsSplit[1], user);
                    break;
                case "folder":
                    OpenFolderStopShareAction(currentFolders, commandArgumentsSplit[1], user);
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

            if (commandArgumentsSplit.Length != 3 || 
               (commandArgumentsSplit[0].ToLower() != "file" && commandArgumentsSplit[0].ToLower() != "folder") ||
               (commandArgumentsSplit[0].ToLower() == "file" && !DiskUtils.IsFileNameValid(commandArgumentsSplit[1])) ||
               (commandArgumentsSplit[0].ToLower() == "folder" && !DiskUtils.IsFolderNameValid(commandArgumentsSplit[1])))
                return false;

            return true;
        }

        public void OpenFileStopShareAction(ICollection<File> currentFiles, string file, User user)
        {
            var fileSplitByDot = file.Split('.');

            var fileToShare = DiskUtils.GetFileByName(currentFiles, fileSplitByDot[0], fileSplitByDot[1]);

            if (fileToShare == null)
            {
                Writer.Error("The file doesn't exist in this directory");
                return;
            }

            var fileShareAction = new FileStopShareAction(RepositoryFactory.Create<SharedFileRepository>(), fileToShare, user);
            fileShareAction.Open();
        }

        public void OpenFolderStopShareAction(ICollection<Folder> currentFolders, string folderName, User user)
        {
            var folderToShare = DiskUtils.GetFolderByName(currentFolders, folderName);

            if (folderToShare == null)
            {
                Writer.Error("The folder doesn't exist in this directory");
                return;
            }

            var folderShareAction = new FolderStopShareAction(RepositoryFactory.Create<SharedFolderRepository>(), RepositoryFactory.Create<FolderRepository>(), RepositoryFactory.Create<FileRepository>(), folderToShare, user);
            folderShareAction.Open();
        }
    }
}