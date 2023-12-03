using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Easysave.Views;
using Easysave.Models;
using Easysave.ViewModels;


namespace Easysave
{
    class Program
    {



        public static void Main(string[] args)
        {
            Config configObj = Config.getConfig();
            View view = new();

            bool configExists = configObj.checkConfig();

            if (!configExists)
            {
                View.ShowFirstLaunchMenu();
                DataConfig data = view.GetParametersInput(false, 0);
                configObj.DataConfig = data;
                configObj.SaveConfig();
                configObj.LoadConfig();
            }
            view.ShowMainMenu();
        }
    }
}
