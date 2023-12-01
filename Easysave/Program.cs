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
            ViewModel viewmodel  = new ViewModel();

            bool configExists = configObj.checkConfig();

            if (configExists)
            {
                Navigation.ShowMainMenu();
                var Input = view.GetInput(0);
                viewmodel.NavigateTo(Input);

            }

            else
            
            {
                Navigation.ShowFirstLaunchMenu();
                DataConfig data = view.GetParametersInput(false);
                configObj.DataConfig = data;
                configObj.SaveConfig();
            }
        }
    }
}







