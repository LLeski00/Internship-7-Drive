using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileStopShareAction : IAction
    {
        private readonly SharedFileRepository _sharedFileRepository;
        public File SharedFile { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Stop sharing the file with another user";
        public int MenuIndex { get; set; }

        public FileStopShareAction(SharedFileRepository sharedFileRepository, File sharedFile, User user)
        {
            _sharedFileRepository = sharedFileRepository;
            SharedFile = sharedFile;
            User = user;
        }

        public void Open()
        {
            var response = _sharedFileRepository.Delete(SharedFile.Id, User.Id);

            switch (response)
            {
                case ResponseResultType.Success:
                    Console.WriteLine($"{SharedFile.Name}.{SharedFile.Extension} successfully stopped sharing to {User.Email}.");
                    break;
                case ResponseResultType.NotFound:
                    Console.WriteLine("The file is not shared with the user!");
                    break;
                default:
                    Writer.Error("ERROR: Something went wrong with stopping the share of the file.");
                    break;
            }
        }
    }
}