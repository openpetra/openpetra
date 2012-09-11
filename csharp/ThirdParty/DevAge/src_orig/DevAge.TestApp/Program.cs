using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GenericTest.Run();

            Application.Run(new MainForm());
        }
    }
}