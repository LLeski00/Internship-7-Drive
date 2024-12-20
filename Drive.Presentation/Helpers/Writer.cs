﻿using Drive.Data.Entities.Models;

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

    public static string GenerateRandomCaptcha()
    {
        var randomString = Guid.NewGuid().ToString("N").Substring(0, 5);
        
        return randomString;
    }
}