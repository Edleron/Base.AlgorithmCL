namespace SingleResponsibilityPrinciple.CorrectUse;

// Yalnızca kullanıcı bilgilerini yönetir
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// Kullanıcıyı veritabanına kaydetme sorumluluğu
public class UserRepository
{
    public void Save(User user)
    {
        Console.WriteLine($"Kullanıcı {user.Name}, {user.Email} veritabanına kaydedildi.");
    }
}
