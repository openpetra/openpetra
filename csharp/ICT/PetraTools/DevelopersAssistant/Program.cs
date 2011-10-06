using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ict.Tools.DevelopersAssistant
{
    static class Program
    {
        public const string APP_TITLE = "OpenPetra Developer's Assistant";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
