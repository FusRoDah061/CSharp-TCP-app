using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TCPExample
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
            var main_form = new Form1();
            main_form.Show();
            Application.Run();
        }
    }
}
