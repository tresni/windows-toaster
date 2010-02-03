using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Toaster
{
    partial class ToastForm : Form
    {
        Timer TimeToLive = new Timer();

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        public ToastForm(string title, string text, Image icon)
        {
            InitializeComponent();
            lblText.Text = text;
            lblTitle.Text = title;
            pctIcon.Image = new Bitmap(pctIcon.ClientSize.Width, pctIcon.ClientSize.Height);
            Graphics graphic = Graphics.FromImage(pctIcon.Image);
            
            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            graphic.DrawImage(icon, new Rectangle(new Point(0,0), pctIcon.Size));

            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - this.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TimeToLive.Interval = 9000;
            TimeToLive.Tag = false;
            TimeToLive.Tick += new EventHandler(TimeToLive_Tick);
            TimeToLive.Start();
        }

        void TimeToLive_Tick(object sender, EventArgs e)
        {
            Timer t = (Timer)sender;
            SetupFadeOut(t);
            t.Start();
        }

        void TimeToLive_FadeOut(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler<EventArgs> eh = TimeToLive_FadeOut;
                this.BeginInvoke(eh, new object[] { sender, e });
                return;
            }

            if (this.Opacity > 0.00)
                this.Opacity -= 0.01;
            else
                this.Close();
        }

        private void ClickToClose(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            this.Opacity = 1.00;
            SetupFadeOut(this.TimeToLive);
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            TimeToLive.Start();
        }

        private void SetupFadeOut(Timer t)
        {
            t.Stop();
            t.Interval = 1;
            t.Tick -= new EventHandler(TimeToLive_Tick);

            if ((bool)t.Tag == false)
            {
                t.Tick += new EventHandler(TimeToLive_FadeOut);
                t.Tag = true;
            }
        }
    }
}
