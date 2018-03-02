using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;

namespace SlimUpdater
{
    static class Program
    {
        static MainWindow MainForm;

        /// <summary>  
        /// The main entry point for the application.  
        /// </summary>  
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new MainWindow();
            SingleInstanceApplication.Run(MainForm, FirstInstanceHandler, SecondInstanceHandler);
        }

        private static void FirstInstanceHandler(object sender, StartupEventArgs e)
        {
            MainForm.trayIcon.Visible = true;
        }

        public static void SecondInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            e.BringToForeground = true;
            MainForm.Show();
            MainForm.ShowInTaskbar = true;
            MainForm.WindowState = FormWindowState.Normal;
        }

        public class SingleInstanceApplication : WindowsFormsApplicationBase
        {
            private SingleInstanceApplication()
            {
                base.IsSingleInstance = true;
            }

            public static void Run(Form f, StartupEventHandler firstInstanceHandler, 
                StartupNextInstanceEventHandler secondInstanceHandler)
            {
                SingleInstanceApplication app = new SingleInstanceApplication();
                app.MainForm = f;
                app.Startup += firstInstanceHandler;
                app.StartupNextInstance += secondInstanceHandler;
                app.Run(Environment.GetCommandLineArgs());
            }
        }
    }
}
