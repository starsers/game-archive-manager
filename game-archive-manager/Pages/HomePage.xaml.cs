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

using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

using Microsoft.UI.Xaml.Media.Animation;
using ExampleApp;

using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Storage;
using Windows.Gaming.Preview;
using game_archive_manager.DataItems;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        //public ObservableCollection<ControlInfoDataItem> Items { get; set; }

        private List<string> _gameNames
        {
            get => GamesContentShow.Items.OfType<ImageData>()
                .Select(item => item.Name)
                .ToList();
        }

        public HomePage()
        {
            this.InitializeComponent();
            //ObservableCollection<ControlInfoDataItem> controlInfoDataItems = new ObservableCollection<ControlInfoDataItem>
            //{
            //    new ControlInfoDataItem { Title = "Item 1", ImagePath = "/Assets/pic/pic1.png" },
            //    new ControlInfoDataItem { Title = "Item 2", ImagePath = "/Assets/pic/pic2.png" },
            //    new ControlInfoDataItem { Title = "Item 3", ImagePath = "/Assets/pic/pic3.jpg" }
            //};
            //Items = controlInfoDataItems;
            //this.DataContext = this;
        }
        // Handle text change and present suitable items
        private void Search_Control_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var game in _gameNames)
                {
                    var found = splitText.All((key) =>
                    {
                        return game.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(game);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }
        // Handle user selecting an item, in our case just output the selected item.
        private void Search_Control_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SearchBox.Text = args.SelectedItem.ToString();
        }
        // Add this method to the HomePage class
        private void Search_Control_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            // Handle the QuerySubmitted event here
            // For example, you can log the query text or perform a search
            string queryText = args.QueryText;
            System.Diagnostics.Debug.WriteLine($"Query submitted: {queryText}");

            //这里到时候加入定位的语句
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

        private void AddGame_Button_Click(object sender, RoutedEventArgs e)
        {
            // 添加游戏
        }
    }
}
