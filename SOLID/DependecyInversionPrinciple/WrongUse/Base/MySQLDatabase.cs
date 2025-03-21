namespace  DependecyInversionPrinciple.WrongUse.Base;

public class MySQLDatabase
{
    public void Save(string data) => Console.WriteLine($"MySQL'e kaydedildi: {data}");
}