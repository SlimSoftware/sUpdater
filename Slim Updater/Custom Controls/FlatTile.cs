using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SlimUpdater
{
    public partial class flatTile : UserControl
    {
        public flatTile()
        {
            InitializeComponent();
            WireAllControls(this);
        }

        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color hoverGreen = Color.FromArgb(0, 196, 0);
        Color normalOrange = Color.FromArgb(254, 124, 35);
        Color hoverOrange = Color.FromArgb(254, 134, 35);

        // Required for the text to display correctly
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]  
        [Description("The text associated with the control."), Category("Data")]
        public override string Text
        {
            get { return statusLabel.Text; }
            set { statusLabel.Text = value; }
        }

        [Description("The icon shown on the button."), Category("Appearance")]
        public Image Image
        {
            get { return tileIcon.Image; }
            set { tileIcon.Image = value; }
        }

        // Center text
        private void statusLabel_SizeChanged(object sender, System.EventArgs e)
        {
            statusLabel.Left = (this.ClientSize.Width - statusLabel.Size.Width) / 2;
        }

        #region Mouse Events

        #region Mouse Enter/Leave Events
        private void tileIcon_MouseEnter(object sender, EventArgs e)
        {
            if (this.BackColor == normalOrange)
            {
                this.BackColor = hoverOrange;
            }
            else
            {
                this.BackColor = hoverGreen;
            }
        }

        private void tileIcon_MouseLeave(object sender, EventArgs e)
        {
            if (this.BackColor == hoverOrange)
            {
                this.BackColor = normalOrange;
            }
            else
            {
                this.BackColor = normalGreen;
            }
        }

        private void statusLabel_MouseEnter(object sender, EventArgs e)
        {
            if (this.BackColor == normalOrange)
            {
                this.BackColor = hoverOrange;
            }
            else
            {
                this.BackColor = hoverGreen;
            }
        }

        private void statusLabel_MouseLeave(object sender, EventArgs e)
        {
            if (this.BackColor == hoverOrange)
            {
                this.BackColor = normalOrange;
            }
            else
            {
                this.BackColor = normalGreen;
            }
        }

        private void flatTile_MouseEnter(object sender, EventArgs e)
        {
            if (this.BackColor == normalOrange)
            {
                this.BackColor = hoverOrange;
            }
            else
            {
                this.BackColor = hoverGreen;
            }
        }

        private void flatTile_MouseLeave(object sender, EventArgs e)
        {
            if (this.BackColor == hoverOrange)
            {
                this.BackColor = normalOrange;
            }
            else
            {
                this.BackColor = normalGreen;
            }
        }
        #endregion

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
        #endregion
    }
}
