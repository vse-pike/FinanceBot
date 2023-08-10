namespace FinanceBot;

public class Logger
{
    public static void Log(LoggLevel lvl, object obj)
    {
        var dateTime = DateTime.Now;
        Console.WriteLine(" ");
        Console.WriteLine($"{lvl}: {dateTime}: {Newtonsoft.Json.JsonConvert.SerializeObject(obj)}");
    }
}

public enum LoggLevel
{
    INFO,
    ERROR,
    WARN
}