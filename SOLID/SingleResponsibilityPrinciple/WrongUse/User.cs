namespace SingleResponsibilityPrinciple.WrongUse;

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }

    // Kullanıcı bilgilerini veritabanına kaydeder
    public void SaveUser()
    {
        Console.WriteLine($"Kullanıcı {Name}, {Email} veritabanına kaydedildi.");
    }

    // Kullanıcıya hoş geldin maili gönderir
    public void SendWelcomeEmail()
    {
        Console.WriteLine($"Hoş geldiniz maili {Email} adresine gönderildi.");
    }
}