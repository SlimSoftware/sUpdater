using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Slim_Updater.Custom_Controls
{
    public partial class flatTile : UserControl
    {
        public flatTile()
        {
            InitializeComponent();
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), //required for the text to display
            Description("The text associated with the control."), Category("Data")]
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

        private void statusLabel_SizeChanged(object sender, System.EventArgs e)
        {
            statusLabel.Left = (this.ClientSize.Width - statusLabel.Size.Width) / 2;
        }
    }
}
