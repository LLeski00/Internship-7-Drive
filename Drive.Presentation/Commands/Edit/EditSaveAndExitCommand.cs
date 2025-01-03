using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Edit
{
    public class EditSaveAndExitCommand : IEditCommand
    {
        public string Name { get; set; } = "saveExit";
        public string Description { get; set; } = "Saves the file before exit. Usage: SaveAndExit";
        public File FileToEdit { get; set; }
        public List<string>? NewLinesOfText { get; set; }

        public EditSaveAndExitCommand(File fileToEdit, List<string>? newLinesOfText)
        {
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

            var fileSaveAction = new FileSaveAction(RepositoryFactory.Create<FileRepository>(), FileToEdit, NewLinesOfText);
            fileSaveAction.Open();
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


