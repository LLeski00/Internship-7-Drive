using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileSaveAction : IAction
    {
        private readonly FileRepository _fileRepository;
        public File FileToEdit { get; set; }
        public List<string>? NewLinesOfText { get; set; }

        public string Name { get; set; } = "Save file";
        public int MenuIndex { get; set; }

        public FileSaveAction(FileRepository fileRepository, File fileToEdit, List<string>? newLinesOfText)
        {
            _fileRepository = fileRepository;
            FileToEdit = fileToEdit;
            NewLinesOfText = newLinesOfText;
        }

        public void Open()
        {
            if (NewLinesOfText == null)
                return;

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to edit this file?"))
                return;

            var content = string.Join('\n', NewLinesOfText);
            var response = _fileRepository.EditContent(content, FileToEdit.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with editing the file.");
                return;
            }

            Console.WriteLine("The file was successfully edited.");
        }
    }
}