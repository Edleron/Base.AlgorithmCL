namespace SingleResponsibilityPrinciple;

class Program
{
    /* 
        Single Responsibility Principle (SRP) yani Tek Sorumluluk Prensibi, SOLID prensiplerinin ilki ve belki de en önemlisidir.
        Bir class'ın değişmesi için yalnızca tek bir sebep olmalıdır. Yani bir class'ın sadece bir sorumluluğu olmalı ve o sorumluluk doğrultusunda değişmelidir.
    */
    static void Main(string[] args)
    {
        WrongUsed();
        CorrectUsed();
    }

    static void WrongUsed()
    {
        var user = new WrongUse.User { Name = "Ertuğrul", Email = "ertugrul@example.com" };
        user.SaveUser();
        user.SendWelcomeEmail();
    }

    static void CorrectUsed()
    {
        var user = new CorrectUse.User { Name = "Ertuğrul", Email = "ertugrul@example.com" };

        var repository = new CorrectUse.UserRepository();
        repository.Save(user);

        var emailService = new CorrectUse.EmailService();
        emailService.SendWelcomeEmail(user);
    }
}
