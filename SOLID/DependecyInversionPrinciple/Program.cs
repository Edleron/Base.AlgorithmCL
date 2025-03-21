namespace  DependecyInversionPrinciple;

class Program
{

    /*
        # Dependency Inversion Principle (DIP) - Bağımlılıkların Terslenmesi Prensibi
        DIP, SOLID prensiplerinin beşincisi ve sonuncusudur. Açıklaması şöyledir:

        "Yüksek seviye modüller, düşük seviye modüllere bağımlı olmamalıdır; her ikisi de soyutlamalara (abstraction/interface) bağımlı olmalıdır."

        Daha basit bir anlatımla:

        Üst seviye (iş mantığı) kodlar, alt seviye (detaylar, teknik işler) kodlara değil, interfacelere (soyutlamalara) bağımlı olmalıdır.
        Interface'ler, implementation detaylarından bağımsızdır. Bu yüzden her zaman interface'lere bağımlı olursun.

        🔸 Dependency Injection (DI) - 

        DIP prensibini uygulamak için kullanılan bir tekniktir (design pattern).
        Bağımlılıklar dışarıdan verilir (constructor, metot veya property injection gibi).
        Nasıl yapacağını söyler:
        "Sınıf içinde bağımlılıkları oluşturma. Onları dışarıdan (örneğin constructor üzerinden) al."

        // Description
        Aslında DIP ve DI sayesinde bağımlılıklar en fazla esneklik kazanır:
        Bağımlılıklar soyutlamaya bağlı olduğundan, istediğin zaman gerçek sınıfı değiştirebilirsin.
        Bu yöntem test yazmayı kolaylaştırır.
        Kod daha modüler ve genişletilebilir olur.

        Özetle:
        DIP prensip olarak "soyutlama kullan" der.
        DI ise "bu bağımlılıkları nasıl sağlayacağını" açıklar (constructor vb.).
        Bu yaklaşım projene müthiş bir esneklik sağlar.

        * Kendime Not, DIP ile DI arasındaki farkı şu şekilde özetleyebilirim:
        DIP: Bağımlılıkların soyutlamalara bağlı olması gerektiğini söyler.
        DI: Bu bağımlılıkları nasıl sağlayacağını söyler (constructor, metot, property injection gibi).
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
        
        CorrectUse.Interface.ILogger logger     = new CorrectUse.Logic.FileLogger(); 
        CorrectUse.User.AIManager aiManager     = new CorrectUse.User.AIManager(logger); 


        userManager.SaveUser("Ertuğrul");
        aiManager.SaveAI("AI-1");
    }

    static void WrongUsed()
    {
        WrongUse.User.UserManager userManager = new WrongUse.User.UserManager();
        userManager.SaveUser("Ertuğrul");
    }
}
