using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Domain.Repositories;
using Drive.Presentation.Extensions;

namespace Drive.Presentation.Actions.Account
{
    public class LoginAction : IAction
    {
        private readonly UserRepository _userRepository;
        public string Name { get; set; } = "Log in";
        public int MenuIndex { get; set; }

        public LoginAction(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void Open()
        {
            User? user = FindUser();
            while (user == null)
            {
                Thread.Sleep(30000);
                bool cont = Reader.DoYouWantToContinue();
                if (cont)
                    user = FindUser();
                else
                    return;
            }

            Console.WriteLine("Successfully logged in.");
            Console.ReadLine();
            UserExtensions.PrintUserActions(user);
        }

        public User? FindUser()
        {
            Console.Clear();
            Reader.TryReadLine("Enter your email", out string email);
            Reader.TryReadLine("Enter your password", out string password);

            User? user = _userRepository.GetByEmailAndPassword(email, password);
            return user;
        }
    }
}