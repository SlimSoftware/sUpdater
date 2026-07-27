using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using sUpdater.Models.Apps;

namespace sUpdater.Controls
{
    public partial class CustomListViewStyle : ResourceDictionary
    {
        private void OnListViewItemPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Don't deselect item when right clicked
            e.Handled = true;
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image image && image.DataContext is WinGetApp winGetApp)
            {
                // Load icon asynchronously when the image becomes visible
                _ = winGetApp.LoadIconAsync();
            }
        }
    }
}
