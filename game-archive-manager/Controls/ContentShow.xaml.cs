using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using game_archive_manager.DataItems;
namespace game_archive_manager.Controls
{
    public sealed partial class ContentShow : UserControl, INotifyPropertyChanged
    {
        // 数据源 需要更改 这里为测试
        private List<GameInfo> _items = new List<GameInfo> {
            GameInfo("/Assets/pic/pic1.png","龙"),
            GameInfo("/Assets/pic/pic2.png","虎"),
            GameInfo("/Assets/pic/pic3.jpg","狼")
        };
        public List<GameInfo> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private static ImageData ImageData(string v1, string v2)
        {
            return new DataItems.ImageData(v1, v2);
        }
        private static GameInfo GameInfo(string pic, string name)
        {
            return new DataItems.GameInfo
            {
                ImageData = new ImageData(pic, name),
                GameName = name
            };
        }


        // 当前索引
        private int _currentIndex = 0;

        // 绑定属性
        public GameInfo? Component1 { get; set; }
        public GameInfo? Component2 { get; set; }
        public GameInfo? Component3 { get; set; }

        public ContentShow()
        {
            this.InitializeComponent();
            DataContext = this;

            // 初始化显示
            UpdateComponents();

            // 绑定鼠标滚轮事件
            this.PointerWheelChanged += OnMouseWheel;

        }

        private void UpdateComponents()
        {
            _currentIndex = (_currentIndex + _items.Count) % _items.Count;
            Component1Control.Content = new GameArchiveShow(GetItemAt(_currentIndex - 1));
            Component2Control.Content = new GameArchiveShow(GetItemAt(_currentIndex));
            Component3Control.Content = new GameArchiveShow(GetItemAt(_currentIndex + 1));

            // 更新绑定
            OnPropertyChanged(nameof(Component1));
            OnPropertyChanged(nameof(Component2));
            OnPropertyChanged(nameof(Component3));
        }

        // 获取指定索引的项（循环）
        private GameInfo GetItemAt(int index)
        {
            if (index < 0)
            {
                index = _items.Count - 1;
            }
            else if (index >= _items.Count)
            {
                index = index % _items.Count;
            }
            return _items[index];
        }

        private void OnPreviousClick(object sender, RoutedEventArgs e)
        {
            AnimateShiftRight();
            _currentIndex--;
            UpdateComponents();
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            AnimateShiftLeft();
            _currentIndex++;
            UpdateComponents();
        }

        private void OnMouseWheel(object sender, PointerRoutedEventArgs e)
        {
            var properties = e.GetCurrentPoint(this).Properties;
            if (properties.MouseWheelDelta > 0)
            {
                // 向上滚动，执行相应操作
                OnPreviousClick(sender, e);
            }
            else
            {
                // 向下滚动，执行相应操作
                OnNextClick(sender, e);
            }
        }

        // INotifyPropertyChanged接口实现
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AnimateShiftLeft()
        {
            var transform1 = new TranslateTransform();
            var transform2 = new TranslateTransform();
            var transform3 = new TranslateTransform();

            //假设控件分别为Component1,Component2,Component3
            // 这里应该是控件而不是字符串
            var component1Control = (UIElement)FindName("Component1Control");
            var component2Control = (UIElement)FindName("Component2Control");
            var component3Control = (UIElement)FindName("Component3Control");

            component1Control.RenderTransform = transform1;
            component2Control.RenderTransform = transform2;
            component3Control.RenderTransform = transform3;

            // 动画
            var animation1 = new DoubleAnimation
            {
                From = component1Control.RenderSize.Width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase()
            };

            var animation2 = new DoubleAnimation
            {
                From = component2Control.RenderSize.Width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase()
            };

            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase()
            };

            // 创建一个Storyboard来处理动画
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation1);
            storyboard.Children.Add(animation2);
            storyboard.Children.Add(fadeInAnimation);

            // 设置动画目标
            Storyboard.SetTarget(animation1, transform1);
            Storyboard.SetTargetProperty(animation1, "X");

            Storyboard.SetTarget(animation2, transform2);
            Storyboard.SetTargetProperty(animation2, "X");

            Storyboard.SetTarget(fadeInAnimation, component3Control);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

            storyboard.Begin();
        }
    
        private void AnimateShiftRight()
        {
            var transform1 = new TranslateTransform();
            var transform2 = new TranslateTransform();
            var transform3 = new TranslateTransform();

            var component1Control = (UIElement)FindName("Component1Control");
            var component2Control = (UIElement)FindName("Component2Control");
            var component3Control = (UIElement)FindName("Component3Control");

            component1Control.RenderTransform = transform1;
            component2Control.RenderTransform = transform2;
            component3Control.RenderTransform = transform3;

            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase()
            };

            var animation2 = new DoubleAnimation
            {
                From = -component2Control.RenderSize.Width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase()
            };


            var animation3 = new DoubleAnimation
            {
                From = -component3Control.RenderSize.Width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase()
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Children.Add(animation2);
            storyboard.Children.Add(animation3);

            Storyboard.SetTarget(fadeInAnimation, component1Control);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

            Storyboard.SetTarget(animation2, transform2);
            Storyboard.SetTargetProperty(animation2, "X");

            Storyboard.SetTarget(animation3, transform3);
            Storyboard.SetTargetProperty(animation3, "X");

            storyboard.Begin();
        }

    }
}
