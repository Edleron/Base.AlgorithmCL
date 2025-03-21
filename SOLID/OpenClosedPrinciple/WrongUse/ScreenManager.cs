namespace OpenClosedPrinciple.WrongUse;

public enum UserRole
{
    Admin,
    User
}

public class ScreenManager
{
    public void ShowScreen(UserRole role)
    {
        if (role == UserRole.Admin)
            Console.WriteLine("Admin ekranı gösterildi.");
        else if (role == UserRole.User)
            Console.WriteLine("Kullanıcı ekranı gösterildi.");
    }
}
