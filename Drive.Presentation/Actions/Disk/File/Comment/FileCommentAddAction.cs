using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileCommentAddAction : IAction
    {
        private readonly FileCommentRepository _fileCommentRepository;
        public File File { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Add file comment";
        public int MenuIndex { get; set; }

        public FileCommentAddAction(FileCommentRepository fileCommentRepository, File file, User user)
        {
            _fileCommentRepository = fileCommentRepository;
            File = file;
            User = user;
        }

        public void Open()
        {
            Console.WriteLine("Enter the comment:");
            var newComment = Console.ReadLine();

            if (string.IsNullOrEmpty(newComment))
            {
                Writer.Error("The comment cannot be empty!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            var fileComment = new FileComment(newComment, User.Id, File.Id);
            var response = _fileCommentRepository.Add(fileComment);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("Something went wrong with adding the comment.");
                return;
            }

            Console.WriteLine("Comment successfully posted.");
        }
    }
}