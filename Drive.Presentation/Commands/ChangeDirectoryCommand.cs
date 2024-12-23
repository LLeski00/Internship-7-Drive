using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Commands
{
    public class ChangeDirectoryCommand : ICommand
    {
        public string Name { get; set; } = "cd";
        public string Description { get; set; } = "Changes current directory. Usage: cd 'path' ";

        public ChangeDirectoryCommand()
        {
        }

        public void Execute()
        {
            
        }
    }
}