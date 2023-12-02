using System;
using Easysave.Models;
using Easysave.ViewModels;

namespace Easysave.Views
{
    public class View
    {
        ViewModel viewModel = new ViewModel();
        Config configObj = Config.getConfig();

        public void ShowSaveInfo(int saveNb)
        {
            DataState saveList = viewModel.GetSaveInfo(saveNb);
        }
        public void ShowSaveList()
        {
            DataState[] resultArr = viewModel.GetSavelist();

            foreach (var result in resultArr)
            {
                Console.WriteLine($"|==== [{result.SaveId}] {result.SaveName}  {result.Time}");
            }
        }
        public void ShowSaveError()
        {
        }
        public void ShowSaveProgress() { }
        public void GetSaveInput()
        {
            // formulaire de sauvegarde 
        }
        public DataConfig GetParametersInput(bool isFirstTime, int step)
        {
            string lang;
            string targetDir;
            string saveLogDir;
            string saveStateDir;

            if (isFirstTime || step == 0)
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
                lang = Console.ReadLine() ?? "";

                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===           Veuillez choisir le chemin des sauvegarde             ===|");
                Console.WriteLine("|===                Choose the path of the backups                   ===|");
                Console.WriteLine("|=======================================================================|");
                targetDir = Console.ReadLine() ?? "";

                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===        Veuillez choisir le chemin du fichier log.json           ===|");
                Console.WriteLine("|===             Choose the path of the log.json file                ===|");
                Console.WriteLine("|=======================================================================|");
                saveLogDir = Console.ReadLine() ?? "";

                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===        Veuillez choisir le chemin du fichier state.json         ===|");
                Console.WriteLine("|===            Choose the path of the state.json file               ===|");
                Console.WriteLine("|=======================================================================|");
                saveStateDir = Console.ReadLine() ?? "";
            }
            else
            {
                string selectedLang = configObj.Language;
                lang = selectedLang;
                targetDir = configObj.TargetDir;
                saveStateDir = configObj.SaveStateDir;
                saveLogDir = configObj.SaveLogDir;


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
                        lang = Console.ReadLine();
                        break;

                    case 2:
                        if (selectedLang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===           Veuillez choisir le chemin des sauvegardes          ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===                Choose the path of the backups                 ===|");
                        }

                        targetDir = Console.ReadLine();
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
                        saveLogDir = Console.ReadLine();
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
                        saveStateDir = Console.ReadLine();
                        break;
                }


            }

            DataConfig inputObj = new DataConfig();
            inputObj.Language = lang;
            inputObj.TargetDir = targetDir;
            inputObj.SaveStateDir = saveStateDir;
            inputObj.SaveLogDir = saveLogDir;

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

        public int GetInput(int step)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Entrée invalide. Veuillez saisir un nombre entier.");
                return GetInput(step);
            }
        }

    }
}