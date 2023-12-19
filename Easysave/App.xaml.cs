using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace EasySave
{
    public partial class App : Application
    {
        private Mutex _appMutex;
        private Timer _processMonitorTimer;
        private bool isBusinessAppRunning = false;

        private SaveViewModel _viewModel = new SaveViewModel();
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _appMutex = new Mutex(true, "Easysave_V3", out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Another instance of the application is already running.");
                Current.Shutdown();
                return;
            }

            // Start process monitoring timer
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
            string processName = "Safari"; // Replace with your desired process name

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
