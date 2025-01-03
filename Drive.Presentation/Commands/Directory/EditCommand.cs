using Drive.Data.Entities.Models;
using Drive.Presentation.Helpers;
using Drive.Presentation.Utils;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Abstractions.Commands;

namespace Drive.Presentation.Commands.Directory
{
    public class EditCommand : IDirectoryCommand
    {
        public string Name { get; set; } = "edit";
        public string Description { get; set; } = "Opens the editor of the file in the current directory. Usage: edit 'name.extension'";
        public User User { get; set; }

        public EditCommand(User user)
        {
            User = user;
        }

        public void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments)
        {
            if (commandArguments == null || !IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var commandArgumentsSplit = commandArguments.Split('.');
            var fileName = commandArgumentsSplit[0];
            var fileExtension = commandArgumentsSplit[1];

            var fileToEdit = currentFiles.FirstOrDefault(f => f.Name == fileName && f.Extension == fileExtension);

            if (fileToEdit == null)
            {
                Writer.Error("File with that name doesn't exist in this folder!");
                return;
            }

            var fileEditAction = new FileEditAction(fileToEdit, User);
            fileEditAction.Open();
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (string.IsNullOrEmpty(commandArguments))
                return false;

            if (commandArguments.Contains(' ') || !DiskUtils.IsFileNameValid(commandArguments))
                return false;

            return true;
        }
    }
}