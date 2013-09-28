// s. http://blog.marcel-kloubert.de


using MarcelJoachimKloubert.Blog.WPF;

namespace MarcelJoachimKloubert.Blog.TestWpf
{
    partial class MainWindow
    {
        #region Nested Classes (1)

        public sealed class ViewModel : NotificationObjectBase
        {
            #region Fields (1)

            private bool _test1;

            #endregion Fields

            #region Properties (1)

            public bool Test1
            {
                get { return this._test1; }

                set { this.SetProperty(ref this._test1, value); }
            }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}
