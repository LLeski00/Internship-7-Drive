using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Comment
{
    public class CommentDeleteCommand : ICommentCommand
    {
        public string Name { get; set; } = "delete";
        public string Description { get; set; } = "Deletes the comment. Usage: delete 'id'";
        private readonly FileCommentRepository _fileCommentRepository;
        public User User { get; set; }
        public File File { get; set; }

        public CommentDeleteCommand(FileCommentRepository fileCommentRepository, User user, File file)
        {
            _fileCommentRepository = fileCommentRepository;
            User = user;
            File=file;
        }

        public void Execute(string? commandArguments)
        {
            if (commandArguments == null || !IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            if (!int.TryParse(commandArguments, out var commentId))
            {
                Writer.Error("Id must be a number!");
                return;
            }

            var commentToDelete = _fileCommentRepository.GetByIdInFile(File, commentId);

            if (commentToDelete == null)
            {
                Writer.Error("The comment was not found.");
                return;
            }

            var fileCommentDeleteAction = new FileCommentDeleteAction(RepositoryFactory.Create<FileCommentRepository>(), commentToDelete, User);
            fileCommentDeleteAction.Open();
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (string.IsNullOrEmpty(commandArguments))
                return false;

            var argumentsSplit = commandArguments.Split(' ');

            if (argumentsSplit.Length != 1)
                return false;

            return true;
        }
    }
}


