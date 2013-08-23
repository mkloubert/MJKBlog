using System;
using System.Windows.Forms;

namespace MarcelJoachimKloubert.Blog.TestWinForms
{
    public partial class Form1 : Form
    {
        #region Constructors (1)

        public Form1()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (1)

        // Private Methods (1) 

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("MarcelJoachimKloubert.Blog.TestWinForms.Edward_Snowden-2.jpg"))
            {
                this.pictureBox1
                    .Image = stream.LoadBitmap();
            }

        }

        #endregion Methods
    }
}
