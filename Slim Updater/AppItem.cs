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
            if (sender != checkBox)
            {
                this.InvokeOnClick(this, EventArgs.Empty);
            }
        }
    }
}
