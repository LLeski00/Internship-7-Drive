using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using Drive.Presentation.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Disk
{
    public class FileEditAction : IAction
    {
        public File FileToEdit { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Edit file";
        public int MenuIndex { get; set; }

        public FileEditAction(File fileToEdit, User user)
        {
            FileToEdit = fileToEdit;
            User = user;
        }

        public void Open()
        {
            var newLinesOfText = RunEditor(out var command);

            if (command == null)
                return;

            var commandType = CommandUtils.GetEditCommandFromString(command);
            var commandArguments = string.Join(' ', command.Split(' ').Skip(1));
            commandType.Execute(commandArguments, User, FileToEdit, newLinesOfText);
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
                Writer.PrintLines(linesOfText);

                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Backspace && currentLine > 1)
                {
                    linesOfText.RemoveAt(linesOfText.Count - 1);
                    currentLine--;
                }
                else if (key.KeyChar == ':')
                {
                    command = Console.ReadLine();

                    if (!Enum.TryParse(command, out EditCommand editCommand))
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