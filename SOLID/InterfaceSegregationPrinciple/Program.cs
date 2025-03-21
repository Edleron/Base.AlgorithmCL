namespace InterfaceSegregationPrinciple;

class Program
{
    /*
        ISP, SOLID prensiplerinin dördüncüsüdür. Basitçe şöyle der:

        "Sınıflar, kullanmadıkları metotları içeren interfacelere bağımlı olmaya zorlanmamalıdır."

        Daha açık ifadeyle:

        Bir sınıf, ihtiyaç duymadığı yetenekleri içeren büyük interfaceleri implement etmeye zorlanmamalı.
        Büyük interfaceleri küçük, spesifik interfacelere bölmeli ve sınıflar yalnızca ihtiyaç duydukları interface’leri kullanmalıdır.
    */
    static void Main(string[] args)
    {
        WrongUsed();
        CorrectUse();
    }

    static void WrongUsed()
    {
        WrongUse.Interface.IMultiFunctionPrinter basicPrinter = new WrongUse.Logic.BasicPrinter();
        basicPrinter.Print();
    }

    static void CorrectUse() 
    {
        CorrectUse.Interface.IPrinter basicPrinter  = new CorrectUse.Logic.BasicPrinter();
        basicPrinter.Print();

        CorrectUse.Interface.IMultiFunction mfp     = new CorrectUse.Logic.MultiFunctionPrinter();
        mfp.Print();
        mfp.Scan();
        mfp.Fax();
    }
}
