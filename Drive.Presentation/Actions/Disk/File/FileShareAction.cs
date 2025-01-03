using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Actions;

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
            if (FileToShare.OwnerId ==  User.Id)
            {
                Writer.Error("You cannot share the file with yourself!");
                return;
            }

            if (_sharedFileRepository.GetAllFilesByUser(User).Any(f => f.Id == FileToShare.Id))
            {
                Writer.Error("The file is already shared with this user!");
                return;
            }

            var sharedFile = new SharedFile(User.Id, FileToShare.Id);
            var response = _sharedFileRepository.Add(sharedFile);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with sharing the file.");
                return;
            }

            Console.WriteLine($"{FileToShare.Name}.{FileToShare.Extension} successfully shared.");
        }
    }
}