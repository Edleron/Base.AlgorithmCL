namespace InterfaceSegregationPrinciple.CorrectUse.Logic;

public class MultiFunctionPrinter : Interface.IMultiFunction
{
    public void Print() => Console.WriteLine("Yazdırıyor.");

    public void Scan() => Console.WriteLine("Tarama yapıyor.");

    public void Fax() => Console.WriteLine("Fax gönderiyor.");
}