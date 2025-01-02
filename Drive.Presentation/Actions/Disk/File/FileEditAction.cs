using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
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
        public User User { get; set; }

        public string Name { get; set; } = "Edit file";
        public int MenuIndex { get; set; }

        public FileEditAction(FileRepository fileRepository, File fileToEdit, User user)
        {
            _fileRepository = fileRepository;
            FileToEdit = fileToEdit;
            User = user;
        }

        //NEEDS REFACTORING

        public void Open()
        {
            var newLinesOfText = RunEditor(out var command);

            if (command == null)
                return;

            var commandType = CommandExtensions.GetEditCommandFromString(command);
            var commandArguments = string.Join(' ', command.Split(' ').Skip(1));
            commandType.Execute(commandArguments, User, FileToEdit, newLinesOfText);
        }

        //Could be in command extensions
        public ResponseResultType? ReadCommand(string? input)
        {
            if (!Enum.TryParse(input, out EditCommand command))
                return ResponseResultType.NotFound;
            else
                return ResponseResultType.Success;
        }

        public List<string>? RunEditor(out string? command)
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
                    command = Console.ReadLine();

                    if (ReadCommand(command) != ResponseResultType.Success)
                        Writer.Error("Invalid edit command! Type 'help' for a list of commands.");
                    else
                        break;
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

            return linesOfText;
        }
    }
}