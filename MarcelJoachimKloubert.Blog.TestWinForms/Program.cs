// s. http://blog.marcel-kloubert.de


using System;
using System.Windows.Forms;

namespace MarcelJoachimKloubert.Blog.TestWinForms
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
