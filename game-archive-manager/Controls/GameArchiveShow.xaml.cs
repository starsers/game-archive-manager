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

namespace game_archive_manager.Controls
{
    // GameArchiveShow.xaml.cs
    public sealed partial class GameArchiveShow : UserControl
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
            set { SetValue(ImageDataProperty, value); }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开编辑窗口
            // 待完成
        }
    }
}
