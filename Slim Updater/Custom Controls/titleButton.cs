using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Slim_Updater
{
    public partial class titleButton : UserControl
    {
        Color hoverGreen = Color.FromArgb(0, 196, 0);
        Color normalGreen = Color.FromArgb(0, 186, 0);

        public titleButton()
        {
            InitializeComponent();
            WireAllControls(this);

        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), //required for the text to display
            Description("The text associated with the control."), Category("Data")] 
        public override string Text
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        [Description("Indicates if an arrow should be shown on the left side of the control."), 
            Category("Appearance")] 
        public Boolean Arrow
        {
            get { return backArrow.Visible; }
            set
            {
                if (value == true)
                {
                    backArrow.Visible = true;
                    titleLabel.Location = new Point(30, 4);
                }
                if (value == false)
                {
                    backArrow.Visible = false;
                    titleLabel.Location = new Point(0, 4);
                }
            }
        }

        private void titleButton_MouseEnter(object sender, EventArgs e)
        {
            if (Arrow == true)
            {
                this.BackColor = hoverGreen;
                backArrow.Image = Slim_Updater.Properties.Resources.ArrowWhite;
                titleLabel.ForeColor = Color.White;
            }
        }

        private void titleButton_MouseLeave(object sender, EventArgs e)
        {
            if (Arrow == true)
            {
                this.BackColor = Color.White;
                backArrow.Image = Slim_Updater.Properties.Resources.ArrowGreen;
                titleLabel.ForeColor = normalGreen;
            }
        }

        private void titleLabel_MouseEnter(object sender, EventArgs e)
        {
            if (Arrow == true)
            {
                this.BackColor = hoverGreen;
                backArrow.Image = Slim_Updater.Properties.Resources.ArrowWhite;
                titleLabel.ForeColor = Color.White;
            }
        }

        private void titleLabel_MouseLeave(object sender, EventArgs e)
        {
            if (Arrow == true)
            {
                this.BackColor = Color.White;
                backArrow.Image = Slim_Updater.Properties.Resources.ArrowGreen;
                titleLabel.ForeColor = normalGreen;
            }
        }

        private void backArrow_MouseEnter(object sender, EventArgs e)
        {
            if (Arrow == true)
            {
                this.BackColor = hoverGreen;
                backArrow.Image = Slim_Updater.Properties.Resources.ArrowWhite;
                titleLabel.ForeColor = Color.White;
            }
        }

        private void backArrow_MouseLeave(object sender, EventArgs e)
        {
            if (Arrow == true)
            {
                this.BackColor = Color.White;
                backArrow.Image = Slim_Updater.Properties.Resources.ArrowGreen;
                titleLabel.ForeColor = normalGreen;
            }
        }

        // Fire the button's click event when clicked on a child control
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

        private void titleButton_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            backArrow.Image = Slim_Updater.Properties.Resources.ArrowGreen;
            titleLabel.ForeColor = normalGreen;
        }
    }
}

