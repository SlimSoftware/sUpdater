using System;

namespace sUpdater.Models
{
    public static class Log
    {
        public static event EventHandler<LogAppendEventArgs> LogAppend;

        public static string LogText { get; private set; }

        public enum LogLevel
        {
            INFO,
            WARN,
            ERROR
        }

        public static void Append(string text, LogLevel logLevel)
        {
            string logLine = string.Format("[{0}] [{1}] {2}",
                DateTime.Now.ToLongTimeString(), logLevel, text + Environment.NewLine);
            LogText += logLine;

            LogAppend?.Invoke(null, new LogAppendEventArgs() { AppendedLine = logLine });
        }
    }

    public class LogAppendEventArgs
    {
        /// <summary>
        /// The line that was appended to the log.
        /// </summary>
        public string AppendedLine { get; set; }
    }
}
