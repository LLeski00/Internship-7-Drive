using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Actions.Disk
{
    public class MyDiskAction : IAction
    {
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        public User User { get; set; }
        public Folder? Root {  get; set; }

        public string Name { get; set; } = "My disk";
        public int MenuIndex { get; set; }

        public MyDiskAction(FileRepository fileRepository, FolderRepository folderRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            User = user;
            Root = _folderRepository.GetUsersRoot(User);
        }

        public void Open()
        {
            if (Root == null)
            {
                Writer.Error("Error while fetching users root folder.");
                return;
            }

            Console.Clear();
            var currentDirectory = Root;
            var currentFolders = _folderRepository.GetByUser(User, currentDirectory.Id);
            var currentFiles = _fileRepository.GetByUser(User, currentDirectory.Id);

            do
            {
                DiskExtensions.PrintDirectory(currentFolders, currentFiles);
                Reader.ReadCommand(currentDirectory, out var userInput);
                var command = CommandExtensions.GetCommandFromString(userInput);

                if (command == null)
                {
                    Writer.Error("Invalid command. Use 'help' for a list of commands.");
                    continue;
                }

                if (command == Command.exit)
                    break;

                var commandArguments = string.Join(' ', userInput.Split(' ').Skip(1));
                command.Execute(ref currentDirectory, ref currentFolders, ref currentFiles, commandArguments);
            } while (true);
        }
    }
}