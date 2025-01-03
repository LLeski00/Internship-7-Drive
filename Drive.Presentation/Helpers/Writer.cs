using Drive.Data.Entities.Models;

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
        var random = new Random();
        var captcha = new[]
        {
            (char)('A' + random.Next(26)),
            (char)('0' + random.Next(10)),
            (char)('A' + random.Next(26)),
            (char)('0' + random.Next(10)),
            (char)('A' + random.Next(26)) 
        };
        return new string(captcha.OrderBy(_ => random.Next()).ToArray());
    }

    public static void PrintLines(List<string> lines)
    {
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }
}