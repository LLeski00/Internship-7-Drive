using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Commands
{
    public class EditExitCommand : IEditCommand
    {
        public string Name { get; set; } = "exit";
        public string Description { get; set; } = "Exits the editor. Usage: exit";

        public EditExitCommand()
        {
        }

        public void Execute(string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }
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