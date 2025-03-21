namespace  DependecyInversionPrinciple;

class Program
{

    /*
        DIP, SOLID prensiplerinin beşincisi ve sonuncusudur. Açıklaması şöyledir:

        "Yüksek seviye modüller, düşük seviye modüllere bağımlı olmamalıdır; her ikisi de soyutlamalara (abstraction/interface) bağımlı olmalıdır."

        Daha basit bir anlatımla:

        Üst seviye (iş mantığı) kodlar, alt seviye (detaylar, teknik işler) kodlara değil, interfacelere (soyutlamalara) bağımlı olmalıdır.
        Interface'ler, implementation detaylarından bağımsızdır. Bu yüzden her zaman interface'lere bağımlı olursun.
    */
    static void Main(string[] args)
    {
        CorrectUsed();
        WrongUsed();
    }

    static void CorrectUsed()
    {
        CorrectUse.Interface.IDatabase database = new CorrectUse.Base.MySQLDatabase(); // veya MongoDatabase()
        CorrectUse.User.UserManager userManager = new CorrectUse.User.UserManager(database);

        userManager.SaveUser("Ertuğrul");
    }

    static void WrongUsed()
    {
        WrongUse.User.UserManager userManager = new WrongUse.User.UserManager();

        userManager.SaveUser("Ertuğrul");
    }
}
