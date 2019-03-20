﻿using System;
using System.Windows.Controls;

namespace SlimUpdater
{
    public static class Log
    {
        public static void Append(string text, LogLevel logLevel, RichTextBox textBox)
        {
            textBox.Dispatcher.BeginInvoke(new Action(() =>
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
