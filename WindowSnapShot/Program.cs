using System;
using System.Windows.Forms;
using MarcelJoachimKloubert.Blog.WindowSnapShot.Windows;

namespace MarcelJoachimKloubert.Blog.WindowSnapShot
{
    internal static class Program
    {
        #region Methods (1)

        // Private Methods (1) 

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        #endregion Methods
    }
}
