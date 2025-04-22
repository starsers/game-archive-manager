using game_archive_manager.Helper;
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
using Microsoft.UI;
using static game_archive_manager.Helper.PasswordHasher;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager.Pages
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private UserRepository _userRepository;

        public RegisterPage()
        {
            this.InitializeComponent();
            _userRepository = new UserRepository();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var strength = PasswordHasher.CheckPasswordStrength(txtPassword.Password);

            // 更新UI显示
            switch (strength)
            {
                case PasswordStrength.Weak:
                    strengthIndicator.Fill = new SolidColorBrush(Colors.Red);
                    strengthText.Text = "弱";
                    break;
                case PasswordStrength.Medium:
                    strengthIndicator.Fill = new SolidColorBrush(Colors.Orange);
                    strengthText.Text = "中";
                    break;
                case PasswordStrength.Strong:
                    strengthIndicator.Fill = new SolidColorBrush(Colors.Green);
                    strengthText.Text = "强";
                    break;
            }
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("用户名和密码不能为空");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("两次输入的密码不一致");
                return;
            }
            var strength = PasswordHasher.CheckPasswordStrength(password);
            if (strength == PasswordStrength.Weak)
            {
                ShowError("密码强度不足，请使用至少8位包含字母和数字的组合");
                return;
            }
                DataItems.User existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                ShowError("用户名已存在");
                return;
            }

            User newUser = new User
            {
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(password)
            };

            await _userRepository.AddUserAsync(new DataItems.User(newUser));

            infoBar.Title = "成功";
            infoBar.Message = "注册成功";
            infoBar.Severity = InfoBarSeverity.Success;
            infoBar.IsOpen = true;

            // 注册成功后延迟返回登录页
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) => {
                timer.Stop();
                Frame.Navigate(typeof(LoginPage));
            };
            timer.Start();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void ShowError(string message)
        {
            infoBar.Title = "错误";
            infoBar.Message = message;
            infoBar.Severity = InfoBarSeverity.Error;
            infoBar.IsOpen = true;
        }
    }
}
