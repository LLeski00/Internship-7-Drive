using Drive.Data.Entities.Models;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using Drive.Presentation.Enums;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class SharedDiskAction : IAction
    {
        private readonly FolderRepository _folderRepository;
        private readonly SharedFileRepository _sharedFileRepository;
        private readonly SharedFolderRepository _sharedFolderRepository;
        public User User { get; set; }

        public string Name { get; set; } = "Shared disk";
        public int MenuIndex { get; set; }

        public SharedDiskAction(FolderRepository folderRepository, SharedFileRepository sharedFileRepository, SharedFolderRepository sharedFolderRepository, User user)
        {
            _folderRepository = folderRepository;
            _sharedFileRepository = sharedFileRepository;
            _sharedFolderRepository = sharedFolderRepository;
            User = user;
        }

        public void Open()
        {
            var root = _folderRepository.GetUsersRoot(User);

            if (root == null)
            {
                Writer.Error("Error while fetching users root folder.");
                return;
            }

            Console.Clear();
            ProcessUserCommands(root, User);
        }

        public void ProcessUserCommands(Folder currentDirectory, User user)
        {
            do
            {
                if (!_sharedFolderRepository.GetUsersByFolderId(currentDirectory.Id).Any(u => u.Id == user.Id))
                {
                    var root = _folderRepository.GetUsersRoot(User);

                    if (root == null)
                    {
                        Writer.Error("Error while fetching users root folder.");
                        break;
                    }

                    currentDirectory = root;
                }

                var currentFolders = _sharedFolderRepository.GetFoldersByUser(User, currentDirectory.Id);
                var currentFiles = _sharedFileRepository.GetFilesByUser(User, currentDirectory.Id);
                DiskUtils.PrintDirectory(currentFolders, currentFiles);
                Reader.ReadCommand(currentDirectory, out var userInput);
                var command = CommandUtils.GetSharedDiskCommandFromString(userInput);

                if (command == null)
                {
                    Writer.Error("Invalid command. Use 'help' for a list of commands.");
                    continue;
                }

                if (userInput == SharedDiskCommand.exit.ToString())
                    break;

                var commandArguments = string.Join(' ', userInput.Split(' ').Skip(1));
                command.Execute(ref currentDirectory, currentFolders, currentFiles, commandArguments, user);
            } while (true);
        }
    }
}