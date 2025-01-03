using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;

namespace Drive.Presentation.Actions.Disk
{
    public class FileCommentDeleteAction : IAction
    {
        private readonly FileCommentRepository _fileCommentRepository;
        public FileComment CommentToDelete { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Delete file comment";
        public int MenuIndex { get; set; }

        public FileCommentDeleteAction(FileCommentRepository fileCommentRepository, FileComment fileComment, User user)
        {
            _fileCommentRepository = fileCommentRepository;
            CommentToDelete = fileComment;
            User = user;
        }

        public void Open()
        {
            if (CommentToDelete.AuthorId != User.Id)
            {
                Writer.Error("You can't delete other peoples comments!");
                return;
            }

            var response = _fileCommentRepository.Delete(CommentToDelete.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("Something went wrong with deleting the comment.");
                return;
            }

            Console.WriteLine("Comment successfully deleted.");
        }
    }
}