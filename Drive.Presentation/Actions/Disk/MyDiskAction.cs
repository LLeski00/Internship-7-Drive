using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Extensions;
using Drive.Domain.Repositories;

namespace Drive.Presentation.Actions.Disk
{
    public class MyDiskAction : IAction
    {
        private readonly UserRepository _userRepository;
        public User User { get; set; }
        public string Name { get; set; } = "My disk";
        public int MenuIndex { get; set; }

        public MyDiskAction(UserRepository userRepository, User user)
        {
            _userRepository = userRepository;
            User = user;
        }

        public void Open()
        {
            UserExtensions.PrintUsersDisk(User);
        }
    }
}