using System.Windows;
using System.Windows.Input;

namespace sUpdater.Controls
{
    public partial class CustomListViewStyle : ResourceDictionary
    {
        private void OnListViewItemPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Don't deselect item when right clicked
            e.Handled = true;
        }
    }
}
