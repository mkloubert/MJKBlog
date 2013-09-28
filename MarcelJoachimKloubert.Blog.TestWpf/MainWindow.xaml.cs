// s. http://blog.marcel-kloubert.de


using System.Windows;

namespace MarcelJoachimKloubert.Blog.TestWpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors (1)

        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new ViewModel();
        }

        #endregion Constructors
    }
}
