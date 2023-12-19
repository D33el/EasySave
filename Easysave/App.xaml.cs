using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _appMutex;

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

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _appMutex?.ReleaseMutex();
            _appMutex?.Dispose();
            base.OnExit(e);
        }
    }


}
