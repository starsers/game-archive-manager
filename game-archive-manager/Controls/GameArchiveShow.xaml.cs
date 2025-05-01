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
using System.Diagnostics;
using game_archive_manager.ContentDialogs;
using System.Collections.ObjectModel;

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
        public GameArchiveShow(GameInfo gameInfo)
        {
            this.InitializeComponent();
            GameInfo = gameInfo;
            this.DataContext = this; // 确保 DataContext 正确设置
        }

        //public static readonly DependencyProperty ImageDataProperty =
        //    DependencyProperty.Register("ImageData", typeof(ImageData), typeof(GameArchiveShow),
        //        new PropertyMetadata(null));

        public ImageData ImageData
        {
            get {
                //return (ImageData)GetValue(ImageDataProperty);
                // 使用GameInfo代替原本的ImageData
                return gameInfo.ImageData;
            }
            set { 
                //SetValue(ImageDataProperty, value);
                //OnPropertyChanged(nameof(ImageDataProperty));
                // 使用GameInfo代替原本的ImageData
                if (value != null)
                {
                    gameInfo.ImageData = value;
                }
                else
                {
                    gameInfo.ImageData = new ImageData();
                }
                OnPropertyChanged(nameof(ImageData));
            }
        }

        private GameInfo gameInfo = new GameInfo();
        public GameInfo GameInfo { get => gameInfo; set => gameInfo = value; } 
        public Archive SelectArchive = new Archive();
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([AllowNull] string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Rule> ActiveRules
        {
            get
            {
                return GameInfo.ActiveRules;
            }
        }
        private void Rules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 处理规则选择变化
            if (sender is ComboBox comboBox && comboBox.SelectedItem is Rule selectedRule)
            {
                // 更新选中的规则
                SelectedRule = selectedRule;
                Debug.WriteLine($"Selected Rule: {SelectedRule.RuleName}");
                // 这里可以添加其他逻辑，比如更新界面或保存选中的规则
                RuleContentMatch.Text = selectedRule.RuleName;
            }

        }
        public Rule SelectedRule
        {
            get; set;
        } = new Rule(); // 选中的规则
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // 打开编辑窗口
            // 创建一个
            ContentDialog dialog = new ContentDialog
            {

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "GameConfig",
                PrimaryButtonText = "Save",
                SecondaryButtonText = "Don't Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = new ContentDialogs.GameConfig()
            };


            var result = await dialog.ShowAsync();

            // 检查用户点击了哪个按钮
            if (result == ContentDialogResult.Primary)
            {
                // 用户点击了 "Save"，获取 GameConfig 的成员变量
                GameConfig? a = dialog.Content as GameConfig;
                if (a == null)
                {
                    Debug.WriteLine("GameConfig is null");
                    return;
                }
                if (a.ActiveMatchRules == null)
                {
                    Debug.WriteLine("ActiveMatchRules is null");
                    return;
                }
                else
                {
                    Debug.WriteLine("ActiveMatchRules is not null");
                }

                // 将值传递到外部逻辑
                Debug.WriteLine($"Config Value: {a.ActiveMatchRules}");
                GameInfo.ActiveRules.Clear();
                foreach (var item in a.ActiveMatchRules)
                {
                    GameInfo.ActiveRules.Add(
                        new Rule
                        {
                            RuleName = item.RuleName,
                            RuleId = item.RuleId
                        }
                    );
                    Debug.WriteLine(
                        $"RuleName: {item.RuleName}, RuleId: {item.RuleId} "
                    );
                }
                OnPropertyChanged(nameof(GameInfo));

            }
            else
            {
                // 用户点击了 "Don't Save" 或 "Cancel"
                Debug.WriteLine("Dialog was canceled or not saved.");
            }

        }

        private void Archive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 处理选择变化
            if (sender is ComboBox comboBox && comboBox.SelectedItem is Archive SelectArchive)
            {
                // 加载文件
                // 这里需要根据加密属性判断是否需要解密文件并加载

            }
        }
    }
}
