using ATEA_TechnicalTask.Shared.Interfaces;

namespace ATEA_TechnicalTask.Shared
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Info;


        public void LogError(string message)
        {
            if (LogLevel < LogLevel.Error) return;
            Console.WriteLine("\n<ERROR>: " + message + '\n');
        }

        public void LogInfo(string message)
        {
            if (LogLevel < LogLevel.Info) return;
            Console.WriteLine("\n<INFO>: " + message + '\n');
        }

        public void LogWarning(string message)
        {
            if (LogLevel < LogLevel.Warning) return;
            Console.WriteLine("\n<WARNING>: " + message + '\n');
        }
    }
}
