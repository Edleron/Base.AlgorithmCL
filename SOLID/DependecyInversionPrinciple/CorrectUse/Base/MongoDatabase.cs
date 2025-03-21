namespace  DependecyInversionPrinciple.CorrectUse.Base;

public class MongoDatabase : Interface.IDatabase
{
    public void Save(string data) => Console.WriteLine($"MongoDB'ye kaydedildi: {data}");
}