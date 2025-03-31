using System;

namespace SimpleMyExample
{

    /*
    "+=" ile ekleme yaptığınızda, delegate'in invocation listesine yeni metotlar ekleyip çoklu işlemleri zincirleyebilirsiniz.
    "=" ile atama yaptığınızda, önceki metod referansları silinir ve sadece en son atanan metot delegate tarafından çağrılır.
    Implicit olarak atama yapmak, arka planda otomatik olarak instance (new) oluşturur, fakat kodun netliğini artırmak veya uyumluluk için explicit new kullanımı tercih edilebilir.
    */
    protected delegate void MyMathProcess(double x, double y);
    protected MyMathProcess myDelegateInstance;
    class Program
    {
        public void Main(string[] args)
        {
            double x = 5;
            double y = 10;

            myDelegateInstance += Add;
            myDelegateInstance += Subtract;
            myDelegateInstance += Multiply;
            myDelegateInstance += Divide;

            myMathProcess.Invoke(x, y);
        }


        static double Add(double a, double b)
        {
            return a + b;
        }

        // Çıkarma işlemi yapan metot.
        static double Subtract(double a, double b)
        {
            return a - b;
        }

        // Çarpma işlemi yapan metot.
        static double Multiply(double a, double b)
        {
            return a * b;
        }

        // Bölme işlemi yapan metot.
        // b değeri 0 olduğunda uyarı verip NaN döndürerek hatayı yönetir.
        static double Divide(double a, double b)
        {
            if (b == 0)
            {
                Console.WriteLine("Hata: Sıfıra bölme işlemi gerçekleştirilemez.");
                return double.NaN;
            }
            return a / b;
        }
    }
}
