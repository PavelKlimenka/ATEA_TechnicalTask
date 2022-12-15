namespace CLI.Interfaces
{
    public interface ILogger
    {
        public LogLevel LogLevel { get; set; }

        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }

    public enum LogLevel { NoLog, Error, Warning, Info }
}
