using System.Drawing;
using System.Windows.Forms;

namespace BrowserClient.Views
{
    partial class BrowserTabUserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.browserPanel = new System.Windows.Forms.Panel();
            this.browserPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // browserPanel
            // 
            this.browserPanel.BackColor = System.Drawing.Color.Transparent;
            this.browserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserPanel.Location = new System.Drawing.Point(0, 0);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(730, 490);
            this.browserPanel.TabIndex = 2;
            // 
            // BrowserTabUserControl
            // 
            this.Controls.Add(this.browserPanel);
            this.Name = "BrowserTabUserControl";
            this.Size = new System.Drawing.Size(730, 490);
            this.browserPanel.ResumeLayout(false);
            this.browserPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel browserPanel;

        #endregion
    }
}