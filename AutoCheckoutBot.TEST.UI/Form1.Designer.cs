
namespace AutoCheckoutBot.TEST.UI
{
    partial class Form1
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
            this.panel_browser_container = new System.Windows.Forms.Panel();
            this.btnTest1 = new System.Windows.Forms.Button();
            this.btnReloadBrowser = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel_browser_container
            // 
            this.panel_browser_container.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_browser_container.Location = new System.Drawing.Point(13, 13);
            this.panel_browser_container.Name = "panel_browser_container";
            this.panel_browser_container.Size = new System.Drawing.Size(918, 513);
            this.panel_browser_container.TabIndex = 0;
            // 
            // btnTest1
            // 
            this.btnTest1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest1.Location = new System.Drawing.Point(961, 52);
            this.btnTest1.Name = "btnTest1";
            this.btnTest1.Size = new System.Drawing.Size(75, 23);
            this.btnTest1.TabIndex = 1;
            this.btnTest1.Text = "Test1";
            this.btnTest1.UseVisualStyleBackColor = true;
            this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
            // 
            // btnReloadBrowser
            // 
            this.btnReloadBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadBrowser.Location = new System.Drawing.Point(961, 12);
            this.btnReloadBrowser.Name = "btnReloadBrowser";
            this.btnReloadBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnReloadBrowser.TabIndex = 2;
            this.btnReloadBrowser.Text = "Reload";
            this.btnReloadBrowser.UseVisualStyleBackColor = true;
            this.btnReloadBrowser.Click += new System.EventHandler(this.btnReloadBrowser_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(961, 81);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Test1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 538);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnReloadBrowser);
            this.Controls.Add(this.btnTest1);
            this.Controls.Add(this.panel_browser_container);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_browser_container;
        private System.Windows.Forms.Button btnTest1;
        private System.Windows.Forms.Button btnReloadBrowser;
        private System.Windows.Forms.Button button1;
    }
}

