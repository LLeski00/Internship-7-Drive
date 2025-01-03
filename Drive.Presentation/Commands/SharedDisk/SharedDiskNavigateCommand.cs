using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands
{
    public class SharedDiskNavigateCommand : ICommand
    {
        public string Name { get; set; } = "navigate";
        public string Description { get; set; } = "Enters navigation mode. Command: navigate, Usage: navigate through the folders with arrow keys (left for back and enter for forward). Press escape to exit navigation mode.";
        private readonly FolderRepository _folderRepository;
        private readonly SharedFolderRepository _sharedFolderRepository;
        private readonly SharedFileRepository _sharedFileRepository;
        public User User { get; set; }

        public SharedDiskNavigateCommand(FolderRepository folderRepository, SharedFolderRepository sharedFolderRepository, SharedFileRepository sharedFileRepository, User user)
        {
            _folderRepository = folderRepository;
            _sharedFolderRepository = sharedFolderRepository;
            _sharedFileRepository = sharedFileRepository;
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
                currentFolders = _sharedFolderRepository.GetFoldersByUser(User, currentDirectory.Id);
                currentFiles = _sharedFileRepository.GetFilesByUser(User, currentDirectory.Id);
                Console.Clear();
                DiskExtensions.PrintDirectoryNavigate(currentFolders, currentFiles, ref currentIndex);

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