using game_archive_manager.Helper;
using game_archive_manager.Pages;
using game_archive_manager.Services;
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
using game_archive_manager.Models;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private UserRepository _userRepository;

        public LoginPage()
        {
            this.InitializeComponent();
            _userRepository = new UserRepository();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("用户名和密码不能为空");
                return;
            }

            DataItems.User user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                ShowError("用户名不存在");
                return;
            }

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                ShowError("密码错误");
                return;
            }

            // 登录成功
            infoBar.Title = "成功";
            infoBar.Message = "登录成功";
            infoBar.Severity = InfoBarSeverity.Success;
            infoBar.IsOpen = true;

            // 导航到应用主页面
            Frame.Navigate(typeof(HomePage), null, new DrillInNavigationTransitionInfo());

        }

        private void lnkRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void ShowError(string message)
        {
            infoBar.Title = "错误";
            infoBar.Message = message;
            infoBar.Severity = InfoBarSeverity.Error;
            infoBar.IsOpen = true;
        }
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                btnLogin_Click(this, new RoutedEventArgs());
            }
        }
    }
}
