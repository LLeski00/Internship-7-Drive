using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileEditAction : IAction
    {
        private readonly FileRepository _fileRepository;
        public File FileToEdit { get; set; }

        public string Name { get; set; } = "Edit file";
        public int MenuIndex { get; set; }

        public FileEditAction(FileRepository fileRepository, File fileToEdit)
        {
            _fileRepository = fileRepository;
            FileToEdit = fileToEdit;
        }

        //NEEDS REFACTORING

        public void Open()
        {
            var newLinesOfText = RunEditor();

            if (newLinesOfText == null)
                return;

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to edit this file?"))
                return;

            var content = string.Join('\n', newLinesOfText);
            FileToEdit.Content = content;
            FileToEdit.LastChanged = DateTime.UtcNow;
            var response = _fileRepository.EditContent(content, FileToEdit.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with editing the file.");
                return;
            }
        }

        public ResponseResultType? ReadCommand(string? input)
        {
            if (!Enum.TryParse(input, out EditCommand command))
                return ResponseResultType.NotFound;

            switch (command)
            {
                case EditCommand.help:
                    return null;
                case EditCommand.saveExit:
                    return ResponseResultType.Success;
                case EditCommand.exit:
                    return ResponseResultType.NoChanges;
                default:
                    return ResponseResultType.NotFound;
            }
        }

        public List<string>? RunEditor()
        {

            var linesOfText = new List<string>();

            if (FileToEdit.Content != null)
                linesOfText = FileToEdit.Content.Split('\n').ToList();

            var currentLine = linesOfText.Count + 1;

            do
            {
                Console.Clear();
                Writer.PrintLines(linesOfText.Take(linesOfText.Count).ToList());

                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Backspace && currentLine > 1)
                {
                    linesOfText.RemoveAt(linesOfText.Count - 1);
                    currentLine--;
                }
                else if (key.KeyChar == ':')
                {
                    var command = Console.ReadLine();

                    if (ReadCommand(command) == ResponseResultType.NotFound)
                        Writer.Error("Invalid edit command! Type 'help' for a list of commands.");
                    else if (ReadCommand(command) == ResponseResultType.Success)
                        return linesOfText;
                    else if (ReadCommand(command) == ResponseResultType.NoChanges)
                        return null;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    linesOfText.Add(string.Empty);
                    currentLine++;
                }
                else
                {
                    var line = Console.ReadLine();
                    line = key.KeyChar + line;
                    linesOfText.Add(line ?? string.Empty);
                    currentLine++;
                }
            } while (true);
        }
    }
}