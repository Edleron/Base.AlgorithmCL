using System;
using System.Collections.Generic;

namespace DesignPatterns.Command
{
    // --- 1. Receiver (Alıcı) ---
    // İşlemin gerçekte yapıldığı, mantığın döndüğü nesne.
    // Komut nesnesi sadece "Yap" der, işi bu sınıf yapar.
    public class GameUnit
    {
        public string Name { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public GameUnit(string name)
        {
            Name = name;
            X = 0;
            Y = 0;
        }

        public void MoveTo(int x, int y)
        {
            X = x;
            Y = y;
            Console.WriteLine($"[Unit] {Name} hareket etti -> ({X}, {Y})");
        }

        // Örnek başka bir eylem
        public void Attack()
        {
            Console.WriteLine($"[Unit] {Name} saldırı yaptı!");
        }
    }

    // --- 2. Command Interface (Komut Arayüzü) ---
    // Tüm komutların uyması gereken standart.
    public interface ICommand
    {
        void Execute(); // Komutu çalıştır
        void Undo();    // Komutu geri al
    }

    // --- 3. Concrete Commands (Somut Komutlar) ---

    // Hareket Komutu: Geri alınabilir (Undoable) bir işlemdir.
    public class MoveCommand : ICommand
    {
        private GameUnit _unit;      // Kim hareket edecek?
        private int _targetX, _targetY; // Nereye gidecek?
        
        // Undo için gerekli: Önceki konumu saklamalıyız (State).
        private int _previousX, _previousY; 

        public MoveCommand(GameUnit unit, int x, int y)
        {
            _unit = unit;
            _targetX = x;
            _targetY = y;
        }

        public void Execute()
        {
            // Eylem gerçekleşmeden önce mevcut durumu kaydet (Yedekle)
            _previousX = _unit.X;
            _previousY = _unit.Y;

            // İşlemi yap
            _unit.MoveTo(_targetX, _targetY);
        }

        public void Undo()
        {
            // Kaydedilen eski duruma geri dön
            Console.WriteLine($"<<< Geri Alınıyor (Undo): {_unit.Name} eski konumuna dönüyor...");
            _unit.MoveTo(_previousX, _previousY);
        }
    }

    // Saldırı Komutu: Geri alınamaz (örnek senaryo) veya basit eylem.
    public class AttackCommand : ICommand
    {
        private GameUnit _unit;

        public AttackCommand(GameUnit unit)
        {
            _unit = unit;
        }

        public void Execute()
        {
            _unit.Attack();
        }

        public void Undo()
        {
            Console.WriteLine("<<< Hata: Saldırı geri alınamaz!");
        }
    }

    // --- 4. Invoker (Çağırıcı / Komut Yöneticisi) ---
    // Komutları tetikleyen ve geçmişi (History) tutan sınıf.
    // Unity'de bu genellikle "InputManager" veya "GameManager" olur.
    public class CommandManager
    {
        // Geri alma işlemleri için komut yığını (LIFO - Last In First Out)
        private Stack<ICommand> _commandHistory = new Stack<ICommand>();

        // Komutu çalıştırır ve geçmişe ekler
        public void ExecuteCommand(ICommand cmd)
        {
            cmd.Execute();
            _commandHistory.Push(cmd);
        }

        // Son yapılan işlemi geri alır
        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                ICommand lastCmd = _commandHistory.Pop();
                lastCmd.Undo();
            }
            else
            {
                Console.WriteLine("[Manager] Geri alınacak işlem yok.");
            }
        }
    }

    // --- 5. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Command Pattern (RTS Hareket & Undo) ---\n");

            // 1. Sahne kurulumu
            GameUnit soldier = new GameUnit("Er Ryan");
            CommandManager inputManager = new CommandManager();

            Console.WriteLine($"> Başlangıç Konumu: ({soldier.X}, {soldier.Y})\n");

            // 2. Oyuncu komut veriyor: (10, 5) noktasına git
            ICommand move1 = new MoveCommand(soldier, 10, 5);
            inputManager.ExecuteCommand(move1);

            // 3. Oyuncu komut veriyor: (20, 15) noktasına git
            ICommand move2 = new MoveCommand(soldier, 20, 15);
            inputManager.ExecuteCommand(move2);

            Console.WriteLine("\n--- Hata yaptık, geri saralım ---");

            // 4. Geri Al (Undo) - Son komut (Move2) iptal edilir.
            // Asker (20,15)'ten (10,5)'e döner.
            inputManager.UndoLastCommand();

            // 5. Bir daha Geri Al (Undo) - Move1 iptal edilir.
            // Asker (10,5)'ten (0,0)'a döner.
            inputManager.UndoLastCommand();

            // 6. Bir daha dene (Liste boş)
            inputManager.UndoLastCommand();

            Console.ReadKey();
        }
    }
}