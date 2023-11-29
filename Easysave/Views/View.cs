using System;
using EasySave.ViewModels;

namespace EasySave.Views
{
    public class View
    {
        ViewModel viewModel = new ViewModel();

        public void ShowSaveInfo(int saveNb)
        {
            String[] saveList = viewModel.GetSaveInfo(saveNb);
        }
        public static void ShowSaveList()
        {
            Console.WriteLine("list");
        }
        public void ShowSaveError() { }
        public void ShowSaveProgress() { }
        public void GetSaveInput() { }
        public object GetParametersInput(bool isFirstTime, int step)
        {
            string Lang = "";
            string TargetDir = "";
            string SaveLogDir = "";
            string SaveStateDir = "";
            if (isFirstTime)
            {
                Console.WriteLine("");
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===               Paramétrage initial / Initial Setup               ===|");
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("");
                
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===            Veuillez choisir une langue [eng / fr]               ===|");
                Console.WriteLine("|===                 Choose a language [eng / fr]                    ===|");
                Console.WriteLine("|=======================================================================|");
                Lang = Console.ReadLine() ?? "";
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===           Veuillez choisir le chemin des sauvegarde             ===|");
                Console.WriteLine("|===                Choose the path of the backups                   ===|");
                Console.WriteLine("|=======================================================================|");
                TargetDir = Console.ReadLine() ?? "";
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===        Veuillez choisir le chemin du fichier log.json           ===|");
                Console.WriteLine("|===             Choose the path of the log.json file                ===|");
                Console.WriteLine("|=======================================================================|");
                SaveLogDir = Console.ReadLine() ?? "";
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===        Veuillez choisir le chemin du fichier state.json         ===|");
                Console.WriteLine("|===            Choose the path of the state.json file               ===|");
                Console.WriteLine("|=======================================================================|");
                SaveStateDir = Console.ReadLine() ?? "";
            }
            else
            {
                string selectedLang = "fr";
                switch (step)
                {
                    case 1:
                        if (selectedLang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===             Veuillez choisir une langue [eng / fr]            ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===                 Choose a language [eng / fr]                  ===|");
                        }
                        Lang = Console.ReadLine() ?? "";
                        break;
                    case 2:
                        if (selectedLang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===           Veuillez choisir le chemin des sauvegarde           ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===                Choose the path of the backups                 ===|");
                        }
                        TargetDir = Console.ReadLine() ?? "";
                        break;
                    case 3:
                        if (selectedLang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===        Veuillez choisir le chemin du fichier log.json         ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===             Choose the path of the log.json file              ===|");
                        }
                        SaveLogDir = Console.ReadLine() ?? "";
                        break;
                    case 4:
                        if (selectedLang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===        Veuillez choisir le chemin du fichier state.json       ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===            Choose the path of the state.json file             ===|");
                        }
                        SaveStateDir = Console.ReadLine() ?? "";
                        break;
                    default:
                        Console.WriteLine("Error : Unrecognized step");
                        break;
                }
            }

            object inputObj = new
            {
                lang = Lang,
                targetDir = TargetDir,
                saveLogDir = SaveLogDir,
                saveStateDir = SaveStateDir
            };

            return inputObj;
        }
        public bool ValidateInput(string inputType, string input)
        {
            if (inputType.Length == 0)
            {
                Console.WriteLine("No input given");
                return false;
            }
            switch (inputType)
            {
                case "navigation":

                    break;

                case "SaveInfo":

                    break;

                case "parameters":

                    break;

                default:
                    break;
            }

            return true;
        }
    }
}