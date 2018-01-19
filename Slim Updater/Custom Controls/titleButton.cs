using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Slim_Updater
{
    public partial class TitleButton : UserControl
    {
        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color hoverGreen = Color.FromArgb(0, 196, 0);

        public TitleButton()
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

        [Description("Indicates if the arrow should be shown on the left side of the control."),
    Category("Appearance")]
        public Boolean ArrowLeft
        {
            get { return backArrowLeft.Visible; }
            set
            {
                if (value == true)
                {
                    if (ArrowRight == true)
                    {
                        backArrowLeft.Visible = true;
                        backArrowRight.Visible = true;
                    }
                    else
                    {
                        backArrowLeft.Visible = true;
                        backArrowRight.Visible = false;
                        titleLabel.Location = new Point(32, 4);
                    }
                }
                else
                {
                    if (ArrowRight == true)
                    {
                        backArrowLeft.Visible = false;
                        backArrowRight.Visible = true;
                    }
                    else
                    {
                        backArrowLeft.Visible = false;
                        backArrowRight.Visible = false;
                        titleLabel.Location = new Point(5, 4);
                    }
                }
            }
        }

        [Description("Indicates if the arrow should be shown on the right side of the control."),
Category("Appearance")]
        public Boolean ArrowRight
        {
            get { return backArrowRight.Visible; }
            set
            {
                if (value == true)
                {
                    if (ArrowLeft == true)
                    {
                        backArrowLeft.Visible = true;
                        backArrowRight.Visible = true;
                        titleLabel.Location = new Point(32, 4);
                    }
                    else
                    {
                        backArrowLeft.Visible = false;
                        backArrowRight.Visible = true;
                        titleLabel.Location = new Point(0, 4);
                    }
                }
                else
                {
                    if (ArrowLeft == true)
                    {
                        backArrowLeft.Visible = true;
                        backArrowRight.Visible = false;
                        titleLabel.Location = new Point(32, 4);
                    }
                    else
                    {
                        backArrowLeft.Visible = false;
                        backArrowRight.Visible = false;
                        titleLabel.Location = new Point(5, 4);
                    }
                }
            }
        }

        private void TitleButton_MouseEnter(object sender, EventArgs e)
        {
            if (ArrowLeft == true | ArrowRight == true)
            {
                this.BackColor = hoverGreen;
                backArrowLeft.Image = Properties.Resources.ArrowWhite;
                backArrowRight.Image = Properties.Resources.ArrowWhiteRight;
                titleLabel.ForeColor = Color.White;
            }
        }

        private void TitleButton_MouseLeave(object sender, EventArgs e)
        {
            if (ArrowLeft == true | ArrowRight == true)
            {
                this.BackColor = Color.White;
                backArrowLeft.Image = Properties.Resources.ArrowGreen;
                backArrowRight.Image = Properties.Resources.ArrowGreenRight;
                titleLabel.ForeColor = normalGreen;
            }
        }

        private void TitleLabel_MouseEnter(object sender, EventArgs e)
        {
            if (ArrowLeft == true | ArrowRight == true)
            {
                this.BackColor = hoverGreen;
                backArrowLeft.Image = Properties.Resources.ArrowWhite;
                backArrowRight.Image = Properties.Resources.ArrowWhiteRight;
                titleLabel.ForeColor = Color.White;
            }
        }

        private void TitleLabel_MouseLeave(object sender, EventArgs e)
        {
            if (ArrowLeft == true | ArrowRight == true)
            {
                this.BackColor = Color.White;
                backArrowLeft.Image = Properties.Resources.ArrowGreen;
                backArrowRight.Image = Properties.Resources.ArrowGreenRight;
                titleLabel.ForeColor = normalGreen;
            }
        }

        private void BackArrow_MouseEnter(object sender, EventArgs e)
        {
            if (ArrowLeft == true | ArrowRight == true)
            {
                this.BackColor = hoverGreen;
                backArrowLeft.Image = Properties.Resources.ArrowWhite;
                backArrowRight.Image = Properties.Resources.ArrowWhiteRight;
                titleLabel.ForeColor = Color.White;
            }
        }

        private void BackArrow_MouseLeave(object sender, EventArgs e)
        {
            if (ArrowLeft == true | ArrowRight == true)
            {
                this.BackColor = Color.White;
                backArrowLeft.Image = Properties.Resources.ArrowGreen;
                backArrowRight.Image = Properties.Resources.ArrowGreenRight;
                titleLabel.ForeColor = normalGreen;
            }
        }

        // Fire the button's click event when clicked on a child control
        private void WireAllControls(Control cont)
        {
            foreach (Control ctl in cont.Controls)
            {
                ctl.Click += Ctl_Click;
                if (ctl.HasChildren)
                {
                    WireAllControls(ctl);
                }
            }
        }

        private void Ctl_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, EventArgs.Empty);
        }

        private void TitleButton_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            backArrowLeft.Image = Slim_Updater.Properties.Resources.ArrowGreen;
            backArrowRight.Image = Slim_Updater.Properties.Resources.ArrowGreenRight;
            titleLabel.ForeColor = normalGreen;
        }
    }
}