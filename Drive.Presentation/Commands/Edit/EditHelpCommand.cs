using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Edit
{
    public class EditHelpCommand : IEditCommand
    {
        public string Name { get; set; } = "help";
        public string Description { get; set; } = "Lists all edit commands. Usage: help";
        public User User { get; set; }
        public File FileToEdit { get; set; }
        public List<string>? NewLinesOfText { get; set; }

        public EditHelpCommand(User user, File fileToEdit, List<string>? newLinesOfText)
        {
            User=user;
            FileToEdit=fileToEdit;
            NewLinesOfText=newLinesOfText;
        }

        public void Execute(string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            Console.WriteLine("All commands:");
            CommandExtensions.PrintAllEditCommands(User, FileToEdit, NewLinesOfText);
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (!string.IsNullOrEmpty(commandArguments))
            {
                return false;
            }

            return true;
        }
    }
}