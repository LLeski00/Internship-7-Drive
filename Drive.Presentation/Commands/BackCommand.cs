using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;
using Drive.Presentation.Helpers;

namespace Drive.Presentation.Commands
{
    public class BackCommand : ICommand
    {
        public string Name { get; set; } = "back";
        public string Description { get; set; } = "Goes back";

        public BackCommand()
        {
        }

        public void Execute()
        {
        }
    }
}