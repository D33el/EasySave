using System;
using System.Text.RegularExpressions;
using Easysave.Models;
using Easysave.ViewModels;

namespace Easysave.Views
{
    public class View
    {
        ViewModel viewModel = new ViewModel();
        Config configObj = Config.getConfig();

        public static void ShowFirstLaunchMenu()
        {
            Console.WriteLine("|==(EasySave V1.0)======================================================|");
            Console.WriteLine("|======================= Bienvenue / Welcome ===========================|");
            Console.WriteLine("|=======================================================================|");
            Console.WriteLine("|===                  Merci d'avoir choisi EasySave                  ===|");
            Console.WriteLine("|===                 Thank you for choosing EasySave                 ===|");
            Console.WriteLine("|=======================================================================|");
            Console.WriteLine("|===   Configurez l'application avant de commencer les sauvegardes   ===|");
            Console.WriteLine("|===              Setup the app before starting backups              ===|");
            Console.WriteLine("|=======================================================================|");
        }

        public int ShowParameters()
        {
            string lang = configObj.Language;
            int step = 0;

            Console.Clear();

            if (lang == "fr")
            {
                Console.WriteLine("|==(EasySave V1.0)====================================================|");
                Console.WriteLine("|=========================== Paramétres ==============================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [0] Tout reconfigurer                                               |");
                Console.WriteLine("| [1] Changer la langue                                               |");
                Console.WriteLine("| [2] Configurer l'emplacement dossier de destination des sauvegardes |");
                Console.WriteLine("| [3] Configurer l'emplacement du fichier de log                      |");
                Console.WriteLine("| [4] Configurer l'emplacement du fichier state                       |");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [5] Revenir au menu principal                                       |");
                Console.WriteLine("|=====================================================================|");

            }
            else
            {
                Console.WriteLine("|==(EasySave V1.0)====================================================|");
                Console.WriteLine("|============================ Settings ===============================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [0] Reset All                                                       |");
                Console.WriteLine("| [1] Change language                                                 |");
                Console.WriteLine("| [2] Set backup destination path                                     |");
                Console.WriteLine("| [3] Set log file path                                               |");
                Console.WriteLine("| [4] Set state file path                                             |");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [5] Go back to main menu                                            |");
                Console.WriteLine("|=====================================================================|");
            }

            string choice = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(choice)) {
                showErrorInput(); 
                ShowParameters();
            }
            else 
            {
                if(int.TryParse(choice,out int result)) 
                { 
                    step = result; 

                    if(step <= 5)
                    {
                         return step;
                    }
                    else
                    {
                        showErrorInput(); 
                        ShowParameters(); 
                    }
                }
            }

            return step;
        }

