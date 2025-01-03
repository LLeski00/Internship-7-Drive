using Drive.Presentation.Abstractions;
using Drive.Data.Entities.Models;
using Drive.Presentation.Utils;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using Drive.Domain.Enums;

namespace Drive.Presentation.Actions.Disk
{
    public class FileCommentsAction : IAction
    {
        public File File { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Enters file comments";
        public int MenuIndex { get; set; }

        public FileCommentsAction(File file, User user)
        {
            File = file;
            User = user;
        }

        public void Open()
        {
            Console.Clear();

            do
            {
                CommentUtils.PrintAllFileComments(File);
                Console.Write(">>");
                var userInput = Console.ReadLine();
                var command = CommandExtensions.GetCommentCommandFromString(userInput);

                if (command == null || userInput == null)
                {
                    Writer.Error("Invalid command. Type 'help' for a list of commands");
                    continue;
                }

                if (command == CommentCommand.exit)
                    break;

                var commandArguments = string.Join(' ', userInput.Split(' ').Skip(1));
                command.Execute(commandArguments, User, File);
            } while (true);
        }
    }
}