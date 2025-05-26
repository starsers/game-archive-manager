using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;
using Microsoft.UI.Xaml;
using game_archive_manager;
namespace game_archive_manager.Helper
{
    internal static class WindowHelper
    {
        public static nint hwnd
        {
            get => WindowNative.GetWindowHandle(target: ((App)Application.Current).GetWindow());
        }
        private static void addYinYong()
        {
            FileHelper.OpenEncryptedFile("a","a");
            FileHelper.SaveEncryptedFile("a", "a", "a");
        }

    }

}
