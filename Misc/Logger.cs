namespace Agarme_Server.Misc
{
    public enum LogLevel
    {
        System,
        Info,
        Warning,
        Error
    }

    public static class Logger
    {
        private static ConsoleColor GetColorForLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.System:
                    return ConsoleColor.DarkCyan;
                case LogLevel.Info:
                    return ConsoleColor.Green;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Gray;
            }
        }

        public static void Log(string message, LogLevel level = LogLevel.Info, ConsoleColor? color = null)
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            Console.ForegroundColor = color ?? GetColorForLogLevel(level);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} {Enum.GetName(typeof(LogLevel), level)}] {message}");

            Console.ForegroundColor = currentForeground;
        }
    }
}
