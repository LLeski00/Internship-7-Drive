using Drive.Data.Entities.Models;
using Drive.Presentation.Helpers;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Account.Settings
{
    public class AccountEditEmailAction : IAction
    {
        public string Name { get; set; } = "Change email";
        private readonly UserRepository _userRepository;
        public int MenuIndex { get; set; }
        public User User { get; set; }

        public AccountEditEmailAction(UserRepository userRepository, User user)
        {
            _userRepository = userRepository;
            User = user;
        }
        public void Open()
        {
            Console.Clear();
            Console.WriteLine($"Your current email: {User.Email}");

            if (!Reader.TryReadEmail("Enter your new email:", out string newEmail))
            {
                Writer.Error("Invalid email!");

                if(Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            User? user = _userRepository.GetByEmail(newEmail);

            if (user != null)
            {
                Writer.Error("There is already a user with that email.");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            var captcha = Writer.GenerateRandomCaptcha();

            Console.WriteLine($"Enter this captcha: {captcha}");

            if (Console.ReadLine() != captcha)
            {
                Writer.Error("Invalid captcha!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            User.Email = newEmail;
            var response = _userRepository.Update(User, User.Id);

            if (response != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with updating the user");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            Console.WriteLine("The email was successfully changed!");
            Console.ReadLine();
        }
    }
}