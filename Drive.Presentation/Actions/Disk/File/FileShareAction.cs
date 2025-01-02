using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Data.Entities.Models;

namespace Drive.Presentation.Actions.Disk
{
    public class FileShareAction : IAction
    {
        private readonly SharedFileRepository _sharedFileRepository;
        public File FileToShare { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Share file with another user";
        public int MenuIndex { get; set; }

        public FileShareAction(SharedFileRepository sharedFileRepository, File fileToShare, User user)
        {
            _sharedFileRepository = sharedFileRepository;
            FileToShare = fileToShare;
            User = user;
        }

        public void Open()
        {
            var sharedFile = new SharedFile(User.Id, FileToShare.Id);
            var response = _sharedFileRepository.Add(sharedFile);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with renaming the file.");
                return;
            }

            Console.WriteLine($"{FileToShare.Name}.{FileToShare.Extension} successfully shared.");
        }
    }
}