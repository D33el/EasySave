using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using EasySave.ViewModels;
using EasySave.Views;

namespace EasySave
{
    public partial class App : Application
    {
        private Cloud cloudServer;
        private Mutex _appMutex;
        private Timer _processMonitorTimer;
        private bool isBusinessAppRunning = false;
        private Config _config = Config.GetConfig();
        private SaveViewModel _viewModel = new SaveViewModel();
        private Cloud server = new Cloud();
        public static bool LanguageSet = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            server.StartServer();

            base.OnStartup(e);

            bool createdNew;
            _appMutex = new Mutex(true, "Easysave", out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Another instance of the application is already running.");
                Current.Shutdown();
                return;
            }

            _processMonitorTimer = new Timer(CheckBusinessApp, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            Debug.WriteLine("Application Started");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //server.StopServer();
            _appMutex?.ReleaseMutex();
            _appMutex?.Dispose();
            _processMonitorTimer?.Dispose();
            Debug.WriteLine("Application Exiting");
            base.OnExit(e);
        }

        private void CheckBusinessApp(object state)
        {
            try
            {
                string processName = _config.BlockingApp;
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    if (!isBusinessAppRunning)
                    {
                        isBusinessAppRunning = true;
                        Current.Dispatcher.Invoke(() =>
                        {
                            _viewModel.PauseBackups();
                        });
                    }
                }
                else
                {
                    if (isBusinessAppRunning)
                    {
                        isBusinessAppRunning = false;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _viewModel.ResumeBackups();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CheckBusinessApp: {ex.Message}");
            }
        }

        public static void ChangeCulture(CultureInfo newCulture)
        {
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            var oldWindow = Application.Current.MainWindow;
            LanguageSet = true;
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            oldWindow.Close();
        }
    }
}

