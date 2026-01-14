using System;

namespace DesignPatterns.Decorator
{
    // --- 1. Component (Bileşen Arayüzü) ---
    // Temel nesne ve süslemelerin (decorators) ortak atası.
    // Client (Oyun mantığı) bu arayüzü kullanır, böylece süslenmiş mi sade mi olduğunu bilmesine gerek kalmaz.
    public interface IWeapon
    {
        string GetDescription();
        double GetDamage();
    }

    // --- 2. Concrete Component (Somut Bileşen) ---
    // Üzerine özellik eklenecek olan "Çıplak" nesne.
    public class BasicSword : IWeapon
    {
        public string GetDescription()
        {
            return "Demir Kılıç";
        }

        public double GetDamage()
        {
            return 10.0; // Temel hasar
        }
    }

    // --- 3. Base Decorator (Temel Bezeyici) ---
    // Diğer tüm süslemeler buradan türeyecek.
    // İçinde "sarılan" nesneyi (wrapee) tutar.
    public abstract class WeaponDecorator : IWeapon
    {
        // Protected, çünkü alt sınıflar (Ateş, Buz) bu referansa erişmeli.
        protected IWeapon _wrappedWeapon;

        public WeaponDecorator(IWeapon weapon)
        {
            _wrappedWeapon = weapon;
        }

        // Varsayılan olarak işi sarılan nesneye devreder.
        // Alt sınıflar bu metotları ezecek (override) ve kendi özelliklerini ekleyecek.
        public virtual string GetDescription()
        {
            return _wrappedWeapon.GetDescription();
        }

        public virtual double GetDamage()
        {
            return _wrappedWeapon.GetDamage();
        }
    }

    // --- 4. Concrete Decorators (Somut Bezeyiciler) ---
    
    // Özellik: Ateş Hasarı
    public class FireEnchantment : WeaponDecorator
    {
        public FireEnchantment(IWeapon weapon) : base(weapon) { }

        public override string GetDescription()
        {
            // Önceki isme "Yanar Dönerli" ekle
            return $"Yanar Dönerli {_wrappedWeapon.GetDescription()}";
        }

        public override double GetDamage()
        {
            // Önceki hasara +5 ekle
            return _wrappedWeapon.GetDamage() + 5.0;
        }
    }

    // Özellik: Buz Hasarı
    public class IceEnchantment : WeaponDecorator
    {
        public IceEnchantment(IWeapon weapon) : base(weapon) { }

        public override string GetDescription()
        {
            return $"Buzlu {_wrappedWeapon.GetDescription()}";
        }

        public override double GetDamage()
        {
            return _wrappedWeapon.GetDamage() + 3.0;
        }
    }

    // Özellik: Kritik Vuruş (Hasarı katlar)
    public class CriticalStrikeGem : WeaponDecorator
    {
        public CriticalStrikeGem(IWeapon weapon) : base(weapon) { }

        public override string GetDescription()
        {
            return $"Keskin {_wrappedWeapon.GetDescription()}";
        }

        public override double GetDamage()
        {
            // Mevcut toplam hasarı %20 artırır
            return _wrappedWeapon.GetDamage() * 1.2;
        }
    }

    // --- 5. Client (Oyun Mantığı) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Decorator Pattern (RPG Silah Efsunlama) ---\n");

            // 1. Sade bir kılıç yarat
            IWeapon mySword = new BasicSword();
            PrintWeaponStats(mySword);

            // 2. Kılıca ATEŞ özelliği ekle
            // Artık mySword referansı bir "FireEnchantment" tutuyor ama içinde "BasicSword" var.
            Console.WriteLine("\n>>> Demircide Alev Taşı basıldı...");
            mySword = new FireEnchantment(mySword);
            PrintWeaponStats(mySword);

            // 3. Aynı kılıca BUZ özelliği ekle (Hem Ateşli Hem Buzlu)
            Console.WriteLine("\n>>> Demircide Buz Rünü basıldı...");
            mySword = new IceEnchantment(mySword);
            PrintWeaponStats(mySword);

            // 4. Kılıca KRİTİK GEM ekle (Tüm hasarı katlayacak)
            Console.WriteLine("\n>>> Efsanevi Kritik Gem bulundu ve takıldı...");
            mySword = new CriticalStrikeGem(mySword);
            PrintWeaponStats(mySword);

            Console.WriteLine("\n--------------------------------");
            // Matematiksel Kontrol:
            // Temel: 10
            // + Ateş (5): 15
            // + Buz (3): 18
            // + Kritik (*1.2): 18 * 1.2 = 21.6
            Console.WriteLine($"Beklenen Hasar: 21.6 | Hesaplanan: {mySword.GetDamage()}");

            Console.ReadKey();
        }

        static void PrintWeaponStats(IWeapon weapon)
        {
            Console.WriteLine($"SİLAH: {weapon.GetDescription()}");
            Console.WriteLine($"HASAR: {weapon.GetDamage()}");
        }
    }
}