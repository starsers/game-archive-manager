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
using game_archive_manager.DataItems;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace game_archive_manager.Controls
{
    // GameArchiveShow.xaml.cs
    public partial class GameArchiveShow : UserControl, INotifyPropertyChanged
    {

        public GameArchiveShow()
        {
            this.InitializeComponent();
            this.DataContext = this; // 确保 DataContext 正确设置
        }

        public GameArchiveShow(ImageData imageData)
        {
            this.InitializeComponent();
            ImageData = imageData;
            this.DataContext = this; // 确保 DataContext 正确设置
        }

        public GameArchiveShow(string imagePath, string name)
        {
            this.InitializeComponent();
            ImageData = new ImageData { ImagePath = imagePath, Name = name };
            this.DataContext = this; // 确保 DataContext 正确设置
        }

        public static readonly DependencyProperty ImageDataProperty =
            DependencyProperty.Register("ImageData", typeof(ImageData), typeof(GameArchiveShow),
                new PropertyMetadata(null));

        public ImageData ImageData
        {
            get { return (ImageData)GetValue(ImageDataProperty); }
            set { 
                SetValue(ImageDataProperty, value);
                OnPropertyChanged(nameof(ImageDataProperty));
            }
        }
        // 解决 CS8612 错误：将事件声明为可空类型
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([AllowNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开编辑窗口
            // 创建一个
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "GameConfig";
            dialog.PrimaryButtonText = "Save";
            dialog.SecondaryButtonText = "Don't Save";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new ContentDialogs.GameConfig();

            var result = await dialog.ShowAsync();

        }
    
    }
}
