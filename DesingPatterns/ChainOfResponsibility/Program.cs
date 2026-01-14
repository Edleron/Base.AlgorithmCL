using System;

namespace DesignPatterns.ChainOfResponsibility
{
    // --- 1. Handler Interface (İşleyici Arayüzü) ---
    // Zincirin her halkasının uyması gereken yapı.
    public abstract class DamageHandler
    {
        protected DamageHandler _nextHandler;

        // Zincirin bir sonraki halkasını belirler.
        // Fluent Interface (Zincirleme metod) kullanımı için 'this' döner.
        public DamageHandler SetNext(DamageHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return nextHandler;
        }

        // İsteği işleyen ana metot.
        // Alt sınıflar bunu ezecek (override) ama genellikle base.Handle() ile sonrakine paslayacak.
        public virtual void HandleDamage(int damageAmount, string damageType)
        {
            // Eğer bir sonraki halka varsa, hasarı ona ilet.
            if (_nextHandler != null)
            {
                _nextHandler.HandleDamage(damageAmount, damageType);
            }
        }
    }

    // --- 2. Concrete Handlers (Somut İşleyiciler) ---

    // Halka 1: Büyü Kalkanı
    // Sadece büyü hasarını engeller, fiziksel hasarı direkt geçirir.
    public class MagicShield : DamageHandler
    {
        private int _shieldPoints = 50;

        public override void HandleDamage(int damageAmount, string damageType)
        {
            if (damageType == "Magic")
            {
                if (_shieldPoints > 0)
                {
                    int absorbed = Math.Min(damageAmount, _shieldPoints);
                    _shieldPoints -= absorbed;
                    damageAmount -= absorbed;

                    Console.WriteLine($"[Magic Shield] {absorbed} büyü hasarı emildi. (Kalan Kalkan: {_shieldPoints})");
                }
                else
                {
                    Console.WriteLine("[Magic Shield] Kalkan kırık! Hasar geçiyor.");
                }
            }
            
            // Hasar hala 0'dan büyükse veya tip büyü değilse zincirde aşağı gönder.
            if (damageAmount > 0)
            {
                base.HandleDamage(damageAmount, damageType);
            }
        }
    }

    // Halka 2: Demir Zırh
    // Gelen hasarı sabit bir oranda azaltır (Defans).
    public class IronArmor : DamageHandler
    {
        private int _defensePower = 10; // Her vuruştan 10 hasar siler.

        public override void HandleDamage(int damageAmount, string damageType)
        {
            // Zırh sadece fiziksel hasarı azaltır
            if (damageType == "Physical")
            {
                int reducedAmount = Math.Max(0, damageAmount - _defensePower);
                Console.WriteLine($"[Iron Armor] Zırh {damageAmount - reducedAmount} hasarı blokladı.");
                damageAmount = reducedAmount;
            }

            // Hasar hala varsa sonrakine ilet.
            if (damageAmount > 0)
            {
                base.HandleDamage(damageAmount, damageType);
            }
            else
            {
                Console.WriteLine("[Iron Armor] Saldırı tamamen bloklandı!");
            }
        }
    }

    // Halka 3: Karakterin Bedeni (Son Durak)
    // Buraya gelen hasar artık can yakar. Zincirin sonu.
    public class CharacterBody : DamageHandler
    {
        public string Name { get; private set; }
        public int Health { get; private set; }

        public CharacterBody(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public override void HandleDamage(int damageAmount, string damageType)
        {
            Health -= damageAmount;
            Console.WriteLine($"[Body] {Name} {damageAmount} hasar aldı! (Kalan Can: {Health})");

            if (Health <= 0)
            {
                Console.WriteLine($"*** {Name} öldü! ***");
            }
        }
    }

    // --- 3. Client (Oyun Mantığı) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Chain of Responsibility (Hasar Sistemi) ---\n");

            // 1. Zincirin halkaları oluşturuluyor
            var magicShield = new MagicShield();
            var ironArmor = new IronArmor();
            var body = new CharacterBody("Şövalye Ertuğrul", 100);

            // 2. Zincir kuruluyor: Shield -> Armor -> Body
            // Hasar önce kalkana, artarsa zırha, artarsa bedene gidecek.
            magicShield.SetNext(ironArmor).SetNext(body);

            // Zincirin başı 'magicShield' olduğu için istekleri ona atacağız.
            
            // Senaryo 1: Fiziksel Saldırı (50 Hasar)
            // Kalkan (Büyü değil) -> Pas geçer.
            // Zırh (10 azaltır) -> 40 kalır.
            // Beden -> 40 hasar yer.
            Console.WriteLine(">>> Düşman Kılıçla Saldırıyor (50 Fiziksel) <<<");
            magicShield.HandleDamage(50, "Physical");

            Console.WriteLine("\n" + new string('-', 40) + "\n");

            // Senaryo 2: Büyü Saldırısı (60 Hasar)
            // Kalkan (50 emer) -> 10 kalır.
            // Zırh (Büyü koruması yok) -> Pas geçer.
            // Beden -> 10 hasar yer.
            Console.WriteLine(">>> Düşman Ateş Topu Atıyor (60 Büyü) <<<");
            magicShield.HandleDamage(60, "Magic");

            Console.WriteLine("\n" + new string('-', 40) + "\n");

            // Senaryo 3: Zayıf Saldırı
            // Kalkan pas geçer.
            // Zırh (10 azaltır) -> 0 kalır.
            // Beden hasar almaz.
            Console.WriteLine(">>> Düşman Taş Atıyor (5 Fiziksel) <<<");
            magicShield.HandleDamage(5, "Physical");

            Console.ReadKey();
        }
    }
}