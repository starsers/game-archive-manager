using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using System.Windows;
using System.Runtime.InteropServices;

namespace game_archive_manager
{
    public sealed partial class LoginWindow : Window
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public Window? Owner; // Make Owner nullable
        private double Left, Top;

        public LoginWindow()
        {
            InitializeComponent();
            //Owner = this;
            //SetTopmost();
            //this.Activate(); // ¼¤»î´°¿Ú
            MainFrame= new Frame();
            this.Content = MainFrame;
            MainFrame.Navigate(typeof(LoginPage));
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle login logic here
            // this.DialogResult = true; // Removed as DialogResult is not defined in LoginWindow
            this.Close();
        }

        public void CenterOnScreen()
        {
            var windowBounds = this.Bounds;
            //var ownerBounds = Owner?.Bounds ?? new Rect(0, 0, 0, 0);
            //this.Left = (windowBounds.Right - windowBounds.Width) / 2;
            //this.Top = (windowBounds.Top - windowBounds.Height) / 2;
            SetTopmost();
        }


        private void SetTopmost()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }
    }
}