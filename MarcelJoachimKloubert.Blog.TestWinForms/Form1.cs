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
            this.pictureBox1
                .Image = this.GetType()
                             .Assembly
                             .GetManifestResourceBitmap("MarcelJoachimKloubert.Blog.TestWinForms.Edward_Snowden-21.jpg")
                             .Grayscale();
        }

        #endregion Methods
    }
}
