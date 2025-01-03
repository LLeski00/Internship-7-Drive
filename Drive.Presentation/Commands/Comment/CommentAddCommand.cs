using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Comment
{
    public class CommentAddCommand : ICommentCommand
    {
        public string Name { get; set; } = "add";
        public string Description { get; set; } = "Posts the comment to the file. Usage: add";
        public User User { get; set; }
        public File File { get; set; }

        public CommentAddCommand(User user, File file)
        {
            User=user;
            File=file;
        }

        public void Execute(string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            Console.WriteLine("Enter the comment:");
            var newComment = Console.ReadLine();

            if (string.IsNullOrEmpty(newComment))
            {
                Writer.Error("The comment cannot be empty!");

                if (Reader.PromptUserConfirmation())
                    Execute(commandArguments);

                return;
            }

            var fileComment = new FileComment(newComment, User.Id, File.Id);

            var fileCommentAddAction = new FileCommentAddAction(RepositoryFactory.Create<FileCommentRepository>(), fileComment);
            fileCommentAddAction.Open();
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (!string.IsNullOrEmpty(commandArguments))
                return false;

            return true;
        }
    }
}


