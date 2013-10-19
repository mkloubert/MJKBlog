namespace MarcelJoachimKloubert.Blog.WindowSnapShot.Windows
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.SplitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.PictureBox_Screenshot = new System.Windows.Forms.PictureBox();
            this.ComboBox_ProcessList = new System.Windows.Forms.ComboBox();
            this.Button_MakeScreenshot = new System.Windows.Forms.Button();
            this.SplitContainer_Main.Panel1.SuspendLayout();
            this.SplitContainer_Main.Panel2.SuspendLayout();
            this.SplitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Screenshot)).BeginInit();
            this.SuspendLayout();
            // 
            // SplitContainer_Main
            // 
            this.SplitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer_Main.Name = "SplitContainer_Main";
            this.SplitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer_Main.Panel1
            // 
            this.SplitContainer_Main.Panel1.Controls.Add(this.PictureBox_Screenshot);
            this.SplitContainer_Main.Panel1.Controls.Add(this.ComboBox_ProcessList);
            // 
            // SplitContainer_Main.Panel2
            // 
            this.SplitContainer_Main.Panel2.Controls.Add(this.Button_MakeScreenshot);
            this.SplitContainer_Main.Size = new System.Drawing.Size(1145, 695);
            this.SplitContainer_Main.SplitterDistance = 628;
            this.SplitContainer_Main.TabIndex = 0;
            // 
            // PictureBox_Screenshot
            // 
            this.PictureBox_Screenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox_Screenshot.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_Screenshot.Name = "PictureBox_Screenshot";
            this.PictureBox_Screenshot.Size = new System.Drawing.Size(1145, 607);
            this.PictureBox_Screenshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox_Screenshot.TabIndex = 4;
            this.PictureBox_Screenshot.TabStop = false;
            // 
            // ComboBox_ProcessList
            // 
            this.ComboBox_ProcessList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ComboBox_ProcessList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_ProcessList.FormattingEnabled = true;
            this.ComboBox_ProcessList.Location = new System.Drawing.Point(0, 607);
            this.ComboBox_ProcessList.Name = "ComboBox_ProcessList";
            this.ComboBox_ProcessList.Size = new System.Drawing.Size(1145, 21);
            this.ComboBox_ProcessList.TabIndex = 0;
            // 
            // Button_MakeScreenshot
            // 
            this.Button_MakeScreenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Button_MakeScreenshot.Location = new System.Drawing.Point(0, 0);
            this.Button_MakeScreenshot.Name = "Button_MakeScreenshot";
            this.Button_MakeScreenshot.Size = new System.Drawing.Size(1145, 63);
            this.Button_MakeScreenshot.TabIndex = 3;
            this.Button_MakeScreenshot.Text = "SnapShot!";
            this.Button_MakeScreenshot.UseVisualStyleBackColor = true;
            this.Button_MakeScreenshot.Click += new System.EventHandler(this.Button_MakeScreenshot_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 695);
            this.Controls.Add(this.SplitContainer_Main);
            this.Name = "MainForm";
            this.Text = "Win32SnapshotTest";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SplitContainer_Main.Panel1.ResumeLayout(false);
            this.SplitContainer_Main.Panel2.ResumeLayout(false);
            this.SplitContainer_Main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Screenshot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer_Main;
        private System.Windows.Forms.PictureBox PictureBox_Screenshot;
        private System.Windows.Forms.ComboBox ComboBox_ProcessList;
        private System.Windows.Forms.Button Button_MakeScreenshot;

    }
}

