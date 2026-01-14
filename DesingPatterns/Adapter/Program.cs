using System;
using System.Collections.Generic;

namespace DesignPatterns.Adapter
{
    // --- 1. Target (Hedef) Interface ---
    // Oyunumuzun (Client) halihazırda bildiği ve kullandığı standart yapı.
    public interface IEnemy
    {
        void Attack();
        void TakeDamage(int damage);
    }

    // --- 2. Existing Concrete Class (Mevcut Sınıf) ---
    // Standart arayüzü uygulayan basit bir zombi.
    public class Zombie : IEnemy
    {
        public void Attack()
        {
            Console.WriteLine("Zombi: Isırmaya çalışıyor!");
        }

        public void TakeDamage(int damage)
        {
            Console.WriteLine($"Zombi: {damage} hasar aldı. 'Aaargh!'");
        }
    }

    // --- 3. Adaptee (Uyumsuz Sınıf / Dış Kaynak) ---
    // Asset Store'dan indirdiğimiz veya eski projeden gelen karmaşık bir sınıf.
    // Metot isimleri ve parametre tipleri IEnemy ile UYUŞMUYOR.
    public class SuperRobot
    {
        private double _batteryLevel = 100.0;

        // Robotun saldırma mantığı farklı
        public void SmashWithHammer()
        {
            Console.WriteLine("Süper Robot: Dev çekiciyle yeri sarstı!");
        }

        // Robotun hasar alma mantığı farklı (Double kullanıyor ve mantık içeriyor)
        public void DeductEnergy(double amount)
        {
            _batteryLevel -= amount;
            Console.WriteLine($"Süper Robot: Enerji seviyesi %{_batteryLevel}'e düştü. Sistem uyarısı!");
        }

        // Ekstra bir metot (Örn: Sadece robota özel)
        public void Recharge()
        {
            Console.WriteLine("Süper Robot: Şarj oluyor...");
        }
    }

    // --- 4. Adapter (Dönüştürücü) ---
    // Robotu, oyunumuzun anlayacağı IEnemy kılığına sokan sınıf.
    // Robot'tan miras ALMAZ (Inheritance değil), onu içinde BARINDIRIR (Composition).
    public class RobotAdapter : IEnemy
    {
        private readonly SuperRobot _robot;

        public RobotAdapter(SuperRobot robot)
        {
            _robot = robot;
        }

        // Oyun "Attack" dediğinde, Adaptör bunu Robotun anlayacağı "SmashWithHammer"a çevirir.
        public void Attack()
        {
            _robot.SmashWithHammer();
        }

        // Oyun "TakeDamage(int)" dediğinde, Adaptör bunu Robotun "DeductEnergy(double)" metoduna çevirir.
        public void TakeDamage(int damage)
        {
            // Gerekirse arada veri dönüşümü (int -> double) veya ekstra mantık çalıştırabiliriz.
            double energyLoss = (double)damage / 2; // Robot zırhlı olduğu için hasarı yarıya indiriyoruz.
            _robot.DeductEnergy(energyLoss);
        }
    }

    // --- 5. Client (Oyun Mantığı) ---
    // Oyun motorumuz sadece IEnemy tanır. Karşısındakinin gerçekte bir Zombi mi yoksa Adapter mi olduğunu bilmez.
    class LevelManager
    {
        public void ProcessEnemies(List<IEnemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                // Polymorphism sayesinde hepsi aynı şekilde çalışır
                enemy.Attack();
                enemy.TakeDamage(20);
                Console.WriteLine("---");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Adapter Pattern Demo ---\n");

            // 1. Standart düşmanımız
            Zombie normalZombie = new Zombie();

            // 2. Uyumsuz, havalı robotumuz
            SuperRobot shinyRobot = new SuperRobot();

            // 3. Robotu sisteme dahil etmek için Adapter giydiriyoruz
            IEnemy robotInDisguise = new RobotAdapter(shinyRobot);

            // 4. Oyun döngüsü (Listeye hem Zombiyi hem de Adapter'ı atabiliyoruz)
            List<IEnemy> levelEnemies = new List<IEnemy>
            {
                normalZombie,
                robotInDisguise
            };

            // LevelManager hiçbir değişiklik yapmadan her ikisini de çalıştırır.
            LevelManager level = new LevelManager();
            level.ProcessEnemies(levelEnemies);

            Console.ReadKey();
        }
    }
}