using System;

namespace DesignPatterns.AbstractFactory
{
    // --- 1. Abstract Products (Soyut Ürünler) ---
    // Tüm silahların ortak arayüzü
    public interface IWeapon
    {
        void Attack();
    }

    // Tüm zırhların ortak arayüzü
    public interface IArmor
    {
        void Defend();
    }

    // --- 2. Concrete Products (Somut Ürünler) ---
    
    // Orta Çağ Ürünleri
    public class Sword : IWeapon
    {
        public void Attack() => Console.WriteLine("Kılıç: Düşmana keskin bir darbe indirdi!");
    }

    public class PlateArmor : IArmor
    {
        public void Defend() => Console.WriteLine("Plaka Zırh: Okları ve kılıç darbelerini engelledi.");
    }

    // Bilim Kurgu Ürünleri
    public class LaserGun : IWeapon
    {
        public void Attack() => Console.WriteLine("Lazer Silahı: 'Pew pew!' Plazma atışı yaptı.");
    }

    public class NanoSuit : IArmor
    {
        public void Defend() => Console.WriteLine("Nano Giysi: Enerji kalkanını devreye soktu.");
    }

    // --- 3. Abstract Factory (Soyut Fabrika Arayüzü) ---
    // Bu arayüz, bir "ürün ailesi" yaratacağını taahhüt eder.
    public interface IGameThemeFactory
    {
        IWeapon CreateWeapon();
        IArmor CreateArmor();
    }

    // --- 4. Concrete Factories (Somut Fabrikalar) ---

    // Orta Çağ Fabrikası: Sadece Orta Çağ ürünleri üretir.
    public class MedievalFactory : IGameThemeFactory
    {
        public IWeapon CreateWeapon()
        {
            return new Sword();
        }

        public IArmor CreateArmor()
        {
            return new PlateArmor();
        }
    }

    // Bilim Kurgu Fabrikası: Sadece Bilim Kurgu ürünleri üretir.
    public class SciFiFactory : IGameThemeFactory
    {
        public IWeapon CreateWeapon()
        {
            return new LaserGun();
        }

        public IArmor CreateArmor()
        {
            return new NanoSuit();
        }
    }

    // --- 5. Client (Müşteri) ---
    // Karakterimiz hangi çağda olduğunu bilmez, sadece fabrikadan ekipman ister.
    public class Hero
    {
        private readonly IWeapon _weapon;
        private readonly IArmor _armor;

        public Hero(IGameThemeFactory factory)
        {
            // Dependency Injection: Fabrikayı dışarıdan alıyoruz.
            _weapon = factory.CreateWeapon();
            _armor = factory.CreateArmor();
        }

        public void RunAction()
        {
            _weapon.Attack();
            _armor.Defend();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Abstract Factory Pattern Demo ---\n");

            // Senaryo 1: Oyun ayarları "Medieval" olarak seçildi.
            Console.WriteLine("[Oyun Modu: Orta Çağ]");
            IGameThemeFactory medievalFactory = new MedievalFactory();
            Hero knight = new Hero(medievalFactory);
            knight.RunAction();

            Console.WriteLine();

            // Senaryo 2: Oyun ayarları "Sci-Fi" olarak seçildi.
            // Hero sınıfında HİÇBİR kod değişikliği yapmadan tamamen farklı nesnelerle çalışıyoruz.
            Console.WriteLine("[Oyun Modu: Bilim Kurgu]");
            IGameThemeFactory sciFiFactory = new SciFiFactory();
            Hero spaceMarine = new Hero(sciFiFactory);
            spaceMarine.RunAction();

            Console.ReadKey();
        }
    }
}