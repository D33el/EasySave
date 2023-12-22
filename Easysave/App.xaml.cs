using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using EasySave.ViewModels;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            Cloud server = new Cloud();
            server.StartServer();

            bool createdNew;
            _appMutex = new Mutex(true, "Easysave_V3", out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Another instance of the application is already running.");
                Current.Shutdown();
                return;
            }

            // check for process every second
            _processMonitorTimer = new Timer(CheckBusinessApp, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _appMutex?.ReleaseMutex();
            _appMutex?.Dispose();

            _processMonitorTimer?.Dispose();

            base.OnExit(e);
        }

        private void CheckBusinessApp(object state)
        {
            string processName = _config.BlockingApp;

            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                if (!isBusinessAppRunning)
                {
                    isBusinessAppRunning = true;
                    Application.Current.Dispatcher.Invoke(() =>
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
    }
}
