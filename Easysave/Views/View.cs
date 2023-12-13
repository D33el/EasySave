using System.Text.RegularExpressions;
using EasySave.ViewModels;

namespace EasySave.Views
{
    public class View
    {
        private SaveViewModel _saveViewModel = new();
        private Config _config = Config.GetConfig();

        public void Navigate(int page)
        {
            switch (page)
            {
                case 1: // Show the saves
                    SaveViewModel.ShowSaveList();
                    ShowMainMenu();
                    break;
                case 2: // Create a new save
                    ShowSaveMenu();
                    ShowMainMenu();
                    break;

                case 3: // Delete a save
                    ShowDeleteMenu();
                    ShowMainMenu();
                    break;

                case 4: // Configuration
                    int step = ShowParameters();
                    if (step == 0) { ShowMainMenu(); }
                    else
                    {
                        SetParameters(step);
                        Navigate(4);
                    }
                    break;

                case 5: // Exit
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }

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
            Console.WriteLine("");
            Console.WriteLine("|=======================================================================|");
            Console.WriteLine("|===               Paramétrage initial / Initial Setup               ===|");
            Console.WriteLine("|=======================================================================|");
            Console.WriteLine("");
        }

        public int ShowParameters()
        {
            string lang = _config.Language;
            int page;
            string[] possibleChoices = { "0", "1", "2", "3", "4", "5" };
            string choice;
            Console.Clear();

            if (lang == "fr")
            {
                Console.WriteLine("|==(EasySave V1.0)====================================================|");
                Console.WriteLine("|=========================== Paramétres ==============================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Tout reconfigurer                                               |");
                Console.WriteLine("| [2] Changer la langue                                               |");
                Console.WriteLine("| [3] Configurer l'emplacement dossier de destination des sauvegardes |");
                Console.WriteLine("| [4] Configurer l'emplacement du fichier de log                      |");
                Console.WriteLine("| [5] Changer le type de logs                                         |");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [0] Revenir au menu principal                                       |");
                Console.WriteLine("|=====================================================================|");
            }
            else
            {
                Console.WriteLine("|==(EasySave V1.0)====================================================|");
                Console.WriteLine("|============================ Settings ===============================|");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [1] Reset All                                                       |");
                Console.WriteLine("| [2] Change language                                                 |");
                Console.WriteLine("| [3] Set backup destination path                                     |");
                Console.WriteLine("| [4] Set log file path                                               |");
                Console.WriteLine("| [5] Change logs type                                                |");
                Console.WriteLine("|=====================================================================|");
                Console.WriteLine("| [0] Go back to main menu                                            |");
                Console.WriteLine("|=====================================================================|");
            }

            do
            {
                if (lang == "fr")
                {
                    Console.Write("Veuillez choisir un chiffre : ");
                }
                else
                {
                    Console.Write("Please choose a number : ");
                }
                choice = Console.ReadLine();
            } while (!possibleChoices.Contains(choice));

            page = int.Parse(choice);

            return page;
        }

        public void ShowMainMenu()
        {
            string lang = _config.Language;

            string[] possibleChoices = { "1", "2", "3", "4", "5" };
            string choice;
            int page;

            Console.ForegroundColor = ConsoleColor.White;
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
            do
            {
                if (lang == "fr")
                {
                    Console.Write("Veuillez choisir un chiffre : ");
                }
                else
                {
                    Console.Write("Please choose a number : ");
                }
                choice = Console.ReadLine();
            } while (!possibleChoices.Contains(choice));

            page = int.Parse(choice);

            Navigate(page);
        }

