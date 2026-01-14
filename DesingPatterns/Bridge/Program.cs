using System;

namespace DesignPatterns.Bridge
{
    // --- 1. Implementor (Uygulayıcı Arayüzü) ---
    // Bu arayüz, "NASIL" yapılacağını tanımlar.
    // Soyutlamadan (Entity) tamamen bağımsızdır. Sadece işin teknik kısmını (çizimi) bilir.
    public interface IRenderAPI
    {
        void RenderModel(string modelName, string textureType);
        void PlaySound(string soundName);
    }

    // --- 2. Concrete Implementors (Somut Uygulayıcılar) ---
    
    // Senaryo: Yüksek performanslı PC Render Motoru
    public class DirectXRenderAPI : IRenderAPI
    {
        public void RenderModel(string modelName, string textureType)
        {
            Console.WriteLine($"[DirectX - PC] '{modelName}' modeli '{textureType}' ile 4K çözünürlükte çiziliyor. (Shader: High)");
        }

        public void PlaySound(string soundName)
        {
            Console.WriteLine($"[DirectX - Audio] '{soundName}' Dolby Surround 7.1 olarak çalınıyor.");
        }
    }

    // Senaryo: Düşük güç tüketen Mobil Render Motoru
    public class OpenGLESRenderAPI : IRenderAPI
    {
        public void RenderModel(string modelName, string textureType)
        {
            Console.WriteLine($"[OpenGL ES - Mobile] '{modelName}' modeli düşük poligon ve basit '{textureType}' ile çiziliyor. (Shader: Basic)");
        }

        public void PlaySound(string soundName)
        {
            Console.WriteLine($"[OpenGL ES - Audio] '{soundName}' Mono formatta çalınıyor.");
        }
    }

    // --- 3. Abstraction (Soyutlama) ---
    // Oyun tarafındaki nesnelerin atası.
    // Render API'sini (Implementor) içinde barındırır (Bridge - Köprü burasıdır).
    public abstract class GameEntity
    {
        // Köprüyü kuran referans
        protected IRenderAPI _renderer;

        protected GameEntity(IRenderAPI renderer)
        {
            _renderer = renderer;
        }

        // Soyutlamanın kendi metodları.
        // Alt sınıflar (Hero, Enemy) bunu kendilerine göre dolduracak.
        public abstract void Draw();
    }

    // --- 4. Refined Abstraction (Geliştirilmiş Soyutlama) ---
    
    // Oyuncu Karakteri
    public class Hero : GameEntity
    {
        public Hero(IRenderAPI renderer) : base(renderer) { }

        public override void Draw()
        {
            Console.WriteLine("--- Kahraman Sahneye Çıkıyor ---");
            // Hero ne olduğunu bilir ama NASIL çizileceğini renderer'a sorar.
            _renderer.RenderModel("PaladinMesh", "GoldenArmorTexture");
            _renderer.PlaySound("HeroVoice_BattleCry");
        }
    }

    // Düşman Karakteri
    public class Enemy : GameEntity
    {
        public Enemy(IRenderAPI renderer) : base(renderer) { }

        public override void Draw()
        {
            Console.WriteLine("--- Düşman Belirdi ---");
            _renderer.RenderModel("OrcMesh", "DirtySkinTexture");
            _renderer.PlaySound("Orc_Grunt");
        }
    }

    // --- 5. Client (Oyun Motoru Başlatıcı) ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Bridge Pattern Demo (Cross-Platform Rendering) ---\n");

            // Senaryo 1: Oyun PC'de açıldı (DirectX API Yüklendi)
            Console.WriteLine(">>> PLATFORM TESPİT EDİLDİ: Windows PC (High Settings)");
            IRenderAPI pcRenderer = new DirectXRenderAPI();

            // Aynı Entity sınıflarını kullanıyoruz, kod değişmiyor.
            GameEntity heroOnPC = new Hero(pcRenderer);
            GameEntity enemyOnPC = new Enemy(pcRenderer);

            heroOnPC.Draw();
            enemyOnPC.Draw();

            Console.WriteLine("\n" + new string('-', 50) + "\n");

            // Senaryo 2: Oyun Mobilde açıldı (OpenGL ES API Yüklendi)
            Console.WriteLine(">>> PLATFORM TESPİT EDİLDİ: Android Mobile (Low Settings)");
            IRenderAPI mobileRenderer = new OpenGLESRenderAPI();

            // Dikkat: Hero ve Enemy sınıflarında HİÇBİR değişiklik yapmadık.
            // Sadece onlara verdiğimiz "Uygulayıcıyı" (Tool) değiştirdik.
            GameEntity heroOnMobile = new Hero(mobileRenderer);
            GameEntity enemyOnMobile = new Enemy(mobileRenderer);

            heroOnMobile.Draw();
            enemyOnMobile.Draw();

            Console.WriteLine("\n--- Sonuç ---");
            Console.WriteLine("Bridge sayesinde 'Karakterler' ve 'Render Motorları' birbirinden bağımsız gelişebilir.");
            Console.WriteLine("Yeni bir 'VulkanAPI' eklemek için 'Hero' sınıfına dokunmamıza gerek yok.");

            Console.ReadKey();
        }
    }
}