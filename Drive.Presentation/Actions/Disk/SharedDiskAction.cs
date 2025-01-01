using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Enums;

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

            var currentFolders = _sharedFolderRepository.GetFoldersFromRootByUser(User);
            var currentFiles = _sharedFileRepository.GetFilesFromRootByUser(User);

            Console.Clear();
            ProcessUserCommands(root, currentFolders, currentFiles, User);
        }

        public void ProcessUserCommands(Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, User user)
        {
            do
            {
                DiskExtensions.PrintDirectory(currentFolders, currentFiles);
                Reader.ReadCommand(currentDirectory, out var userInput);
                var command = CommandExtensions.GetCommandFromString(userInput);

                if (!IsCommandValid(command))
                {
                    Writer.Error("Invalid command. Use 'help' for a list of commands.");
                    continue;
                }

                if (userInput == Command.exit.ToString())
                    break;

                var commandArguments = string.Join(' ', userInput.Split(' ').Skip(1));
                command.SharedExecute(ref currentDirectory, ref currentFolders, ref currentFiles, commandArguments, user);
                currentFolders = _sharedFolderRepository.GetFoldersByUser(User, currentDirectory.Id);
                currentFiles = _sharedFileRepository.GetFilesByUser(User, currentDirectory.Id);
            } while (true);
        }

        public bool IsCommandValid(Command? command)
        {
            if (command == null || command == Command.create)
                return false;

            return true;
        }
    }
}