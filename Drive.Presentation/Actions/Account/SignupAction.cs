using Drive.Data.Entities.Models;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using Drive.Domain.Repositories;
using Drive.Domain.Enums;
using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Account
{
    public class SignupAction : IAction
    {
        public string Name { get; set; } = "Sign up";
        private readonly UserRepository _userRepository;
        private readonly FolderRepository _folderRepository;
        public int MenuIndex { get; set; }

        public SignupAction(UserRepository userRepository, FolderRepository folderRepository)
        {
            _userRepository = userRepository;
            _folderRepository = folderRepository;
        }
        public void Open()
        {
            if (!Reader.TryReadName("Enter your first name:", out string firstName))
            {
                Writer.Error("Invalid first name!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            if (!Reader.TryReadName("Enter your last name:", out string lastName))
            {
                Writer.Error("Invalid first name!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            if (!Reader.TryReadEmail("Enter your email:", out string email))
            {
                Writer.Error("Invalid email!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            User? user = _userRepository.GetByEmail(email);

            if (user != null)
            {
                Writer.Error("There is already a user with that email.");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            if (!Reader.TryReadNewPassword("Enter your password:", out string password))
            {
                Writer.Error("Invalid password!");

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

            var newUser = new User(email, password, firstName, lastName);
            var responseResult = _userRepository.Add(newUser);

            if (responseResult != ResponseResultType.Success)
            {
                Writer.Error("ERROR: There was an issue with adding the user!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            Console.WriteLine("The user was successfully added!");

            var rootFolder = new Folder("root", newUser.Id, null);
            responseResult = _folderRepository.Add(rootFolder);

            if (responseResult != ResponseResultType.Success)
            {
                Writer.Error("ERROR: There was an issue with adding the users root folder!");

                if (Reader.PromptUserConfirmation())
                    Open();

                return;
            }

            Console.WriteLine("The users root folder was successfully added!");
            Console.ReadLine();
        }
    }
}