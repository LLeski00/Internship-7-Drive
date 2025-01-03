using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileCommentEditAction : IAction
    {
        private readonly FileCommentRepository _fileCommentRepository;
        public FileComment CommentToEdit { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Edit file comment";
        public int MenuIndex { get; set; }

        public FileCommentEditAction(FileCommentRepository fileCommentRepository, FileComment commentToEdit, User user)
        {
            _fileCommentRepository = fileCommentRepository;
            CommentToEdit = commentToEdit;
            User = user;
        }

        public void Open()
        {
            if (CommentToEdit.AuthorId != User.Id)
            {
                Writer.Error("You can't edit other peoples comments!");
                return;
            }

            Console.WriteLine("Enter the comment: ");
            var newComment = Console.ReadLine();

            if (string.IsNullOrEmpty(newComment))
            {
                Writer.Error("Please enter the comment!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            CommentToEdit.Content = newComment;
            var response = _fileCommentRepository.Update(CommentToEdit, CommentToEdit.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("Something went wrong with editing the comment.");
                return;
            }

            Console.WriteLine("Comment successfully deleted.");
        }
    }
}