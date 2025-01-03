using Drive.Presentation.Abstractions.Actions;

namespace Drive.Presentation.Actions.Account
{
    public class LogoutAction : IAction
    {
        public string Name { get; set; } = "Log out";
        public int MenuIndex { get; set; }

        public LogoutAction()
        {
        }

        public void Open()
        {
            Console.WriteLine("You have been logged out.");
            Console.ReadLine();
        }
    }
}