namespace  DependecyInversionPrinciple.CorrectUse.Logic;

public class FileLogger : Interface.ILogger
{
    public void Log(string message) => Console.WriteLine($"Dosyaya yazıldı: {message}");
}
