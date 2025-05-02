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
using Windows.Storage;
using WinRT.Interop;
using CommunityToolkit.WinUI.Controls;
using System.Threading.Tasks;

using game_archive_manager.Helper;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager.Controls
{
    public sealed partial class ImageUploader : UserControl
    {
        private double ImageAspectRatio = 3d/4d;
        private CropShape ImageCropShape = CropShape.Rectangular;
        private ThumbPlacement ImageCropperSample = ThumbPlacement.Corners;
        private string ImageFileName = string.Empty;
        public string SavingPath = string.Empty;
        public string savingFileName = "Cropped_Image";
        private bool CanCloseUploadImageFlyout = false; // 用于控制 Flyout 是否可以关闭
        public ImageUploader()
        {
            this.InitializeComponent();
            CloseButton.Tag= UploadImageFlyout;
            //_ = Load();

        }
        private async Task Load()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Owl.jpg"));
            await imageCropper.LoadImageFromFile(file);
            ImageFileName = file.DisplayName;
        }

        private async Task PickImage()
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
            };

            var hwnd = WindowNative.GetWindowHandle(target: ((App)Application.Current).GetWindow());
            InitializeWithWindow.Initialize(filePicker, hwnd);

            var file = await filePicker.PickSingleFileAsync();
            if (file != null && imageCropper != null)
            {
                await imageCropper.LoadImageFromFile(file);
                ImageFileName = file.Name;

            }
        }

        private async Task SaveCroppedImage()
        {
            //var savePicker = new FileSavePicker
            //{
            //    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            //    SuggestedFileName = "Cropped_Image",
            //    FileTypeChoices =
            //    {
            //        { "PNG Picture", new List<string> { ".png" } },
            //        { "JPEG Picture", new List<string> { ".jpg" } }
            //    }
            //};
            //var hwnd = WindowHelper.hwnd;
            //InitializeWithWindow.Initialize(savePicker, hwnd);
            //var imageFile = await savePicker.PickSaveFileAsync();
            //if (imageFile != null)
            //{
            //    BitmapFileFormat bitmapFileFormat;
            //    switch (imageFile.FileType.ToLower())
            //    {
            //        case ".png":
            //            bitmapFileFormat = BitmapFileFormat.Png;
            //            break;
            //        case ".jpg":
            //            bitmapFileFormat = BitmapFileFormat.Jpeg;
            //            break;
            //        default:
            //            bitmapFileFormat = BitmapFileFormat.Png;
            //            break;
            //    }

            //    using (var fileStream = await imageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            //    {
            //        await imageCropper.SaveAsync(fileStream, bitmapFileFormat);
            //    }
            //}
                        // 指定保存路径
            if(string.IsNullOrEmpty(ImageFileName))
            {
                return;
            }
            var localFolder = ApplicationData.Current.LocalFolder;
            var fileName = savingFileName + FileHelper.GetFileType(ImageFileName);
            var imageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            SavingPath = imageFile.Path;
            if (imageFile != null)
            {
                BitmapFileFormat bitmapFileFormat;
                switch (imageFile.FileType.ToLower())
                {
                    case ".png":
                        bitmapFileFormat = BitmapFileFormat.Png;
                        break;
                    case ".jpg":
                        bitmapFileFormat = BitmapFileFormat.Jpeg;
                        break;
                    default:
                        bitmapFileFormat = BitmapFileFormat.Png;
                        break;
                }
                using (var fileStream = await imageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
                {
                    await imageCropper.SaveAsync(fileStream, bitmapFileFormat);
                }
            }
        }

        private async void PickButton_Click(object sender, RoutedEventArgs e)
        {
            await PickImage();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveCroppedImage();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            imageCropper.Reset();
        }



        //private async void UploadImageButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var picker = new FileOpenPicker();
        //    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //    picker.FileTypeFilter.Add(".jpg");
        //    picker.FileTypeFilter.Add(".jpeg");
        //    picker.FileTypeFilter.Add(".png");

        //    var hwnd = WindowNative.GetWindowHandle(target: ((App)Application.Current).GetWindow());
        //    InitializeWithWindow.Initialize(picker, hwnd);

        //    var file = await picker.PickSingleFileAsync();
        //    if (file != null)
        //    {
        //        // 将文件复制到本地存储
        //        //var localFolder = ApplicationData.Current.LocalFolder;
        //        //var copiedFile = await file.CopyAsync(localFolder, file.Name, NameCollisionOption.ReplaceExisting);

        //        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
        //        {
        //            BitmapImage bitmapImage = new BitmapImage();
        //            await bitmapImage.SetSourceAsync(fileStream);
        //            UploadedImage.Source = bitmapImage;
        //        }
        //    }
        //}
        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            CanCloseUploadImageFlyout=false; // 设置为不可以关闭
        }
        private void Flyout_Closing(FlyoutBase sender, FlyoutBaseClosingEventArgs args)
        {
            if (CanCloseUploadImageFlyout)
            {
                // 允许 Flyout 关闭
                args.Cancel = false;
            }
            else
            {
                // 不允许 Flyout 关闭
                args.Cancel = true;
            }
        }
        private void CloseFlyout_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.Tag is Flyout flyout)
            {
                CanCloseUploadImageFlyout = true; // 设置为可以关闭
                // 操作 Flyout
                flyout.Hide();
            }
        }
    }
}
