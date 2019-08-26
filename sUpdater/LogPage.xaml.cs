using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();

            Log.LogAppend += OnLogAppend;

            string[] lines = Log.LogText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int last = lines.Length - 1;

            for (int i = 0; i < lines.Length; i++)
            {
                if (i != last)
                {
                    // Need to add a newline here because it is lost due to the splitting
                    AppendLine(lines[i] + Environment.NewLine);
                }
                else
                {
                    AppendLine(lines[i]);
                }
            }
        }

        private void OnLogAppend(object sender, LogAppendEventArgs e)
        {
            AppendLine(e.AppendedLine);
        }

        /// <summary>
        /// Appends a line to the textBox. The color is determined by the severity of the log message.
        /// </summary>
        /// <param name="line">The line to append</param>
        private void AppendLine(string line)
        {
            Brush color = null;

            if (line.Contains("[INFO]") && textBox.Foreground != Brushes.Black)
            {
                color = Brushes.Black;
            }
            else if (line.Contains("[WARN]") && textBox.Foreground != Brushes.Orange)
            {
                color = Brushes.Orange;
            }
            else if (line.Contains("[ERROR]") && textBox.Foreground != Brushes.Red)
            {
                color = Brushes.Red;
            }

            AppendLine(line, color);
        }

        /// <summary>
        /// Appends text to the textBox with the specified color.
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="color">The text color</param>
        public void AppendLine(string text, Brush color)
        {
            TextRange tr = new TextRange(textBox.Document.ContentEnd, textBox.Document.ContentEnd);
            tr.Text = text;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text file (*.txt)|*.txt";

            if (saveDialog.ShowDialog() == true)
            {
                TextRange range = new TextRange(textBox.Document.ContentStart,
                    textBox.Document.ContentEnd);
                range.Text = range.Text.TrimEnd(Environment.NewLine.ToCharArray());
                FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create);
                range.Save(stream, DataFormats.Text);
                stream.Close();
            }
        }
    }
}
