using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using EasySave.Views;
using EasySave.Models;
using EasySave.ViewModels;


namespace EasySave
{
    class Program
    {

        public static void Main(string[] args)
        {
            var view = new View();
            Config configObj = Config.getConfig();

            Console.WriteLine(configObj.TargetDir);

            State state = new State();
            state.UpdateStatefile(2,"testSave","01/01/2025 00:00","full",false,"path", configObj.TargetDir,10,100,0,0,0);
            //state.CreateState();

            return;

            bool configExists = configObj.checkConfig();
            if (configExists)
            {
                Navigation.ShowMainMenu();
            }
            else
            {
                Navigation.ShowFirstLaunchMenu();
                var parametersInput = view.GetParametersInput(true, 0);
                Config.SaveConfig(parametersInput);
            }
        }
    }
}







