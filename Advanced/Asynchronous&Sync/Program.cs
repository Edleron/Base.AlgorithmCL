using System;
using System.Threading.Tasks;

namespace SimpleMyExample
{
    class Program
    {
        // Async Main metodu C# 7.1 ve üzeri sürümlerde kullanılabilir.
        public static async Task Main(string[] args)
        {
            // Synchronous işlemler için örnek:
            Console.WriteLine("Synchronous işlemler için örnek:");
            double a = 10;
            double b = 5;
            Console.WriteLine($"Toplama: {a} + {b} = {Add(a, b)}");
            Console.WriteLine($"Çıkarma: {a} - {b} = {Subtract(a, b)}");
            Console.WriteLine($"Çarpma: {a} * {b} = {Multiply(a, b)}");
            Console.WriteLine($"Bölme: {a} / {b} = {Divide(a, b)}");

            // Asynchronous işlemler için örnek:
            Console.WriteLine("\nAsynchronous işlemler için örnek:");
            double addResult = await AddAsync(a, b);
            double subtractResult = await SubtractAsync(a, b);
            double multiplyResult = await MultiplyAsync(a, b);
            double divideResult = await DivideAsync(a, b);

            Console.WriteLine($"Toplama (Async): {a} + {b} = {addResult}");
            Console.WriteLine($"Çıkarma (Async): {a} - {b} = {subtractResult}");
            Console.WriteLine($"Çarpma (Async): {a} * {b} = {multiplyResult}");
            Console.WriteLine($"Bölme (Async): {a} / {b} = {divideResult}");

            Console.WriteLine("Asynchronous işlemler tamamlanana kadar bekleniyor...");
            Console.ReadKey();
        }

        // Senkron metotlar:
        static double Add(double a, double b) => a + b;
        static double Subtract(double a, double b) => a - b;
        static double Multiply(double a, double b) => a * b;
        static double Divide(double a, double b)
        {
            if (b == 0)
            {
                Console.WriteLine("Hata: Sıfıra bölme işlemi gerçekleştirilemez.");
                return double.NaN;
            }
            return a / b;
        }

        // Asenkron metotlar:
        static Task<double> AddAsync(double a, double b)
        {
            return Task.Run(() => Add(a, b));
        }
        static Task<double> SubtractAsync(double a, double b)
        {
            return Task.Run(() => Subtract(a, b));
        }
        static Task<double> MultiplyAsync(double a, double b)
        {
            return Task.Run(() => Multiply(a, b));
        }
        static Task<double> DivideAsync(double a, double b)
        {
            return Task.Run(() => Divide(a, b));
        }
    }
}
