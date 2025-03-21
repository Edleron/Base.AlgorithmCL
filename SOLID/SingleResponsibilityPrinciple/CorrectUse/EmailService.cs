namespace SingleResponsibilityPrinciple.CorrectUse;

// Sadece email gönderme işlemlerinden sorumludur.
public class EmailService
{
    public void SendWelcomeEmail(User user)
    {
        Console.WriteLine($"Hoş geldiniz maili {user.Email} adresine gönderildi.");
    }
}
