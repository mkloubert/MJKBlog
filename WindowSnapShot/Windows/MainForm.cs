// s. http://blog.marcel-kloubert.de

// s. http://www.codeproject.com/Articles/19192/How-to-capture-a-Window-as-an-Image-and-save-it


using System;
using System.Windows.Forms;
using MarcelJoachimKloubert.Blog.WindowSnapShot.UI;
using MarcelJoachimKloubert.Blog.WindowSnapShot.Win32;

namespace MarcelJoachimKloubert.Blog.WindowSnapShot.Windows
{
    /// <summary>
    /// Das Hauptfenster.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields (1)

        private readonly GuiApp[] _APPS;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="MainForm" />.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            this._APPS = GuiApp.GetValidGuiProcesses()
                               .ToArray();
        }

        #endregion Constructors

        #region Methods (2)

        // Private Methods (2) 

        private void Button_MakeScreenshot_Click(object sender, EventArgs e)
        {
            var index = this.ComboBox_ProcessList.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            try
            {
                this.Button_MakeScreenshot.Enabled = false;

                this.PictureBox_Screenshot
                    .Image = this._APPS[index]
                                 .MakeScreenshot(timeForRedrawing: TimeSpan.FromSeconds(1));
            }
            finally
            {
                this.Button_MakeScreenshot.Enabled = true;

                Win32Api.SetForegroundWindow(this.Handle);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ComboBox_ProcessList.Items.Clear();
            foreach (var app in this._APPS)
            {
                this.ComboBox_ProcessList
                    .Items
                    .Add(app);
            }

            if (this.ComboBox_ProcessList.Items.Count > 0)
            {
                this.ComboBox_ProcessList.SelectedIndex = 0;
            }
        }

        #endregion Methods
    }
}
