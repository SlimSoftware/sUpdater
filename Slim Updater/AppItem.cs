using System.Windows.Forms;
using System.Drawing;
using System;

namespace Slim_Updater
{
    public partial class AppItem : UserControl
    {
        public AppItem()
        {
            InitializeComponent();
            this.Height = 45;
            this.Checked = true;
            WireAllControls(this);
        }

        public virtual new string Name
        {
            get { return appLabel.Text; }
            set { appLabel.Text = value; }
        }

        public string Version
        {
            get { return versionLabel.Text; }
            set { versionLabel.Text = value; }
        }

        public bool Checked
        {
            get { return checkBox.Checked; }
            set { checkBox.Checked = value; }
        }

        public int Progress
        {
            get { return progressBar.Value; }
            set { progressBar.Value = value; }
        }

        public string Status
        {
            get { return statusLabel.Text; }
            set { statusLabel.Text = value; }
        }

        public void Expand()
        {
            this.Size = new Size(this.Width, 200);
            this.BackColor = Color.WhiteSmoke;
        }

        public void Shrink()
        {
            this.Size = new Size(this.Width, 45);
            this.BackColor = Color.White;
        }

        private void WireAllControls(Control cont)
        {
            foreach (Control ctl in cont.Controls)
            {
                ctl.Click += ctl_Click;
                if (ctl.HasChildren)
                {
                    WireAllControls(ctl);
                }
            }
        }

        private void ctl_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, EventArgs.Empty);
        }
    }
}
