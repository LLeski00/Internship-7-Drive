using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Utils;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.SharedDisk
{
    public class CommentHelpCommand : ICommentCommand
    {
        public string Name { get; set; } = "help";
        public string Description { get; set; } = "Lists all comment commands. Usage: help";
        public User User { get; set; }
        public File File { get; set; }

        public CommentHelpCommand(User user, File file)
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

            Console.WriteLine("All commands:");
            CommandUtils.PrintAllCommentCommands(User, File);
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