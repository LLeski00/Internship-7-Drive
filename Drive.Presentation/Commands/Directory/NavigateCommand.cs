using Drive.Data.Entities.Models;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Utils;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Directory
{
    public class NavigateCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "navigate";
        public string Description { get; set; } = "Enters navigation mode. Usage: navigate, Controls: Navigate through the folders with arrow keys (left for back and enter for forward). Press escape to exit navigation mode.";
        private readonly FileRepository _fileRepository;
        private readonly FolderRepository _folderRepository;
        public User User { get; set; }

        public NavigateCommand(FileRepository fileRepository, FolderRepository folderRepository, User user)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            User = user;
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var currentIndex = 0;

            do
            {
                currentFolders = _folderRepository.GetByUser(User, currentDirectory.Id);
                currentFiles = _fileRepository.GetByUser(User, currentDirectory.Id);
                Console.Clear();
                DiskUtils.PrintDirectoryNavigate(currentFolders, currentFiles, ref currentIndex);

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        GoBack(ref currentDirectory);
                        currentIndex = 0;
                        break;
                    case ConsoleKey.UpArrow:
                        currentIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        currentIndex++;
                        break;
                    case ConsoleKey.Enter:
                        EnterDirectory(ref currentDirectory, currentFolders, currentIndex);
                        currentIndex = 0;
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        return;
                    default:
                        break;
                }
            } while (true);
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (!string.IsNullOrEmpty(commandArguments))
            {
                return false;
            }

            return true;
        }

        public void GoBack(ref Folder currentDirectory)
        {
            if (currentDirectory.ParentFolderId == null)
                return;

            var newCurrentDirectory = _folderRepository.GetById(currentDirectory.ParentFolderId);

            if (newCurrentDirectory == null)
                return;

            currentDirectory = newCurrentDirectory;
        }

        public void EnterDirectory(ref Folder currentDirectory, ICollection<Folder> currentFolders, int currentIndex)
        {
            if (currentIndex >= currentFolders.Count)
                return;

            var newCurrentDirectory = currentFolders.OrderBy(cf => cf.Name).ToList()[currentIndex];

            currentDirectory = newCurrentDirectory;
        }
    }
}