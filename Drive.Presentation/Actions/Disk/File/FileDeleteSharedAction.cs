using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;
using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileDeleteSharedAction : IAction
    {
        private readonly SharedFileRepository _sharedFileRepository;
        public File FileToDelete { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Delete shared file";
        public int MenuIndex { get; set; }

        public FileDeleteSharedAction(SharedFileRepository sharedFileRepository, File fileToDelete, User user)
        {
            _sharedFileRepository = sharedFileRepository;
            FileToDelete = fileToDelete;
            User = user;
        }

        public void Open()
        {
            var fileResponse = _sharedFileRepository.Delete(FileToDelete.Id, User.Id);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with deleting the file.");
                return;
            }

            Console.WriteLine($"{FileToDelete.Name}.{FileToDelete.Extension} successfully removed from the shared disk.");
        }
    }
}