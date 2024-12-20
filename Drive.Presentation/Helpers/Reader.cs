namespace Drive.Presentation.Helpers;

public static class Reader
{
    public static bool TryReadNumber(out int number)
    {
        number = 0;
        var isNumber = int.TryParse(Console.ReadLine(), out var inputNumber);
        if (!isNumber)
            return false;

        number = inputNumber;
        return true;
    }

    public static bool TryReadNumber(string message, out int number)
    {
        Console.WriteLine(message);
        return TryReadNumber(out number);
    }

    public static bool TryReadDateTime(string message, out DateTime date)
    {
        date = DateTime.UtcNow;

        Console.WriteLine(message);
        var input = Console.ReadLine();

        var isValidInput = int.TryParse(input, out var number);
        if (isValidInput)
            date = date.AddDays(number);

        return isValidInput;
    }

    public static bool TryReadLine(string message, out string line)
    {
        line = string.Empty;

        Console.WriteLine(message);
        var input = Console.ReadLine();
        var isEmpty = string.IsNullOrWhiteSpace(input);

        if (!isEmpty && input is not null)
            line = input;

        return !isEmpty;
    }

    public static bool TryReadName(out string name)
    {
        name = Console.ReadLine() ?? string.Empty;
        if (name == string.Empty || !name.All(char.IsLetter) || char.IsLower(name[0]))
            return false;

        return true;
    }

    public static bool TryReadName(string message, out string name)
    {
        Console.WriteLine(message);
        return TryReadName(out name);
    }

    public static bool TryReadEmail(string message, out string email)
    {
        Console.WriteLine(message);
        email = Console.ReadLine() ?? string.Empty;

        if (email == string.Empty)
            return false;

        string[] inputSplitByMonkey = email.Split('@');

        if (inputSplitByMonkey.Length !=2 || inputSplitByMonkey[0].Length<1)
            return false;
        
        string[] inputSplitByTheDot = inputSplitByMonkey[1].Split(".");

        if (inputSplitByTheDot.Length != 2 || inputSplitByTheDot[0].Length<2 || inputSplitByTheDot[1].Length < 3)
            return false;

        return true;
    }

    public static bool TryReadNewPassword(string message, out string password)
    {
        Console.WriteLine(message);
        password = Console.ReadLine() ?? string.Empty;

        if (password == string.Empty)
            return false;

        Console.WriteLine("Repeat the password:");

        var repeatedPassword = Console.ReadLine();

        if ( repeatedPassword is null || repeatedPassword != password)
            return false;

        return true;
    }

    public static void ReadInput(string message, out string input)
    {
        Console.WriteLine(message);
        input = Console.ReadLine() ?? string.Empty;
    }

    public static bool DoYouWantToContinue()
    {
        Console.WriteLine("If you want to go back to previous page press y");
        var input = Console.ReadLine();
        if (input == "y")
            return false;
        return true;
    }
}