        public void ShowSaveMenu()
        {
            string lang = _config.Language;
            int savesNumber = SaveViewModel.GetSavesNumber();

            Console.Clear();

            string type;
            string saveName;
            string sourcePath;
            int id = 0;


            if (lang == "fr")
            {
                Console.WriteLine("|==== Veuillez saisir le type de sauvegarde");
                Console.WriteLine("|==== [1] Complete");
                Console.WriteLine("|==== [2] Différentielle");
            }
            else
            {
                Console.WriteLine("|==== Please enter the type of backup");
                Console.WriteLine("|==== [1] Full");
                Console.WriteLine("|==== [2] Differential");
            }
            do
            {
                if (lang == "fr")
                {
                    Console.Write("Veuillez choisir un chiffre : ");
                }
                else
                {
                    Console.Write("Please choose a number : ");
                }
                type = Console.ReadLine();
            } while (type != "1" && type != "2");
            if (type == "1") { type = "full"; } else if (type == "2") { type = "diff"; }

            if (type == "full")
            {
                if (lang == "fr")
                {
                    Console.WriteLine("|==== Etape 2 sur 3");
                    Console.WriteLine("|==== Veuillez saisir le nom de la sauvegarde :");

                }
                else
                {
                    Console.WriteLine("|==== Step 2 of 3");
                    Console.Write("|==== Please enter the name of the backup : ");
                }

                do
                {
                    saveName = Console.ReadLine();
                } while (!Regex.IsMatch(saveName, @"^[\w\s -]+$"));


                if (lang == "fr")
                {
                    Console.WriteLine("==== Etape 3 sur 3");
                    Console.WriteLine("==== Veuillez saisir le chemin source du fichier : ");
                }
                else
                {
                    Console.WriteLine("|==== Step 3 of 3");
                    Console.WriteLine("|==== Please enter the source path of the files : ");
                }
                do
                {
                    sourcePath = Console.ReadLine();
                } while (!Regex.IsMatch(sourcePath, @"^(.+)\/([^\/]+)$"));


                _saveViewModel.InitializeSave(saveName, type, sourcePath, id);
            }
            else if (type == "diff")
            {
                if (savesNumber == 0)
                {
                    if (lang == "fr")
                    {
                        Console.WriteLine("/!\\ Vous n'avez aucune sauvegarde.");
                    }
                    else
                    {
                        Console.WriteLine("/!\\ You dont have any backup");
                    }

                    ShowMainMenu();
                    return;
                }

                if (lang == "fr")
                {
                    Console.WriteLine("|==== Etape 2 sur 3");
                    Console.WriteLine("|==== Veuillez selectionner une sauvegarde.");
                }
                else
                {
                    Console.WriteLine("|==== Step 2 of 3");
                    Console.WriteLine("|==== Please select a save.");
                }
                int[] savesIds = SaveViewModel.ShowSaveList();
                do
                {
                    if (lang == "fr")
                    {
                        Console.Write("Veuillez choisir un chiffre : ");
                    }
                    else
                    {
                        Console.Write("Please choose a number : ");
                    }
                    id = int.Parse(Console.ReadLine());
                } while (!savesIds.Contains(id));

                if (lang == "fr")
                {
                    Console.WriteLine("|==== Etape 3 sur 3");
                    Console.Write("|==== Veuillez saisir le chemin source des fichiers :");
                }
                else
                {
                    Console.WriteLine("==== Step 3 of 3");
                    Console.Write("==== Please enter the source path of the files :");
                }
                do
                {
                    sourcePath = Console.ReadLine();
                } while (!Regex.IsMatch(sourcePath, @"^(.+)\/([^\/]+)$"));

                _saveViewModel.InitializeSave("", type, sourcePath, id);
            }
        }

