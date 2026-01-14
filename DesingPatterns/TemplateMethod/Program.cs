using System;

namespace DesignPatterns.TemplateMethod
{
    // --- 1. Abstract Class (Şablon) ---
    // Algoritmanın iskeletini burada kuruyoruz.
    public abstract class EnemyAI
    {
        // TEMPLATE METHOD: Algoritmanın iskeleti.
        // 'final' mantığında düşünebilirsin, genellikle alt sınıfların bu akışı bozmasını istemeyiz.
        // Bu yüzden 'virtual' yapmadık.
        public void ExecuteTurn()
        {
            Console.WriteLine($"\n--- {this.GetType().Name} Sırası Başlıyor ---");
            
            StartTurnAnimation(); // Adım 1: Ortak
            MoveToTarget();       // Adım 2: Özelleştirilebilir (Abstract)
            PerformAction();      // Adım 3: Özelleştirilebilir (Abstract)
            
            // Hook (Kanca): İsteğe bağlı adım.
            // Varsayılan olarak boş olabilir, isteyen ezer.
            if (CanTaunt())
            {
                TauntPlayer();
            }

            EndTurn();            // Adım 4: Ortak
        }

        // --- Ortak Metotlar (Herkes için aynı) ---
        protected void StartTurnAnimation()
        {
            Console.WriteLine("Sistem: Karakter seçildi, highlight efekti oynatıldı.");
        }

        protected void EndTurn()
        {
            Console.WriteLine("Sistem: Sıra bitti, AP (Action Points) sıfırlandı.");
        }

        // --- Abstract Metotlar (Alt sınıflar doldurmak ZORUNDA) ---
        protected abstract void MoveToTarget();
        protected abstract void PerformAction();

        // --- Hooks (Kancalar - İsteğe Bağlı) ---
        // Varsayılan olarak false döner, isteyen true yapıp kullanır.
        protected virtual bool CanTaunt()
        {
            return false;
        }

        protected virtual void TauntPlayer()
        {
            // Varsayılan boş.
        }
    }

    // --- 2. Concrete Class: Orc (Savaşçı) ---
    public class OrcAI : EnemyAI
    {
        protected override void MoveToTarget()
        {
            Console.WriteLine("Ork: Ağır adımlarla oyuncunun dibine kadar yürüdü.");
        }

        protected override void PerformAction()
        {
            Console.WriteLine("Ork: BALTA DARBESİ! (20 Hasar)");
        }

        // Ork kışkırtmayı sever, hook'u override ediyoruz.
        protected override bool CanTaunt()
        {
            return true;
        }

        protected override void TauntPlayer()
        {
            Console.WriteLine("Ork Bağırıyor: 'Eziik! Bu kadar mı gücün var?'");
        }
    }

    // --- 3. Concrete Class: Healer (Şifacı) ---
    public class HealerAI : EnemyAI
    {
        protected override void MoveToTarget()
        {
            Console.WriteLine("Şifacı: Oyuncudan uzaklaşarak güvenli bir noktaya süzüldü.");
        }

        protected override void PerformAction()
        {
            Console.WriteLine("Şifacı: Kutsal Işık! Kendini ve dostlarını iyileştirdi (+15 HP).");
        }

        // Healer kışkırtma yapmaz, o yüzden CanTaunt() ve TauntPlayer() metodlarına dokunmuyoruz.
        // Base sınıftaki varsayılan (false) çalışır.
    }

    // --- 4. Concrete Class: Kamikaze Goblin ---
    public class KamikazeGoblin : EnemyAI
    {
        protected override void MoveToTarget()
        {
            Console.WriteLine("Goblin: Çılgınlar gibi koşuyor!");
        }

        protected override void PerformAction()
        {
            Console.WriteLine("Goblin: BOOM! Kendini patlattı.");
        }
    }

    // --- 5. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Template Method Pattern (Sıra Tabanlı AI) ---\n");

            // Farklı düşman tipleri
            EnemyAI orc = new OrcAI();
            EnemyAI healer = new HealerAI();
            EnemyAI goblin = new KamikazeGoblin();

            // Hepsi aynı 'ExecuteTurn' metodunu çağırır ama farklı davranırlar.
            // Ancak Akış (Animasyon -> Hareket -> Aksiyon -> Bitiş) ASLA değişmez.
            
            orc.ExecuteTurn();
            healer.ExecuteTurn();
            goblin.ExecuteTurn();

            Console.ReadKey();
        }
    }
}