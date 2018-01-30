using System.Windows.Forms;
using System.Drawing;
using System;


namespace SlimUpdater
{
    public partial class AppItem : UserControl
    {
        public AppItem()
        {
            InitializeComponent();
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
            set
            {
                versionLabel.Text = value;
                if (value == null)
                {
                    appLabel.Location = new Point(22, 12);
                }
            }
        }

        public bool Checkbox
        {
            get { return checkBox.Visible; }
            set
            {
                if (value == true)
                {
                    checkBox.Visible = true;
                    appLabel.Location = new Point(22, appLabel.Location.Y);
                    versionLabel.Location = new Point(22, versionLabel.Location.Y);
                }
                else
                {
                    checkBox.Visible = false;
                    appLabel.Location = new Point(5, appLabel.Location.Y);
                    versionLabel.Location = new Point(5, versionLabel.Location.Y);
                }
            }
        }

        public bool Checked
        {
            get { return checkBox.Checked; }
            set { checkBox.Checked = value; }
        }

        public int Progress
        {
            get { return progressBar.Value; }
            set
            {
                progressBar.Value = value;
                if (value != 0)
                {
                    progressBar.Visible = true;
                    statusLabel.Location = new Point(530, 27);
                    statusLabel.Size = new Size(250, 20);
                }
                else
                {
                    progressBar.Visible = false;
                    statusLabel.Location = new Point(530, 0);
                    statusLabel.Size = new Size(250, 47);
                }
            }
        }

        public string Status
        {
            get { return statusLabel.Text; }
            set
            {
                statusLabel.Text = value;
                if (value != null)
                {
                    statusLabel.Visible = true;
                }
                else
                {
                    statusLabel.Visible = false;
                }
            }
        }

        // Link 1 & 2 Properties and Events
        public bool ShowLink1
        {
            get
            {
                if (link1.Visible == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    link1.Visible = true;
                }
                else
                {
                    link1.Visible = false;
                }
            }
        }

        public string Link1Text
        {
            get { return link1.Text; }
            set { link1.Text = value; }
        }

        public event EventHandler Link1Clicked;
        private void Link1_Clicked(object sender, EventArgs e)
        {
            Link1Clicked?.Invoke(sender, e);
        }

        public bool ShowLink2
        {
            get
            {
                if (link2.Visible == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    link2.Visible = true;
                }
                else
                {
                    link2.Visible = false;
                }
            }
        }

        public string Link2Text
        {
            get { return link2.Text; }
            set { link2.Text = value; }
        }
        
        public event EventHandler Link2Clicked;
        private void Link2_Clicked(object sender, EventArgs e)
        {
            Link2Clicked?.Invoke(sender, e);
        }

        private void WireAllControls(Control cont)
        {
            foreach (Control ctl in cont.Controls)
            {
                if (ctl == link2)
                {
                    ctl.Click += Link2Clicked;
                }
                else
                {
                    ctl.Click += Ctl_Click;
                }
                if (ctl.HasChildren)
                {
                    WireAllControls(ctl);
                }
            }
        }

        private void Ctl_Click(object sender, EventArgs e)
        {
            if (sender != checkBox)
            {
                this.InvokeOnClick(this, EventArgs.Empty);
            }
        }
    }
}