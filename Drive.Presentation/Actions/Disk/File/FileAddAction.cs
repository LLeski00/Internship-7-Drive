using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Abstractions.Actions;
using Drive.Domain.Factories;
using Drive.Presentation.Utils;

namespace Drive.Presentation.Actions.Disk
{
    public class FileAddAction : IAction
    {
        private readonly FileRepository _fileRepository; 
        private readonly SharedFolderRepository _sharedFolderRepository;
        public File FileToAdd { get; set; }

        public string Name { get; set; } = "Add file";
        public int MenuIndex { get; set; }

        public FileAddAction(FileRepository fileRepository, SharedFolderRepository sharedfolderRepository, File fileToAdd)
        {
            _fileRepository = fileRepository;
            _sharedFolderRepository = sharedfolderRepository;
            FileToAdd = fileToAdd;
        }

        public void Open()
        {
            if (!UserUtils.ConfirmUserAction("Are you sure you want to add this file?"))
                return;

            var fileResponse = _fileRepository.Add(FileToAdd);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the file.");
                return;
            }

            Console.WriteLine("File successfully added.");

            var usersToShare = _sharedFolderRepository.GetUsersByFolderId(FileToAdd.ParentFolderId);

            if (usersToShare.Count == 0)
                return;

            foreach ( var user in usersToShare)
            {
                var fileShareAction = new FileShareAction(RepositoryFactory.Create<SharedFileRepository>(), FileToAdd, user);
                fileShareAction.Open();
            }
        }
    }
}