        public void ShowMainMenu()
        {
            ViewModel viewmodel = new ViewModel();
            Console.ForegroundColor = ConsoleColor.White;
            string lang = configObj.Language;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("|=====================================================================|");
            Console.WriteLine();
            Console.WriteLine();

            if (lang == "fr")
            {
                Console.WriteLine("|==(EasySave V1.0 / Console)==========================================|");
                Console.WriteLine("|===================== Bienvenue sur EasySave ========================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Afficher les sauvegardes                                        |");
                Console.WriteLine("| [2] Créer une nouvelle sauvegarde                                   |");
                Console.WriteLine("| [3] Supprimer une sauvegarde                                        |");
                Console.WriteLine("| [4] Paramétres                                                      |");
                Console.WriteLine("| [5] Quitter                                                         |");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("|=====>               Veuillez inserer un chiffre               <=====|");

            }
            else
            {
                Console.WriteLine("|==(EasySave V1.0 / Console)==========================================|");
                Console.WriteLine("|======================= Welcome to EasySave =========================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Show backups                                                    |");
                Console.WriteLine("| [2] Create Backup                                                   |");
                Console.WriteLine("| [3] Delete Backup                                                   |");
                Console.WriteLine("| [4] Settings                                                        |");
                Console.WriteLine("| [5] Exit                                                            |");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("|=====>                 Please insert a number                  <=====|");
            }

            string pageString = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(pageString))
            {
                showErrorInput();
                ShowMainMenu();
            }
            else 
            { 
                int page = int.Parse(pageString);
                if(page > 5) { showErrorInput(); ShowMainMenu(); }
                viewmodel.NavigateTo(page); 
            }


        }

        public void showErrorInput()
        {
            string lang = configObj.Language;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            if (lang == "fr")
            {
                Console.WriteLine("Veuillez choisir une option !");
            }
            else
            {
                Console.WriteLine("Please re-enter an option !");
            }

            Thread.Sleep(1500);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ShowSaveMenu()
        {
            ViewModel viewmodel = new ViewModel();
            string lang = configObj.Language;

            int res = viewmodel.checkSavesNumber();

            Console.Clear();

            string type = "";
            string saveName = "";
            string sourcePath = "";
            int Id = 0;


            if (lang == "fr")
            {
                Console.WriteLine("==== Veuillez saisir le type de sauvegarde");
                Console.WriteLine("==== [1] Complete");
                if (res != 0)
                {
                    Console.WriteLine("==== [2] Différentielle");
                }
            }
            else
            {
                Console.WriteLine("==== Please enter the type of backup");
                Console.WriteLine("==== [1] Full");
                if (res != 0)
                {
                    Console.WriteLine("==== [2] Differential");
                }
            }

            type = Console.ReadLine();
            if (type == "1") { type = "full"; } else if (type == "2" && res != 0) { type = "diff"; } else { type = ""; }

            if(string.IsNullOrWhiteSpace(type)) { showErrorInput(); ShowSaveMenu(); }

            if (type == "full")
            {
                if (lang == "fr")
                {

                    Console.WriteLine("==== Etape 2 sur 3");
                    Console.WriteLine("==== Veuillez saisir le nom de la sauvegarde :");
                    saveName = Console.ReadLine();


                    Console.WriteLine("==== Etape 3 sur 3");
                    Console.WriteLine("==== Veuillez saisir le chemin source du fichier :");
                    sourcePath = Console.ReadLine();

                }
                else
                {

                    Console.WriteLine("==== Step 2 of 3");
                    Console.WriteLine("==== Please enter the name of the backup :");
                    saveName = Console.ReadLine();

                    Console.WriteLine("==== Step 3 of 3");
                    Console.WriteLine("==== Please enter the source path of the files :");
                    sourcePath = Console.ReadLine();
                }



                DataState inputObj = new DataState()
                {
                    SaveId = Id,
                    SaveName = saveName,
                    SourcePath = sourcePath,
                    Type = type,
                };

                viewmodel.InitializeSave(inputObj);

            }
            else if (type == "diff")// differential backup
            {
                if (lang == "fr")
                {
                    Console.WriteLine("==== Etape 2 sur 3");
                    Console.WriteLine("==== Veuillez selectionner une sauvegarde :");
                }
                else
                {
                    Console.WriteLine("==== Step 2 of 3");
                    Console.WriteLine("==== Please select a save  :");
                }
                ShowSaveList();
                Console.ForegroundColor = ConsoleColor.White;
                Id = int.Parse(Console.ReadLine());
                DataState saveInfo = viewmodel.GetSaveInfo(Id);

                if (lang == "fr")
                {
                    Console.WriteLine("==== Etape 3 sur 3");
                    Console.WriteLine("==== Veuillez saisir le chemin source des fichiers :");
                    sourcePath = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("==== Step 3 of 3");
                    Console.WriteLine("==== Please enter the source path of the files :");
                    sourcePath = Console.ReadLine();
                }

                DataState inputObj = new DataState()
                {
                    SaveId = saveInfo.SaveId,
                    SaveName = saveInfo.SaveName,
                    SourcePath = sourcePath,
                    Type = type
                };

                viewmodel.InitializeSave(inputObj);
            }
        }

        public void ShowDeleteMenu()
        {

            ViewModel viewmodel = new ViewModel();
            string lang = configObj.Language;

            int userChoice = 0;
            ConsoleKeyInfo userConfirm;

            Console.Clear();

            if (lang == "fr")
            {
                Console.WriteLine("==== Etape 1 sur 2");
                Console.WriteLine("==== Veuillez selectionner la sauvegarde a supprimer");
            }
            else
            {
                Console.WriteLine("==== Step 1 of 2");
                Console.WriteLine("==== Please select the save to delete");
            }

            ShowSaveList();
            string choice = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(choice)) { showErrorInput(); ShowDeleteMenu(); }
            userChoice = int.Parse(choice);


            if (lang == "fr")
            {
                Console.WriteLine("==== Etape 2 sur 2");
                Console.WriteLine("==== Êtes-vous sûr de vouloir supprimer cette sauvegarde ? [O / N]");
            }
            else
            {
                Console.WriteLine("==== Step 2 of 2");
                Console.WriteLine("==== Are you sure you want to delete this backup ? [Y / N]");
            }

            userConfirm = Console.ReadKey();

            if (userConfirm.Key == ConsoleKey.Y || userConfirm.Key == ConsoleKey.O)
            {
                viewmodel.InitializeDeleteSave(userChoice);
            }
            else if (userConfirm.Key == ConsoleKey.N)
            {
                if (lang == "fr")
                {
                    Console.WriteLine("Suppression annulée");
                }
                else
                {
                    Console.WriteLine("deletion canceled");
                }
            }

        }

        public void ShowSaveList()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            DataState[] resultArr = viewModel.GetSavelist();

            foreach (var result in resultArr)
            {
                Console.WriteLine($"|==== [{result.SaveId}] {result.SaveName}  {result.Time}");
            }
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
                Console.WriteLine("|===          Veuillez choisir le chemin des sauvegardes             ===|");
                Console.WriteLine("|===                Choose the path of the backups                   ===|");
                Console.WriteLine("|=======================================================================|");
                targetDir = Console.ReadLine() ?? "";

                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===   Veuillez choisir le chemin du dossier ou iront les logs       ===|");
                Console.WriteLine("|===            Choose the path of the logs directory                ===|");
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