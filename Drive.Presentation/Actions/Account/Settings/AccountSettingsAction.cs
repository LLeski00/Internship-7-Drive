using Drive.Presentation.Abstractions;

namespace Drive.Presentation.Actions.Account.Settings;

public class AccountSettingsAction : IBaseMenuAction
{
    public AccountSettingsAction(IList<IAction> actions) : base(actions)
    {
        Name = "Account settings";
    }

    public override void Open()
    {
        Console.WriteLine(Name);
        base.Open();
    }
}