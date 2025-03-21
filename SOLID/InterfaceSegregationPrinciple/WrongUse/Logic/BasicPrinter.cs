namespace InterfaceSegregationPrinciple.WrongUse.Logic;

public class BasicPrinter : Interface.IMultiFunctionPrinter
{
    public void Print() => Console.WriteLine("Yazdırıyor.");

    public void Scan() => throw new NotImplementedException();

    public void Fax() => throw new NotImplementedException();
}