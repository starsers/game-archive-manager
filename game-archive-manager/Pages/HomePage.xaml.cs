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


using Microsoft.UI.Xaml.Media.Animation;
using ExampleApp;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }
        


        private void NavigateToSettingsPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage), null, new DrillInNavigationTransitionInfo());
        }
        private async void ShowSignInDialogButton_Click(object sender, RoutedEventArgs e)
        {
            SignInContentDialog signInDialog = new SignInContentDialog();
            signInDialog.XamlRoot = this.Content.XamlRoot;
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

    }
}
