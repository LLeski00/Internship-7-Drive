using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileCommentAddAction : IAction
    {
        private readonly FileCommentRepository _fileCommentRepository;
        public FileComment FileComment { get; set; }

        public string Name { get; set; } = "Add file comment";
        public int MenuIndex { get; set; }

        public FileCommentAddAction(FileCommentRepository fileCommentRepository, FileComment fileComment)
        {
            _fileCommentRepository = fileCommentRepository;
            FileComment = fileComment;
        }

        public void Open()
        {
            var response = _fileCommentRepository.Add(FileComment);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("Something went wrong with adding the comment.");
                return;
            }

            Console.WriteLine("Comment successfully posted.");
        }
    }
}