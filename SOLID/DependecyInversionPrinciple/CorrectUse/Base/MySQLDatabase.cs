namespace  DependecyInversionPrinciple.CorrectUse.Base;

public class MySQLDatabase : Interface.IDatabase
{
    public void Save(string data) => Console.WriteLine($"MySQL'e kaydedildi: {data}");
}