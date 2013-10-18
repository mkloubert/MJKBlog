// s. http://blog.marcel-kloubert.de


using System;
using System.Windows.Input;
using MarcelJoachimKloubert.Blog.Input;
using MarcelJoachimKloubert.Blog.WPF;

namespace MarcelJoachimKloubert.Blog.TestWpf
{
    partial class MainWindow
    {
        #region Nested Classes (1)

        public sealed class ViewModel : NotificationObjectBase
        {
            public ViewModel()
            {
                this.Test2 = "<html><b>test</b></html>";

                this.Test3 = new DelegateCommand(() => Test2 = null);
            }

            #region Fields (3)

            private bool _test1;
            private string _test2;
            private ICommand _test3;

            #endregion Fields

            #region Properties (3)

            public bool Test1
            {
                get { return this._test1; }

                set { this.SetProperty(ref this._test1, value); }
            }

            public string Test2
            {
                get { return this._test2; }

                set { this.SetProperty(ref this._test2, value); }
            }

            public ICommand Test3
            {
                get { return this._test3; }

                set { this.SetProperty(ref this._test3, value); }
            }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}
