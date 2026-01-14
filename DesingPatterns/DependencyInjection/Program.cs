using System;
using System.Collections.Generic;

namespace DesignPatterns.DependencyInjection
{
    // --- 1. Abstractions (Soyutlamalar/Servisler) ---
    // Karakterimizin ihtiyaç duyacağı servislerin kontratları.
    
    // Giriş Kontrolü Arayüzü
    public interface IInputService
    {
        string GetInputData();
    }

    // Ses/Log Servisi Arayüzü
    public interface ILoggerService
    {
        void Log(string message);
    }

    // --- 2. Concrete Implementations (Somut Uygulamalar) ---

    // PC için Klavye girişi
    public class KeyboardInput : IInputService
    {
        public string GetInputData()
        {
            return "W, A, S, D tuşlarına basılıyor (PC)";
        }
    }

    // Konsol için Gamepad girişi
    public class GamepadInput : IInputService
    {
        public string GetInputData()
        {
            return "Sol Analog Çubuğu ileri itiliyor (Console)";
        }
    }

    // Gerçek oyun içi konsola yazan logger
    public class ConsoleLogger : ILoggerService
    {
        public void Log(string message)
        {
            Console.WriteLine($"[Oyun Logu] {DateTime.Now.ToShortTimeString()}: {message}");
        }
    }

    // Test veya Sessiz mod için boş logger (Null Object Pattern vari)
    public class SilentLogger : ILoggerService
    {
        public void Log(string message)
        {
            // Hiçbir şey yapma, sessiz kal.
        }
    }

    // --- 3. Client (İstemci / Bağımlı Sınıf) ---
    // Bağımlılıkları kullanan ana sınıfımız.
    // DİKKAT: İçeride asla 'new KeyboardInput()' demiyoruz!
    public class Character
    {
        private readonly IInputService _input;
        private readonly ILoggerService _logger;

        // Constructor Injection: En yaygın ve önerilen yöntem.
        // Karakter yaratılırken ona "Neyi kullanacağını" zorla veriyoruz.
        public Character(IInputService input, ILoggerService logger)
        {
            // Guard clause (Boş gelirse hata ver)
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Update()
        {
            // Bağımlılıkları kullan
            string inputData = _input.GetInputData();
            _logger.Log($"Karakter Hareket Ediyor: {inputData}");
        }
    }

    // --- 4. DI Container (Basit Simülasyon) ---
    // Gerçek dünyada Zenject, VContainer veya Microsoft.Extensions.DependencyInjection bu işi yapar.
    // Burası "Composition Root" dediğimiz yerdir.
    public class SimpleDIContainer
    {
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        // Servis Kayıt (Register)
        public void Register<TInterface>(object implementation)
        {
            _services[typeof(TInterface)] = implementation;
        }

        // Servis Çözümleme (Resolve)
        public TInterface Resolve<TInterface>()
        {
            return (TInterface)_services[typeof(TInterface)];
        }
    }

    // --- 5. Program (Composition Root) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Dependency Injection Pattern ---\n");

            // YÖNTEM 1: Pure DI (Manuel Enjeksiyon)
            // Hiçbir framework kullanmadan elle bağlama. Küçük projeler için en iyisi.
            Console.WriteLine(">>> Senaryo 1: PC Ortamı (Manuel Bağlama)");
            
            // 1. Önce bağımlılıkları yarat
            IInputService pcInput = new KeyboardInput();
            ILoggerService gameLogger = new ConsoleLogger();

            // 2. Karakteri yaratırken bunları içine at (Inject et)
            Character heroPC = new Character(pcInput, gameLogger);
            heroPC.Update();


            Console.WriteLine("\n" + new string('-', 40) + "\n");


            // YÖNTEM 2: Container Kullanımı (Otomatik Bağlama Simülasyonu)
            // Büyük projelerde bu bağlama işini Container yapar.
            Console.WriteLine(">>> Senaryo 2: Konsol Ortamı (Container ile)");

            SimpleDIContainer container = new SimpleDIContainer();

            // Setup (Konfigürasyon) aşaması:
            // "IInputService istendiğinde bana GamepadInput ver" diyoruz.
            container.Register<IInputService>(new GamepadInput());
            container.Register<ILoggerService>(new ConsoleLogger()); // SilentLogger denersek çıktı gelmez

            // Resolution (Çözümleme) aşaması:
            // Karakteri oluştururken Container'dan parçaları istiyoruz.
            var inputService = container.Resolve<IInputService>();
            var logService = container.Resolve<ILoggerService>();
            
            Character heroConsole = new Character(inputService, logService);
            heroConsole.Update();

            Console.ReadKey();
        }
    }
}