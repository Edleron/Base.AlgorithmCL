namespace OpenClosedPrinciple;

class Program
{
    /*
        Open/Closed Principle yani Açık/Kapalı Prensibi, SOLID prensiplerinin ikincisidir:

        "Sınıflar ve metotlar, yeni davranışlara açık (Open) olmalı, ancak mevcut kodun değiştirilmesine kapalı (Closed) olmalıdır."

        Daha basit anlatımla:

        Yeni özellik ekleyebilirsin (Open).
        Mevcut çalışan kodu değiştirmemelisin (Closed).
        Kodunun sürdürülebilir, kolayca genişletilebilir olmasını sağlar.
    */
    static void Main(string[] args)
    {
        CorrectUsed();
        WrongUsed();
    }

    static void CorrectUsed()
    {
        var screenManager = new CorrectUse.Manager.ScreenManager();

        CorrectUse.Interface.IScreen adminScreen      = new CorrectUse.Logic.AdminScreen();
        CorrectUse.Interface.IScreen userScreen       = new CorrectUse.Logic.UserScreen();
        CorrectUse.Interface.IScreen moderatorScreen  = new CorrectUse.Logic.ModeratorScreen();

        screenManager.ShowScreen(adminScreen);      // Admin ekranı gösterildi.
        screenManager.ShowScreen(userScreen);       // Kullanıcı ekranı gösterildi.
        screenManager.ShowScreen(moderatorScreen);  // Moderatör ekranı gösterildi.
    }

    static void WrongUsed()
    {
        var screenManager = new WrongUse.ScreenManager();

        screenManager.ShowScreen(WrongUse.UserRole.Admin);  // Admin ekranı gösterildi.
        screenManager.ShowScreen(WrongUse.UserRole.User);   // Kullanıcı ekranı gösterildi.
    }
}
