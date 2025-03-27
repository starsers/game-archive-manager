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

using ExampleApp;
using System.Threading;
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
            //ShowSignInDialog();  
            //ShowSignInDialogButton_Click(loginButton, new RoutedEventArgs());
            //myButton_Click(myButton, new RoutedEventArgs());
        }

        private async void ShowSignInDialogButton_Click(object sender, RoutedEventArgs e) // Changed from private to public  
        {
            SignInContentDialog signInDialog = new SignInContentDialog();
            signInDialog.XamlRoot = this.Content.XamlRoot; // �ܽ��������֪��ʲôԭ��  
            await signInDialog.ShowAsync();

            if (signInDialog.Result == SignInResult.SignInOK)
            {
                // Sign in was successful.  
            }
            else if (signInDialog.Result == SignInResult.SignInFail)
            {
                // Sign in failed.  
            }
            else if (signInDialog.Result == SignInResult.SignInCancel)
            {
                // Sign in was cancelled by the user.  
            }
        }


        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
