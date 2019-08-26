using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sUpdater.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class FlatTile : UserControl
    {
        public FlatTile()
        {
            InitializeComponent();
            Background = Colors.normalGreenBrush;
        }

        public string Title
        {
            get { return title.Content.ToString(); }
            set { title.Content = value.ToString(); }
        }

        public ImageSource Image
        {
            get { return icon.Source; }
            set { icon.Source = value; }
        }
      
        private void Title_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Center text
            title.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void FlatTile_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Background == Colors.normalOrangeBrush)
            {
                Background = Colors.hoverOrangeBrush;
            }
            else if (Background == Colors.normalGreyBrush)
            {
                Background = Colors.hoverGreyBrush;
            }
            else
            {
                Background = Colors.hoverGreenBrush;
            }
        }

        private void FlatTile_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Background == Colors.hoverOrangeBrush)
            {
                Background = Colors.normalOrangeBrush;
            }
            else if (Background == Colors.hoverGreyBrush)
            {
                Background = Colors.normalGreyBrush;
            }
            else
            {
                Background = Colors.normalGreenBrush;
            }
        }
    }
}
