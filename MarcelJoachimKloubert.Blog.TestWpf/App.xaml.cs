// s. http://blog.marcel-kloubert.de


using System;
using System.Windows;

namespace MarcelJoachimKloubert.Blog.TestWpf
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        private static int Main(string[] args)
        {
            return new App().Run(new MainWindow());
        }
    }
}
