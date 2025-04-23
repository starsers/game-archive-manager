using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Serilog;
using System.Diagnostics;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace game_archive_manager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Debug.WriteLine($"Current Directory: {currentDirectory}");
            string logPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GameArchiveManger", "logs","log.txt");
            // 配置 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // 设置日志级别
                .WriteTo.Console() // 输出到控制台
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day) // 输出到文件
                .CreateLogger();

            Log.Information("Application starting...");
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ExtendsContentIntoTitleBar = true;
            
            loginWindow.Activate();
            Log.Information("Login window activated.");
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window? m_window;
        public Window GetWindow()
        {
            return m_window ?? throw new InvalidOperationException("Window has not been initialized.");
        }
        ~App()
        {
            Log.Information("Application exiting...");
            Log.CloseAndFlush();
        }
        public Window MainWindow
        {
            get => GetWindow();
        }
    }
}
