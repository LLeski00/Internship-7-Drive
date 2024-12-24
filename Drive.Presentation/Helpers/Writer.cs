using Drive.Data.Entities.Models;
using System.Xml.Linq;

namespace Drive.Presentation.Helpers;

public class Writer
{
    public static void Write(User user)
        => Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName}");

    public static void Write(ICollection<User> users)
    {
        foreach (var user in users)
            Write(user);
    }

    public static void Error(string message)
    {
        Console.WriteLine(message);
        Thread.Sleep(1000);
        Console.Clear();
    }

    public static void CommandError(string name, string description)
    {
        Console.WriteLine($"Invalid use of {name}\n\tDescription: {description}\n");
    }

    public static string GenerateRandomCaptcha()
    {
        var randomString = Guid.NewGuid().ToString("N").Substring(0, 5);
        
        return randomString;
    }
}