using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;

namespace Drive.Presentation.Actions.Disk
{
    public class MyDiskAction : IAction
    {
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        public User User { get; set; }

        public string Name { get; set; } = "My disk";
        public int MenuIndex { get; set; }

        public MyDiskAction(FileRepository fileRepository, FolderRepository folderRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
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
                var currentFolders = _folderRepository.GetByUser(User, currentDirectory.Id);
                var currentFiles = _fileRepository.GetByUser(User, currentDirectory.Id);
                DiskExtensions.PrintDirectory(currentFolders, currentFiles);
                Reader.ReadCommand(currentDirectory, out var userInput);
                var command = CommandExtensions.GetCommandFromString(userInput);

                if (command == null)
                {
                    Writer.Error("Invalid command. Use 'help' for a list of commands.");
                    continue;
                }

                if (userInput == Command.exit.ToString())
                    break;

                var commandArguments = string.Join(' ', userInput.Split(' ').Skip(1));
                command.Execute(ref currentDirectory, currentFolders, currentFiles, commandArguments, user);
            } while (true);
        }
    }
}