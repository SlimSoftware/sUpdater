using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace SlimUpdater
{
    public static class Log
    {
        public static void Append(string text, LogLevel logLevel, RichTextBox textBox)
        {
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (logLevel == LogLevel.INFO && textBox.Foreground != Brushes.Black)
                {
                    textBox.Foreground = Brushes.Black;
                }
                else if (logLevel == LogLevel.WARN && textBox.Foreground != Brushes.Orange)
                {
                    textBox.Foreground = Brushes.Orange;
                }
                else if (logLevel == LogLevel.ERROR && textBox.Foreground != Brushes.Red)
                {
                    textBox.Foreground = Brushes.Red;
                }

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
