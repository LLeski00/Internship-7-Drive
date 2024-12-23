using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;
using Drive.Presentation.Factories;

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
            DiskExtensions.PrintDirectory(currentFolders, currentFiles);

            do
            {
                Reader.ReadCommand(currentDirectory, out var userInput);
                var command = CommandExtensions.GetCommandFromString(userInput);

                if (command == null)
                {
                    Writer.Error("Invalid command. Use 'help' for a list of commands.");
                    continue;
                }

                command.Execute();
            } while (true);
        }
    }
}