using System;

namespace DesignPatterns.Strategy
{
    // --- 1. Strategy Interface (Strateji Arayüzü) ---
    // Tüm algoritmaların (davranışların) ortak imzası.
    // Context (Enemy) kendini parametre olarak geçer, böylece strateji onun verilerine erişebilir.
    public interface IBossStrategy
    {
        void ExecuteBehavior(BossEnemy context);
    }

    // --- 2. Concrete Strategies (Somut Stratejiler) ---

    // Strateji A: Agresif Yakın Dövüş
    public class AggressiveMeleeStrategy : IBossStrategy
    {
        public void ExecuteBehavior(BossEnemy context)
        {
            Console.WriteLine($"[Agresif] Boss kükrüyor! Oyuncuya doğru koşuyor (Hız: {context.Speed * 2}).");
            Console.WriteLine("--> 'Balyoz Darbesi' vurdu! (Hasar: 50)");
        }
    }

    // Strateji B: Uzak Mesafe ve Büyü
    public class DefensiveMagicStrategy : IBossStrategy
    {
        public void ExecuteBehavior(BossEnemy context)
        {
            Console.WriteLine($"[Defansif] Boss geri çekiliyor ve mesafe açıyor.");
            Console.WriteLine("--> 'Ateş Topu' fırlattı! (Hasar: 20)");
            
            if (context.Health < 100)
            {
                Console.WriteLine("--> Kendine iyileştirme büyüsü yaptı (+10 HP).");
                context.Health += 10;
            }
        }
    }

    // Strateji C: Çılgınlık Modu (Can kritik seviyedeyken)
    public class BerserkStrategy : IBossStrategy
    {
        public void ExecuteBehavior(BossEnemy context)
        {
            Console.WriteLine("[ÇILGINLIK] Boss gözlerinden lazer saçıyor!");
            Console.WriteLine("--> Rastgele her yere saldırıyor! (Alan Hasarı: 100)");
            Console.WriteLine("--> Kendi canından harcıyor (-5 HP).");
            context.Health -= 5;
        }
    }

    // --- 3. Context (Bağlam) ---
    // Stratejiyi kullanan ana sınıfımız.
    public class BossEnemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Speed { get; set; }

        // Mevcut stratejiyi tutan referans
        private IBossStrategy _currentStrategy;

        public BossEnemy(string name)
        {
            Name = name;
            Health = 100;
            Speed = 10;
            // Varsayılan strateji
            _currentStrategy = new AggressiveMeleeStrategy();
        }

        // Çalışma zamanında (Runtime) davranışı değiştirme metodu
        public void SetStrategy(IBossStrategy newStrategy)
        {
            Console.WriteLine($"\n*** {Name} Taktik Değiştiriyor! ***");
            _currentStrategy = newStrategy;
        }

        // Oyun döngüsünde (Update) çağrılan metot
        public void Update()
        {
            Console.WriteLine($"\n--- Boss Durumu: HP {Health} ---");
            // Boss ne yapacağını bilmez, sadece elindeki stratejiyi çalıştırır.
            if (_currentStrategy != null)
            {
                _currentStrategy.ExecuteBehavior(this);
            }
            else
            {
                Console.WriteLine("Boss boş boş bakıyor.");
            }
        }
    }

    // --- 4. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Strategy Pattern (Boss AI Sistemi) ---\n");

            // Boss yaratılıyor (Varsayılan olarak Agresif başlar)
            BossEnemy boss = new BossEnemy("Kadim Ejderha");

            // Tur 1: Agresif Saldırı
            boss.Update();

            // Senaryo: Boss hasar aldı ve canı düştü.
            // Oyun mantığı (veya bir Event) stratejiyi değiştirmeye karar verir.
            boss.Health = 60;
            boss.SetStrategy(new DefensiveMagicStrategy());

            // Tur 2: Artık Büyücü gibi davranıyor
            boss.Update();

            // Senaryo: Boss ölmek üzere, son çırpınış.
            boss.Health = 20;
            boss.SetStrategy(new BerserkStrategy());

            // Tur 3: Çılgınlık modu
            boss.Update();
            boss.Update(); // Berserk modunda kendi canını da yiyor.

            Console.ReadKey();
        }
    }
}