﻿using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Presentation.Extensions;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;
using Drive.Domain.Factories;

namespace Drive.Presentation.Commands
{
    public class ShareCommand : ICommand
    {
        public string Name { get; set; } = "share";
        public string Description { get; set; } = "Shares the file or folder in the current directory to another user. Usage: share file 'name.extension' 'usersEmail' or share folder 'name' 'usersEmail'";
        private readonly UserRepository _userRepository;

        public ShareCommand(UserRepository userRepository)
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
                    OpenFileShareAction(currentFiles, commandArgumentsSplit[1], user);
                    break;
                case "folder":
                    OpenFolderShareAction(currentFolders, commandArgumentsSplit[1], user);
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

            if (commandArgumentsSplit[0].ToLower() == "file" && (!DiskExtensions.IsFileNameValid(commandArgumentsSplit[1])))
                return false;
            else if (commandArgumentsSplit[0].ToLower() == "folder" && (!DiskExtensions.IsFolderNameValid(commandArgumentsSplit[1])))
                return false;

            return true;
        }

        public void OpenFileShareAction(ICollection<File> currentFiles, string file, User user)
        {
            var fileSplitByDot = file.Split('.');

            var fileToShare = DiskExtensions.GetFileByName(currentFiles, fileSplitByDot[0], fileSplitByDot[1]);

            if (fileToShare == null)
            {
                Writer.Error("The file doesn't exist in this directory");
                return;
            }

            var fileShareAction = new FileShareAction(RepositoryFactory.Create<SharedFileRepository>(), fileToShare, user);
            fileShareAction.Open();
        }

        public void OpenFolderShareAction(ICollection<Folder> currentFolders, string folderName, User user)
        {
            var folderToShare = DiskExtensions.GetFolderByName(currentFolders, folderName);

            if (folderToShare == null)
            {
                Writer.Error("The folder doesn't exist in this directory");
                return;
            }

            var folderShareAction = new FolderShareAction(RepositoryFactory.Create<FolderRepository>(), RepositoryFactory.Create<FileRepository>(), RepositoryFactory.Create<SharedFolderRepository>(), folderToShare, user);
            folderShareAction.Open();
        }
    }
}