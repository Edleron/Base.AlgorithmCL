using System;

namespace DesignPatterns.Factory
{
    // 1. Product Interface: Tüm düşmanların ortak özelliklerini belirler.
    public interface IEnemy
    {
        void Attack();
        void Move();
    }

    // 2. Concrete Product A: Ork sınıfı
    public class Orc : IEnemy
    {
        public void Attack()
        {
            Console.WriteLine("Ork: Baltasıyla ağır hasar verdi!");
        }

        public void Move()
        {
            Console.WriteLine("Ork: Yavaş ve gürültülü adımlarla yürüyor.");
        }
    }

    // 3. Concrete Product B: Hayalet sınıfı
    public class Ghost : IEnemy
    {
        public void Attack()
        {
            Console.WriteLine("Hayalet: Ruh emerek hasar verdi!");
        }

        public void Move()
        {
            Console.WriteLine("Hayalet: Duvarların içinden süzülerek geçiyor.");
        }
    }

    // Düşman tiplerini yönetmek için Enum (String karşılaştırmasından daha güvenlidir)
    public enum EnemyType
    {
        Orc,
        Ghost
    }

    // 4. Factory Class: Nesne üretiminden sorumlu sınıf.
    // Unity'de bu genellikle "EnemySpawner" veya "LevelManager" içinde bir metot olabilir.
    public static class EnemyFactory
    {
        public static IEnemy CreateEnemy(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Orc:
                    return new Orc();
                
                case EnemyType.Ghost:
                    return new Ghost();

                default:
                    throw new ArgumentException("Bilinmeyen düşman tipi!");
            }
        }
    }

    // Client Code: Nesneleri kullanan taraf
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Factory Pattern Oyun Senaryosu ---\n");

            // Client (Oyun mantığı), hangi sınıfın nasıl new'leneceğini bilmez.
            // Sadece Factory'den talepte bulunur.

            try
            {
                // Senaryo 1: Bir Ork yarat
                Console.WriteLine("Dalga 1 Başlıyor: Ork Saldırısı!");
                IEnemy enemy1 = EnemyFactory.CreateEnemy(EnemyType.Orc);
                enemy1.Move();
                enemy1.Attack();

                Console.WriteLine();

                // Senaryo 2: Bir Hayalet yarat
                Console.WriteLine("Dalga 2 Başlıyor: Hayalet Saldırısı!");
                IEnemy enemy2 = EnemyFactory.CreateEnemy(EnemyType.Ghost);
                enemy2.Move();
                enemy2.Attack();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}