namespace InterfaceSegregationPrinciple.CorrectUse.Logic;

public class BasicPrinter : Interface.IPrinter
{
    public void Print() => Console.WriteLine("Yazdırıyor.");
}