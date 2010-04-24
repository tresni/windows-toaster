namespace Toaster
{
    partial class ToastForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.pctIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTitle.Location = new System.Drawing.Point(105, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            this.lblTitle.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            this.lblTitle.Click += new System.EventHandler(this.ClickToClose);
            this.lblTitle.MouseEnter += new System.EventHandler(this.Form1_MouseEnter);
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(12, 35);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(116, 66);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Text";
            this.lblText.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            this.lblText.Click += new System.EventHandler(this.ClickToClose);
            this.lblText.MouseEnter += new System.EventHandler(this.Form1_MouseEnter);
            // 
            // pctIcon
            // 
            this.pctIcon.Location = new System.Drawing.Point(0, 0);
            this.pctIcon.Name = "pctIcon";
            this.pctIcon.Size = new System.Drawing.Size(32, 32);
            this.pctIcon.TabIndex = 2;
            this.pctIcon.TabStop = false;
            this.pctIcon.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            this.pctIcon.Click += new System.EventHandler(this.ClickToClose);
            this.pctIcon.MouseEnter += new System.EventHandler(this.Form1_MouseEnter);
            // 
            // ToastForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(132, 102);
            this.ControlBox = false;
            this.Controls.Add(this.pctIcon);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToastForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseEnter += new System.EventHandler(this.Form1_MouseEnter);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToastForm_FormClosed);
            this.Click += new System.EventHandler(this.ClickToClose);
            this.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pctIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.PictureBox pctIcon;
    }
}