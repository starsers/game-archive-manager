using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Microsoft.UI.Windowing;

using ExampleApp;
using System.Threading;
using Windows.UI.WindowManagement;
using WinRT.Interop;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager
{
    /// <summary>  
    /// An empty window that can be used on its own or navigated to within a Frame.  
    /// </summary>  
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MainFrame = new Frame(); // Initialize MainFrame
            this.Content = MainFrame; // Set MainFrame as the content of the window
            MainFrame.Navigate(typeof(HomePage));
        }

      
        private async void MainWindow_Closing(object sender, AppWindowClosingEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Content = "Close it?";
            contentDialog.XamlRoot = this.Content.XamlRoot;
            contentDialog.PrimaryButtonText = "Yes";
            contentDialog.CloseButtonText = "No";
            contentDialog.IsPrimaryButtonEnabled = true;
            var r = await contentDialog.ShowAsync();
            if (r == ContentDialogResult.Primary)
            {
                // Cancel the close event
                e.Cancel = true;
            }
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