        public void ShowSaveExecutionMenu()
        {
            string lang = _config.Language;

            if (lang == "fr")
            {
                Console.WriteLine("|=== Voulez vous réexecuter une ou plusieurs sauvegardes ? [O / N]");
            }
            else
            {
                Console.WriteLine("|=== Do you want to re execute backups ? [O / N]");
            }
            ConsoleKeyInfo choice = Console.ReadKey();

            if (choice.Key == ConsoleKey.Y || choice.Key == ConsoleKey.O)
            {
                if (lang == "fr")
                {
                    Console.WriteLine("|=== Choisissez les sauvegardes a executer.");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\\Info/ [ 1-3 ] pour executer de 1 a 3 \\Info/");
                    Console.WriteLine("\\Info/ [ 1,3 ] pour executer 1 et 3 \\Info/");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine("|=== Choose the backups to execute.");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\\Info/ [ 1-3 ] to execute from 1 to 3 \\Info/");
                    Console.WriteLine("\\Info/ [ 1,3 ] to execute 1 and 3 \\Info/");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                string input;
                do
                {
                    input = Console.ReadLine() ?? "";
                } while (!Regex.IsMatch(input, @"^\d+(-\d+|,\d+)?$"));

                string[] parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (string part in parts)
                {
                    // Handle range
                    if (part.Contains('-'))
                    {
                        string[] range = part.Split('-');
                        if (range.Length == 2 && int.TryParse(range[0], out int start) && int.TryParse(range[1], out int end))
                        {
                            for (int i = start; i <= end; i++)
                            {
                                _saveViewModel.InitializeSaveReexecution(i);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid range format.");
                        }
                    }
                    // Handle single number
                    else if (int.TryParse(part, out int backupNumber))
                    {
                        _saveViewModel.InitializeSaveReexecution(backupNumber);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format.");
                    }
                }
            }
            else if (choice.Key == ConsoleKey.N)
            {
                return;
            }

        }

        public void ShowDeleteMenu()
        {
            string lang = _config.Language;

            int savesNumber = SaveViewModel.GetSavesNumber();

            if (savesNumber == 0)
            {
                if (lang == "fr")
                {
                    Console.WriteLine("/!\\ Il n'y a rien a supprimer, vous n'avez aucune sauvegarde.");
                }
                else
                {
                    Console.WriteLine("/!\\ Nothing to delete, you have no backups.");
                }
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            int choice;
            ConsoleKeyInfo userConfirm;

            Console.Clear();

            if (lang == "fr")
            {
                Console.WriteLine("|==== Etape 1 sur 2");
                Console.WriteLine("|==== Veuillez selectionner la sauvegarde a supprimer");
            }
            else
            {
                Console.WriteLine("|==== Step 1 of 2");
                Console.WriteLine("|==== Please select the save to delete");
            }
            int[] savesIds = SaveViewModel.ShowSaveList();

            do
            {
                if (lang == "fr")
                {
                    Console.Write("Veuillez choisir un chiffre : ");
                }
                else
                {
                    Console.Write("Please choose a number : ");
                }
                choice = int.Parse(Console.ReadLine());
            } while (!savesIds.Contains(choice));

            if (lang == "fr")
            {
                Console.WriteLine("|==== Etape 2 sur 2");
                Console.Write("|==== Êtes-vous sûr de vouloir supprimer cette sauvegarde ? [O / N]");
            }
            else
            {
                Console.WriteLine("|==== Step 2 of 2");
                Console.Write("|==== Are you sure you want to delete this backup ? [Y / N]");
            }

            userConfirm = Console.ReadKey();

            if (userConfirm.Key == ConsoleKey.Y || userConfirm.Key == ConsoleKey.O)
            {
                _saveViewModel.InitializeDeleteSave(choice);
            }
            else
            {
                ShowMainMenu();
            }
        }

        public void SetParameters(int step)
        {
            string lang;
            string targetDir;
            string logsDir;
            string logsType;
            string stateFilePath;

            if (step == 1)
            {
                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===            Veuillez choisir une langue [eng / fr]               ===|");
                Console.WriteLine("|===                 Choose a language [eng / fr]                    ===|");
                Console.WriteLine("|=======================================================================|");

                do
                {
                    lang = Console.ReadLine();

                } while (lang != "fr" && lang != "eng");


                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===          Veuillez choisir le chemin des sauvegardes             ===|");
                Console.WriteLine("|===                Choose the path of the backups                   ===|");
                Console.WriteLine("|=======================================================================|");
                do
                {
                    targetDir = Console.ReadLine();

                } while (!Regex.IsMatch(targetDir, @"^(.+)\/([^\/]+)$"));

                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===   Veuillez choisir le chemin du dossier ou iront les logs       ===|");
                Console.WriteLine("|===            Choose the path of the logs directory                ===|");
                Console.WriteLine("|=======================================================================|");
                do
                {
                    logsDir = Console.ReadLine();

                } while (!Regex.IsMatch(targetDir, @"^(.+)\/([^\/]+)$"));

                Console.WriteLine("|=======================================================================|");
                Console.WriteLine("|===          Veuillez choisir le type de logs [json / xml]          ===|");
                Console.WriteLine("|===              Choose the type of logs [json / xml]               ===|");
                Console.WriteLine("|=======================================================================|");
                do
                {
                    logsType = Console.ReadLine();

                } while (logsType != "json" && logsType != "xml");

            }
            else
            {
                lang = _config.Language;
                targetDir = _config.TargetDir;
                logsDir = _config.LogsDir;
                logsType = _config.LogsType;

                switch (step)
                {
                    case 2:
                        if (lang == "fr")
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

                    case 3:
                        if (lang == "fr")
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

                    case 4:
                        if (lang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===        Veuillez choisir le chemin du dossier des logs         ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===             Choose the path of the logs directory             ===|");
                        }
                        logsDir = Console.ReadLine();
                        break;

                    case 5:
                        if (lang == "fr")
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===        Veuillez choisir le type de logs [ json / xml ]         ===|");
                        }
                        else
                        {
                            Console.WriteLine("|=====================================================================|");
                            Console.WriteLine("|===             Choose the type of logs [ json / xml ]             ===|");
                        }
                        logsType = Console.ReadLine();
                        break;

                }
            }

            _config.Language = lang;
            _config.LogsDir = logsDir;
            _config.LogsType = logsType;
            _config.TargetDir = targetDir;

            _config.SaveConfig();
            _config.LoadConfig();
        }
    }
}