using System;

namespace DesignPatterns.State
{
    // --- 1. State Interface (Durum Arayüzü) ---
    // Her durumun (Idle, Jump, Duck) uygulaması gereken davranışlar.
    // Context (Character) referansını parametre olarak alırız ki durumu değiştirebilelim.
    public interface IPlayerState
    {
        void HandleInput(Character player, ConsoleKey key);
        void Update(Character player);
    }

    // --- 2. Context (Bağlam) ---
    // Durumu değişen ana karakterimiz.
    // Şu anki durumunu (CurrentState) tutar ve işleri ona devreder.
    public class Character
    {
        // Şu anki aktif durum
        private IPlayerState _currentState;

        public Character()
        {
            // Başlangıç durumu
            _currentState = new StandingState();
        }

        // Durum değiştirme metodu (Transition)
        public void SetState(IPlayerState newState)
        {
            Console.WriteLine($"\n[Sistem] Durum Değişti: {_currentState.GetType().Name} -> {newState.GetType().Name}");
            _currentState = newState;
        }

        // Oyun döngüsünden gelen inputları aktif duruma iletir
        public void HandleInput(ConsoleKey key)
        {
            _currentState.HandleInput(this, key);
        }

        // Oyun döngüsünde (Update) sürekli çalışır
        public void Update()
        {
            _currentState.Update(this);
        }
    }

    // --- 3. Concrete States (Somut Durumlar) ---

    // Durum: AYAKTA (Standing)
    // - Zıplayabilir (Up)
    // - Eğilebilir (Down)
    public class StandingState : IPlayerState
    {
        public void HandleInput(Character player, ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow)
            {
                Console.WriteLine("Ayakta: Zıplama tuşuna basıldı.");
                // Transition: Standing -> Jumping
                player.SetState(new JumpingState());
            }
            else if (key == ConsoleKey.DownArrow)
            {
                Console.WriteLine("Ayakta: Eğilme tuşuna basıldı.");
                // Transition: Standing -> Ducking
                player.SetState(new DuckingState());
            }
            else
            {
                Console.WriteLine("Ayakta: Bekliyor...");
            }
        }

        public void Update(Character player)
        {
            // Ayaktayken stamina dolabilir vs.
        }
    }

    // Durum: ZIPLAMA (Jumping)
    // - Havada olduğu için eğilemez.
    // - Tekrar zıplayamaz (Bu örnekte).
    // - Yere inebilir (Enter tuşu simülasyonu ile).
    public class JumpingState : IPlayerState
    {
        public void HandleInput(Character player, ConsoleKey key)
        {
            if (key == ConsoleKey.Enter)
            {
                Console.WriteLine("Havadayken: Yere iniliyor...");
                // Transition: Jumping -> Standing
                player.SetState(new StandingState());
            }
            else if (key == ConsoleKey.UpArrow)
            {
                Console.WriteLine("Havadayken: Zaten havadayız, tekrar zıplayamayız!");
            }
            else if (key == ConsoleKey.DownArrow)
            {
                Console.WriteLine("Havadayken: Havada eğilemeyiz!"); // (Smasher mekaniği yoksa)
            }
        }

        public void Update(Character player)
        {
            Console.WriteLine("--- Karakter süzülüyor... Yer çekimi aktif. ---");
        }
    }

    // Durum: EĞİLME (Ducking)
    // - Zıplayamaz (Kafası çarpar).
    // - Tuş bırakılınca (veya Up'a basınca) ayağa kalkar.
    public class DuckingState : IPlayerState
    {
        public void HandleInput(Character player, ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow) // Veya tuşu bırakma mantığı
            {
                Console.WriteLine("Eğilirken: Ayağa kalkılıyor.");
                // Transition: Ducking -> Standing
                player.SetState(new StandingState());
            }
            else
            {
                Console.WriteLine("Eğilirken: Siperde bekliyor. Güvende.");
            }
        }

        public void Update(Character player)
        {
            // Eğilirken can daha hızlı dolabilir vs.
        }
    }

    // --- 4. Client (Oyun Döngüsü) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- State Pattern (Karakter Kontrolcüsü FSM) ---\n");
            Console.WriteLine("Kontroller: [UP: Zıpla/Kalk] [DOWN: Eğil] [ENTER: Yere İn]\n");

            Character hero = new Character();

            // Simüle edilmiş bir oyun döngüsü (Game Loop)
            // Normalde burası `void Update()` içinde olur ve Input.GetKeyDown ile çalışır.
            
            // Senaryo 1: Ayaktayız -> Eğiliyoruz
            hero.HandleInput(ConsoleKey.DownArrow);
            hero.Update();

            // Senaryo 2: Eğilirken zıplamaya çalışıyoruz (Mantıken olmamalı veya kalkmalı)
            // DuckingState'in HandleInput'u çalışacak.
            hero.HandleInput(ConsoleKey.UpArrow); // Ayağa kalktı
            hero.Update();

            // Senaryo 3: Ayaktayız -> Zıplıyoruz
            hero.HandleInput(ConsoleKey.UpArrow); // Zıpladı
            hero.Update();

            // Senaryo 4: Havadayken tekrar zıplamaya çalışıyoruz
            hero.HandleInput(ConsoleKey.UpArrow); // İzin vermez

            // Senaryo 5: Yere iniyoruz
            hero.HandleInput(ConsoleKey.Enter);

            Console.ReadKey();
        }
    }
}