using Microsoft.Win32;
using sUpdater.Models;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace sUpdater.Helpers
{
    public static class IconHelper
    {
        public static BitmapSource GetIconFromFile(string filePath)
        {
            using var sysicon = Icon.ExtractAssociatedIcon(filePath);

            var bitmap = Imaging.CreateBitmapSourceFromHIcon(sysicon.Handle, Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());
            bitmap.Freeze();

            return bitmap;
        }

        public static void PopulatePortableAppIcon(PortableApp portableApp, string exePath)
        {
            if (portableApp.Icon == null && File.Exists(exePath))
            {
                portableApp.Icon = GetIconFromFile(exePath);
            }
        }

        public static BitmapSource GetIconFromPackageId(string packageId)
        {
            var splitId = packageId.Split("\\");
            if (splitId.Length < 4) return null;

            string regKey = "";
            regKey += splitId[1] == "Machine" ? "HKEY_LOCAL_MACHINE" : "HKEY_CURRENT_USER";
            regKey += "\\SOFTWARE";

            if (splitId[2] == "X86")
                regKey += "\\WOW6432Node";

            regKey += "\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\";
            regKey += splitId[3];

            string iconPath = (string)Registry.GetValue(regKey, "DisplayIcon", null);
            if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
                return GetIconFromFile(iconPath);

            return null;
        }
    }
}
