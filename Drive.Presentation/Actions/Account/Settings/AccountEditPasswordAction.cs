using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;

namespace Drive.Presentation.Actions.Account.Settings
{
    public class AccountEditPasswordAction : IAction
    {
        public string Name { get; set; } = "Change password";
        public int MenuIndex { get; set; }
        UserRepository _userRepository {  get; set; }
        public User User { get; set; }

        public AccountEditPasswordAction(UserRepository userRepository, User user)
        {
            _userRepository = userRepository;
            User = user;
        }
        public void Open()
        {
            Console.Clear();
            Console.WriteLine("Enter your current password:");

            if (Console.ReadLine() != User.Password)
            {
                Writer.Error("Incorrect password!");
                Open(); //Do you want to continue
                return;
            
            }

            if (!Reader.TryReadNewPassword("Enter your new password:", out string newPassword))
            {
                Writer.Error("Invalid password!");
                Open(); //Do you want to continue
                return;
            }

            User.Password = newPassword;
            var response = _userRepository.Update(User, User.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with updating the user");
                //Do you want to continue
                return;
            }

            Console.WriteLine("The password was successfully changed!");
            Console.ReadLine();
        }
    }
}