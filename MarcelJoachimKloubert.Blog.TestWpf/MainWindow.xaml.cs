// s. http://blog.marcel-kloubert.de


using System.Threading;
using System.Threading.Tasks;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNewTask((state, token) =>
                {
                    Thread.Sleep(5000);

                    state.TestTextBlock
                         .BeginInvokeSafe((tb, state2) =>
                         {
                             tb.Visibility = state2.V;
                         }, new
                         {
                             V = Visibility.Visible,
                         });
                }, new
                {
                    TestTextBlock = this.TextBlock_Test,
                });
        }
    }
}
