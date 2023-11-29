using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using EasySave.Views;
using EasySave.ViewModels;


namespace EasySave
{ 
    class Program
    {
        public static void Main(string[] args)
        {
            Config configObj = Config.getConfig();
            bool configExists = configObj.checkConfig();
            if(configExists)
            {
                Navigation.ShowMainMenu(); 
            }
            else
            {
                Navigation.ShowFirstLaunchMenu();
                View.GetParametersInput();
            }


        }
    }
}







