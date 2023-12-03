using Easysave.Models;
using Easysave.ViewModels;
using System;
namespace Easysave.Views
{
    public class Navigation : View
    {
        Config ConfigObj = Config.getConfig();


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
            string lang = ConfigObj.Language;

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

            int step = int.Parse(Console.ReadLine());
            return step;

        }

        public void ShowMainMenu()
        {
            ViewModel viewmodel = new ViewModel();
            Console.ForegroundColor = ConsoleColor.White;
            string lang = ConfigObj.Language;
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


            int page = int.Parse(Console.ReadLine());
            viewmodel.NavigateTo(page);

        }

        public void ShowSaveMenu()
        {
            ViewModel viewmodel = new ViewModel();
            string lang = ConfigObj.Language;

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
                if(res !=  0)
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
            if (type == "1") { type = "full"; } else if (type == "2" && res != 0) { type = "diff"; } else { type = "autre"; }

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
            else if(type == "diff")// differential backup
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
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                if (lang == "fr") { Console.WriteLine("Veuillez resaisir votre choix !"); } else { Console.WriteLine("Please re-enter your choice!"); }
                Thread.Sleep(1500);
                Console.ForegroundColor= ConsoleColor.White;
                ShowSaveMenu();
            }
        }



        public void ShowDeleteMenu()
        {

            ViewModel viewmodel = new ViewModel();
            string lang = ConfigObj.Language;

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
            userChoice = int.Parse(Console.ReadLine());

            if (lang == "fr")
            {
                Console.WriteLine("==== Etape 2 sur 2");
                Console.WriteLine("==== Êtes-vous sûr de vouloir supprimer cette sauvegarde ? (O / N)");
            }
            else
            {
                Console.WriteLine("==== Step 2 of 2");
                Console.WriteLine("==== Are you sure you want to delete this backup ? (Y / N)");
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
                } else
                {
                    Console.WriteLine("deletion canceled");
                }
            }

        }

    }
}

