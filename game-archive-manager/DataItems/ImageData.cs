using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;


namespace game_archive_manager.DataItems
{
    public class ImageData : INotifyPropertyChanged
    {
        private string _imagePath = string.Empty; // 初始化为非 null 值
        private string _name = string.Empty; // 初始化为非 null 值

        public ImageData()
        {
            _imagePath = string.Empty;
            _name = string.Empty;
        }
        public ImageData(string imagePath, string name)
        {
            ImagePath = imagePath;
            Name = name;
        }
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ImageSource)); // 当路径改变时也通知ImageSource更新
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        // Updated the ImageSource property to ensure it does not return null by using a default BitmapImage instance.
        public BitmapImage ImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    // Return a default BitmapImage instance instead of null
                    return new BitmapImage();
                }

                var bitmap = new BitmapImage();
                Uri imageUri;
                if (Uri.IsWellFormedUriString(ImagePath, UriKind.Absolute))
                {
                    imageUri = new Uri(ImagePath, UriKind.Absolute);
                }
                else
                {
                    // Assuming the base path is the application's base directory
                    var basePath = "ms-appx:///";
                    imageUri = new Uri(basePath + ImagePath.TrimStart('/'), UriKind.Absolute);
                }
                bitmap.UriSource = imageUri;
                return bitmap;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
