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
            View view = new View();
            State state = new State();
            Navigation navigation = new Navigation();
            ViewModel viewmodel  = new ViewModel();

            bool configExists = configObj.checkConfig();

            if (configExists)
            {
                navigation.ShowMainMenu();

            }

            else
            
            {
                Navigation.ShowFirstLaunchMenu();
                DataConfig data = view.GetParametersInput(false, 0);
                configObj.DataConfig = data;
                configObj.SaveConfig();
            }
        }
    }
}







