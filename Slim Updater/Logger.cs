using System;
using System.Windows.Forms;

namespace SlimUpdater
{
    public static class Logger
    {
        public static void Log(string text, LogLevel logLevel, RichTextBox textBox)
        {
            textBox.Invoke(new Action(() =>
            {
                textBox.AppendText(string.Format("[{0}] [{1}] {2}",
                     DateTime.Now.ToLongTimeString(), logLevel, text + Environment.NewLine));
            }));
        }

        public enum LogLevel
        {
            INFO,
            WARN,
            ERROR,
        }
    }
}
