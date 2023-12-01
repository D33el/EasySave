using System;
using Easysave.ViewModels;

namespace Easysave.Views
{
    public class View
    {
        ViewModel viewModel = new ViewModel();

        public void ShowSaveInfo(int saveNb)
        {
            String[] saveList = viewModel.GetSaveInfo(saveNb);
        }
        public void ShowSaveList()
        {
            Console.WriteLine("list");
        }
        public void ShowSaveError() { }
        public void ShowSaveProgress() { }
        public void GetSaveInput() {
            // formulaire de sauvegarde 
        }
        public DataConfig GetParametersInput(bool isFirstTime)
        {
            string lang = "";
            string targetDir = "";
            string saveLogDir = "";
            string saveStateDir = "";

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
                string selectedLang = "fr";
                
                if (selectedLang == "fr")
                {
                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===             Veuillez choisir une langue [eng / fr]            ===|");
                    lang = Console.ReadLine();
                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===           Veuillez choisir le chemin des sauvegarde           ===|");
                    targetDir = Console.ReadLine();
                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===        Veuillez choisir le chemin du fichier log.json         ===|");
                    saveLogDir = Console.ReadLine();
                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===        Veuillez choisir le chemin du fichier state.json       ===|");
                    saveStateDir = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===                 Choose a language [eng / fr]                  ===|");
                    lang = Console.ReadLine();

                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===                Choose the path of the backups                 ===|");
                    targetDir = Console.ReadLine();

                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===             Choose the path of the log.json file              ===|");
                    saveLogDir = Console.ReadLine();

                    Console.WriteLine("|=====================================================================|");
                    Console.WriteLine("|===            Choose the path of the state.json file             ===|");
                    saveStateDir = Console.ReadLine();

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